using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{198f186f-5c01-4ceb-97ba-336d2a36e030}</MetaDataID>
    public interface IChannel
    {
        [Association("", Roles.RoleA, "28ed4671-9e0a-41ba-8f59-b4becfe469eb")]
        [RoleAMultiplicityRange(1, 1)]
        IEndPoint EndPoint { get;  }
        ChannelState ChannelState { get;  }

        /// <MetaDataID>{b198c192-6fc4-4bea-aeea-84bf480b3f2e}</MetaDataID>
        ResponseData ProcessRequest(RequestData request);

        /// <MetaDataID>{a5e08253-b56d-42cc-a5dc-69b508b86fec}</MetaDataID>
        Task<ResponseData> AsyncProcessRequest(RequestData request);

        /// <MetaDataID>{768884af-5678-46c5-930e-dca2402235dc}</MetaDataID>
        void Close();

        /// <MetaDataID>{7e411ece-e8fc-40c3-a23c-ba9569516de3}</MetaDataID>
        /// <summary>
        /// Inform client that physical connection has been dropped.
        /// this happened because computing context which serve client has been moved from computing resources allocation mechanism.
        /// </summary>
        void PhysicalConnectionDropped();




    }
}
