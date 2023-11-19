namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{cff3d2f0-d8cb-45ff-bc47-ca222b3e7a86}</MetaDataID>
    public interface IEndPoint
    {

        /// <MetaDataID>{12f78c7e-e562-44c1-ab24-c42f32e57bae}</MetaDataID>
        void RejectRequest(System.Threading.Tasks.Task<ResponseData> task);
        /// <MetaDataID>{2fab8dab-345c-431f-963a-fcaf60597ec7}</MetaDataID>
        void SendResponce(ResponseData responseData);

        /// <MetaDataID>{6c653529-cea7-4b40-985f-cc1ed47209f8}</MetaDataID>
        System.Threading.Tasks.Task<ResponseData> SendRequestAsync(RequestData request);

        /// <MetaDataID>{ed911a83-7559-4200-94b6-625107ac7d27}</MetaDataID>
        ResponseData SendRequest(RequestData requestData);

        /// <MetaDataID>{c4c4d690-60d5-4ea7-8c57-133019ce7c39}</MetaDataID>
        bool ConnectionIsOpen { get; }

        string EndPointID { get; }


        ///// <MetaDataID>{4d01a536-83bd-4e33-a0ab-8fea1197a922}</MetaDataID>
        //void SendRequestAsync(RequestData requestData);
    }
}