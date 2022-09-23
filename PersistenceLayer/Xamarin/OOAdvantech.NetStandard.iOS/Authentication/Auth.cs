using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Firebase.Auth;
using Foundation;
using Google.SignIn;
using Xamarin.Forms;

namespace OOAdvantech.Authentication.iOS
{
    /// <MetaDataID>{75344996-fce5-46fc-804e-4b1865a16b34}</MetaDataID>
    public class Auth : OOAdvantech.Remoting.MarshalByRefObject, IAuth, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        public Auth()
        {
            FirebaseAuthentication.FirebaseAuth.AddAuthStateDidChangeListener((auth, user) => FirebaseAuth_AuthState(auth, user)); 
            FirebaseAuthentication.FirebaseAuth.AddIdTokenDidChangeListener((auth, user) => FirebaseAuth_IdToken(auth, user));
        }

        private void FirebaseAuth_IdToken(Firebase.Auth.Auth auth, User user)
        {
            if (auth?.CurrentUser != null)
            {
                string name = auth?.CurrentUser.DisplayName;
                System.Diagnostics.Debug.WriteLine(name);
            }

            IdTokenChange?.Invoke(this, new IdTokenEventArgs(this));
        }

        private void FirebaseAuth_AuthState(Firebase.Auth.Auth auth, User user)
        {
            if(auth?.CurrentUser!=null)
            {
                string name = auth?.CurrentUser.DisplayName;
                System.Diagnostics.Debug.WriteLine(name);
            }
            _AuthStateChange?.Invoke(this, new AuthStateEventArgs(this));
        }


    

        public async Task<string> GetIdToken()
        {
            if (FirebaseAuthentication.FirebaseAuth.CurrentUser != null)
            {
                string authToken = await FirebaseAuthentication.FirebaseAuth.CurrentUser.GetIdTokenAsync(false);

                //var tokenResult = await FirebaseAuthentication.FirebaseAuth.CurrentUser.GetIdTokenAsync(false);
                return authToken;
            }
            else
                return null;
        }

        public bool SignInWith(SignInProvider provider)
        {
#if DeviceDotNet

            provider = SignInProvider.Apple;

            if (provider == SignInProvider.Google)
            {
                Authentication.iOS.FirebaseAuthentication.GoogleSignIn();
                //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                //device.Signin(OOAdvantech.AuthProvider.Google);

                return true;
            }

            if (provider == SignInProvider.Facebook)
            {
                Authentication.iOS.FirebaseAuthentication.FacebookSignIn();
                //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                //device.Signin(OOAdvantech.AuthProvider.Facebook);

                return true;
            }

            if (provider == SignInProvider.Apple)
            {
                Authentication.iOS.FirebaseAuthentication.AppleSignIn();
                //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                //device.Signin(OOAdvantech.AuthProvider.Facebook);

                return true;
            }
            return false;
#else
            return false;
#endif
        }


        public Task<string>  EmailSignIn(string email, string password)
        {
            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //return device.EmailSignIn(email, password);
            return Authentication.iOS.FirebaseAuthentication.EmailSignIn(email, password);

        }

        public Task<string> EmailSignUp(string email, string password)
        {
            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

            //return device.EmailSignUp( email, password);


            //return null;

            return Authentication.iOS.FirebaseAuthentication.EmailSignUp(email, password);
        }
        public void SendPasswordResetEmail(string email)
        {
            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //device.SendPasswordResetEmail(email);
            Authentication.iOS.FirebaseAuthentication.SendPasswordResetEmail(email);
        }


        public void SignOut()
        {

            Authentication.iOS.FirebaseAuthentication.SignOut();
            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //device.SignOut();

            NSError error = null;
            FirebaseAuthentication.FirebaseAuth.SignOut(out error);
        }

