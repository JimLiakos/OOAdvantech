using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{f4bd8f27-d34a-489b-a5ec-28c5d72a61e8}</MetaDataID>
    class RoseVisualStudioBridge
    {
        /// <MetaDataID>{826a828c-61ad-4ea4-b7ac-9873fc809416}</MetaDataID>
        internal static string GetTypeName(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            string typeName = null;
            if (classifier.OwnedTemplateSignature != null)
                typeName = GetTemplateName(classifier.OwnedTemplateSignature);
            else if (classifier.TemplateBinding != null)
                typeName = GetTemplateInstantiationName(classifier.TemplateBinding);
            else
                typeName = classifier.Name;
            return typeName;
        }
        /// <MetaDataID>{866809b3-4394-445d-b2cd-31781b2fe01c}</MetaDataID>
        internal static string GetTypeFullName(OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            string typeFullName = null;
            if (classifier.OwnedTemplateSignature != null)
                typeFullName = GetTemplateName(classifier.OwnedTemplateSignature);
            else if (classifier.TemplateBinding != null)
                typeFullName = GetTemplateInstantiationName(classifier.TemplateBinding);
            else
                typeFullName = classifier.Name;

            OOAdvantech.MetaDataRepository.Namespace _namespace = classifier.Namespace;
            while (_namespace != null)
            {
                typeFullName = _namespace.Name + "." + typeFullName;
                _namespace = _namespace.Namespace;
            }
            if (classifier.IsTemplate || classifier.IsTemplateInstantiation)
            {
                int pos = typeFullName.IndexOf("`");
                if (pos != -1)
                    typeFullName = typeFullName.Substring(0, pos);
            }
            return typeFullName;


        }
        /// <MetaDataID>{4c67e8af-409d-45b8-9d15-f862c212b15e}</MetaDataID>
        public static string GetTemplateName(OOAdvantech.MetaDataRepository.TemplateSignature signature)
        {

            string name = (signature.Template as OOAdvantech.MetaDataRepository.MetaObject).Name;
            name = name.Substring(0, name.IndexOf("`"));

            //  string identityString = name;
            string templateParma = null;

            foreach (OOAdvantech.MetaDataRepository.TemplateParameter parameter in signature.OwnedParameters)
            {
                //MetaDataRepository.Classifier parameterClassifier = parameterSubstitution.ActualParameters[0] as MetaDataRepository.Classifier;
                //  identityString += "[" + parameter + "," + parameterClassifier.ImplementationUnit.Identity + "]";
                if (templateParma == null)
                    templateParma += "<";
                else
                    templateParma += ",";

                templateParma += parameter.Name;
            }
            if (templateParma != null)
                name += templateParma + ">";

            return name;
        }



        /// <MetaDataID>{b1f04113-90dc-40a3-98da-0399b6fa4f3e}</MetaDataID>
        public static string GetTemplateInstantiationName(OOAdvantech.MetaDataRepository.TemplateBinding templateBinding)
        {

            string name = (templateBinding.Signature.Template as OOAdvantech.MetaDataRepository.MetaObject).Name;
            name = name.Substring(0, name.IndexOf("`"));

            string templateParma = null;

            foreach (OOAdvantech.MetaDataRepository.TemplateParameterSubstitution parameterSubstitution in templateBinding.ParameterSubstitutions)
            {
                OOAdvantech.MetaDataRepository.IParameterableElement parameterClassifier = parameterSubstitution.ActualParameters[0];
                if (templateParma == null)
                    templateParma += "<";
                else
                    templateParma += ",";

                if (parameterClassifier is OOAdvantech.MetaDataRepository.Classifier)
                    templateParma +=MetaObjectMapper.GetShortNameFor((parameterClassifier as OOAdvantech.MetaDataRepository.Classifier).FullName);
                else
                    templateParma += parameterClassifier.Name;

            }
            if (templateParma != null)
                name += templateParma + ">";

            return name;
        }

        /// <MetaDataID>{97dc0739-7c4b-4eff-8e70-71e187e74e01}</MetaDataID>
        internal void UpdateRoseItem(RationalRose.RoseApplication roseApplication)
        {
            if (!IsAdmin)
            {
                System.Windows.Forms.MessageBox.Show("You can't access Visual Studio \r\nRun Rose as administrator.", "Rose Visual Studio Synchronizer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            try
            {
                System.DateTime startTime = DateTime.Now;

                if (roseApplication.CurrentModel.GetSelectedClasses().Count > 0)
                {
                    //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {
                        MetaObjectMapper.Clear();
                        metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                        OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack(roseApplication);
                        metaObjectsStack.StartSynchronize();
                        OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                        for (int i = 0; i < roseApplication.CurrentModel.GetSelectedClasses().Count; i++)
                        {
                            RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedClasses().GetAt((short)(i + 1));
                            OOAdvantech.MetaDataRepository.Classifier classifier = null;
                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);

                            Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseComponent);
                            EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                            OOAdvantech.CodeMetaDataRepository.Project project = null;
                            foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                            {
                                if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                                    project = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(vsProject.DTE).GetProject(OOAdvantech.CodeMetaDataRepository.Project.GetProjectIdentity(vsProject));
                            }
                            if (project == null)
                                continue;

                            if (roseClass.Stereotype == "Interface")
                                classifier = new Interface(roseClass, component);
                            else if (roseClass.Stereotype.Trim().ToLower() == "Structure".ToLower())
                                classifier = new Structure(roseClass, component);
                            else
                                classifier = new Class(roseClass, component);
                            OOAdvantech.MetaDataRepository.Classifier codeClassifier = project.GetClassifier(classifier.Identity.ToString());
                            if (codeClassifier == null)
                                codeClassifier = MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(classifier, null) as OOAdvantech.MetaDataRepository.Classifier;
                            classifier.Synchronize(codeClassifier);

                        }
                       // stateTransition.Consistent = true;
                    }
                }
                System.TimeSpan timeSpan = System.DateTime.Now - startTime;
                System.Diagnostics.Debug.WriteLine("GetExternalClassifier  " + timeSpan.TotalMilliseconds.ToString());
                 

            }
            catch (System.Exception error)
            {

            }
            finally
            {
                if (metaObjectsStack != null)
                {
                    metaObjectsStack.StopSynchronize();
                    MetaObjectMapper.Clear();
                    if (MetaObjectsStack.CurrentMetaObjectCreator is MetaObjectsStack)
                    {
                        (MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).Clear();
                        MetaObjectsStack.CurrentMetaObjectCreator = null;
                    }

                }

            }

        }
        /// <MetaDataID>{74c840f8-f30b-4be1-a14a-a019f05dbede}</MetaDataID>
        internal void UpdateMemberModel(RationalRose.RoseApplication roseApplication)
        {
            if (!IsAdmin)
            {
                System.Windows.Forms.MessageBox.Show("You can't access Visual Studio \r\nRun Rose as administrator.", "Rose Visual Studio Synchronizer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            try
            {

                if (roseApplication.CurrentModel.GetSelectedItems().Count > 0)
                {

                    MetaObjectMapper.Clear();
                    metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                    OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack(roseApplication);
                    metaObjectsStack.StartSynchronize();
                    OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                    //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {

                        for (int i = 0; i < roseApplication.CurrentModel.GetSelectedItems().Count; i++)
                        {
                            RationalRose.RoseOperation roseOperetion = roseApplication.CurrentModel.GetSelectedItems().GetAt((short)(i + 1)) as RationalRose.RoseOperation;
                            RationalRose.RoseAttribute roseAttribute = roseApplication.CurrentModel.GetSelectedItems().GetAt((short)(i + 1)) as RationalRose.RoseAttribute;
                            RationalRose.RoseRole roseRole = roseApplication.CurrentModel.GetSelectedItems().GetAt((short)(i + 1)) as RationalRose.RoseRole;

                            RationalRose.RoseClass roseClass = null;
                            if (roseOperetion != null)
                                roseClass = roseOperetion.ParentClass;

                            if (roseAttribute != null)
                                roseClass = roseAttribute.ParentClass;

                            if (roseRole != null)
                            {
                                if (!roseRole.Navigable)
                                    return;
                                if (roseRole.Equals(roseRole.Association.Role1))
                                    roseClass = roseRole.Association.Role2.Class;
                                else
                                    roseClass = roseRole.Association.Role1.Class;
                                if (roseClass == null)
                                    return;

                            }


                            OOAdvantech.MetaDataRepository.Classifier classifier = null;
                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);

                            Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseComponent);
                            EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);

                            OOAdvantech.CodeMetaDataRepository.Project project = null;
                            foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                            {
                                if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                                    project = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(vsProject.DTE).GetProject(OOAdvantech.CodeMetaDataRepository.Project.GetProjectIdentity(vsProject));
                            }
                            if (project == null)
                                continue;

                            if (roseClass.Stereotype == "Interface")
                                classifier = new Interface(roseClass, component);
                            else if (roseClass.Stereotype.Trim().ToLower() == "Structure".ToLower())
                                classifier = new Structure(roseClass, component);
                            else
                                classifier = new Class(roseClass, component);
                            OOAdvantech.MetaDataRepository.Classifier codeClassifier = project.GetClassifier(classifier.Identity.ToString());
                            if (codeClassifier == null)
                                codeClassifier = MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(classifier, null) as OOAdvantech.MetaDataRepository.Classifier;
                            OOAdvantech.CodeMetaDataRepository.MetaObjectsStack.ActiveProject = project;


                            if (roseOperetion != null || roseAttribute != null)
                            {
                                foreach (OOAdvantech.MetaDataRepository.Feature feature in classifier.Features)
                                {
                                    if (roseOperetion != null)
                                    {
                                        if (feature.Owner==classifier && feature is Operation && (feature as Operation).RoseOperation.Equals(roseOperetion))
                                        {
                                            OOAdvantech.MetaDataRepository.Feature codeFeature= codeClassifier.GetFeature(feature.Identity.ToString(), false) ;
                                            OOAdvantech.MetaDataRepository.Operation codeOperation = codeFeature as OOAdvantech.MetaDataRepository.Operation; 

                                            if (codeOperation != null)
                                                feature.Synchronize(codeOperation);//.Synchronize(feature);
                                            else
                                            {
                                                if (codeFeature is OOAdvantech.MetaDataRepository.Method)
                                                    feature.Synchronize(codeFeature);
                                                //codeOperation = new OOAdvantech.CodeMetaDataRepository.Operation(feature.Name, codeClassifier);
                                                //codeOperation.Synchronize(feature);
                                            }
                                            break;
                                        }
                                    }


                                    if (roseAttribute != null)
                                    {
                                        if (feature is Attribute && (feature as Attribute).RoseAttribute.Equals(roseAttribute))
                                        {
                                            OOAdvantech.MetaDataRepository.Feature codeFeature = codeClassifier.GetFeature(feature.Identity.ToString(), false);
                                            OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeFeature as OOAdvantech.MetaDataRepository.Attribute;
                                            if (codeAttribute != null)
                                                feature.Synchronize(codeAttribute);
                                            else
                                            {

                                                OOAdvantech.MetaDataRepository.AttributeRealization codeAttributeRealization = codeFeature as OOAdvantech.MetaDataRepository.AttributeRealization;
                                                if (codeAttributeRealization != null)
                                                    feature.Synchronize(codeAttributeRealization.Specification);

                                                //OOAdvantech.MetaDataRepository.AssociationEndRealization codeAssociationEndRealization = codeFeature as OOAdvantech.MetaDataRepository.AssociationEndRealization;
                                                //if (codeAssociationEndRealization != null)
                                                //    feature.Synchronize(codeAttributeRealization.Specification);



                                                //codeAttribute = new OOAdvantech.CodeMetaDataRepository.Attribute(feature.Name, codeClassifier);
                                                //codeAttribute.Synchronize(feature);
                                            }
                                            break;
                                        }
                                        if (feature is AttributeRealization && (feature as AttributeRealization).RoseAttribute.Equals(roseAttribute))
                                        {
                                            OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Attribute;
                                            if (codeAttribute != null)
                                                feature.Synchronize(codeAttribute);
                                            else
                                            {
                                                OOAdvantech.MetaDataRepository.AttributeRealization codeAttributeRealization = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.AttributeRealization;
                                                feature.Synchronize(codeAttributeRealization);
                                                //codeAttribute = new OOAdvantech.CodeMetaDataRepository.Attribute(feature.Name, codeClassifier);
                                                //codeAttribute.Synchronize(feature);
                                            }
                                            break;
                                        }
                                    }
                                }

                            }
                            if (roseRole != null)
                            {
                                foreach (AssociationEnd associationEnd in classifier.GetAssociateRoles(false))
                                {
                                    if (associationEnd.RoseRole.Equals(roseRole))
                                    {
                                        OOAdvantech.MetaDataRepository.AssociationEnd codeAssociationEnd = codeClassifier.GetRole(associationEnd.GetOtherEnd().Identity.ToString());
                                        if (codeAssociationEnd != null)
                                            associationEnd.GetOtherEnd().Synchronize(codeAssociationEnd);//.Synchronize();
                                        else
                                        {
                                            //OOAdvantech.MetaDataRepository.Classifier otherEndCodeClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(associationEnd.Specification, null) as OOAdvantech.MetaDataRepository.Classifier;
                                            //if (otherEndCodeClassifier == null)
                                            //    return;

                                            //OOAdvantech.MetaDataRepository.AssociationEnd otherEndCodeAssociationEnd = null;
                                            //if (associationEnd.IsRoleA)
                                            //    otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(associationEnd.Name, otherEndCodeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleA);
                                            //else
                                            //    otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(associationEnd.Name, otherEndCodeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleB);
                                            //if (associationEnd.GetOtherEnd().IsRoleA)
                                            //    codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd(associationEnd.GetOtherEnd().Name, codeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleA);
                                            //else
                                            //    codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd(associationEnd.GetOtherEnd().Name, codeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleB);

                                            //OOAdvantech.CodeMetaDataRepository.Association association = null;
                                            //if (codeAssociationEnd.IsRoleA)
                                            //    association = new OOAdvantech.CodeMetaDataRepository.Association(associationEnd.Name, codeAssociationEnd, otherEndCodeAssociationEnd, associationEnd.Association.Identity.ToString());
                                            //else
                                            //    association = new OOAdvantech.CodeMetaDataRepository.Association(associationEnd.Name, otherEndCodeAssociationEnd, codeAssociationEnd, associationEnd.Association.Identity.ToString());
                                            //codeAssociationEnd.Synchronize(associationEnd.GetOtherEnd());



                                            //codeAttribute = new OOAdvantech.CodeMetaDataRepository.Attribute(feature.Name, codeClassifier);
                                            //codeAttribute.Synchronize(feature);
                                        }
                                        break;
                                    }

                                }
                            }


                            //codeClassifier.Synchronize(classifier);
                           // project.IDEManager.RefreshProject(project.Identity.ToString(), codeClassifier.Identity.ToString());

                        }
                        //stateTransition.Consistent = true;
                    }
                }

            }
            catch (System.Exception error)
            {

            }
            finally
            {
                if (metaObjectsStack != null)
                {
                    metaObjectsStack.StopSynchronize();
                    MetaObjectMapper.Clear();
                }
            }

        }
        /// <MetaDataID>{b0228bd5-ab7c-471b-974e-9929af9c40f0}</MetaDataID>
        internal void UpdateMemberCode(RationalRose.RoseApplication roseApplication)
        {
            if (!IsAdmin)
            {
                System.Windows.Forms.MessageBox.Show("You can't access Visual Studio \r\nRun Rose as administrator.", "Rose Visual Studio Synchronizer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            //OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            OOAdvantech.MetaDataRepository.Classifier classifier = null;
            OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager = null;
            try
            {

                if (roseApplication.CurrentModel.GetSelectedItems().Count > 0)
                {

                    //MetaObjectMapper.Clear();
                    //OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                    //(OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as OOAdvantech.CodeMetaDataRepository.MetaObjectsStack).StartSynchronize();
                    //OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                    // using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {

                        for (int i = 0; i < roseApplication.CurrentModel.GetSelectedItems().Count; i++)
                        {
                            RationalRose.RoseItem roseItem = roseApplication.CurrentModel.GetSelectedItems().GetAt((short)(i + 1));

                            RationalRose.RoseOperation roseOperetion = roseItem as RationalRose.RoseOperation;
                            RationalRose.RoseAttribute roseAttribute = roseItem as RationalRose.RoseAttribute;
                            RationalRose.RoseRole roseRole =roseItem as RationalRose.RoseRole;

                            RationalRose.RoseClass roseClass = null;
                            if (roseOperetion != null)
                                roseClass = roseOperetion.ParentClass;

                            if (roseAttribute != null)
                                roseClass = roseAttribute.ParentClass;

                            if (roseRole != null)
                            {
                                if (!roseRole.Navigable)
                                    return;
                                if (roseRole.Equals(roseRole.Association.Role1))
                                    roseClass = roseRole.Association.Role2.Class;
                                else
                                    roseClass = roseRole.Association.Role1.Class;
                                if (roseClass == null)
                                    return;

                            }



                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);

                            Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseComponent);

                            EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                            IDEManager = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(dte);
                            IDEManager.StartCodeModelSynchronize();
                            OOAdvantech.CodeMetaDataRepository.Project project = null;
                            foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                            {
                                if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                                    project = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(vsProject.DTE).GetProject(OOAdvantech.CodeMetaDataRepository.Project.GetProjectIdentity(vsProject));
                            }
                            if (project == null)
                                continue;



                            if (roseClass.Stereotype == "Interface")
                                classifier = new Interface(roseClass, component);
                            else if (roseClass.Stereotype.Trim().ToLower() == "Structure".ToLower())
                                classifier = new Structure(roseClass, component);
                            else
                                classifier = new Class(roseClass, component);
                            OOAdvantech.MetaDataRepository.Classifier codeClassifier = project.GetClassifier(classifier.Identity.ToString());
                            if (codeClassifier == null)
                                codeClassifier = MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(classifier, null) as OOAdvantech.MetaDataRepository.Classifier;
                            OOAdvantech.CodeMetaDataRepository.MetaObjectsStack.ActiveProject = project;

                            #region Update Features
                            if (roseOperetion != null || roseAttribute != null)
                            {
                                foreach (OOAdvantech.MetaDataRepository.Feature feature in classifier.Features)
                                {
                                    if (roseOperetion != null)
                                    {
                                        if (feature is Operation && (feature as Operation).RoseOperation.Equals(roseOperetion))
                                        {
                                            OOAdvantech.MetaDataRepository.Operation codeOperation = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Operation;
                                            if (codeOperation != null)
                                            {
                                                object obj = (feature as OOAdvantech.MetaDataRepository.Operation).Parameters;
                                                obj = (feature as OOAdvantech.MetaDataRepository.Operation).ReturnType;
                                                obj = (feature as OOAdvantech.MetaDataRepository.Operation).ParameterizedReturnType;
                                                codeOperation.Synchronize(feature);
                                            }
                                            else
                                            {
                                                codeOperation = codeClassifier.AddOperation(feature.Name, (feature as OOAdvantech.MetaDataRepository.Operation).ReturnType);
                                                object obj = (feature as OOAdvantech.MetaDataRepository.Operation).Parameters;
                                                obj = (feature as OOAdvantech.MetaDataRepository.Operation).ReturnType;
                                                obj = (feature as OOAdvantech.MetaDataRepository.Operation).ParameterizedReturnType;
                                                codeOperation.Synchronize(feature);
                                            }
                                            break;
                                        }
                                        if (feature is Method && (feature as Method).RoseOperation.Equals(roseOperetion))
                                        {
                                            OOAdvantech.MetaDataRepository.Method codeMethod = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Method;
                                            if (codeMethod != null)
                                            {
                                                object obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.Parameters;
                                                obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ReturnType;
                                                obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ParameterizedReturnType;

                                                codeMethod.Synchronize(feature);
                                            }
                                            else
                                            {
                                                OOAdvantech.MetaDataRepository.Operation codeOperation = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Operation;
                                                if (codeOperation != null)
                                                {
                                                    object obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.Parameters;
                                                    obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ReturnType;
                                                    obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ParameterizedReturnType;
                                                    codeOperation.Synchronize(feature);
                                                }
                                                else
                                                {
                                                    codeOperation = codeClassifier.AddOperation(feature.Name, (feature as OOAdvantech.MetaDataRepository.Method).Specification.ReturnType);
                                                    object obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.Parameters;
                                                    obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ReturnType;
                                                    obj = (feature as OOAdvantech.MetaDataRepository.Method).Specification.ParameterizedReturnType;
                                                    codeOperation.Synchronize(feature);
                                                }
                                            }
                                            break;
                                        }
                                    }
                                    if (roseAttribute != null)
                                    {
                                        if (feature is Attribute && (feature as Attribute).RoseAttribute.Equals(roseAttribute))
                                        {
                                            OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Attribute;
                                            OOAdvantech.MetaDataRepository.AttributeRealization codeAttributeRealization = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.AttributeRealization;
                                            OOAdvantech.MetaDataRepository.AssociationEndRealization codeAttributeAssociationEndRealization = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.AssociationEndRealization;

                                            if (codeAttribute != null)
                                                codeAttribute.Synchronize(feature);
                                            else if (codeAttributeRealization != null)
                                                codeAttributeRealization.Synchronize(feature);
                                            else if (codeAttributeAssociationEndRealization != null)
                                                codeAttributeAssociationEndRealization.Synchronize(feature);
                                            else
                                            {
                                                codeAttribute = codeClassifier.AddAttribute(feature.Name, (feature as Attribute).Type, "");
                                                codeAttribute.Synchronize(feature);
                                            }
                                            break;
                                        }
                                        if (feature is AttributeRealization && (feature as AttributeRealization).RoseAttribute.Equals(roseAttribute))
                                        {
                                            OOAdvantech.MetaDataRepository.AttributeRealization codeAttributeRealization = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.AttributeRealization;
                                            if (codeAttributeRealization != null)
                                            {
                                                object obj = (feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification.Type;
                                                codeAttributeRealization.Synchronize(feature);
                                            }
                                            else
                                            {
                                                object obj = (feature as OOAdvantech.MetaDataRepository.AttributeRealization).Specification.Type;
                                                OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Attribute;
                                                if (codeAttribute != null)
                                                    codeAttribute.Synchronize(feature);
                                                else
                                                {
                                                    codeAttribute = codeClassifier.AddAttribute(feature.Name, (feature as AttributeRealization).Type, "");
                                                    codeAttribute.Synchronize(feature);
                                                }
                                            }

                                            break;
                                        }

                                        if (feature is AssociationEndRealization && (feature as AssociationEndRealization).RoseAttribute.Equals(roseAttribute))
                                        {
                                            OOAdvantech.MetaDataRepository.AssociationEndRealization codeAssociationEndRealization = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.AssociationEndRealization;
                                            if (codeAssociationEndRealization != null)
                                            {
                                                object obj = (feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification.Specification;
                                                codeAssociationEndRealization.Synchronize(feature);
                                            }
                                            else
                                            {
                                                object obj = (feature as OOAdvantech.MetaDataRepository.AssociationEndRealization).Specification.Specification;
                                                OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Attribute;
                                                if (codeAttribute != null)
                                                    codeAttribute.Synchronize(feature);
                                                else
                                                {
                                                    codeAttribute = codeClassifier.AddAttribute(feature.Name, (feature as AssociationEndRealization).Specification.Specification, "");
                                                    codeAttribute.Synchronize(feature);
                                                }
                                            }

                                            break;
                                        }

                                    }
                                }

                            }
                            #endregion


                            if (roseRole != null)
                            {
                                foreach (AssociationEnd associationEnd in classifier.GetAssociateRoles(false))
                                {
                                    if (associationEnd.RoseRole.Equals(roseRole))
                                    {
                                        OOAdvantech.MetaDataRepository.AssociationEnd codeAssociationEnd = codeClassifier.GetRole(associationEnd.GetOtherEnd().Identity.ToString());
                                        if (codeAssociationEnd != null)
                                        {
                                            object obj = associationEnd.Association.LinkClass;
                                            if (associationEnd.Association.LinkClass != null)
                                                obj = associationEnd.Association.LinkClass.FullName;
                                            obj = associationEnd.Multiplicity;
                                            obj = associationEnd.Specification.FullName;
                                            obj = associationEnd.GetOtherEnd().Multiplicity;
                                            obj = associationEnd.GetOtherEnd().FullName;

                                            codeAssociationEnd.Synchronize(associationEnd.GetOtherEnd());
                                        }
                                        else
                                        {
                                            OOAdvantech.MetaDataRepository.Classifier otherEndCodeClassifier = project.GetExternalClassifier(associationEnd.Specification.FullName);
                                            if (otherEndCodeClassifier == null)
                                                return;

                                            object obj = associationEnd.Association.LinkClass;
                                            if (associationEnd.Association.LinkClass != null)
                                                obj = associationEnd.Association.LinkClass.FullName;
                                            obj = associationEnd.Multiplicity;
                                            obj = associationEnd.Specification.FullName;
                                            obj = associationEnd.GetOtherEnd().Multiplicity;
                                            obj = associationEnd.GetOtherEnd().FullName;

                                            if (associationEnd.IsRoleA)
                                                codeClassifier.AddAssociation(associationEnd.Name, OOAdvantech.MetaDataRepository.Roles.RoleA, otherEndCodeClassifier, associationEnd.Association.Identity.ToString()).RoleA.Synchronize(associationEnd);
                                            else
                                                codeClassifier.AddAssociation(associationEnd.Name, OOAdvantech.MetaDataRepository.Roles.RoleB, otherEndCodeClassifier, associationEnd.Association.Identity.ToString()).RoleB.Synchronize(associationEnd);


                                            //OOAdvantech.MetaDataRepository.AssociationEnd otherEndCodeAssociationEnd = null;
                                            //if (associationEnd.IsRoleA)
                                            //    otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(associationEnd.Name, otherEndCodeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleA, associationEnd.Association.Identity.ToString());
                                            //else
                                            //    otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(associationEnd.Name, otherEndCodeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleB, associationEnd.Association.Identity.ToString());
                                            //if (associationEnd.GetOtherEnd().IsRoleA)
                                            //    codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd(associationEnd.GetOtherEnd().Name, codeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleA);
                                            //else
                                            //    codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd(associationEnd.GetOtherEnd().Name, codeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleB);

                                            //OOAdvantech.CodeMetaDataRepository.Association association = null;
                                            //if (codeAssociationEnd.IsRoleA)
                                            //    association = new OOAdvantech.CodeMetaDataRepository.Association(associationEnd.Name, codeAssociationEnd, otherEndCodeAssociationEnd, associationEnd.Association.Identity.ToString(), null);
                                            //else
                                            //    association = new OOAdvantech.CodeMetaDataRepository.Association(associationEnd.Name, otherEndCodeAssociationEnd, codeAssociationEnd, associationEnd.Association.Identity.ToString(), null);
                                            //codeAssociationEnd.Synchronize(associationEnd.GetOtherEnd());



                                            //codeAttribute = new OOAdvantech.CodeMetaDataRepository.Attribute(feature.Name, codeClassifier);
                                            //codeAttribute.Synchronize(feature);
                                        }
                                        break;
                                    }

                                }
                            }


                            //codeClassifier.Synchronize(classifier);
                            // project.IDEManager.RefreshProject(project.Identity.ToString(), codeClassifier.Identity.ToString());

                        }
                        // stateTransition.Consistent = true;
                    }
                }

            }
            catch (System.Exception error)
            {
                if (classifier == null)
                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(error.Message);
                else
                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(classifier.FullName + ":  " + error.Message);


            }
            finally
            {
                if (IDEManager != null)
                {
                    IDEManager.EndCodeModelSynchronize();

                }
                MetaObjectMapper.Clear();
                //if (OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator != null)
                //{
                //    (OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator as OOAdvantech.CodeMetaDataRepository.MetaObjectsStack) .StopSynchronize();
                //    MetaObjectMapper.Clear();
                //}
            }

        }


        /// <MetaDataID>{cfbdd8bf-51b3-4512-a1da-536f5175dd5e}</MetaDataID>
        internal void RemoveUnassignedItems(RationalRose.RoseApplication roseApplication)
        {


            if (roseApplication.CurrentModel.GetSelectedItems().Count > 0)
            {
                OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
                try
                {
                    

                    if (!IsAdmin)
                    {
                        System.Windows.Forms.MessageBox.Show("You can't access Visual Studio \r\nRun Rose as administrator.", "Rose Visual Studio Synchronizer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }
                    if (!(roseApplication.CurrentModel.GetSelectedItems().GetAt(1) is RationalRose.RoseClass))
                        return;

                    RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedItems().GetAt(1) as RationalRose.RoseClass;

                    RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);
                    Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                    if (component == null)
                        component = new Component(roseComponent);
                    OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager = null;
                    EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                    IDEManager = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(dte);
                    IDEManager.StartCodeModelSynchronize();
                    MetaObjectMapper.Clear();
                    metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                    OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack(roseApplication);
                    metaObjectsStack.StartSynchronize();
                    OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                    try
                    {
                        CodeModelSychronizeView codeModelSychronizeView = new CodeModelSychronizeView();
                        OOAdvantech.CodeMetaDataRepository.Project project = null;
                        foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                        {
                            if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                                project = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(vsProject.DTE).GetProject(OOAdvantech.CodeMetaDataRepository.Project.GetProjectIdentity(vsProject));
                        }
                        if (project == null)
                            return;
                        OOAdvantech.MetaDataRepository.Classifier classifier = null;

                        if (roseClass.Stereotype == "Interface")
                        {

                            classifier = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                            if (classifier == null)
                                classifier = new Interface(roseClass, component);
                            string ful = classifier.FullName;
                            (classifier as Interface).LoadCompleteModel();

                        }
                        else if (roseClass.Stereotype.Trim().ToLower() == "Structure".ToLower())
                        {
                            classifier = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                            if (classifier == null)
                                classifier = new Structure(roseClass, component);
                            (classifier as Structure).LoadCompleteModel();
                        }
                        else
                        {
                            classifier = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                            if (classifier == null)
                                classifier = new Class(roseClass, component);
                            (classifier as Class).LoadCompleteModel();
                        }
                        OOAdvantech.MetaDataRepository.Classifier codeClassifier = project.GetClassifier(classifier.Identity.ToString());



                        codeModelSychronizeView.Connection.Instance = new CodeModelSychronizer(classifier, codeClassifier);
                        codeModelSychronizeView.ShowDialog();
                        


                    }
                    catch (System.Exception error)
                    {
                        if (OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog != null)
                        {
                            if (error.InnerException != null)
                                error = error.InnerException;

                            OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError("Error :  " + error.Message);

                        }
                    }
                    finally
                    {
                        if (IDEManager != null)
                        {
                            IDEManager.EndCodeModelSynchronize();

                        }
                        if (metaObjectsStack != null)
                        {
                            metaObjectsStack.StopSynchronize();
                            MetaObjectMapper.Clear();
                            if (MetaObjectsStack.CurrentMetaObjectCreator is MetaObjectsStack)
                            {
                                (MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).Clear();
                                MetaObjectsStack.CurrentMetaObjectCreator = null;
                            }

                        }

                        MetaObjectMapper.Clear();
                    }

                }
                catch (System.Exception error)
                {

                }
            }

        }


        /// <MetaDataID>{8bfdf04e-22cb-4753-b639-5289fedfbae0}</MetaDataID>
        internal void UpdateCode(RationalRose.RoseApplication roseApplication)
        {
            if (!IsAdmin)
            {
                System.Windows.Forms.MessageBox.Show("You can't access Visual Studio \r\nRun Rose as administrator.", "Rose Visual Studio Synchronizer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            //OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager = null;
            try
            {

                if (roseApplication.CurrentModel.GetSelectedClasses().Count > 0)
                {
                    //MetaObjectMapper.Clear();
                    //metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                    //OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = metaObjectsStack;
                    //metaObjectsStack.StartSynchronize();
                    //OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();

                    //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {

                        for (int i = 0; i < roseApplication.CurrentModel.GetSelectedClasses().Count; i++)
                        {
                            RationalRose.RoseClass roseClass = roseApplication.CurrentModel.GetSelectedClasses().GetAt((short)(i + 1));
                            OOAdvantech.MetaDataRepository.Classifier classifier = null;
                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            if (roseClass.GetAssignedModules().Count == 0)
                                continue;
                            RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);

                            Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseComponent);
                            EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                            IDEManager = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(dte);
                            IDEManager.StartCodeModelSynchronize();

                            OOAdvantech.CodeMetaDataRepository.Project project = null;
                            foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                            {
                                if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                                    project = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(vsProject.DTE).GetProject(OOAdvantech.CodeMetaDataRepository.Project.GetProjectIdentity(vsProject));
                            }
                            if (project == null)
                                continue;

                            if (roseClass.Stereotype == "Interface")
                            {

                                classifier = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                                if(classifier ==null)
                                    classifier = new Interface(roseClass, component);
                                string ful = classifier.FullName;
                                (classifier as Interface).LoadCompleteModel();
                                
                            }
                            else if (roseClass.Stereotype.Trim().ToLower() == "Structure".ToLower())
                            {
                                classifier = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                                if (classifier == null)
                                    classifier = new Structure(roseClass, component);
                                (classifier as Structure).LoadCompleteModel();
                            }
                            else
                            {
                                classifier = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as OOAdvantech.MetaDataRepository.Classifier;
                                if (classifier == null)
                                    classifier = new Class(roseClass, component);
                                (classifier as Class).LoadCompleteModel();
                            }
                            OOAdvantech.MetaDataRepository.Classifier codeClassifier = project.GetClassifier(classifier.Identity.ToString());

                            if (codeClassifier == null)
                            {
                                string namespaceName = null;
                                if (classifier.Namespace != null)
                                    namespaceName = classifier.Namespace.FullName;
                                if (classifier is Class)
                                    codeClassifier = project.CreateClass(classifier.Name, namespaceName);
                                if (classifier is Interface)
                                    codeClassifier = project.CreateInterface(classifier.Name, namespaceName);
                                if (classifier is Structure)
                                    codeClassifier = project.CreateStructure(classifier.Name, namespaceName);
                            }

                            if (classifier.LinkAssociation != null)
                            {
                                OOAdvantech.MetaDataRepository.AssociationEnd associationEnd = classifier.LinkAssociation.RoleA;
                                associationEnd = classifier.LinkAssociation.RoleB;
                            }
                            codeClassifier.Synchronize(classifier);
                            //project.IDEManager.RefreshProject(project.Identity.ToString(), codeClassifier.Identity.ToString());

                        }
                        //stateTransition.Consistent = true;
                    }
                }

            }
            catch (System.Exception error)
            {
                if (OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog != null)
                {
                    if (error.InnerException != null)
                        error = error.InnerException;

                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError("Error :  " + error.Message);

                }
            }
            finally
            {
                if (IDEManager != null)
                {
                    IDEManager.EndCodeModelSynchronize();

                }
                MetaObjectMapper.Clear();

                //if (metaObjectsStack != null)
                //{
                //    metaObjectsStack.StopSynchronize();
                //    MetaObjectMapper.Clear();
                //}
            }

        }
        /// <MetaDataID>{33f47aba-1e97-4e5e-8c2a-c7de7982bfbf}</MetaDataID>
        internal void Browse(RationalRose.RoseApplication roseApplication)
        { 

            for (int i = 0; i < roseApplication.AddInManager.AddIns.Count; i++)
            {
                RationalRose.IRoseAddIn addin = roseApplication.AddInManager.AddIns.GetAt((short)(i + 1));
                string rtr = addin.Name;
                string rtr2 = addin.GetDisplayName();
                object obj = addin.EventHandler;
                if (obj!=null)
                {
                    string ty= obj.GetType().FullName;
                }

            }
            if (!IsAdmin)
            {
                System.Windows.Forms.MessageBox.Show("You can't access Visual Studio \r\nRun run Rose as administrator.", "Rose Visual Studio Synchronizer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (roseApplication.CurrentModel.GetSelectedItems().Count > 0)
                {

                    MetaObjectMapper.Clear();
                    OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.Clear();
                    RationalRose.RoseClass roseClass = null;
                    RationalRose.RoseItem roseItem = roseApplication.CurrentModel.GetSelectedItems().GetAt(1);
                    if (roseItem is RationalRose.RoseMessage && (roseItem as RationalRose.RoseMessage).GetOperation() != null)
                    {
                        roseItem = (roseItem as RationalRose.RoseMessage).GetOperation() as RationalRose.RoseItem;
                    }
                    if (roseItem is RationalRose.RoseClass)
                        roseClass = roseItem as RationalRose.RoseClass;
                    else if (roseItem is RationalRose.RoseOperation)
                        roseClass = (roseItem as RationalRose.RoseOperation).ParentClass;
                    else if (roseItem is RationalRose.RoseAttribute)
                        roseClass = (roseItem as RationalRose.RoseAttribute).ParentClass;
                    else if (roseItem is RationalRose.RoseRole)
                    {
                        if ((roseItem as RationalRose.RoseRole).Equals((roseItem as RationalRose.RoseRole).Association.Role1))
                            roseClass = (roseItem as RationalRose.RoseRole).Association.Role2.Class;
                        else
                            roseClass = (roseItem as RationalRose.RoseRole).Association.Role1.Class;

                    }

                    if (roseClass == null)
                        return;

                    string roseClassName = roseClass.Name;

                    OOAdvantech.MetaDataRepository.Classifier classifier = null;
                    if (roseClass.GetAssignedModules().Count == 0)
                        return;
                    if (roseClass.GetAssignedModules().Count == 0)
                        return;
                    RationalRose.RoseModule roseComponent = roseClass.GetAssignedModules().GetAt(1);

                    Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                    if (component == null)
                        component = new Component(roseComponent);
                    EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                    OOAdvantech.CodeMetaDataRepository.Project project = null;
                    if (project == null)
                    {
                        foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                        {
                            if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                                project = new OOAdvantech.CodeMetaDataRepository.Project(vsProject);
                        }
                    }
                    if (project == null)
                        return;
                    if (roseClass.Stereotype == "Interface")
                        classifier = new Interface(roseClass, component);
                    else if (roseClass.Stereotype.Trim().ToLower() == "Structure".ToLower())
                        classifier = new Structure(roseClass, component);
                    else
                        classifier = new Class(roseClass, component);

                    string memberIdentity = null;
                    if (!(roseItem is RationalRose.RoseClass))
                        memberIdentity = roseItem.GetPropertyValue("MetaData", "MetaObjectID");

                    if (roseItem is RationalRose.RoseRole)
                        memberIdentity = (roseItem as RationalRose.RoseRole).Association.GetPropertyValue("C#", "Identity");


 


                    EnvDTE.ProjectItem projectItem = (project.GetClassifier(classifier.Identity.ToString()) as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement.ProjectItem;

                    if (projectItem.FileCount == 1)
                    {
                        string fileName = projectItem.get_FileNames(0);
                        projectItem.DTE.ItemOperations.OpenFile(fileName, "{7651A703-06E5-11D1-8EBD-00A0C90F26EA}");

                        (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).StartOfDocument(false);
                        if (!string.IsNullOrEmpty(memberIdentity))
                            (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).FindText(memberIdentity, 0);
                        else
                            (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).FindText(classifier.Identity.ToString(), 0);
                        (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).StartOfLine(EnvDTE.vsStartOfLineOptions.vsStartOfLineOptionsFirstText, false);
                        MsdevManager.Msdev.ShowIDE(projectItem.DTE);
                    }


                    //(project.GetClassifier(classifier.Identity.ToString()) as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement.ProjectItem.get_FileNames()
                    //project.IDEManager.Browse(project.Identity.ToString(), classifier.Identity.ToString(), memberIdentity);



                }
            }
            catch (System.Exception error)
            {
                if (OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog != null)
                {
                    if (error.InnerException != null)
                        error = error.InnerException;

                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError("Error :  " + error.Message);
                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError("StackTrace :  " + error.StackTrace);

                }
            }
            finally
            {
                MetaObjectMapper.Clear();
            }

        }
        /// <MetaDataID>{7de09f98-e5db-48ef-ad7e-61f3e81811e5}</MetaDataID>
        internal void UpdateModuleCode(RationalRose.RoseApplication roseApplication)
        {
            return;
            try
            {

                if (roseApplication.CurrentModel.GetSelectedModules().Count > 0)
                {
                    MetaObjectMapper.Clear();
                    RationalRose.RoseModule roseComponent = roseApplication.CurrentModel.GetSelectedModules().GetAt(1);
                    Component component = new Component(roseComponent);
                    System.Type type = ModulePublisher.ClassRepository.GetType("OOAdvantech.CodeMetaDataRepository.IDEManager", "");
                    EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                    OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
                    try
                    {

                        foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                        {
                            if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                            {

                                OOAdvantech.CodeMetaDataRepository.Project project = new OOAdvantech.CodeMetaDataRepository.Project(vsProject);

                                metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                                OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = metaObjectsStack;
                                metaObjectsStack.StartSynchronize();
                                OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                                component.SetIdentity(project.Identity.ToString());
                                project.Synchronize(component);
                            }
                        }
                    }
                    catch (System.Exception error)
                    {
                    }
                    finally
                    {
                        if (metaObjectsStack != null)
                        {
                            metaObjectsStack.StopSynchronize();
                            MetaObjectMapper.Clear();
                        }
                    }


                    component.RoseComponent = null;
                }



            }
            catch (System.Exception error)
            {

            }
        }


        /// <MetaDataID>{45cfeaa6-9d46-474f-806a-3708294f2677}</MetaDataID>
        internal static OOAdvantech.MetaDataRepository.Component GetComponentFor(EnvDTE.Project vsProject)
        {
            return new OOAdvantech.CodeMetaDataRepository.Project(vsProject);

        }

        /// <MetaDataID>{bebfb77c-e8be-4bc0-821d-674461b9279c}</MetaDataID>
        internal static OOAdvantech.MetaDataRepository.MetaObjectsStack InitMetaObjectsStack()
        {
            OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
            OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = metaObjectsStack;
            metaObjectsStack.StartSynchronize();
            return metaObjectsStack;

        }
        /// <MetaDataID>{99be694f-e3b8-4434-91f6-e37d01e69755}</MetaDataID>
        internal static void ClearMetaObjectsStack()
        {
            OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.Clear();

        }



        /// <MetaDataID>{3b29bfca-3731-454d-9d0f-6d222accfd44}</MetaDataID>
        public static bool IsAdmin
        {
            get
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }  


        /// <MetaDataID>{e62d22ee-dd2a-41e8-b1bf-f931f4436518}</MetaDataID>
        internal static void UpdateRoseModule(RationalRose.RoseApplication roseApplication)
        {
            if (!IsAdmin)
            {
                System.Windows.Forms.MessageBox.Show("You can't access Visual Studio \r\nRun Rose as administrator.", "Rose Visual Studio Synchronizer", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning); 
                return;
            }
            OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            try
            {

                System.DateTime startTime = DateTime.Now;

                if (roseApplication.CurrentModel.GetSelectedModules().Count > 0)
                {
                    //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                    {
                        MetaObjectMapper.Clear();
                        metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                        OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = new MetaObjectsStack(roseApplication);
                        metaObjectsStack.StartSynchronize();
                        OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                        for (int i = 0; i < roseApplication.CurrentModel.GetSelectedModules().Count; i++)
                        {
                            RationalRose.RoseModule roseComponent = roseApplication.CurrentModel.GetSelectedModules().GetAt((short)(i + 1));
                            Component component = MetaObjectMapper.FindMetaObjectFor(roseComponent.GetUniqueID()) as Component;
                            if (component == null)
                                component = new Component(roseComponent);

                            EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                            OOAdvantech.CodeMetaDataRepository.Project project = null;
                            foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                            {
                                if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                                    project = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(vsProject.DTE).GetProject(OOAdvantech.CodeMetaDataRepository.Project.GetProjectIdentity(vsProject));
                            }
                            if (project == null)
                                continue;
                            string identity = project.Identity.ToString();
                            component.SetIdentity(project.Identity.ToString());
                            MetaObjectMapper.AddTypeMap(roseComponent, component);
                            component.Synchronize(project);
                        }
                        //stateTransition.Consistent = true;
                    }
                }
                System.TimeSpan timeSpan = System.DateTime.Now - startTime;
                System.Diagnostics.Debug.WriteLine("GetExternalClassifier  " + timeSpan.TotalMilliseconds.ToString());


            }
            catch (System.Exception error)
            {

            }
            finally
            {
                if (metaObjectsStack != null)
                {
                    metaObjectsStack.StopSynchronize();
                    MetaObjectMapper.Clear();
                    if (MetaObjectsStack.CurrentMetaObjectCreator is MetaObjectsStack)
                    {
                        (MetaObjectsStack.CurrentMetaObjectCreator as MetaObjectsStack).Clear();
                        MetaObjectsStack.CurrentMetaObjectCreator = null;
                    }

                }

            }

        }
    }
}
