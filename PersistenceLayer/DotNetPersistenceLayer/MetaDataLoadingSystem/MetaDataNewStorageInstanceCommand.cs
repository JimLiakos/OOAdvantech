using System.Xml.Linq;
using System.Reflection;
namespace OOAdvantech.MetaDataLoadingSystem.Commands
{
    /// <MetaDataID>{731B46DA-958C-4C4F-9990-87C361B2A574}</MetaDataID>
    public class MetaDataNewStorageInstanceCommand : PersistenceLayerRunTime.Commands.NewStorageInstanceCommand
    {
        public MetaDataNewStorageInstanceCommand(MetaDataStorageInstanceRef storageInstanceRef) : base(storageInstanceRef)
        {
        }
        /// <MetaDataID>{30E4B6B1-7A87-4991-A580-F42B2D88315E}</MetaDataID>
        public override void Execute()
        {
            //if(OnFlyStorageInstance.Class.ClassHierarchyLinkAssociation==null)
            CreateStorageInstance();
            //else
            //	CreateRelationStorageInstance();

        }

        void CreateStorageInstance()
        {

            ulong ObjID = ulong.Parse(((XElement)NextObjectIDNode).Attribute("ObjID").Value);
            ObjID++;
            NextObjectIDNode.SetAttributeValue("ObjID", ObjID.ToString());
            string FileName = NextObjectIDNode.Document.BaseUri;
            XElement newXmlElement = new XElement("Object");// ObjectCollectionNode.Document.CreateElement("Object");
            ObjectCollectionNode.Add(newXmlElement);
            ObjID--;
            newXmlElement.SetAttributeValue("oid", ObjID.ToString());

            OnFlyStorageInstance.PersistentObjectID = new ObjectID(ObjID);
            ((MetaDataStorageInstanceRef)OnFlyStorageInstance).TheStorageIstance = newXmlElement;
            ((MetaDataStorageInstanceRef)OnFlyStorageInstance).SaveObjectState();

            MetaDataStorageSession ObjectStorageSession =
                (MetaDataStorageSession)OnFlyStorageInstance.ObjectStorage;
            ObjectStorageSession.Dirty = true;

            ObjectStorageSession.NewNodeUnderTransaction(newXmlElement, OwnerTransactiont);

             
            /*System.Uri FileUri =new System.Uri(FileName);
			FileName=FileUri.LocalPath;
			NewXmlElement.OwnerDocument.Save(FileName);*/

        }


