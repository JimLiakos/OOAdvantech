namespace OOAdvantech.Remoting
{
    /// <MetaDataID>{DD6BBF8C-4036-4E5D-A0E2-D7F200001326}</MetaDataID>
    public interface IPersistentObjectLifeTimeController
    {
        /// <MetaDataID>{31B9B066-2B71-4C74-B6FB-846AB8E79563}</MetaDataID>
        object GetObject(string persistentObjectUri);
        /// <MetaDataID>{30147D53-2F1A-43AE-9A7F-DB499CFB6C8A}</MetaDataID>
        string GetPersistentObjectUri(object obj);
    }
}
