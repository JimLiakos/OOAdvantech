namespace OOAdvantech.CodeMetaDataRepository
{
    /// <MetaDataID>{E307D188-B73F-42FD-B8F7-1847ECF825CA}</MetaDataID>
    public class Realization : OOAdvantech.MetaDataRepository.Realization
    {
        /// <MetaDataID>{704902f0-3a5a-4afd-9273-0ba19caf8c0a}</MetaDataID>
        internal Realization()
        {
        }
        /// <MetaDataID>{8a5cfa25-f20f-4007-ab31-47e943204c8d}</MetaDataID>
        public Realization(string name, OOAdvantech.MetaDataRepository.Interface _interface, OOAdvantech.MetaDataRepository.Class _class)
            : base(name, _interface, _class)
        {
            string identity = null;
            if(!(_interface is Interface))
                identity = _class.GetPropertyValue<string>( "MetaData", "MetaObjectID")  + "." + _interface.Identity.ToString();
            else
                identity = _class.GetPropertyValue<string>( "MetaData", "MetaObjectID")  + "." + _interface.GetPropertyValue<string>( "MetaData", "MetaObjectID") ;
            PutPropertyValue("MetaData", "MetaObjectID", identity);
        }
        /// <MetaDataID>{d9f7f46e-7ba4-4870-80df-bf28e6298804}</MetaDataID>
        public Realization(string name, OOAdvantech.MetaDataRepository.Interface _interface, OOAdvantech.MetaDataRepository.Structure _struct)
            : base(name, _interface, _struct)
        {
            string identity = null;

            if (!(_interface is Interface))
                identity = _struct.GetPropertyValue<string>("MetaData", "MetaObjectID") + "." + _interface.Identity.ToString();
            else
                identity = _struct.GetPropertyValue<string>("MetaData", "MetaObjectID") + "." + _interface.GetPropertyValue<string>("MetaData", "MetaObjectID");

            PutPropertyValue("MetaData", "MetaObjectID", identity);
        }


        public override void Synchronize(OOAdvantech.MetaDataRepository.MetaObject OriginMetaObject)
        {
            if (IDEManager.SynchroForm.InvokeRequired)
            {
                IDEManager.SynchroForm.Synchronize(this, OriginMetaObject);
                return;
            }

            base.Synchronize(OriginMetaObject);
            if (Abstarction == null)
                return;
            bool alreadyExist = false;
            if (Implementor is Class)
            {
                foreach (EnvDTE.CodeElement codeElement in (Implementor as Class).VSClass.ImplementedInterfaces)
                {
                    if (codeElement is EnvDTE.CodeInterface)
                    {
                        if ((codeElement as EnvDTE.CodeInterface).FullName == Abstarction.FullName)
                            alreadyExist = true;
                    }
                }
            }
            if (Implementor is Structure)
            {
                foreach (EnvDTE.CodeElement codeElement in (Implementor as Structure).VSStruct.ImplementedInterfaces)
                {
                    if (codeElement is EnvDTE.CodeInterface)
                    {
                        if ((codeElement as EnvDTE.CodeInterface).FullName == Abstarction.FullName)
                            alreadyExist = true;
                    }
                }
            }
            try
            {
                if (!alreadyExist)
                {
                    if (Implementor is Class)
                        (Implementor as Class).VSClass.AddImplementedInterface(Abstarction.FullName, 0);

                    if (Implementor is Structure)
                        (Implementor as Structure).VSStruct.AddImplementedInterface(Abstarction.FullName, 0);
                }

            }
            catch (System.Exception error)
            {
            }
        }

    }
}