        void CreateRelationStorageInstance()
        {
            //Met AssociationClass=
            //TODO:Να ελεγχθεί αν δουλεύει μετά καινούρια δεδομένα της LinkClass
            MetaDataStorageInstanceRef AssociationObject = (MetaDataStorageInstanceRef)OnFlyStorageInstance;

            DotNetMetaDataRepository.Class AssociationClass = AssociationObject.Class;

            PersistenceLayerRunTime.StorageInstanceAgent AssociationRoleA = null;
            PersistenceLayerRunTime.StorageInstanceAgent AssociationRoleB = null;
            GetRolesObject(AssociationClass, OnFlyStorageInstance, ref AssociationRoleA, ref AssociationRoleB);


            MetaDataStorageSession objectStorageSession =
                (MetaDataStorageSession)AssociationObject.ObjectStorage;
            objectStorageSession.Dirty = true;
            XDocument XmlDocument = objectStorageSession.XMLDocument;
            string StorageName = XmlDocument.Root.Name.LocalName;
            XElement RoleAStorageInstance = objectStorageSession.GetXMLElement(AssociationRoleA.MemoryInstance.GetType(), (ObjectID)AssociationRoleA.PersistentObjectID);
            XElement RoleBStorageInstance = objectStorageSession.GetXMLElement(AssociationRoleB.MemoryInstance.GetType(), (ObjectID)AssociationRoleB.PersistentObjectID);
            XElement AssociationObjectStorageInstance = objectStorageSession.GetXMLElement(AssociationObject.MemoryInstance.GetType(), (ObjectID)AssociationObject.PersistentObjectID);
            RoleAStorageInstance.SetAttribute("ReferentialIntegrityCount", AssociationRoleA.RealStorageInstanceRef.ReferentialIntegrityCount.ToString());
            RoleBStorageInstance.SetAttribute("ReferentialIntegrityCount", AssociationRoleB.RealStorageInstanceRef.ReferentialIntegrityCount.ToString());
            XElement RoleACollection = null;
            XElement RoleBCollection = null;


            string roleAName = objectStorageSession.GetMappedTagName(AssociationClass.ClassHierarchyLinkAssociation.RoleA.Identity.ToString().ToLower());
            if (string.IsNullOrWhiteSpace(roleAName))
            {
                roleAName = AssociationClass.ClassHierarchyLinkAssociation.RoleA.Name;
                if (roleAName == null || roleAName.Trim().Length == 0)
                    roleAName = AssociationClass.ClassHierarchyLinkAssociation.Name + "RoleAName";
            }
            string roleBName = objectStorageSession.GetMappedTagName(AssociationClass.ClassHierarchyLinkAssociation.RoleB.Identity.ToString().ToLower());
            if (string.IsNullOrWhiteSpace(roleBName))
            {
                roleBName = AssociationClass.ClassHierarchyLinkAssociation.RoleB.Name;
                if (roleBName == null || roleBName.Trim().Length == 0)
                    roleBName = AssociationClass.ClassHierarchyLinkAssociation.Name + "RoleBName";
            }


            bool AlreadyExist = false;
            foreach (XElement CurrNode in RoleAStorageInstance.Elements())
            {
                string StrObjectID = AssociationObject.PersistentObjectID.ToString();
                XElement Element = CurrNode;
                if (Element.Name == roleBName)
                {
                    RoleBCollection = Element;
                    foreach (XElement inCurrNode in RoleBCollection.Elements())
                    {
                        XElement inElement = inCurrNode;
                        if (inElement.GetAttribute("oid") == StrObjectID)
                        {
                            AlreadyExist = true;
                            break;
                        }
                    }
                    break;
                }
            }
            if (!AlreadyExist)
            {
                if (RoleBCollection == null)
                {
                    RoleBCollection = new XElement(roleBName);
                    RoleAStorageInstance.Add(RoleBCollection);
                    objectStorageSession.SetMappedTagName(AssociationClass.ClassHierarchyLinkAssociation.RoleB.Identity.ToString().ToLower(), roleBName);
                }
                XElement NewXmlElement = new XElement("oid");
                RoleBCollection.Add(NewXmlElement);//(System.Xml.XmlNode)
                NewXmlElement.SetAttribute("oid", AssociationObject.PersistentObjectID.ToString());
                NewXmlElement.SetAttribute("ClassInstaditationName", AssociationObject.MemoryInstance.GetType().FullName);
                NewXmlElement.SetAttribute("AssemblyFullName", AssociationObject.MemoryInstance.GetType().GetMetaData().Assembly.FullName);
            }
            AlreadyExist = false;

            foreach (XNode CurrNode in RoleBStorageInstance.Elements())
            {
                string StrObjectID = AssociationObject.PersistentObjectID.ToString();
                XElement Element = (XElement)CurrNode;
                  if (Element.Name == roleAName)
                {
                    RoleACollection = Element;
                    foreach (XElement inCurrNode in RoleACollection.Elements())
                    {
                        XElement inElement = inCurrNode;
                        if (inElement.Attribute("oid") != null && inElement.Attribute("oid").Value == StrObjectID)
                        {
                            AlreadyExist = true;
                            break;
                        }
                    }
                    break;
                }
            }
            if (!AlreadyExist)
            {
                if (RoleACollection == null)
                {
                   
                    RoleACollection = new XElement(roleAName);
                    RoleBStorageInstance.Add(RoleACollection);
                    objectStorageSession.SetMappedTagName(AssociationClass.ClassHierarchyLinkAssociation.RoleA.Identity.ToString().ToLower(), roleAName);

                }
                XElement NewXmlElement = new XElement("oid");
                RoleACollection.Add(NewXmlElement);//(System.Xml.XmlNode)
                NewXmlElement.SetAttributeValue("oid", AssociationObject.PersistentObjectID.ToString());
                NewXmlElement.SetAttributeValue("ClassInstaditationName", AssociationObject.MemoryInstance.GetType().FullName);
                NewXmlElement.SetAttribute("AssemblyFullName", AssociationObject.MemoryInstance.GetType().GetMetaData().Assembly.FullName);
            }
            AlreadyExist = false;
            AssociationObjectStorageInstance.SetAttribute("RoleA", AssociationRoleA.PersistentObjectID.ToString());
            AssociationObjectStorageInstance.SetAttribute("RoleAClassInstaditationName", AssociationRoleA.MemoryInstance.GetType().FullName);
            AssociationObjectStorageInstance.SetAttribute("RoleB", AssociationRoleB.PersistentObjectID.ToString());
            AssociationObjectStorageInstance.SetAttribute("RoleBClassInstaditationName", AssociationRoleB.MemoryInstance.GetType().FullName);
        }

        public XElement NextObjectIDNode;
        public XElement ObjectCollectionNode;
    }
}
