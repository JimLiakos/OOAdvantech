namespace RoseMetaDataRepository
{
    /// <MetaDataID>{8AB17476-A6A4-4EBC-BE43-3221D4FA5F83}</MetaDataID>
    internal class UnknownClassifier : OOAdvantech.MetaDataRepository.Classifier
    {
        static System.Collections.Generic.Dictionary<string, System.WeakReference> UnknownClassifiers = new System.Collections.Generic.Dictionary<string, System.WeakReference>();
        UnknownClassifier()
        {
        }
        UnknownClassifier(string name)
        {
            _Name = name;
            if (_Name != null)
                _Name = _Name.Trim();
 
        }
        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            
            base.Synchronize(OriginMetaObject);
        }

        static public UnknownClassifier GetClassifier(string name)
        {

            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentException("The parameter must be not null or empty");
            if (UnknownClassifiers.ContainsKey(name.Trim()) && UnknownClassifiers[name.Trim()].IsAlive)
                return UnknownClassifiers[name.Trim()].Target as UnknownClassifier;
            else
            {
                if (UnknownClassifiers.ContainsKey(name.Trim()) && !UnknownClassifiers[name.Trim()].IsAlive)
                    UnknownClassifiers.Remove(name.Trim());
                UnknownClassifier unknownClassifier = new UnknownClassifier(name);
                System.WeakReference weakReference = new System.WeakReference(unknownClassifier);
                UnknownClassifiers.Add(unknownClassifier.FullName, weakReference);
                return unknownClassifier;
            }
        }
        


        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                if (value == _Name)
                    return;
                else
                {
                    if (string.IsNullOrEmpty(value))
                        throw new System.ArgumentException("The parameter must be not null or empty");

                    string fullName = FullName;
                    base.Name = value;
                    if (fullName != FullName)
                    {
                        if (UnknownClassifiers.ContainsKey(fullName))
                        {
                            System.WeakReference weakReference = UnknownClassifiers[fullName];
                            UnknownClassifiers.Remove(fullName);
                        }
                        UnknownClassifiers[FullName]=new System.WeakReference(this);
                    }



                }
            }
        }
    }
}
