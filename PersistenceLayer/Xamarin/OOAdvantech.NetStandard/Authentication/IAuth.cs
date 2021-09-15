using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#if DeviceDotNet
using Xamarin.Forms;
#endif

namespace OOAdvantech.Authentication
{

    public delegate void AuthStateChangeHandler(object sender, AuthStateEventArgs authArgs);

    public delegate void IdTokenChangeHandler(object sender, IdTokenEventArgs idTokenArgs);


#if DeviceDotNet
    /// <MetaDataID>{fda3809a-9673-4f40-a158-6c83e0d116e2}</MetaDataID>
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
#endif
    /// <MetaDataID>{bbb74ab0-28e1-4dc3-8e46-09b753b5db08}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IAuth
    {
        [OOAdvantech.MetaDataRepository.GenerateEventConsumerProxy]
        event AuthStateChangeHandler AuthStateChange;

        [OOAdvantech.MetaDataRepository.GenerateEventConsumerProxy]
        event IdTokenChangeHandler IdTokenChange;

        IAuthUser CurrentUser { get; }

        Task<string> GetIdToken();

        bool SignInWith(SignInProvider provider);

        Task<string> EmailSignIn(string email, string password);

        Task<string> EmailSignUp(string email, string password);

        void SendPasswordResetEmail(string email);



        void SignOut();

    }
    /// <MetaDataID>{179d71b7-2a40-4525-bd1d-cef724192522}</MetaDataID>
    public enum SignInProvider
    {
        Google = 0,
        Facebook = 1,
        Twitter = 2,
        Email = 3
    }
    /// <MetaDataID>{e95f3c30-c154-43d9-9694-027c33114745}</MetaDataID>
    public class OOAdvantechAuth
    {
        static IAuth _Auth;
        public static IAuth Auth
        {
            get
            {
                if (_Auth == null)
                {
#if DeviceDotNet
                    IDeviceInstantiator deviceInstantiator = DependencyService.Get<IDeviceInstantiator>();
                    _Auth = deviceInstantiator.GetDeviceSpecific(typeof(IAuth)) as IAuth;
#endif
                }
                return _Auth;
            }
        }
    }

    /// <MetaDataID>{ffbd96e8-353f-45b0-ad70-ae7d4cd978dd}</MetaDataID>
    public class AuthStateEventArgs : EventArgs
    {
        public AuthStateEventArgs(IAuth auth)
        {
            Auth = auth;
        }
        IAuthUser CurrentUser { get; }
        public IAuth Auth { get; private set; }
    }
    /// <MetaDataID>{d04e0552-a165-4cf0-bdc7-681246926ba4}</MetaDataID>
    public class IdTokenEventArgs : EventArgs
    {
        public IdTokenEventArgs(IAuth auth)
        {
            Auth = auth;
        }

        public IAuth Auth { get; private set; }
    }

    /// <MetaDataID>{990247cb-4a4a-4b27-9cc5-0abf5117dd2f}</MetaDataID>
    public interface IAuthUser
    {
        string DisplayName { get; }
        bool IsEmailVerified { get; }
        bool IsAnonymous { get; }

        string Email { get; }
        string PhotoUrl { get; }
        IList<IUserInfo> ProviderData { get; }
        string ProviderId { get; }
        IList<string> Providers { get; }
        string PhoneNumber { get; }
        string Uid { get; }

    }

    /// <MetaDataID>{b9b65383-6104-462a-bcf6-dcf56d465c71}</MetaDataID>
    public class AuthUser : IAuthUser
    {


        public bool IsEmailVerified { get; set; }

        public bool IsAnonymous { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }


        public IList<IUserInfo> ProviderData { get; set; }


        public string ProviderId { get; set; }

        public IList<string> Providers { get; set; }

        public string PhoneNumber { get; set; }

        public string Uid { get; set; }
        public string DisplayName { get; set; }

        public AuthSubsystem AuthSubsystem { get; set; } = AuthSubsystem.Device;
    }

    /// <MetaDataID>{575d4f5e-2c31-4bff-89d7-cdee483ee371}</MetaDataID>
    public enum AuthSubsystem
    {
        Device = 0,
        Web = 1
    }


    /// <MetaDataID>{1fc57859-3b1b-42d6-a720-01ddc158e94e}</MetaDataID>
    public interface IUserInfo
    {
        string DisplayName { get; }
        string Email { get; }
        bool IsEmailVerified { get; }
        string PhoneNumber { get; }
        string PhotoUrl { get; }
        string ProviderId { get; }
        string Uid { get; }
    }

    /// <MetaDataID>{320bbc08-b971-4834-a90a-d3c0e27cd5e0}</MetaDataID>
    public class UserInfo : IUserInfo
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string ProviderId { get; set; }
        public string Uid { get; set; }
        public bool IsAnonymous { get; set; }

    }

}
