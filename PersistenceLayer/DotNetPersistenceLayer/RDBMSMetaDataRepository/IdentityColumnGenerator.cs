namespace OOAdvantech.RDBMSMetaDataRepository
{
	/// <MetaDataID>{7A08B7DE-5660-4BC1-B065-B4D541A73312}</MetaDataID>
	public abstract class AutoProduceColumnsGenerator
	{
		/// <MetaDataID>{38BF1A91-F71A-4741-93CE-0E5E1B5743D4}</MetaDataID>
		public static AutoProduceColumnsGenerator CurrentAutoProduceColumnsGenerator=new UUIDAutoProduceColumnsGenerator(); //new HighLowAutoProduceColumnsGenerator();

		/// <MetaDataID>{1D40C347-B799-4C54-8953-13CF1E879F59}</MetaDataID>
		public abstract MetaDataRepository.ObjectIdentityType GetObjectIdentityType(MetaDataRepository.MetaObject placeIdentifier);

        /// <MetaDataID>{d04a0e29-a642-469d-8d17-89070ff29c31}</MetaDataID>
        public abstract MetaDataRepository.ObjectIdentityType GetObjectIdentityRefernce(MetaDataRepository.MetaObject placeIdentifier, AssociationEnd associationEnd, string associationName);

		/// <MetaDataID>{4ED64F70-9263-497F-92B5-2C47E80CA17D}</MetaDataID>
		public abstract Collections.Generic.Set<Column> GetAuxiliaryColumns(MetaDataRepository.MetaObject placeIdentifier);
        public IdentityColumn GetIdentityColumn(MetaDataRepository.IIdentityPart identityPart, MetaDataRepository.MetaObject placeIdentifier)
        {
            Storage storage = null;
            if (placeIdentifier.ImplementationUnit != null)
                storage = placeIdentifier.ImplementationUnit.Context as Storage;
            else if (placeIdentifier.Namespace is MetaDataRepository.Classifier)
                storage = placeIdentifier.Namespace.ImplementationUnit.Context as Storage;
            else if (placeIdentifier is MetaDataRepository.Feature)
                storage = (placeIdentifier as MetaDataRepository.Feature).Owner.ImplementationUnit.Context as Storage;
            else if (placeIdentifier.Namespace is Storage)
                storage = placeIdentifier.Namespace as   Storage;


            MetaDataRepository.Classifier doteNetClassifier = MetaDataRepository.Classifier.GetClassifier(identityPart.Type);
            MetaDataRepository.Classifier rdbmsIdentityPartType = storage.GetEquivalentMetaObject(doteNetClassifier) as MetaDataRepository.Classifier;
            if (rdbmsIdentityPartType == null)
            {
                if (MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator is RDBMSMetaDataRepository.MetaObjectsStack)
                {
                    rdbmsIdentityPartType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(doteNetClassifier, placeIdentifier) as MetaDataRepository.Classifier;
                    if (rdbmsIdentityPartType == null)
                    {
                        rdbmsIdentityPartType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(doteNetClassifier, placeIdentifier) as MetaDataRepository.Classifier;
                        rdbmsIdentityPartType.Synchronize(doteNetClassifier);
                    }
                }
                else
                {
                    if (doteNetClassifier is MetaDataRepository.Structure)
                        rdbmsIdentityPartType = new RDBMSMetaDataRepository.Structure();
                    else if (doteNetClassifier is MetaDataRepository.Primitive)
                        rdbmsIdentityPartType = new RDBMSMetaDataRepository.Primitive();
                    else
                        throw new System.Exception(string.Format("'{0}': Invalid identity part type", doteNetClassifier.FullName));
                    rdbmsIdentityPartType.Synchronize(doteNetClassifier);
                }

            }
            return new IdentityColumn(identityPart.Name, rdbmsIdentityPartType, identityPart.PartTypeName, false);

        }

  

		/// <MetaDataID>{B6288ADD-0DAD-410C-9AAA-A85635E5F834}</MetaDataID>
		public abstract string[] GetObjectIdentityColumnsName();
		/// <MetaDataID>{44B6DCB1-1BC6-4B3B-8C4C-DF2C364D9C2B}</MetaDataID>
		public abstract string[] GetAuxiliaryColumnsName();
        /// <MetaDataID>{304a6c3f-42eb-4ac1-921b-edd3211980cc}</MetaDataID>
        public static IdentityColumn GetObjectIdentityColumn(string columnName, OOAdvantech.MetaDataRepository.Classifier classifier, MetaDataRepository.MetaObject placeIdentifier)
        {    
            //TODO καλείται και όταν και όταν το MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator δεν είναι RDBMSMetaDataRepository.MetaObjectsStack
            Collections.Generic.Set<IdentityColumn> identityColums = new OOAdvantech.Collections.Generic.Set<IdentityColumn>();
            PersistenceLayer.ObjectStorage objectStorage = PersistenceLayer.ObjectStorage.GetStorageOfObject(placeIdentifier);
            MetaDataRepository.Classifier systemType = null;
            if (placeIdentifier is MetaDataRepository.Classifier)
                systemType = (placeIdentifier.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(classifier) as MetaDataRepository.Classifier;
            else if (placeIdentifier.Namespace is MetaDataRepository.Classifier)
                systemType = (placeIdentifier.Namespace.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(classifier) as MetaDataRepository.Classifier;
            else
                if (placeIdentifier is MetaDataRepository.Feature)
                    systemType = ((placeIdentifier as MetaDataRepository.Feature).Owner.ImplementationUnit.Context as Storage).GetEquivalentMetaObject(classifier) as MetaDataRepository.Classifier;

            if (systemType == null && MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator is RDBMSMetaDataRepository.MetaObjectsStack)
            {
                systemType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(classifier, placeIdentifier) as MetaDataRepository.Classifier;
                if (systemType == null)
                {
                    //System.Diagnostics.Debug.Assert(false);
                    systemType = MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(classifier, placeIdentifier) as MetaDataRepository.Classifier;
                    systemType.Synchronize(classifier);
                }
            }

            //Αυτό γιατί αν αποτύχη η transaction η τιμή του name θα γυρίσει σε null
            //TODO πρέπει να λυθεί το πρόβλημα με την χρήση new transaction.
            if (systemType == null || systemType.Name == null)
            {
                systemType = new RDBMSMetaDataRepository.Structure();
                objectStorage.CommitTransientObjectState(systemType);
                systemType.Synchronize(classifier);
            }
            IdentityColumn ObjectIDColumn = new IdentityColumn(columnName, systemType, columnName, false);

            return ObjectIDColumn;
        }
		
	}
}
