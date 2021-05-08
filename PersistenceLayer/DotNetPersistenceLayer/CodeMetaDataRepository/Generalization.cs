namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{A53DBC82-4284-4370-A175-53B336D90A73}</MetaDataID>
    public class Generalization : OOAdvantech.MetaDataRepository.Generalization
    {
        /// <MetaDataID>{877c25de-e0fe-4bc1-8297-52d24878517d}</MetaDataID>
        public Generalization(string name, MetaDataRepository.Classifier parentClassifier, MetaDataRepository.Classifier childClassifier)
            : base(name, parentClassifier, childClassifier)
        {
            string identity = null;
            if (!(parentClassifier is CodeElementContainer))
                identity = childClassifier.GetPropertyValue<string>("MetaData", "MetaObjectID") + "." + parentClassifier.Identity.ToString();
            else
                identity = (childClassifier.GetPropertyValue(typeof(string), "MetaData", "MetaObjectID") as string) + "." + (parentClassifier.GetPropertyValue(typeof(string), "MetaData", "MetaObjectID") as string);
            PutPropertyValue("MetaData", "MetaObjectID", identity);
        }

        /// <MetaDataID>{8f0fd04f-7938-4baa-9f7c-e484b61d5ae6}</MetaDataID>
        public Generalization()
        {
        }

        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }
            try
            {
                base.Synchronize(OriginMetaObject);
                if (Parent == null)
                    return;

                bool alreadyExist = false;
                if (Child is Class&&(Child as Class).VSClass!=null)
                {
                    foreach (EnvDTE.CodeElement codeElement in (Child as Class).VSClass.Bases)
                    {
                        if (codeElement is EnvDTE.CodeClass)
                        {
                            if ((codeElement as EnvDTE.CodeClass).FullName == Parent.FullName)
                                alreadyExist = true;
                        }

                    }
                }
                if (Child is Interface && (Child as Interface).VSInterface!=null)
                {
                    foreach (EnvDTE.CodeElement codeElement in (Child as Interface).VSInterface.Bases)
                    {
                        if (codeElement is EnvDTE.CodeInterface)
                        {
                            if ((codeElement as EnvDTE.CodeInterface).FullName == Parent.FullName)
                                alreadyExist = true;
                        }

                    }
                }

                if (Child is Structure && (Child as Structure).VSStruct != null)
                {
                    foreach (EnvDTE.CodeElement codeElement in (Child as Structure).VSStruct.Bases)
                    {
                        if (codeElement is EnvDTE.CodeStruct)
                        {
                            if ((codeElement as EnvDTE.CodeStruct).FullName == Parent.FullName)
                                alreadyExist = true;
                        }

                    }
                }

                try
                {
                    if (!alreadyExist)
                    {
                        if (Child is Class && (Child as Class).VSClass!=null)
                            (Child as Class).VSClass.AddBase(Parent.FullName, 0);
                        if (Child is Interface && (Child as Interface).VSInterface!=null)
                            (Child as Interface).VSInterface.AddBase(Parent.FullName, 0);
                        if (Child is Structure && (Child as Structure).VSStruct != null)
                            (Child as Structure).VSStruct.AddBase(Parent.FullName, 0);

                    }

                }
                catch (System.Exception error)
                {

                }
            }
            catch (System.Exception error)
            {
                throw;
            }




        }
    }
}
