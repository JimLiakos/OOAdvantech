namespace OOAdvantech.PersistenceLayer
{
    /// <MetaDataID>{64b21cab-4bfc-417c-a933-54f1cd7f3ac8}</MetaDataID>
    public interface IBackupArchive
    {


        string LocalFileName { get; }


        void Write(string blockID, byte[] dataChunk);
    }
}