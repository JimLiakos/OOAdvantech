namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{2B55942A-75E9-49F0-8FB0-0F3C09B12141}</MetaDataID>
    public class MetaObjectsStack : OOAdvantech.MetaDataRepository.MetaObjectsStack
    {
        public override OOAdvantech.MetaDataRepository.ContainedItemsSynchronizer BuildItemsSychronizer(System.Collections.IList theSource, System.Collections.IList theUpdated, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
        {
            return new ContainedItemsSynchronizer(theSource, theUpdated, placeIdentifier);
        }
        public override OOAdvantech.MetaDataRepository.MetaObjectID GetIdentity(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            string identity = metaObject.GetPropertyValue(typeof(string), "MetaData", "MetaObjectID") as string;
            if (identity == null)
                return metaObject.Identity;
            return new OOAdvantech.MetaDataRepository.MetaObjectID(identity);

            
        }
        public override void InitializeMetaObject(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject, OOAdvantech.MetaDataRepository.MetaObject NewMetaObject)
        {
            NewMetaObject.PutPropertyValue("MetaData", "MetaObjectID", GetIdentity(OriginMetaObject).ToString());
            base.InitializeMetaObject(OriginMetaObject, NewMetaObject);
        }
        /// <MetaDataID>{8d1b6930-74d7-41ca-a938-2d254045c30b}</MetaDataID>
        internal readonly bool OutOfProcess;
        /// <MetaDataID>{2dde9dad-c258-49e5-96a7-1eb231640570}</MetaDataID>
        public static Project ActiveProject;
        /// <MetaDataID>{fabf5de3-5f37-4d75-9095-bbc2fbd11724}</MetaDataID>
        public MetaObjectsStack(bool outOfProcess)
        {
            //DTE = dte;
            OutOfProcess = outOfProcess;
            ActiveProject = null;
            //ActiveProject = activeProject;

        }
        /// <MetaDataID>{7f3cadb2-d877-4b6e-83ec-ec82c1b76887}</MetaDataID>
        public void Clear()
        {
            if (OutOfProcess)
                CodeMetaDataRepository.MetaObjectMapper.Clear();


        }
        ///// <MetaDataID>{7D7AC63F-19DE-4646-A57A-9B93150E0F77}</MetaDataID>
        //System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject> MetaObjects = new System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>(1000);
        ///// <MetaDataID>{89D73733-F74E-4A07-A7EB-8FDBB010426F}</MetaDataID>
        //bool MetObjectsLoaded = false;
        /// <MetaDataID>{E3681881-2415-4114-B827-8336B17A4081}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObject FindMetaObjectInPLace(OOAdvantech.MetaDataRepository.MetaObject originMetaObject, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
        {

            OOAdvantech.MetaDataRepository.MetaObject metaObject = MetaObjectMapper.FindMetaObject(GetIdentity(originMetaObject));

            //    FindMetaObjectInPLace(originMetaObject.Identity.ToString(), placeIdentifier);
            if (metaObject == null)
            {
                if (!(originMetaObject is OOAdvantech.MetaDataRepository.Classifier))
                {
                    metaObject = MetaObjectMapper.FindMetaObjectFor(originMetaObject.FullName);
                    if (metaObject != null)
                        return metaObject;
                }

                if (originMetaObject is OOAdvantech.MetaDataRepository.Component)
                {

                    while (placeIdentifier is CodeMetaDataRepository.CodeElementContainer && (placeIdentifier as CodeMetaDataRepository.CodeElementContainer).CodeElement == null)
                        placeIdentifier = placeIdentifier.Namespace;

                    if (placeIdentifier is CodeMetaDataRepository.CodeElementContainer && (placeIdentifier as CodeMetaDataRepository.CodeElementContainer).CodeElement!=null)
                    {
                        return MetaObjectMapper.FindMetaObjectFor((placeIdentifier as CodeMetaDataRepository.CodeElementContainer).CodeElement.ProjectItem.ContainingProject);
                       
                    }
                    return null;
                }

                if (ActiveProject == null)
                    return null;
                if (originMetaObject is OOAdvantech.MetaDataRepository.Classifier|| originMetaObject is OOAdvantech.MetaDataRepository.Interface)
                {
                    return ActiveProject.GetExternalClassifier(originMetaObject.FullName);
                    return null;
                }
            

                if (originMetaObject is OOAdvantech.MetaDataRepository.Namespace)
                {
                  
                }
            }
            return metaObject;

        }
        /// <MetaDataID>{3B910160-2CD4-4A9A-B0D8-57A77BF881BD}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObject FindMetaObjectInPLace(string MetaObjectID, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
        {
            OOAdvantech.MetaDataRepository.MetaObject metaObject = MetaObjectMapper.FindMetaObject(new MetaDataRepository.MetaObjectID(MetaObjectID) );
            return metaObject;

           
        }

        ///// <MetaDataID>{6837AFE8-6EFB-49DD-A9DE-0947D97BD1FD}</MetaDataID>
        //private System.Collections.Hashtable NewCreatedObjects;
        /// <MetaDataID>{592A0A55-A1AC-4618-B644-78763A19BD2A}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObject CreateMetaObjectInPlace(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
        {
            if(OriginMetaObject is MetaDataRepository.Component)
                throw new System.Exception("Code Generator can't create project for component \""+OriginMetaObject.Name+"\"");
            

            try
            {
                OOAdvantech.MetaDataRepository.MetaObject NewMetaObject = null;

                if (typeof(OOAdvantech.MetaDataRepository.Class).IsInstanceOfType(OriginMetaObject))
                {
                    if (OriginMetaObject.ImplementationUnit == null)
                        return null;
                    Project project = MetaObjectMapper.FindMetaObject(GetIdentity(OriginMetaObject.ImplementationUnit)) as Project;
                    if (project == null)
                        return null;
                    //System.IO.FileInfo fileInfo=new System.IO.FileInfo( project.VSProject.FileName);

                    if (OriginMetaObject.Namespace != null)
                        NewMetaObject = project.CreateClass(OriginMetaObject.Name, OriginMetaObject.Namespace.FullName);
                    else
                        NewMetaObject = project.CreateClass(OriginMetaObject.Name, "");

                    goto End;
                }

                if (typeof(OOAdvantech.MetaDataRepository.Interface).IsInstanceOfType(OriginMetaObject))
                {
                    if (OriginMetaObject.ImplementationUnit == null)
                        return null;
                    Project project = MetaObjectMapper.FindMetaObject(GetIdentity(OriginMetaObject.ImplementationUnit)) as Project;
                    //System.IO.FileInfo fileInfo=new System.IO.FileInfo( project.VSProject.FileName);



                    if (OriginMetaObject.Namespace != null)
                        NewMetaObject = project.CreateInterface(OriginMetaObject.Name, OriginMetaObject.Namespace.FullName);
                    else
                        NewMetaObject = project.CreateInterface(OriginMetaObject.Name, "");

                    goto End;
                }

                if (typeof(OOAdvantech.MetaDataRepository.Generalization).IsInstanceOfType(OriginMetaObject))
                {
                    NewMetaObject = new Generalization();
                    goto End;
                }

                if (typeof(OOAdvantech.MetaDataRepository.Structure).IsInstanceOfType(OriginMetaObject))
                {
                    Project project = MetaObjectMapper.FindMetaObject(GetIdentity(OriginMetaObject.ImplementationUnit)) as Project;
                    if (OriginMetaObject.Namespace != null)
                        NewMetaObject = project.CreateStructure(OriginMetaObject.Name, OriginMetaObject.Namespace.FullName);
                    else
                        NewMetaObject = project.CreateStructure(OriginMetaObject.Name, "");
                    goto End;
                }

                if (typeof(OOAdvantech.MetaDataRepository.Realization).IsInstanceOfType(OriginMetaObject))
                {
                    NewMetaObject = new Realization();
                    goto End;
                }
                if (typeof(OOAdvantech.MetaDataRepository.Operation).IsInstanceOfType(OriginMetaObject))
                {
                    NewMetaObject = new Operation();
                    goto End;
                }
                if (typeof(OOAdvantech.MetaDataRepository.Method).IsInstanceOfType(OriginMetaObject))
                {
                    NewMetaObject = new Method();
                    goto End;
                }
                if (typeof(OOAdvantech.MetaDataRepository.Attribute).IsInstanceOfType(OriginMetaObject))
                {
                    NewMetaObject = new Attribute();
                    goto End;
                }

                if (typeof(OOAdvantech.MetaDataRepository.AttributeRealization).IsInstanceOfType(OriginMetaObject))
                {
                    NewMetaObject = new AttributeRealization();
                    goto End;
                }
                if (OriginMetaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                {
                    NewMetaObject = new AssociationEnd();
                    goto End;
                }
                if (OriginMetaObject is OOAdvantech.MetaDataRepository.Association)
                {
                    NewMetaObject = new Association();
                    goto End;
                }

                if (OriginMetaObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
                {
                    NewMetaObject = new AssociationEndRealization();
                    goto End;
                }
                if (typeof(OOAdvantech.MetaDataRepository.Namespace).IsInstanceOfType(OriginMetaObject))
                {

                    NewMetaObject = new Namespace(OriginMetaObject.Name);
                    goto End;
                }





            End:
                if (NewMetaObject != null)
                {
                    InitializeMetaObject(OriginMetaObject, NewMetaObject);
                    MetaObjectMapper.AddMetaObject(NewMetaObject, OriginMetaObject.FullName);
                }
                return NewMetaObject;
            }
            catch (System.Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{9e5fb436-ea73-4f67-8879-deccdb39b16f}</MetaDataID>
        public void StartSynchronize()
        {
            OOAdvantech.MetaDataRepository.SynchronizerSession.StartSynchronize();
            if(OutOfProcess )
                CodeMetaDataRepository.MetaObjectMapper.Clear();
                
        }

        /// <MetaDataID>{4d20c1c8-7075-49d0-8e72-dffd3a0392f6}</MetaDataID>
        public void StopSynchronize()
        {
            OOAdvantech.MetaDataRepository.SynchronizerSession.StopSynchronize();
            if (OutOfProcess)
                CodeMetaDataRepository.MetaObjectMapper.Clear();
            ActiveProject = null;
                
        }
    }
}
