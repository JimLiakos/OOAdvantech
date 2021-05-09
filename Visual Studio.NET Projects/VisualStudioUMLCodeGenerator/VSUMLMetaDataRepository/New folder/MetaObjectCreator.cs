using System.Linq;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    using MetaDataRepository = OOAdvantech.MetaDataRepository;
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
            //base.InitializeMetaObject(OriginMetaObject, NewMetaObject);
        }
        /// <MetaDataID>{16129b92-e283-4c46-8d1f-fa12003e546f}</MetaDataID>
        public Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel VSUmlModel;
        public MetaObjectsStack(Microsoft.VisualStudio.Uml.AuxiliaryConstructs.IModel vsUmlModel)
        {
            VSUmlModel = vsUmlModel;
            //for (int i = 0; i < roseApplication.CurrentModel.RootCategory.Categories.Count; i++)
            //    new Namespace(roseApplication.CurrentModel.RootCategory.Categories.GetAt((short)(i + 1)));

        }
        //System.Collections.Generic.Dictionary<string, RationalRose.RoseClass> RoseClasses;
        /// <MetaDataID>{7D7AC63F-19DE-4646-A57A-9B93150E0F77}</MetaDataID>
        internal System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject> MetaObjects = new System.Collections.Generic.Dictionary<string, OOAdvantech.MetaDataRepository.MetaObject>(1000);
        /// <MetaDataID>{89D73733-F74E-4A07-A7EB-8FDBB010426F}</MetaDataID>
        bool MetObjectsLoaded = false;
        /// <MetaDataID>{E3681881-2415-4114-B827-8336B17A4081}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObject FindMetaObjectInPLace(OOAdvantech.MetaDataRepository.MetaObject originMetaObject, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
        {
            System.DateTime start = System.DateTime.Now;
            try
            {
                string tert = originMetaObject.Name;

                OOAdvantech.MetaDataRepository.MetaObject metaObject = FindMetaObjectInPLace(GetIdentity(originMetaObject).ToString(), placeIdentifier);
                if (metaObject == null)
                {
                    if (originMetaObject is OOAdvantech.MetaDataRepository.Class || originMetaObject is OOAdvantech.MetaDataRepository.Interface)
                    {

                        var vsClassifier = (from classifier in VisualStudioUMLHelper.GetTypes((placeIdentifier as IVSUMLModelItemWrapper).UMLModel)
                                            where classifier.Identity.ToString() == originMetaObject.GetPropertyValue<string>("MetaData", "MetaObjectID")
                                            select classifier).FirstOrDefault();


                        return vsClassifier;


                        //if (MetaObjects.ContainsKey(GetIdentity(originMetaObject).ToString()))
                        //    return MetaObjects[GetIdentity(originMetaObject).ToString()] as Class;
                        //if (!MetObjectsLoaded)
                        //{
                        //    RoseClasses = new System.Collections.Generic.Dictionary<string, RationalRose.RoseClass>();
                        //    RationalRose.RoseClassCollection classes = RoseApplication.CurrentModel.GetAllClasses();
                        //    for (int i = 0; i < classes.Count; i++)
                        //    {
                        //        int tmp = classes.Count;
                        //        RationalRose.RoseClass roseClass = classes.GetAt((short)(i + 1));
                        //        string identity = roseClass.GetPropertyValue("MetaData", "MetaObjectID");
                        //        if (string.IsNullOrEmpty(identity))
                        //            continue;
                        //        RoseClasses[identity] = roseClass;
                        //    }
                        //    MetObjectsLoaded = true;
                        //}


                        //if (!string.IsNullOrEmpty(GetIdentity(originMetaObject).ToString()) && RoseClasses.ContainsKey(GetIdentity(originMetaObject).ToString()))
                        //{
                        //    RationalRose.RoseClass roseClass = RoseClasses[GetIdentity(originMetaObject).ToString()];
                        //    OOAdvantech.MetaDataRepository.Classifier _class = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as Class;
                        //    if (_class == null)
                        //        if (roseClass.Stereotype == "Interface")
                        //            _class = new Interface(roseClass, null);
                        //        else
                        //            _class = new Class(roseClass, null);
                        //    if (!MetaObjects.ContainsKey(GetIdentity(_class).ToString()))
                        //        MetaObjects[GetIdentity(_class).ToString()] = _class;
                        //    return MetaObjects[GetIdentity(_class).ToString()];
                        //}
                    }

                    //    //if (RoseClasses == null)
                    //    {



                    //        string fullname = "Logical View::" + originMetaObject.FullName.Replace(".", "::");
                    //        RationalRose.RoseClassCollection findedClasses = RoseApplication.CurrentModel.FindClasses(fullname);
                    //        if (findedClasses.Count > 0)
                    //        {
                    //            RationalRose.RoseClass roseClass = findedClasses.GetAt(1);
                    //            OOAdvantech.MetaDataRepository.Classifier _class = MetaObjectMapper.FindMetaObjectFor(roseClass.GetUniqueID()) as Class;
                    //            if (_class == null)
                    //                if (roseClass.Stereotype == "Interface")
                    //                    _class = new Interface(roseClass, null);
                    //                else
                    //                    _class = new Class(roseClass, null);
                    //            if (!MetaObjects.ContainsKey(GetIdentity(_class).ToString()))
                    //                MetaObjects[GetIdentity(_class).ToString()] = _class;
                    //            return MetaObjects[GetIdentity(_class).ToString()];
                    //        }
                    //    }

                }

                //if (originMetaObject is OOAdvantech.MetaDataRepository.Namespace && !(originMetaObject is OOAdvantech.MetaDataRepository.Classifier))
                //{
                //    RationalRose.RoseCategoryCollection categories = RoseApplication.CurrentModel.GetAllCategories();
                //    for (int i = 0; i < categories.Count; i++)
                //    {
                //        RationalRose.RoseCategory category = categories.GetAt((short)(i + 1));
                //        string tmp = category.GetUniqueID();
                //        Namespace currNamesace = MetaObjectMapper.FindMetaObjectFor(category.GetUniqueID()) as Namespace;
                //        if (currNamesace == null)
                //            continue;
                //        string identity = GetIdentity(currNamesace).ToString();
                //        if (identity == GetIdentity(originMetaObject).ToString())
                //        {
                //            return currNamesace;
                //            Namespace _namespace = new Namespace(category);
                //            MetaObjects[GetIdentity(_namespace).ToString()] = _namespace;
                //            return _namespace;
                //        }

                //    }
                //}

                return metaObject;
            }
            finally
            {
                System.TimeSpan timeSpan = System.DateTime.Now - start;
                System.Diagnostics.Debug.WriteLine("Find  " + timeSpan.TotalMilliseconds.ToString());


            }

            return null;

        }

        /// <MetaDataID>{3B910160-2CD4-4A9A-B0D8-57A77BF881BD}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObject FindMetaObjectInPLace(string MetaObjectID, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
        {
            if (MetaObjectID == null)
                return null;
            return MetaObjectMapper.FindMetaObject(new OOAdvantech.MetaDataRepository.MetaObjectID(MetaObjectID));
            OOAdvantech.MetaDataRepository.MetaObject FindedMetaObject = null;
            if (!MetObjectsLoaded)
            {

                //PersistenceLayer.StructureSet aStructureSet=PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties).Execute("SELECT MetaObject FROM "+typeof(OOAdvantech.MetaDataRepository.MetaObject).FullName+" MetaObject ");//WHERE MetaObjectIDStream = \""+OriginMetaObject.Identity.ToString()+"\" ");
                //foreach( PersistenceLayer.StructureSet Rowset  in aStructureSet)
                //{
                //    OOAdvantech.MetaDataRepository.MetaObject metaObject=(OOAdvantech.MetaDataRepository.MetaObject)Rowset.Members["MetaObject"].Value; 
                //    if(metaObject.Identity.ToString()==null)
                //    {
                //        int were=0;
                //    }
                //    MetaObjects[metaObject.Identity.ToString()]=metaObject;
                //}
                //TODO:Είναι λάθος γιατί φορτώνουμε τα object αυτής της storage και θεωρού ότι εχουμε φορτώσει και για τις άλλε που τυχών θα έρθουν.
                MetObjectsLoaded = true;
            }


            if (!MetaObjects.ContainsKey(MetaObjectID))
                return null;
            return MetaObjects[MetaObjectID];
        }


        /// <MetaDataID>{6837AFE8-6EFB-49DD-A9DE-0947D97BD1FD}</MetaDataID>
        private System.Collections.Hashtable NewCreatedObjects;
        /// <MetaDataID>{592A0A55-A1AC-4618-B644-78763A19BD2A}</MetaDataID>
        public override OOAdvantech.MetaDataRepository.MetaObject CreateMetaObjectInPlace(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
        {
            //PersistenceLayer.ObjectStorage InStorage=PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier.Properties);
            //try
            //{
            if (NewCreatedObjects == null)
                NewCreatedObjects = new System.Collections.Hashtable();
            if (NewCreatedObjects.Contains(GetIdentity(OriginMetaObject).ToString()))
                return (OOAdvantech.MetaDataRepository.MetaObject)NewCreatedObjects[GetIdentity(OriginMetaObject).ToString()];
            OOAdvantech.MetaDataRepository.MetaObject NewMetaObject = null;

            //    if (typeof(OOAdvantech.MetaDataRepository.Class).IsInstanceOfType(OriginMetaObject))
            //    {

            //        NewMetaObject = new RoseMetaDataRepository.Class(RoseApplication.CurrentModel);
            //        goto End;
            //    }



            //    if (typeof(OOAdvantech.MetaDataRepository.Interface).IsInstanceOfType(OriginMetaObject))
            //    {

            //        NewMetaObject = new RoseMetaDataRepository.Interface(RoseApplication.CurrentModel);
            //        goto End;
            //    }


            //    if (OriginMetaObject is MetaDataRepository.Structure)
            //    {

            //        NewMetaObject = new RoseMetaDataRepository.Structure(RoseApplication.CurrentModel);
            //        goto End;
            //    }

            if (OriginMetaObject is OOAdvantech.MetaDataRepository.Generalization)
            {
                NewMetaObject = new Generalization((placeIdentifier as IVSUMLModelItemWrapper).UMLModel);
                goto End;
            }
            if (OriginMetaObject is OOAdvantech.MetaDataRepository.Realization)
            {
                NewMetaObject = new Realization((placeIdentifier as IVSUMLModelItemWrapper).UMLModel);
                goto End;
            }

            //    if (typeof(OOAdvantech.MetaDataRepository.Realization).IsInstanceOfType(OriginMetaObject))
            //    {
            //        NewMetaObject = new RoseMetaDataRepository.Realization();
            //        goto End;
            //    }

            //    if (OriginMetaObject is OOAdvantech.MetaDataRepository.Component)
            //    {
            //        NewMetaObject = new RoseMetaDataRepository.Component();
            //        goto End;
            //    }
            //    //if (OriginMetaObject is OOAdvantech.MetaDataRepository.Parameter)
            //    //{
            //    //    return new 
            //    //}
            //    if (OriginMetaObject is OOAdvantech.MetaDataRepository.Classifier)
            //        return UnknownClassifier.GetClassifier(OriginMetaObject.FullName);


            if (OriginMetaObject is OOAdvantech.MetaDataRepository.Operation)
            {
                NewMetaObject = new Operation(null);
                goto End;
            }
            if (OriginMetaObject is OOAdvantech.MetaDataRepository.Attribute)
            {
                NewMetaObject = new Attribute(null);
                goto End;
            }
        //    if (OriginMetaObject is OOAdvantech.MetaDataRepository.AttributeRealization)
        //    {
        //        NewMetaObject = new RoseMetaDataRepository.AttributeRealization();
        //        goto End;
        //    }
        //    if (OriginMetaObject is OOAdvantech.MetaDataRepository.Method)
        //    {
        //        NewMetaObject = new RoseMetaDataRepository.Method();
        //        goto End;
        //    }
        //    if (OriginMetaObject is OOAdvantech.MetaDataRepository.Parameter)
        //    {
        //        NewMetaObject = new OOAdvantech.MetaDataRepository.Parameter();
        //        goto End;
        //    }

            //    if (OriginMetaObject is OOAdvantech.MetaDataRepository.AssociationEndRealization)
        //    {
        //        NewMetaObject = new RoseMetaDataRepository.AssociationEndRealization();
        //        goto End;
        //    }
        //    if (OriginMetaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
        //    {
        //        OOAdvantech.MetaDataRepository.AssociationEnd originAssociationEnd = OriginMetaObject as OOAdvantech.MetaDataRepository.AssociationEnd;

            //        RationalRose.RoseClass roseClientClass = null, roseSupplierClass = null;
        //        MetaDataRepository.Classifier clientClassifier = null, supplierClassifier = null;
        //        bool _IsRoleA = originAssociationEnd.IsRoleA;
        //        if (_IsRoleA)
        //        {
        //            supplierClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, placeIdentifier) as MetaDataRepository.Classifier;
        //            clientClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.GetOtherEnd().Specification, placeIdentifier) as MetaDataRepository.Classifier;
        //        }
        //        else
        //        {
        //            clientClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.Specification, placeIdentifier) as MetaDataRepository.Classifier;
        //            supplierClassifier = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(originAssociationEnd.GetOtherEnd().Specification, placeIdentifier) as MetaDataRepository.Classifier;
        //        }
        //        if (clientClassifier == null || supplierClassifier == null)
        //            return null;
        //        if (clientClassifier is Interface)
        //            roseClientClass = (clientClassifier as Interface).RoseClass;
        //        else if (clientClassifier is Structure)
        //            roseClientClass = (clientClassifier as Structure).RoseClass;
        //        else
        //            roseClientClass = (clientClassifier as Class).RoseClass;

            //        if (supplierClassifier is Interface)
        //            roseSupplierClass = (supplierClassifier as Interface).RoseClass;
        //        else
        //            roseSupplierClass = (supplierClassifier as Class).RoseClass;

            //        if (roseClientClass == null || roseSupplierClass == null)
        //            return null;
        //        NewMetaObject = new AssociationEnd();
        //        goto End;
        //    }






            //    if (typeof(OOAdvantech.MetaDataRepository.Namespace).IsInstanceOfType(OriginMetaObject))
        //    {
        //        //RationalRose.RoseCategory category = RoseApplication.CurrentModel.RootCategory.AddCategory(OriginMetaObject.Name);
        //        NewMetaObject = new RoseMetaDataRepository.Namespace(RoseApplication.CurrentModel);
        //        goto End;
        //    }

                End:
            if (NewMetaObject != null)
            {
                InitializeMetaObject(OriginMetaObject, NewMetaObject);
                MetaObjectMapper.AddMetaObject(NewMetaObject, OriginMetaObject.FullName);
            }
            NewCreatedObjects[GetIdentity(NewMetaObject).ToString()] = NewMetaObject;
            if (MetaObjects != null)
                MetaObjects[GetIdentity(NewMetaObject).ToString()] = NewMetaObject;

            return NewMetaObject;
            //}
            //catch (System.Exception error)
            //{
            //    throw;
            //}

            return null;
        }
    }
}
