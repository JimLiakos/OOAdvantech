using Microsoft.VisualStudio.Uml.Classes;
namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{abc8b539-1b1b-45aa-8e4e-db0ecd3e8a50}</MetaDataID>
    public class UnknownClassifier : OOAdvantech.MetaDataRepository.Classifier
    {

        protected UnknownClassifier()
        {

        }

        internal IType Type;

        static System.Collections.Generic.Dictionary<string, System.WeakReference> UnknownClassifiers = new System.Collections.Generic.Dictionary<string, System.WeakReference>();

        UnknownClassifier(string name)
        {
            _Name = name;
            if (_Name != null)
                _Name = _Name.Trim();

        }

        static public UnknownClassifier GetClassifier(Microsoft.VisualStudio.Uml.Classes.IType _type)
        {
            UnknownClassifier classifier = GetClassifier(_type.Name);
            classifier.Type = _type;
            return classifier;
        }
        static public UnknownClassifier GetClassifier(string name )
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
                        UnknownClassifiers[FullName] = new System.WeakReference(this);
                    }



                }
            }
        }
    }
}
