﻿using System;
using System.Linq;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Tasks;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Java.Interop;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace OOAdvantech.Authentication.Droid
{

    public class FirebaseAuthentication
    {
        static FirebaseAuthentication()
        {
            if (Xamarin.Forms.Application.Current is IAppLifeTime)
                (Xamarin.Forms.Application.Current as IAppLifeTime).ApplicationResuming += FirebaseAuthentication_ApplicationResuming;
        }



        internal static FirebaseAuthEvents FirebaseAuthEventsConsumer = new FirebaseAuthEvents();
        public static void OnCanceled()
        {
        }

        public static void OnComplete(Task task)
        {
        }

        public static void OnFailure(Java.Lang.Exception e)
        {
        }

        public static async void OnSuccess(Java.Lang.Object result)
        {

            try
            {
                await FirebaseUserSignedIn();

                //var token = await FirebaseAuth.CurrentUser.GetIdTokenAsync(false);
                //string authToken= token.Token;

                ////Remoting.RestApi.DeviceAuthentication.AuthUser
                //var authUser = new Remoting.RestApi.AuthUser()
                //{
                //    AuthToken = authToken,
                //    Email = FirebaseAuth.CurrentUser.Email,
                //    Name = FirebaseAuth.CurrentUser.DisplayName,
                //    Firebase_Sign_in_Provider = FirebaseAuth.CurrentUser.Providers[FirebaseAuth.CurrentUser.Providers.Count - 1],
                //    User_ID = FirebaseAuth.CurrentUser.Uid,
                //    Picture = FirebaseAuth.CurrentUser.PhotoUrl.ToString()
                //};
                //Remoting.RestApi.DeviceAuthentication.SignedIn(authUser);
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                // do stuff
            }



            //Remoting.RestApi.DeviceAuthentication.AuthUser=new Remoting.RestApi.AuthUser() {AuthToken=  FirebaseAuth.CurrentUser.Email}


        }

        private static System.Threading.Timer Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        static GoogleSignInOptions gso;
        static GoogleApiClient googleApiClient;


        static FirebaseUser CurrentUser;

        static GetTokenResult CurrenTokenResult;



        static FirebaseAuth _FirebaseAuth;
        public static FirebaseAuth FirebaseAuth
        {
            get
            {
                if (_FirebaseAuth == null)
                {
                    _FirebaseAuth = FirebaseAuth.Instance;
                    _FirebaseAuth.AuthState += AuthStateChanged;
                    _FirebaseAuth.IdToken += IdTokenChanged;
                }
                return FirebaseAuth.Instance;
            }
        }



        internal static void FacebookSignIn()
        {

            Xamarin.Facebook.Login.LoginManager.Instance.LogIn(Xamarin.Essentials.Platform.CurrentActivity, new string[] { });
        }


        static DateTime authTimestamp;
        static DateTime issuedAtTimestamp;
        static DateTime expirationTimestamp;
        static string idToken;

        private static async void IdTokenChanged(object sender, FirebaseAuth.IdTokenEventArgs e)
        {
            if (FirebaseAuth.CurrentUser != null)
            {
                var token = await FirebaseAuth.CurrentUser.GetIdTokenAsync(false);

                authTimestamp = OOAdvantech.Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.AuthTimestamp);
                issuedAtTimestamp = OOAdvantech.Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.IssuedAtTimestamp);
                expirationTimestamp = OOAdvantech.Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.ExpirationTimestamp);
                idToken = token.Token;

                var firebaseUser = FirebaseAuth.CurrentUser;
                lock (AuthenticationTokenLock)
                {
                    CurrenTokenResult = token;
                }

                var authUser = new OOAdvantech.Authentication.AuthUser()
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

                Remoting.RestApi.DeviceAuthentication.Current.AuthIDTokenChanged(idToken, expirationTimestamp, authUser);
            }
            else
            {
                Remoting.RestApi.DeviceAuthentication.Current.AuthIDTokenChanged(null, expirationTimestamp, null);
            }
        }
        private static async void FirebaseAuthentication_ApplicationResuming(object sender, EventArgs e)
        {
            ValidateAuthenticationToken();
        }
        static async void OnTimer(object state)
        {
             ValidateAuthenticationToken();
        }

        static object AuthenticationTokenLock = new object();
        private static void ValidateAuthenticationToken()
        {
            lock (AuthenticationTokenLock)
            {
                if (CurrentUser != null && CurrenTokenResult != null)
                {
                    bool newToken = false;
                    if (expirationTimestamp - DateTime.UtcNow < TimeSpan.FromMinutes(5))
                    {

                        var tokenTask = FirebaseAuth.CurrentUser.GetIdTokenAsync(true);
                        if (tokenTask.Wait(TimeSpan.FromSeconds(30)))
                        {
                            var token = tokenTask.Result;
                            newToken = CurrenTokenResult.Token != token.Token;
                            CurrenTokenResult = token;
                            authTimestamp = Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.AuthTimestamp);
                            issuedAtTimestamp = Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.IssuedAtTimestamp);
                            expirationTimestamp = Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.ExpirationTimestamp);
                        }
                        //string authToken = token.Token;
                        //var firebaseUser = FirebaseAuth.CurrentUser;

                        ////Remoting.RestApi.DeviceAuthentication.AuthUser
                        //var authUser = new OOAdvantech.Authentication.AuthUser()
                        //{
                        //    DisplayName = firebaseUser.DisplayName,
                        //    Email = firebaseUser.Email,
                        //    IsAnonymous = firebaseUser.IsAnonymous,
                        //    IsEmailVerified = firebaseUser.IsEmailVerified,
                        //    PhoneNumber = firebaseUser.PhoneNumber,
                        //    PhotoUrl = firebaseUser.PhotoUrl.ToString(),
                        //    ProviderId = firebaseUser.ProviderId,
                        //    Uid = firebaseUser.Uid,
                        //    Providers = firebaseUser.Providers.ToList()
                        //};
                        //Remoting.RestApi.DeviceAuthentication.Current.AuthIDTokenChanged(idToken, expirationTimestamp, authUser);

                    }
                }
            }

        }



        private static async void AuthStateChanged(object sender, FirebaseAuth.AuthStateEventArgs e)
        {
            try
            {
                if (e.Auth.CurrentUser != null)
                {
                    await FirebaseUserSignedIn();
                }
                else
                {
                    CurrentUser = null;
                    CurrenTokenResult = null;
                    Remoting.RestApi.DeviceAuthentication.SignedOut();
                }
            }
            catch (FirebaseAuthInvalidUserException firebaseException)
            {
                Remoting.RestApi.DeviceAuthentication.SignedOut();
                // do stuff
            }

        }

        private static async System.Threading.Tasks.Task FirebaseUserSignedIn()
        {
            var token = await FirebaseAuth.CurrentUser.GetIdTokenAsync(false);
            lock (AuthenticationTokenLock)
            {
                CurrentUser = FirebaseAuth.CurrentUser;
                CurrenTokenResult = token;
                authTimestamp = Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.AuthTimestamp);
                issuedAtTimestamp = Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.IssuedAtTimestamp);
                expirationTimestamp = Remoting.RestApi.DeviceAuthentication.FromUnixTime(token.ExpirationTimestamp);
            }
            string authToken = token.Token;

            var authUser = new Remoting.RestApi.AuthUser()
            {
                AuthToken = authToken,
                ExpirationTime = expirationTimestamp,
                Email = FirebaseAuth.CurrentUser.Email,
                Name = FirebaseAuth.CurrentUser.DisplayName,
                Firebase_Sign_in_Provider = FirebaseAuth.CurrentUser.Providers[FirebaseAuth.CurrentUser.Providers.Count - 1],
                User_ID = FirebaseAuth.CurrentUser.Uid,
                Picture = FirebaseAuth.CurrentUser.PhotoUrl.ToString()
            };
            Remoting.RestApi.DeviceAuthentication.SignedIn(authUser);
        }

        internal static void GoogleSignIn()
        {
            var intent = Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            Xamarin.Essentials.Platform.CurrentActivity.StartActivityForResult(intent, 1);
        }

        internal static void GoogleSignOut()
        {
            Android.Gms.Auth.Api.Auth.GoogleSignInApi.SignOut(googleApiClient);
            FirebaseAuth.SignOut();
        }

        internal static void SignOut()
        {


            if (FirebaseAuth != null && FirebaseAuth.CurrentUser != null)
            {
                if (OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser.Firebase_Sign_in_Provider.ToLower() == "google.com")
                    GoogleSignOut();

                if (OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser.Firebase_Sign_in_Provider.ToLower() == "facebook.com")
                {
                    FacebookLoginService.CurrentFacebookLoginService.SignOut();

                }


                //FirebaseAuth.SignOut();
            }
        }

        public static void Init(Context context, string googleAuthWebClientID)
        {
            //FirebaseApp.InitializeApp(firebaseOptions);
            FacebookLoginService.Init(FirebaseAuthEventsConsumer);
            if (!string.IsNullOrWhiteSpace(FacebookLoginService.CurrentFacebookLoginService.AccessToken))
            {
                var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(FacebookLoginService.CurrentFacebookLoginService.AccessToken);

                FirebaseAuth.SignInWithCredential(credentials)
                    .AddOnCompleteListener(FirebaseAuthEventsConsumer)
                    .AddOnSuccessListener(FirebaseAuthEventsConsumer)
                    .AddOnFailureListener(FirebaseAuthEventsConsumer)
                    .AddOnCanceledListener(FirebaseAuthEventsConsumer);

            }



            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestIdToken(googleAuthWebClientID).RequestEmail().Build();
            googleApiClient = new GoogleApiClient.Builder(context).AddApi(Android.Gms.Auth.Api.Auth.GOOGLE_SIGN_IN_API, gso).Build();

            googleApiClient.Connect();
        }
        public static bool OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            if (requestCode == 1)
            {
                GoogleSignInResult result = Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;
                    LoginWithFirebase(account);
                }
            }
            else
                FacebookCallbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode), data);
            return true;
        }

        private static void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            FirebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(FirebaseAuthEventsConsumer)
                .AddOnFailureListener(FirebaseAuthEventsConsumer);

        }

        public static ICallbackManager FacebookCallbackManager
        {
            get
            {
                return FacebookLoginService.CallbackManager;
            }
        }


    }

    /// <summary>
    /// https://firebase.google.com/docs/auth/android/facebook-login
    /// https://firebase.google.com/docs/android/setup/
    /// https://evgenyzborovsky.com/2018/03/09/using-native-facebook-login-button-in-xamarin-forms/
    /// </summary>
    class FacebookLoginService : Facebook.Services.FacebookLoginService
    {

        public static void Init(FirebaseAuthEvents firebaseAuthEvents)
        {

            FacebookLoginService.CallbackManager = CallbackManagerFactory.Create();

            CurrentFacebookLoginService = new FacebookLoginService(firebaseAuthEvents);



        }
        public static ICallbackManager CallbackManager;
        readonly MyAccessTokenTracker myAccessTokenTracker;
        public override Action<string, string> AccessTokenChanged { get; set; }

        public FacebookLoginService(FirebaseAuthEvents firebaseAuthEvents)
        {
            myAccessTokenTracker = new MyAccessTokenTracker(this, firebaseAuthEvents);
            // TODO: Stop tracking
            myAccessTokenTracker.StartTracking();

            //LoginManager.Instance.SetLoginBehavior(LoginBehavior.DeviceAuth)
            var ss = this.AccessToken;
        }



        public override string AccessToken => Xamarin.Facebook.AccessToken.CurrentAccessToken?.Token;

        public override void SignOut()
        {

            LoginManager.Instance.LogOut();
        }

        public override void SignIn()
        {
            throw new NotImplementedException();
        }
    }

    class MyAccessTokenTracker : AccessTokenTracker
    {
        readonly Facebook.Services.FacebookLoginService facebookLoginService;
        readonly FirebaseAuthEvents FirebaseAuthEvents;
        public MyAccessTokenTracker(FacebookLoginService facebookLoginService, FirebaseAuthEvents firebaseAuthEvents)
        {
            this.facebookLoginService = facebookLoginService;
            FirebaseAuthEvents = firebaseAuthEvents;
        }

        protected override void OnCurrentAccessTokenChanged(AccessToken oldAccessToken, AccessToken currentAccessToken)
        {

            if (!string.IsNullOrWhiteSpace(FacebookLoginService.CurrentFacebookLoginService.AccessToken))
            {
                var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(FacebookLoginService.CurrentFacebookLoginService.AccessToken);
                FirebaseAuthentication.FirebaseAuth.SignInWithCredential(credentials)
                    .AddOnCompleteListener(FirebaseAuthEvents)
                    .AddOnSuccessListener(FirebaseAuthEvents)
                    .AddOnFailureListener(FirebaseAuthEvents)
                    .AddOnCanceledListener(FirebaseAuthEvents);
            }

            if (oldAccessToken != null && currentAccessToken == null)
                FirebaseAuthentication.FirebaseAuth.SignOut();


            facebookLoginService.AccessTokenChanged?.Invoke(oldAccessToken?.Token, currentAccessToken?.Token);
        }
    }

    class FirebaseAuthEvents : Java.Lang.Object, IOnSuccessListener, IOnCompleteListener, IOnFailureListener, IOnCanceledListener
    {
        public void OnCanceled()
        {
            FirebaseAuthentication.OnCanceled();
        }

        public void OnComplete(Task task)
        {
            FirebaseAuthentication.OnComplete(task);
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            FirebaseAuthentication.OnFailure(e);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            FirebaseAuthentication.OnSuccess(result);
        }
    }


}