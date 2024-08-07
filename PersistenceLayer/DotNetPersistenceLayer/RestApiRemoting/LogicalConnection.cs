using OOAdvantech.MetaDataRepository;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{fc94323f-b4fc-45de-b310-4dc637a07b7a}</MetaDataID>
    [MetaDataRepository.AssociationClass(typeof(WebSocketClient), typeof(WebSocketServer), "WebSocketsInterconnection")]
    public class InterConnection : PhysicalConnection
    {
        /// <MetaDataID>{51f08478-4beb-4050-b472-5aa8dcc0e953}</MetaDataID>
        [AssociationClassRole(Roles.RoleB)]
        public WebSocketServer Public { get; set; }

        /// <MetaDataID>{5248cf4c-677b-4401-8f78-4dc9ab2a38fe}</MetaDataID>
        [AssociationClassRole(Roles.RoleA)]
        public WebSocketClient Internal { get; set; }


    }


    /// <MetaDataID>{9f3d583b-5447-4e00-818a-28a8e279e4dd}</MetaDataID>
    public class LocalConnection : PhysicalConnection
    {

        /// <MetaDataID>{e2f01fef-b4ad-4679-807d-28c8b712fdc5}</MetaDataID>
        [AssociationClassRole(Roles.RoleA)]
        public IsolatedContext IsolatedContext { get; set; }


        /// <MetaDataID>{f8a894ce-a599-40ba-8a64-d4074ec9d6eb}</MetaDataID>
        [AssociationClassRole(Roles.RoleB)]
        public WebSocketServer CommunicationEndPoints { get; set; }

    } 
}