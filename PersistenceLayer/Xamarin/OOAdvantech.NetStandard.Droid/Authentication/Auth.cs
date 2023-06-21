﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Xamarin.Forms;

namespace OOAdvantech.Authentication.Droid
{
    /// <MetaDataID>{75344996-fce5-46fc-804e-4b1865a16b34}</MetaDataID>
    public class Auth : OOAdvantech.Remoting.MarshalByRefObject, IAuth, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        public Auth()
        {
            FirebaseAuthentication.FirebaseAuth.AuthState += FirebaseAuth_AuthState;
            FirebaseAuthentication.FirebaseAuth.IdToken += FirebaseAuth_IdToken;
        }

        private void FirebaseAuth_IdToken(object sender, Firebase.Auth.FirebaseAuth.IdTokenEventArgs e)
        {
            IdTokenChange?.Invoke(this, new IdTokenEventArgs(this));
        }

        private void FirebaseAuth_AuthState(object sender, Firebase.Auth.FirebaseAuth.AuthStateEventArgs e)
        {
            _AuthStateChange?.Invoke(this, new AuthStateEventArgs(this));
        }

        public async Task<string> GetIdToken()
        {
            if (FirebaseAuthentication.FirebaseAuth.CurrentUser != null)
            {
                var tokenResult = await (FirebaseAuth.Instance.CurrentUser.GetIdToken(false).AsAsync<GetTokenResult>());

                //var tokenResult = await FirebaseAuthentication.FirebaseAuth.CurrentUser.GetIdTokenAsync(false);
                return tokenResult.Token;
            }
            else
                return null;
        }

        public async System.Threading.Tasks.Task<bool> SignInWith(SignInProvider provider)
        {
#if DeviceDotNet
            if (provider == SignInProvider.Google)
            {
                var result = await FirebaseAuthentication.GoogleSignIn();
                //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                //device.Signin(OOAdvantech.AuthProvider.Google);

                return result;
            }

            if (provider == SignInProvider.Facebook)
            {

                var result = await FirebaseAuthentication.FacebookSignIn();
                //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                //device.Signin(OOAdvantech.AuthProvider.Facebook);

                return result;
            }
            return false;
#else
            return false;
#endif
        }


        public List<SignInProvider> Providers { get; } = new List<SignInProvider>() { SignInProvider.Google, SignInProvider.Facebook, SignInProvider.Twitter , SignInProvider.Email };

        public Task<string> EmailSignIn(string email, string password)
        {
            return Authentication.Droid.FirebaseAuthentication.EmailSignIn(email, password);

            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //return device.EmailSignIn(email, password);

        }

        public Task<string> EmailSignUp(string email, string password)
        {
            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

            //return device.EmailSignUp(email, password);
            return Authentication.Droid.FirebaseAuthentication.EmailSignUp(email, password);


            //return null;
        }
        public void SendPasswordResetEmail(string email)
        {
            Authentication.Droid.FirebaseAuthentication.SendPasswordResetEmail(email);

            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //device.SendPasswordResetEmail(email);
        }


        public void SignOut()
        {
            Authentication.Droid.FirebaseAuthentication.SignOut();
            FirebaseAuthentication.FirebaseAuth.SignOut();

            //OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            //device.SignOut();
        }

        public IAuthUser CurrentUser
        {
            get
            {
                if (FirebaseAuthentication.FirebaseAuth.CurrentUser != null)
                {
                    var firebaseUser = FirebaseAuthentication.FirebaseAuth.CurrentUser;
                    string providerId = firebaseUser.ProviderData?.Where(x => x.ProviderId!="firebase").Select(x => x.ProviderId).FirstOrDefault();
                    if (providerId == null)
                        providerId=firebaseUser.ProviderId;


                    var currentUser = new OOAdvantech.Authentication.AuthUser()
                    {
                        DisplayName = firebaseUser.DisplayName,
                        Email = firebaseUser.Email,
                        IsAnonymous = firebaseUser.IsAnonymous,
                        IsEmailVerified = firebaseUser.IsEmailVerified,
                        PhoneNumber = firebaseUser.PhoneNumber,
                        PhotoUrl = firebaseUser.PhotoUrl?.ToString(),
                        ProviderId = providerId,
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
                        IsEmailVerified = x.IsEmailVerified,
                        PhoneNumber = x.PhoneNumber,
                        PhotoUrl = x.PhotoUrl?.ToString(),
                        ProviderId = x.ProviderId,
                        Uid = x.Uid
                    } as IUserInfo).ToList();
                    return currentUser;


                }
                else
                    return null;
            }
        }
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
        private Firebase.Auth.FirebaseUser currentUser;

        public AuthUser(Firebase.Auth.FirebaseUser currentUser)
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

        public IList<string> Providers => currentUser.ProviderData.Select(x => x.ProviderId).ToList();
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

        public bool IsEmailVerified => this.User.IsEmailVerified;

        public string PhoneNumber => this.User.PhoneNumber;

        public string PhotoUrl => this.User.PhotoUrl?.ToString();

        public string ProviderId => this.User.ProviderId;

        public string Uid => this.User.Uid;
    }
}