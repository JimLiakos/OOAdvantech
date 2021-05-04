namespace RoseMetaDataRepository
{
    /// <MetaDataID>{BB9A6ADB-F6CE-40AC-A2A5-CA513433E2EF}</MetaDataID>
    internal class Component : OOAdvantech.MetaDataRepository.Component
    {

        bool ResidentsLoaded = false; 
        public override OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject> Residents
        {
            get
            {
                if (!ResidentsLoaded)
                {
                    _Residents = new OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject>();
                    if (RoseComponent != null)
                    {
                        for (int i = 0; i < RoseComponent.GetAssignedClasses().Count; i++)
                        {
                            RationalRose.RoseClass roseClass= RoseComponent.GetAssignedClasses().GetAt((short)(i + 1));
                            if (roseClass.Stereotype == "Interface")
                                _Residents.Add(new Interface(roseClass, this));
                            else
                                _Residents.Add(new Class(roseClass, this));
                        }
                    }
                    ResidentsLoaded = true;
                }
                return base.Residents;
            }
        }
        /// <MetaDataID>{74BB6C3B-19D1-48EB-882E-1A81391E1BB6}</MetaDataID>
        public RationalRose.RoseModule RoseComponent;

        /// <MetaDataID>{F87E2F60-D692-4B58-88FC-3303827DC3BE}</MetaDataID>
        public Component(RationalRose.RoseModule roseComponent) :this()
        {
            RoseComponent = roseComponent;
            if (RoseComponent != null)
            {
                _Name = RoseComponent.Name;
                MetaObjectMapper.AddTypeMap(RoseComponent.GetUniqueID(), this);
            }
            _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseComponent.GetPropertyValue("MetaData", "MetaObjectID"));
        }
        internal Component()
        {
        }
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {
            long count = Residents.Count;
            base.Synchronize(originMetaObject);
            _Name = originMetaObject.Name;
            if (_Name != null)
                _Name = _Name.Trim();
 
            //foreach(
        }

        /// <MetaDataID>{F30A42DE-F895-4AC7-A0B3-4A48260288AB}</MetaDataID>
        public string Solution
        {
            get
            {
                //dot Net Projects Path
                string dotNetProjectsPath = (RoseComponent.Application as RationalRose.RoseApplication).PathMap.GetActualPath("$dot Net Projects Path");
                if (RoseComponent.GetPropertyValue("DotNet", "Solution")!=null&&RoseComponent.GetPropertyValue("DotNet", "Solution").IndexOf("$dot Net Projects Path") != -1)
                    return RoseComponent.GetPropertyValue("DotNet", "Solution").Replace("$dot Net Projects Path", dotNetProjectsPath);
                else
                    return RoseComponent.GetPropertyValue("DotNet", "Solution");
            }
            set
            {
                string dotNetProjectsPath = (RoseComponent.Application as RationalRose.RoseApplication).PathMap.GetActualPath("$dot Net Projects Path");
                if (value != null && value.IndexOf(dotNetProjectsPath) == 0)
                {
                    value = value.Substring(dotNetProjectsPath.Length);
                    value = "$dot Net Projects Path" + value;
                }

                RoseComponent.OverrideProperty("DotNet", "Solution",value);
            }
        }
        /// <MetaDataID>{4B07CA9F-6099-45B0-86AD-6AA10BDDA296}</MetaDataID>
        public string Project
        {
            get
            {
                //dot Net Projects Path
                string dotNetProjectsPath = (RoseComponent.Application as RationalRose.RoseApplication).PathMap.GetActualPath("$dot Net Projects Path");
                if (RoseComponent.GetPropertyValue("DotNet", "Project")!=null&&RoseComponent.GetPropertyValue("DotNet", "Project").IndexOf("$dot Net Projects Path") != -1)
                    return RoseComponent.GetPropertyValue("DotNet", "Project").Replace("$dot Net Projects Path", dotNetProjectsPath);
                else
                    return RoseComponent.GetPropertyValue("DotNet", "Project");
            }
            set
            {
                string dotNetProjectsPath = (RoseComponent.Application as RationalRose.RoseApplication).PathMap.GetActualPath("$dot Net Projects Path");
                if (value != null && value.IndexOf(dotNetProjectsPath) == 0)
                {
                    value = value.Substring(dotNetProjectsPath.Length);
                    value = "$dot Net Projects Path" + value;
                }

                RoseComponent.OverrideProperty("DotNet", "Project",value);
            }
        }
        internal void SetIdentity(string identity)
        {
            RoseComponent.OverrideProperty("MetaData","MetaObjectID",identity);
            _Identity = null;
            
        }
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                if (_Identity != null)
                    return _Identity;
                if (RoseComponent == null)
                    return base.Identity;
                _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseComponent.GetPropertyValue("MetaData", "MetaObjectID"));
                return _Identity;
            }
        }

        public override void PutPropertyValue(string propertyNamespace, string propertyName, object PropertyValue)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                SetIdentity(new OOAdvantech.MetaDataRepository.MetaObjectID(PropertyValue as string));
            }
            else
            {
                base.PutPropertyValue(propertyNamespace, propertyName, PropertyValue);
            }
        }
        public override object GetPropertyValue(System.Type propertyType, string propertyNamespace, string propertyName)
        {
            if (propertyNamespace.ToLower() == "MetaData".ToLower() && propertyName.ToLower() == "MetaObjectID".ToLower())
            {
                return Identity.ToString();
            }
            else
                return base.GetPropertyValue(propertyType, propertyNamespace, propertyName);
        }


        
    }
}
