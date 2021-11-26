namespace OOAdvantech.MetaDataRepository
{
    using System;
    /// <MetaDataID>{9B0BE316-2D03-418D-8BD4-E40CC94B0C51}</MetaDataID>
    [AttributeUsage(AttributeTargets.Method)]
    public class ObjectActivationCall:System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ObjectsLinkCall: System.Attribute
    {
    }

    /// <MetaDataID>{e0bd735a-82c4-4fb7-9650-a5fde44bdebc}</MetaDataID>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommitObjectStateInStorageCall : System.Attribute
    {
        
    }


    /// <MetaDataID>{e0bd735a-82c4-4fb7-9650-a5fde44bdebc}</MetaDataID>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeCommitObjectStateInStorageCall : System.Attribute
    {

    }


    /// <MetaDataID>{1b443cf8-90fa-403d-81a4-0c5404b5c1ad}</MetaDataID>
    public class DeleteObjectCall : System.Attribute
    {

    }

}
