namespace RoseMetaDataRepository
{
    /// <MetaDataID>{A821B342-61AB-4200-8A88-3210255142D7}</MetaDataID>
    internal class Namespace : OOAdvantech.MetaDataRepository.Namespace
    {

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

        internal Namespace()
        {
        }
        RationalRose.RoseModel RoseModel;
        public Namespace(RationalRose.RoseModel roseModel)
        {
            RoseModel = roseModel;
        } 

        internal RationalRose.RoseCategory RoseCategory;
        public Namespace(RationalRose.RoseCategory roseCategory)
        {
            RoseCategory = roseCategory;
            _Name = RoseCategory.Name;
            if (_Name != null)
                _Name = _Name.Trim();

            string tmp = roseCategory.GetUniqueID();
            MetaObjectMapper.AddTypeMap(roseCategory.GetUniqueID(), this);
            for (int i = 0; i < roseCategory.Categories.Count; i++)
            {
                Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(roseCategory.Categories.GetAt((short)(i + 1)).GetUniqueID()) as Namespace;
                if (_namespace == null && !roseCategory.Equals(roseCategory.Model.RootCategory))
                {

                    _namespace = new Namespace(roseCategory.Categories.GetAt((short)(i + 1)));
                    if (roseCategory.Categories.GetAt((short)(i + 1)).GetPropertyValue("C#", "Root Namespace") != "True")
                        _OwnedElements.Add(_namespace);
                }
                else
                {
                    if (roseCategory.Categories.GetAt((short)(i + 1)).GetPropertyValue("C#", "Root Namespace") != "True")
                        _OwnedElements.Add(_namespace);
                }

            }
            if (roseCategory.ParentCategory != null && !roseCategory.ParentCategory.Equals(roseCategory.Model.RootCategory))
            {
                if (roseCategory.GetPropertyValue("C#", "Root Namespace") != "True")
                {
                    _Namespace.Value = MetaObjectMapper.FindMetaObjectFor(roseCategory.ParentCategory.GetUniqueID()) as Namespace;
                    if (_Namespace.Value == null)
                        _Namespace.Value = new Namespace(roseCategory.ParentCategory);
                }
                else
                {
                    Namespace _namespace = MetaObjectMapper.FindMetaObjectFor(roseCategory.ParentCategory.GetUniqueID()) as Namespace;
                    if (_namespace == null)
                        _namespace = new Namespace(roseCategory.ParentCategory);


                }
                


            }


        }
        string _FullName;
        public override string FullName
        {
            get
            { 
                if (_FullName == null)
                {
                    if (RoseCategory == null || string.IsNullOrEmpty(RoseCategory.GetPropertyValue("C#", "FullName")))
                        _FullName = base.FullName;
                    else
                        _FullName = RoseCategory.GetPropertyValue("C#", "FullName");
                    if (_FullName == null)
                        _FullName = "";
                }
                return _FullName;
            }
        }
        public override OOAdvantech.MetaDataRepository.MetaObjectID Identity
        {
            get
            {
                if (RoseCategory == null)
                    return base.Identity;
                else
                { 
                    if (_Identity != null)
                        return _Identity;
                    //if (string.IsNullOrEmpty(RoseCategory.GetPropertyValue("MetaData", "MetaObjectID")))
                    {
                        RationalRose.RoseCategory category = RoseCategory;
                        string fullName = null;
                        while (!category.Equals((RoseCategory.Application as RationalRose.RoseApplication).CurrentModel.RootCategory))
                        {
                            if (fullName != null)
                                fullName = category.Name + "." + fullName;
                            else
                                fullName = category.Name;
                            category = category.ParentCategory;
                        }
                        _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(fullName);
                        return _Identity;
                    }

                    _Identity = new OOAdvantech.MetaDataRepository.MetaObjectID(RoseCategory.GetPropertyValue("MetaData", "MetaObjectID"));
                    return _Identity;
                }
            }
        }
        public override void ShallowSynchronize(OOAdvantech.MetaDataRepository.MetaObject originMetaObject)
        {
            base.ShallowSynchronize(originMetaObject);

            try
            {
                if (RoseCategory == null && Namespace != null)
                    RoseCategory = (Namespace as Namespace).RoseCategory.AddCategory(originMetaObject.Name);


                if (RoseCategory == null && Namespace == null)
                    RoseCategory = RoseModel.RootCategory.AddCategory(originMetaObject.Name);

                // if (_Identity != null && string.IsNullOrEmpty(RoseCategory.GetPropertyValue("MetaData", "MetaObjectID")))
                RoseCategory.OverrideProperty("MetaData", "MetaObjectID", originMetaObject.Identity.ToString());

                RoseCategory.OverrideProperty("C#", "FullName", originMetaObject.FullName);
            }
            catch (System.Exception error)
            {
                if (OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog != null)
                {
                    if (error.InnerException != null)
                        error = error.InnerException;

                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(FullName + ":  " + error.Message);

                }
                throw;

            }
        }
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            base.Synchronize(OriginMetaObject);
            try
            {
                if (RoseCategory == null && Namespace != null)
                    RoseCategory = (Namespace as Namespace).RoseCategory.AddCategory(OriginMetaObject.Name);


                if (RoseCategory == null && Namespace == null)
                    RoseCategory = RoseModel.RootCategory.AddCategory(OriginMetaObject.Name);

                // if (_Identity != null && string.IsNullOrEmpty(RoseCategory.GetPropertyValue("MetaData", "MetaObjectID")))
                RoseCategory.OverrideProperty("MetaData", "MetaObjectID", OriginMetaObject.Identity.ToString());

                RoseCategory.OverrideProperty("C#", "FullName", OriginMetaObject.FullName);
            }
            catch (System.Exception error)
            {
                if (OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog != null)
                {
                    if (error.InnerException != null)
                        error = error.InnerException;

                    OOAdvantech.MetaDataRepository.SynchronizerSession.ErrorLog.WriteError(FullName + ":  " + error.Message);

                }
                throw;

            }


        }


    }
}
