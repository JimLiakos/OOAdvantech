using System;
using System.Collections.Generic;
using System.Text;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{48c42513-92be-4616-9d1b-00a11317232c}</MetaDataID>
    internal enum ActionType
    {
        Browse,
        UpdateCode,
        UpdateModel
    }
    /// <MetaDataID>{68fb8eab-6647-4825-82b9-f042fbc6acc2}</MetaDataID>
    [System.Runtime.InteropServices.ComVisible(false)]
    internal class ClassifierPresentation : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Classifier>
    {
        ActionType ActionType;
        public ClassifierPresentation(OOAdvantech.MetaDataRepository.Classifier classifier, ActionType actionType)
            : base(null)
        {
            typeof(OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.MetaDataRepository.Classifier>).GetField("_RealObject", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, classifier);
            ActionType = actionType;
        }

        public ClassifierMember SelectedMember
        {
            get
            {
                return null;
            }
            set
            {

                if (ActionType == ActionType.Browse)
                    Browse(value);
                if (ActionType == ActionType.UpdateCode)
                    UpdateCode(value);
                if (ActionType == ActionType.UpdateModel)
                    UpdateModel(value);


            }
        }

        private void UpdateCode(ClassifierMember classifierMember)
        {
            //OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            OOAdvantech.CodeMetaDataRepository.IDEManager IDEManager = null;
            OOAdvantech.MetaDataRepository.Classifier classifier = null;
            try
            {
                
                

                //MetaObjectMapper.Clear();
                //metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                //OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = metaObjectsStack;
                //metaObjectsStack.StartSynchronize();
                //OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                {

                    RationalRose.RoseOperation roseOperetion = classifierMember.RoseItem as RationalRose.RoseOperation;
                    RationalRose.RoseAttribute roseAttribute = classifierMember.RoseItem as RationalRose.RoseAttribute;
                    RationalRose.RoseRole roseRole = classifierMember.RoseItem as RationalRose.RoseRole;


                    classifier = RealObject;
                    if (classifier.ImplementationUnit == null)
                        return;
                    Component component = classifier.ImplementationUnit as Component;


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
                        return;




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
                                if (feature is Operation && (feature as Operation).RoseOperation.Equals(roseOperetion))
                                {
                                    OOAdvantech.MetaDataRepository.Operation codeOperation = codeClassifier.GetFeature(feature.Identity.ToString(),false) as OOAdvantech.MetaDataRepository.Operation;
                                    if (codeOperation != null)
                                        codeOperation.Synchronize(feature);
                                    else
                                    {
                                        codeOperation = new OOAdvantech.CodeMetaDataRepository.Operation(feature.Name, codeClassifier);
                                        codeOperation.Synchronize(feature);
                                    }
                                    break;
                                }
                                if (feature is Method && (feature as Method).RoseOperation.Equals(roseOperetion))
                                {
                                    OOAdvantech.MetaDataRepository.Operation codeOperation = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Operation;
                                    if (codeOperation != null)
                                        codeOperation.Synchronize(feature);
                                    else
                                    {
                                        codeOperation = new OOAdvantech.CodeMetaDataRepository.Operation(feature.Name, codeClassifier);
                                        codeOperation.Synchronize(feature);
                                    }
                                    break;
                                }

                            }


                            if (roseAttribute != null)
                            {
                                if (feature is Attribute && (feature as Attribute).RoseAttribute.Equals(roseAttribute))
                                {
                                    OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Attribute;
                                    if (codeAttribute != null)
                                        codeAttribute.Synchronize(feature);
                                    else
                                    {
                                        codeAttribute = new OOAdvantech.CodeMetaDataRepository.Attribute(feature.Name, codeClassifier);
                                        codeAttribute.Synchronize(feature);
                                    }
                                    break;
                                }
                                if (feature is AttributeRealization && (feature as AttributeRealization).RoseAttribute.Equals(roseAttribute))
                                {
                                    OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Attribute;
                                    if (codeAttribute != null)
                                        codeAttribute.Synchronize(feature);
                                    else
                                    {
                                        codeAttribute = new OOAdvantech.CodeMetaDataRepository.Attribute(feature.Name, codeClassifier);
                                        codeAttribute.Synchronize(feature);
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
                                    codeAssociationEnd.Synchronize(associationEnd.GetOtherEnd());
                                else
                                {
                                    OOAdvantech.MetaDataRepository.Classifier otherEndCodeClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(associationEnd.Specification, null) as OOAdvantech.MetaDataRepository.Classifier;
                                    if (otherEndCodeClassifier == null)
                                        return;

                                    OOAdvantech.MetaDataRepository.AssociationEnd otherEndCodeAssociationEnd = null;
                                    if (associationEnd.IsRoleA)
                                        otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(associationEnd.Name, otherEndCodeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleA,associationEnd.Association.Identity.ToString());
                                    else
                                        otherEndCodeAssociationEnd = new OOAdvantech.CodeMetaDataRepository.AssociationEnd(associationEnd.Name, otherEndCodeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleB, associationEnd.Association.Identity.ToString());
                                    if (associationEnd.GetOtherEnd().IsRoleA)
                                        codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd(associationEnd.GetOtherEnd().Name, codeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleA);
                                    else
                                        codeAssociationEnd = new OOAdvantech.MetaDataRepository.AssociationEnd(associationEnd.GetOtherEnd().Name, codeClassifier, OOAdvantech.MetaDataRepository.Roles.RoleB);

                                    OOAdvantech.CodeMetaDataRepository.Association association = null;
                                    if (codeAssociationEnd.IsRoleA)
                                        association = new OOAdvantech.CodeMetaDataRepository.Association(associationEnd.Name, codeAssociationEnd, otherEndCodeAssociationEnd, associationEnd.Association.Identity.ToString(),null);
                                    else
                                        association = new OOAdvantech.CodeMetaDataRepository.Association(associationEnd.Name, otherEndCodeAssociationEnd, codeAssociationEnd, associationEnd.Association.Identity.ToString(),null);
                                    codeAssociationEnd.Synchronize(associationEnd.GetOtherEnd());



                                    //codeAttribute = new OOAdvantech.CodeMetaDataRepository.Attribute(feature.Name, codeClassifier);
                                    //codeAttribute.Synchronize(feature);
                                }
                                break;
                            }

                        }
                    }


                    ////codeClassifier.Synchronize(classifier);
                    //project.IDEManager.RefreshProject(project.Identity.ToString(), codeClassifier.Identity.ToString());


                    //stateTransition.Consistent = true;
                }
            }
            finally
            {
                if (IDEManager != null)
                    IDEManager.EndCodeModelSynchronize();
                MetaObjectMapper.Clear();
            }
        }

        private void UpdateModel(ClassifierMember classifierMember)
        {
            OOAdvantech.CodeMetaDataRepository.MetaObjectsStack metaObjectsStack = null;
            try
            {
                MetaObjectMapper.Clear();
                metaObjectsStack = new OOAdvantech.CodeMetaDataRepository.MetaObjectsStack(true);
                OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator = metaObjectsStack;
                metaObjectsStack.StartSynchronize();
                OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
                //using (OOAdvantech.Transactions.SystemStateTransition stateTransition = new OOAdvantech.Transactions.SystemStateTransition())
                {

                    RationalRose.RoseOperation roseOperetion = classifierMember.RoseItem as RationalRose.RoseOperation;
                    RationalRose.RoseAttribute roseAttribute = classifierMember.RoseItem as RationalRose.RoseAttribute;
                    RationalRose.RoseRole roseRole = classifierMember.RoseItem as RationalRose.RoseRole;


                    OOAdvantech.MetaDataRepository.Classifier classifier = RealObject;
                    if (classifier.ImplementationUnit == null)
                        return;
                    Component component = classifier.ImplementationUnit as Component;

                    EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);

                    OOAdvantech.CodeMetaDataRepository.Project project = null;
                    foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                    {
                        if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName.Trim().ToLower() == component.Project.Trim().ToLower())
                            project = OOAdvantech.CodeMetaDataRepository.IDEManager.GetIDEManager(vsProject.DTE).GetProject(OOAdvantech.CodeMetaDataRepository.Project.GetProjectIdentity(vsProject));
                    }
                    if (project == null)
                        return;



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
                                if (feature is Operation && (feature as Operation).RoseOperation.Equals(roseOperetion))
                                {
                                    OOAdvantech.MetaDataRepository.Operation codeOperation = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Operation;
                                    if (codeOperation != null)
                                        feature.Synchronize(codeOperation);//.Synchronize(feature);
                                    break;
                                }
                            }


                            if (roseAttribute != null)
                            {
                                if (feature is Attribute && (feature as Attribute).RoseAttribute.Equals(roseAttribute))
                                {
                                    OOAdvantech.MetaDataRepository.Attribute codeAttribute = codeClassifier.GetFeature(feature.Identity.ToString(), false) as OOAdvantech.MetaDataRepository.Attribute;
                                    if (codeAttribute != null)
                                        feature.Synchronize(codeAttribute);
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
                                break;
                            }

                        }
                    }

                    //codeClassifier.Synchronize(classifier);
                    //project.IDEManager.RefreshProject(project.Identity.ToString(), codeClassifier.Identity.ToString());


                    //stateTransition.Consistent = true;
                }
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

        private void Browse(ClassifierMember value)
        {
            try
            {

                MetaObjectMapper.Clear();
                OOAdvantech.CodeMetaDataRepository.MetaObjectMapper.Clear();

                Component component = RealObject.ImplementationUnit as Component;
                if (component == null)
                    return;
                EnvDTE.DTE dte = MsdevManager.Msdev.GetIDEInstance(component.Solution);
                OOAdvantech.CodeMetaDataRepository.Project project = null;
                if (project == null)
                {
                    foreach (EnvDTE.Project vsProject in dte.Solution.Projects)
                    {
                        if (vsProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}" && vsProject.FileName == component.Project)
                            project = new OOAdvantech.CodeMetaDataRepository.Project(vsProject);
                    }
                }
                if (project == null)
                    return;
                string memberIdentity = null;
                if (value.RoseItem is RationalRose.RoseRole)
                    memberIdentity = (value.RoseItem as RationalRose.RoseRole).Association.GetPropertyValue("C#", "Identity");
                else
                    memberIdentity = value.RoseItem.GetPropertyValue("MetaData", "MetaObjectID");




                EnvDTE.ProjectItem projectItem = (project.GetClassifier(RealObject.Identity.ToString()) as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement.ProjectItem;

                if (projectItem.FileCount == 1)
                {
                    string fileName = projectItem.get_FileNames(0);
                    projectItem.DTE.ItemOperations.OpenFile(fileName, "{7651A703-06E5-11D1-8EBD-00A0C90F26EA}");

                    (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).StartOfDocument(false);
                    if (!string.IsNullOrEmpty(memberIdentity))
                        (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).FindText(memberIdentity, 0);
                    else
                        (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).FindText(RealObject.Identity.ToString(), 0);
                    (projectItem.DTE.ActiveDocument.Selection as EnvDTE.TextSelection).StartOfLine(EnvDTE.vsStartOfLineOptions.vsStartOfLineOptionsFirstText, false);
                    MsdevManager.Msdev.ShowIDE(projectItem.DTE);
                }


                //(project.GetClassifier(classifier.Identity.ToString()) as OOAdvantech.CodeMetaDataRepository.CodeElementContainer).CodeElement.ProjectItem.get_FileNames()
                //project.IDEManager.Browse(project.Identity.ToString(), classifier.Identity.ToString(), memberIdentity);




            }
            catch (System.Exception error)
            {

            }
            finally
            {
                MetaObjectMapper.Clear();
            }
        }
        public OOAdvantech.Collections.Generic.Set<ClassifierMember> Members
        {
            get
            {
                OOAdvantech.Collections.Generic.Set<ClassifierMember> members = new OOAdvantech.Collections.Generic.Set<ClassifierMember>();
                RationalRose.RoseClass roseClass = null;
                if (RealObject is Class)
                    roseClass = (RealObject as Class).RoseClass;
                if (RealObject is Interface)
                    roseClass = (RealObject as Interface).RoseClass;




                for (int i = 0; i < roseClass.Attributes.Count; i++)
                    members.Add(new ClassifierMember(roseClass.Attributes.GetAt((short)(i + 1)) as RationalRose.RoseItem));
                for (int i = 0; i < roseClass.Operations.Count; i++)
                    members.Add(new ClassifierMember(roseClass.Operations.GetAt((short)(i + 1)) as RationalRose.RoseItem));
                for (int i = 0; i < roseClass.GetAssociateRoles().Count; i++)
                {
                    if (roseClass.GetAssociateRoles().GetAt((short)(i + 1)).Navigable)
                        members.Add(new ClassifierMember(roseClass.GetAssociateRoles().GetAt((short)(i + 1)) as RationalRose.RoseItem));
                }

                return members;
            }
        }
    }
}
