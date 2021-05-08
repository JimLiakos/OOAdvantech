namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{30E0517B-83DB-491A-93D5-0062F9EEA721}</MetaDataID>
    public class UnknownClassifier : OOAdvantech.MetaDataRepository.Classifier
    {
        /// <MetaDataID>{431ecff8-2cb5-46e2-bcba-c6a244180c1a}</MetaDataID>
        static System.Collections.Generic.Dictionary<string, System.WeakReference> UnknownClassifiers = new System.Collections.Generic.Dictionary<string, System.WeakReference>();
        /// <MetaDataID>{57868abc-c38e-4f3b-8e52-5a4ebe52c5b0}</MetaDataID>
        UnknownClassifier()
        {

        }
        /// <MetaDataID>{e14e6987-a124-4772-ab44-89a4efa073ba}</MetaDataID>
        UnknownClassifier(string name,MetaDataRepository.Component implementationUnit)
        { 
            _ImplementationUnit.Value = implementationUnit;
            _Name = name;
            MetaObjectMapper.AddMetaObject(this, this.FullName);
        }

        /// <MetaDataID>{66d75ee3-5bd0-40b1-b46c-53c44687a86e}</MetaDataID>
        static public UnknownClassifier GetClassifier(string name, MetaDataRepository.Component implementationUnit)
        {

            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentException("The parameter must be not null or empty");
            UnknownClassifier classifier = null;
            if (UnknownClassifiers.ContainsKey(name.Trim()))
                classifier = UnknownClassifiers[name.Trim()].Target as UnknownClassifier;
            if (classifier == null)
            {
                classifier = new UnknownClassifier(name,implementationUnit);
                System.WeakReference weakReference = new System.WeakReference(classifier);
                UnknownClassifiers[name] = weakReference;

            }
            if (MetaObjectMapper.FindMetaObject(classifier.Identity) == null)
                MetaObjectMapper.AddMetaObject(classifier, classifier.FullName);
            return classifier;


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
                    System.WeakReference weakReference = UnknownClassifiers[_Name];
                    UnknownClassifiers.Remove(_Name);
                    base.Name = value;
                    UnknownClassifiers[value] = weakReference;
                }



            }
        }

    }
}