        public IAuthUser CurrentUser
        {
            get
            {
                var googleUser = SignIn.SharedInstance.CurrentUser;

                if (FirebaseAuthentication.FirebaseAuth.CurrentUser != null)
                {
                    var firebaseUser = FirebaseAuthentication.FirebaseAuth.CurrentUser;
                    var currentUser = new OOAdvantech.Authentication.AuthUser()
                    {
                        DisplayName = firebaseUser.DisplayName,
                        Email = firebaseUser.Email,
                        IsAnonymous = firebaseUser.IsAnonymous,
                        IsEmailVerified = firebaseUser.IsEmailVerified,
                        PhoneNumber = firebaseUser.PhoneNumber,
                        PhotoUrl = firebaseUser.PhotoUrl?.ToString(),
                        ProviderId = firebaseUser.ProviderId,
                        Uid = firebaseUser.Uid,
                        //Providers = firebaseUser.Providers.ToList()
                    };

                    if (currentUser.Providers != null && currentUser.Providers.Count > 0)
                        currentUser.ProviderId = currentUser.Providers[0];
                    var cdcd = firebaseUser.ProviderData.ToList();
                    currentUser.ProviderData = firebaseUser.ProviderData.Select(x => new OOAdvantech.Authentication.UserInfo()
                    {
                        DisplayName = x.DisplayName,
                        Email = x.Email,
                        //IsEmailVerified = x..IsEmailVerified,
                        PhoneNumber = x.PhoneNumber,
                        PhotoUrl = x.PhotoUrl?.ToString(),
                        ProviderId = x.ProviderId,
                        Uid = x.Uid
                    } as IUserInfo).ToList();
                    return currentUser;


                }
                else
                {
                    //this.SignInWith(SignInProvider.Google);
                    return null;
                }
            }
        }

        public List<SignInProvider> Providers { get; }=new List<SignInProvider>() { SignInProvider.Apple,SignInProvider.Facebook,SignInProvider.Google,SignInProvider.Twitter};

        event AuthStateChangeHandler _AuthStateChange;

        public event AuthStateChangeHandler AuthStateChange
        {
            add
            {
                _AuthStateChange += value;
            }
            remove
            {
                _AuthStateChange -= value;
            }
        }
        public event IdTokenChangeHandler IdTokenChange;

    }

    /// <MetaDataID>{9e88b2f2-cdfa-4206-939d-42ada36280ed}</MetaDataID>
    class AuthUser : IAuthUser
    {
        private Firebase.Auth.User currentUser;

        public AuthUser(Firebase.Auth.User currentUser)
        {
            this.currentUser = currentUser;
        }

        public bool IsEmailVerified => currentUser.IsEmailVerified;

        public bool IsAnonymous => currentUser.IsAnonymous;

        public string Email => currentUser.Email;

        public string PhotoUrl => currentUser.PhotoUrl?.ToString();

        List<IUserInfo> _ProviderData;
        public IList<IUserInfo> ProviderData
        {
            get
            {
                if (_ProviderData == null)
                {
                    _ProviderData = currentUser.ProviderData.Select(x => new UserInfo(x) as IUserInfo).ToList();
                }
                return _ProviderData;
            }
        }


        public string ProviderId => currentUser.ProviderId;

        public IList<string> Providers => currentUser.ProviderData.Select(x=>x.ProviderId).ToList();
        public string PhoneNumber => currentUser.PhoneNumber;

        public string Uid => currentUser.Uid;

        public string DisplayName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }


    /// <MetaDataID>{de0eab48-4abb-48ae-bec6-148b0aa65158}</MetaDataID>
    class UserInfo : IUserInfo
    {
        private Firebase.Auth.IUserInfo User;

        public UserInfo(Firebase.Auth.IUserInfo user)
        {
            this.User = user;
        }

        public string DisplayName => this.User.DisplayName;

        public string Email => this.User.Email;

        public bool IsEmailVerified => false;//this.User.IsEmailVerified;

        public string PhoneNumber => this.User.PhoneNumber;

        public string PhotoUrl => this.User.PhotoUrl?.ToString();

        public string ProviderId => this.User.ProviderId;

        public string Uid => this.User.Uid;
    }
}