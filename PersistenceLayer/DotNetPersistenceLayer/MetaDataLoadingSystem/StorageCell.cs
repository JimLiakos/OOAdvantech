using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
namespace OOAdvantech.MetaDataLoadingSystem
{
    /// <MetaDataID>{30edeaa2-12fc-440e-8a51-1e7d50fa5405}</MetaDataID>
    public class StorageCell:MetaDataRepository.StorageCell
    {
        public override int SerialNumber
        {
            get 
            {
                if(XmlElement==null)
                return _Type.GetHashCode();
                if (XmlElement.Attribute("SerialNumber")!=null)
                {
                    return int.Parse(XmlElement.GetAttribute("SerialNumber"));
                }
                else
                {
                    var objectStorage = ((Namespace as Storage).ObjectStorage as MetaDataStorageSession);
                    int ObjID = 1;
                    if (objectStorage.ObjectIDNode.Attribute("NextSCSerialNumber")!=null)
                        ObjID = int.Parse(objectStorage.ObjectIDNode.GetAttribute("NextSCSerialNumber"));
                    ObjID++;
                    objectStorage.ObjectIDNode.SetAttribute("NextSCSerialNumber", ObjID.ToString());
                    ObjID--;
                    XmlElement.SetAttribute("SerialNumber", ObjID.ToString());
                    return ObjID;
                }
               
            }
            set
            {
            }
        }

        public override OOAdvantech.MetaDataRepository.ObjectIdentityType ObjectIdentityType
        {
            get
            {
                return new MetaDataRepository.ObjectIdentityType(new List<MetaDataRepository.IIdentityPart>() { new MetaDataRepository.IdentityPart("ObjectID", "ObjectID", typeof(ulong)) });
            }
            set
            {
               
            }
        }

        public override void ActivateAllObjects()
        {
            
        }


        public override bool IsTypeOf(string ofTypeIdentity)
        {
            if (ofTypeIdentity != null)
            {
                var ofTypeClassifier = StorageCell.GetOfTypeClassifier(ofTypeIdentity, Type);
                if (ofTypeClassifier != null&& Type.IsA(ofTypeClassifier))
                    return true;
            }
            return false;
        }
        public override OOAdvantech.Collections.Generic.List<OOAdvantech.MetaDataRepository.StorageCell> GetLinkedStorageCells(OOAdvantech.MetaDataRepository.MetaObjectID associationIdentity, OOAdvantech.MetaDataRepository.Roles linkedStorageCellsRole)
        {
            throw new NotImplementedException();
        }
        public override bool AllObjectsInActiveMode
        {
            get
            {
                if (XmlElement != null)
                {
                    XElement storageCellReferencesElement = XmlElement.Element("StorageCellReferences");
                    if (storageCellReferencesElement != null && storageCellReferencesElement.Elements("StorageCellReference").Count() > 0)
                        return false;
                }
                return true;
            }
        }

        /// <MetaDataID>{c52f10d9-ff8e-47d9-bc33-eb089177fb0a}</MetaDataID>
        protected StorageCell()
        {
        }


        /// <MetaDataID>{413c6171-dcfe-4ab1-a263-86bc53b5d8a1}</MetaDataID>
        internal XElement XmlElement;

        /// <MetaDataID>{f5ff279a-0392-41bb-b0e0-56d3b0a4607b}</MetaDataID>
        public StorageCell(string storageIntentity, MetaDataRepository.Class _class, XElement xmlElement, MetaDataRepository.Namespace _namespace)
        {
            _Namespace.Value = _namespace;
            _StorageIntentity = storageIntentity;
            XmlElement = xmlElement;
            _Type = _class;
            int classAssociateRoles = _class.GetAssociateRoles(true).Count;

            var serialNumber = SerialNumber;

        }
        /// <MetaDataID>{ae80917b-b57a-450f-bbbc-2483d3be858e}</MetaDataID>
        string _StorageIntentity;
        public override string StorageIdentity
        {
            get 
            {
                return _StorageIntentity;
            }
        }

        public override System.Collections.Generic.List<object> GetExtensionMetaObjects()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
