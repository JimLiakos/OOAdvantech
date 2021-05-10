﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace OOAdvantech.Authentication.Droid
{
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
                var er = await FirebaseAuthentication.FirebaseAuth.CurrentUser.GetIdTokenAsync(false);
                return er.Token;
            }
            else
                return null;
        }

        public bool SignInWith(SignInProvider provider)
        {
#if DeviceDotNet
            if (provider == SignInProvider.Google)
            {
                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                device.Signin(OOAdvantech.AuthProvider.Google);

                return true;
            }

            if (provider == SignInProvider.Facebook)
            {
                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                device.Signin(OOAdvantech.AuthProvider.Facebook);

                return true;
            }
            return false;
#else
            return false;
#endif
        }

        public void SignOut()
        {
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.SignOut();
        }

        public IAuthUser CurrentUser
        {
            get
            {
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
                        PhotoUrl = firebaseUser.PhotoUrl.ToString(),
                        ProviderId = firebaseUser.ProviderId,
                        Uid = firebaseUser.Uid,
                        Providers = firebaseUser.Providers.ToList()
                    };

                    currentUser.ProviderId = currentUser.Providers[0];
                    var cdcd = firebaseUser.ProviderData.ToList();
                    currentUser.ProviderData = firebaseUser.ProviderData.Select(x => new OOAdvantech.Authentication.UserInfo()
                    {
                        DisplayName = x.DisplayName,
                        Email = x.Email,
                        IsEmailVerified = x.IsEmailVerified,
                        PhoneNumber = x.PhoneNumber,
                        PhotoUrl = x.PhotoUrl.ToString(),
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

        public string PhotoUrl => currentUser.PhotoUrl.ToString();

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

        public IList<string> Providers => currentUser.Providers;

        public string PhoneNumber => currentUser.PhoneNumber;

        public string Uid => currentUser.Uid;

        public string DisplayName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }


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

        public string PhotoUrl => this.User.PhotoUrl.ToString();

        public string ProviderId => this.User.ProviderId;

        public string Uid => this.User.Uid;
    }
}