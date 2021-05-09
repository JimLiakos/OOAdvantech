using System;
using System.Collections.Generic;
using System.Text;
using MetaDataRepository = OOAdvantech.MetaDataRepository;
using System.Linq;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{da84de51-0526-4ff2-b2e8-ad1fec5110cd}</MetaDataID>
    public class MetaObjectMapper
    {
        public static EnvDTE.SolutionEvents SolutionEvents;
        static System.Collections.Generic.Dictionary<string, string> TypesFullNameDictionary = new Dictionary<string, string>();
        static MetaObjectMapper()
        {
            TypesFullNameDictionary[typeof(int).FullName] = "int";
            TypesFullNameDictionary[typeof(char).FullName] = "char";
            TypesFullNameDictionary[typeof(bool).FullName] = "bool";
            TypesFullNameDictionary[typeof(void).FullName] = "void";
            TypesFullNameDictionary[typeof(double).FullName] = "double";
            TypesFullNameDictionary[typeof(byte).FullName] = "byte";
            TypesFullNameDictionary[typeof(decimal).FullName] = "decimal";
            TypesFullNameDictionary[typeof(long).FullName] = "long";
            TypesFullNameDictionary[typeof(object).FullName] = "object";
            TypesFullNameDictionary[typeof(sbyte).FullName] = "sbyte";
            TypesFullNameDictionary[typeof(short).FullName] = "short";
            TypesFullNameDictionary[typeof(uint).FullName] = "uint";
            TypesFullNameDictionary[typeof(ulong).FullName] = "ulong";
            TypesFullNameDictionary[typeof(ushort).FullName] = "ushort";
            TypesFullNameDictionary[typeof(string).FullName] = "string";

            EnvDTE.DTE dte = CodeMetaDataRepository.IDEManager.GetCurrentDTE();
            SolutionEvents = dte.Events.SolutionEvents;
            SolutionEvents.ProjectRemoved += new EnvDTE._dispSolutionEvents_ProjectRemovedEventHandler(SolutionEvents_ProjectRemoved);
            SolutionEvents.Opened += new EnvDTE._dispSolutionEvents_OpenedEventHandler(SolutionEvents_Opened);
        }

        static void SolutionEvents_Opened()
        {
            MetaObjectDictionary.Clear();
            MetaObjects.Clear();
        }

        static void SolutionEvents_ProjectRemoved(EnvDTE.Project Project)
        {
            MetaObjectDictionary.Clear();
            MetaObjects.Clear();
        }

        public static string GetShortNameFor(string fullName)
        {
            if (!TypesFullNameDictionary.ContainsKey(fullName))
                return fullName;
            else
                return TypesFullNameDictionary[fullName];
        }


        /// <MetaDataID>{4E83FD50-0140-4752-8BD8-271CCE209C49}</MetaDataID>
        internal static System.Collections.Generic.Dictionary<Microsoft.VisualStudio.Modeling.ModelElement, MetaDataRepository.MetaObject> MetaObjectDictionary = new Dictionary<Microsoft.VisualStudio.Modeling.ModelElement, MetaDataRepository.MetaObject>(); 
        /// <MetaDataID>{E9BBE84F-6F76-4B04-8057-7D08AC67466B}</MetaDataID>
        private static System.Collections.Generic.Dictionary<string, MetaDataRepository.MetaObject> MetaObjects = new Dictionary<string, MetaDataRepository.MetaObject>();


        /// <MetaDataID>{4BBBFCFB-CE01-4B1F-BB14-EC1525B3CC95}</MetaDataID>
        public static void AddTypeMap(Microsoft.VisualStudio.Modeling.ModelElement modelElement, MetaDataRepository.MetaObject theMetaObject)
        {
            if (MetaObjectDictionary == null)
                MetaObjectDictionary = new Dictionary<Microsoft.VisualStudio.Modeling.ModelElement, MetaDataRepository.MetaObject>();
            if (!MetaObjectDictionary.ContainsKey(modelElement))
                MetaObjectDictionary.Add(modelElement, theMetaObject);
            else if (MetaObjectDictionary[modelElement] != theMetaObject)
                throw new System.Exception("Already exist metadata for " + modelElement.ToString());

            string identity = modelElement.Id.ToString();

            string metaObjectID = modelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (string.IsNullOrWhiteSpace(metaObjectID))
            {
                modelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", identity);
                metaObjectID = identity;
            }
            else
                identity = metaObjectID;

           
            if (!MetaObjects.ContainsKey( modelElement.GetIdentity()))
                MetaObjects[modelElement.GetIdentity()] = theMetaObject;



        }

        /// <MetaDataID>{2863C603-3B91-4805-8385-B1A8425E0EE1}</MetaDataID>
        public static void RemoveMappedElement(Microsoft.VisualStudio.Modeling.ModelElement modelElement)
        {
            if (MetaObjectDictionary != null && MetaObjectDictionary.ContainsKey(modelElement))
            {
                if (MetaObjects.ContainsKey(GetIdentity(MetaObjectDictionary[modelElement]).ToString()))
                    MetaObjects.Remove(GetIdentity(MetaObjectDictionary[modelElement]).ToString());
                MetaObjectDictionary.Remove(modelElement);
            }
        }
        internal static void RemoveMetaObject(string identityAsString)
        {
            if (MetaObjects.ContainsKey(identityAsString))
            {
                var metaObject = MetaObjects[identityAsString];
                if (metaObject is IVSUMLModelItemWrapper)
                    MetaObjectDictionary.Remove((metaObject as IVSUMLModelItemWrapper).ModelElement);
                else
                {
                    var modelElement = (from entry in MetaObjectDictionary
                                        where entry.Value == metaObject
                                        select entry.Key).FirstOrDefault();
                    if (modelElement != null)
                        MetaObjectDictionary.Remove(modelElement);
                }
                MetaObjects.Remove(identityAsString);
            }
        }
        /// <MetaDataID>{C7266E6A-F35D-4B19-A5F2-69B3FAD55A45}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObject(MetaDataRepository.MetaObjectID identity)
        {
            //if (DisableMetaObjectIdentityMapper)
            //    return null;
            if (identity == null)
                return null;

            if (MetaObjects == null)
                MetaObjects = new Dictionary<string, MetaDataRepository.MetaObject>();

            if (MetaObjects.ContainsKey(identity.ToString()))
                return (MetaDataRepository.MetaObject)MetaObjects[identity.ToString()];

            foreach (var entry in MetaObjectDictionary)
            {
                MetaDataRepository.MetaObject metaObject = entry.Value as MetaDataRepository.MetaObject;
                MetaObjects[GetIdentity(metaObject).ToString()] = metaObject;
            }
            if (MetaObjects.ContainsKey(identity.ToString()))
                return (MetaDataRepository.MetaObject)MetaObjects[identity.ToString()];
            return null;
        }
        public static OOAdvantech.MetaDataRepository.MetaObjectID GetIdentity(OOAdvantech.MetaDataRepository.MetaObject metaObject)
        {
            return new OOAdvantech.MetaDataRepository.MetaObjectID(metaObject.GetPropertyValue(typeof(string), "MetaData", "MetaObjectID") as string);
        }

        /// <MetaDataID>{9112EEE1-7310-4064-852E-C437360C6352}</MetaDataID>
        public static MetaDataRepository.MetaObject FindMetaObjectFor(Microsoft.VisualStudio.Modeling.ModelElement modelElement)
        {
            string identity = modelElement.Id.ToString();
            string metaObjectID = modelElement.GetExtensionData().GetPropertyValue("MetaData", "MetaObjectID");
            if (string.IsNullOrWhiteSpace(metaObjectID))
            {
                modelElement.GetExtensionData().SetPropertyValue("MetaData", "MetaObjectID", identity);
                metaObjectID = identity;
            }
            else
                identity = metaObjectID;

            if (MetaObjectDictionary == null)
                MetaObjectDictionary = new Dictionary<Microsoft.VisualStudio.Modeling.ModelElement, MetaDataRepository.MetaObject>();
            if (MetaObjectDictionary.ContainsKey(modelElement))
            {
                MetaDataRepository.MetaObject metaObject = (MetaDataRepository.MetaObject)MetaObjectDictionary[modelElement];
                if (metaObject is Class || metaObject is Structure)
                {
                    var stereotype = (from appliedStereotype in (modelElement as Microsoft.VisualStudio.Uml.Classes.IElement).AppliedStereotypes
                                      where appliedStereotype.Profile == "OOAdvantechProfile"
                                      select appliedStereotype).FirstOrDefault();
                    if (stereotype != null)
                    {
                        if (metaObject is Class && stereotype.Name != "class")
                        {
                            MetaObjectDictionary.Remove(modelElement);
                            MetaObjects.Remove(identity);
                            return null;
                        }
                        if (metaObject is Structure && stereotype.Name != "struct")
                        {
                            MetaObjectDictionary.Remove(modelElement);
                            MetaObjects.Remove(identity);
                            return null;
                        }

                    }
                }
                return metaObject;
            }

            return null;
        }
        /// <MetaDataID>{6E55C8BD-B217-40AD-80AC-A7ABA8D9492A}</MetaDataID>
        internal static void AddMetaObject(MetaDataRepository.MetaObject theMetaObject, string MetaObjectFullName)
        {
            string identityAsString = GetIdentity(theMetaObject).ToString().Trim();
            if (!MetaObjects.ContainsKey(identityAsString))
                MetaObjects[identityAsString] = theMetaObject;
            else
            {

                MetaDataRepository.MetaObject oldMetaObject = MetaObjects[identityAsString] as MetaDataRepository.MetaObject;
                if (oldMetaObject != theMetaObject)
                {


                    MetaDataRepository.MetaObject metaObject = MetaObjects[identityAsString] as MetaDataRepository.MetaObject;
                    string fullName = metaObject.FullName;

                    //if (metaObject is Project)
                    //{
                    //    if ((metaObject as Project).VSProject != null)
                    //        FullName = (metaObject as Project).FullName;

                    //}
                    //else if (metaObject is Class)
                    //{
                    //    if ((metaObject as Class).VSClass != null)
                    //        FullName = (metaObject as Class).VSClass.FullName;

                    //}
                    //else if (metaObject is Interface)
                    //{
                    //    if ((metaObject as Interface).VSInterface != null)
                    //        FullName = (metaObject as Interface).VSInterface.FullName;

                    //}
                    //else if (metaObject is Namespace)
                    //{
                    //    FullName = (metaObject as Namespace).FullName;
                    //}
                    //else if (metaObject is Attribute)
                    //{

                    //    if ((metaObject as Attribute).wrMember != null)
                    //        FullName = (metaObject as Attribute).wrMember.DeclaringType.FullName + "." + (metaObject as Attribute).wrMember.Name;
                    //}
                    //else if (metaObject is AssociationEnd)
                    //{
                    //    if ((metaObject as AssociationEnd).WrMemberInfo != null)
                    //        FullName = (metaObject as AssociationEnd).WrMemberInfo.DeclaringType.FullName + "." + (metaObject as AssociationEnd).WrMemberInfo.Name;
                    //}

                    //else if (metaObject is Enumeration)
                    //{
                    //    if ((metaObject as Enumeration).Refer != null)
                    //        FullName = (metaObject as Enumeration).Refer.WrType.FullName;

                    //}
                    //else if (metaObject is Method)
                    //{
                    //    if ((metaObject as Method).WrMethod != null)
                    //        FullName = (metaObject as Method).WrMethod.DeclaringType.FullName + "." + (metaObject as Method).WrMethod.Name;
                    //}

                    //else if (metaObject is Operation)
                    //{
                    //    if ((metaObject as Operation).WrMethod != null)
                    //        FullName = (metaObject as Operation).WrMethod.DeclaringType.FullName + "." + (metaObject as Operation).WrMethod.Name;
                    //}
                    //else if (metaObject is Primitive)
                    //{
                    //    if ((metaObject as Primitive).Refer != null)
                    //        FullName = (metaObject as Primitive).Refer.WrType.FullName;
                    //}
                    //else if (metaObject is Structure)
                    //{
                    //    if ((metaObject as Structure).Refer != null)
                    //        FullName = (metaObject as Structure).Refer.WrType.FullName;
                    //}
                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError("The " + MetaObjectFullName + " has the same identity with " + fullName);
                    throw new System.Exception("The " + MetaObjectFullName + " has the same identity with " + fullName);
                }
            }
        }


        public static void Clear()
        {
            MetaObjects.Clear();
            MetaObjectDictionary.Clear();
        }

       
    }
}
