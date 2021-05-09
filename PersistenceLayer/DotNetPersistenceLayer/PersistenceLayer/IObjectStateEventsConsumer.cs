namespace OOAdvantech.PersistenceLayer
{
    /// <MetaDataID>{63e97701-17e8-4eff-9286-f0ba2b3904be}</MetaDataID>
    public interface IObjectStateEventsConsumer
    {
        /// <MetaDataID>{1ba2173d-7456-4f51-9a2a-e5856d2d3d12}</MetaDataID>
        void OnCommitObjectState();
        /// <MetaDataID>{104b89ca-d352-4502-8acf-9c93e69dc3e0}</MetaDataID>
        void OnActivate();

        /// <MetaDataID>{2b1043e8-a64f-4382-9f42-208dd97a3f57}</MetaDataID>
        void OnDeleting();

        /// <MetaDataID>{27d48544-e3b0-4bbb-8e30-4e029d01fed8}</MetaDataID>
        void LinkedObjectAdded(object linkedObject, MetaDataRepository.AssociationEnd associationEnd);
        /// <MetaDataID>{caa1b717-a9c6-4869-a9d4-4c37a5188cb0}</MetaDataID>
        void LinkedObjectRemoved(object linkedObject, MetaDataRepository.AssociationEnd associationEnd);
        /// <MetaDataID>{7a291128-e8fd-4ca6-850c-236725465b01}</MetaDataID>
        void BeforeCommitObjectState();
    }
}
