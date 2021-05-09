using System;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Firebase.Auth;
using Foundation;
using UIKit;
using System.Linq;

namespace OOAdvantech.Authentication.iOS
{

    public class FirebaseAuthentication
    {

        //internal static FirebaseAuthEvents FirebaseAuthEventsConsumer = new FirebaseAuthEvents();
        //public static void OnCanceled()
        //{
        //    Auth.DefaultInstance.CurrentUser
        //}

        //public static void OnComplete(Task task)
        //{
        //}

        //public static void OnFailure(Java.Lang.Exception e)
        //{
        //}

        //public static async void OnSuccess(Java.Lang.Object result)
        //{

        //    try
        //    {
        //        //var token = await FirebaseAuth.CurrentUser.GetIdTokenAsync(false);
        //        //string authToken= token.Token;

        //        ////Remoting.RestApi.DeviceAuthentication.AuthUser
        //        //var authUser = new Remoting.RestApi.AuthUser()
        //        //{
        //        //    AuthToken = authToken,
        //        //    Email = FirebaseAuth.CurrentUser.Email,
        //        //    Name = FirebaseAuth.CurrentUser.DisplayName,
        //        //    Firebase_Sign_in_Provider = FirebaseAuth.CurrentUser.Providers[FirebaseAuth.CurrentUser.Providers.Count - 1],
        //        //    User_ID = FirebaseAuth.CurrentUser.Uid,
        //        //    Picture = FirebaseAuth.CurrentUser.PhotoUrl.ToString()
        //        //};
        //        //Remoting.RestApi.DeviceAuthentication.SignedIn(authUser);
        //    }
        //    catch (FirebaseAuthInvalidUserException e)
        //    {
        //        // do stuff
        //    }



        //    //Remoting.RestApi.DeviceAuthentication.AuthUser=new Remoting.RestApi.AuthUser() {AuthToken=  FirebaseAuth.CurrentUser.Email}


        //}
        //static GoogleSignInOptions gso;
        //static GoogleApiClient googleApiClient;


        static Auth _FirebaseAuth;
        public static Auth FirebaseAuth
        {
            get
            {

                if (_FirebaseAuth == null)
                {
                    _FirebaseAuth = Auth.DefaultInstance;
                    _FirebaseAuth.AddAuthStateDidChangeListener(new AuthStateDidChangeListenerHandler(AuthStateDidChangeListener));

                    _FirebaseAuth.AddIdTokenDidChangeListener(new IdTokenDidChangeListenerHandler(AuthStateDidChangeListener));
                }
                return _FirebaseAuth;
            }
        }
        static async void IdTokenDidChangeListener(Auth auth, User user)
        {

        }
        static async void AuthStateDidChangeListener(Auth auth, User user)
        {
            try
            {
                if (user != null)
                {
                    string authToken = await user.GetIdTokenAsync(false);


                    //Remoting.RestApi.DeviceAuthentication.AuthUser
                    var authUser = new Remoting.RestApi.AuthUser()
                    {
                        AuthToken = authToken,
                        Email = FirebaseAuth.CurrentUser.Email,
                        Name = FirebaseAuth.CurrentUser.DisplayName,

                        Firebase_Sign_in_Provider = "facebook.com",// user.Providers[FirebaseAuth.CurrentUser.Providers.Count - 1],
                        User_ID = FirebaseAuth.CurrentUser.Uid,
                        Picture = FirebaseAuth.CurrentUser.PhotoUrl.ToString()
                    };
                    Remoting.RestApi.DeviceAuthentication.SignedIn(authUser);
                }
                else
                {
                    Remoting.RestApi.DeviceAuthentication.SignedOut();
                }
            }
            catch (Exception firebaseException)
            {
                Remoting.RestApi.DeviceAuthentication.SignedOut();
                // do stuff
            }

        }

        internal static void FacebookSignIn()
        {

            FacebookLoginService.CurrentFacebookLoginService.SignIn();


            //Xamarin.Facebook.Login.LoginManager.Instance.LogIn(Xamarin.Essentials.Platform.CurrentActivity, new string[] { });
        }



        //private static async void AuthStateChanged(object sender, FirebaseAuth.AuthStateEventArgs e)
        //{

        //    try
        //    {
        //        if (e.Auth.CurrentUser != null)
        //        {
        //            var token = await FirebaseAuth.CurrentUser.GetIdTokenAsync(false);
        //            string authToken = token.Token;

        //            //Remoting.RestApi.DeviceAuthentication.AuthUser
        //            var authUser = new Remoting.RestApi.AuthUser()
        //            {
        //                AuthToken = authToken,
        //                Email = FirebaseAuth.CurrentUser.Email,
        //                Name = FirebaseAuth.CurrentUser.DisplayName,
        //                Firebase_Sign_in_Provider = FirebaseAuth.CurrentUser.Providers[FirebaseAuth.CurrentUser.Providers.Count - 1],
        //                User_ID = FirebaseAuth.CurrentUser.Uid,
        //                Picture = FirebaseAuth.CurrentUser.PhotoUrl.ToString()
        //            };
        //            Remoting.RestApi.DeviceAuthentication.SignedIn(authUser);
        //        }
        //        else
        //        {
        //            Remoting.RestApi.DeviceAuthentication.SignedOut();
        //        }
        //    }
        //    catch (FirebaseAuthInvalidUserException firebaseException)
        //    {
        //        Remoting.RestApi.DeviceAuthentication.SignedOut();
        //        // do stuff
        //    }


        //}

        internal static void GoogleSignIn()
        {
            //Google.SignIn.SignIn.SharedInstance.SignInUser();

        }

        internal static void GoogleSignOut()
        {
            //Google.SignIn.SignIn.SharedInstance.SignOutUser();
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

        static void OnAuthDataResult(AuthDataResult authResult, NSError error)
        {

        }
        public static void Init(string googleAuthWebClientID)
        {
            //FirebaseApp.InitializeApp(firebaseOptions);
            FacebookLoginService.Init(/*FirebaseAuthEventsConsumer*/);
            if (!string.IsNullOrWhiteSpace(FacebookLoginService.CurrentFacebookLoginService.AccessToken))
            {
                var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(FacebookLoginService.CurrentFacebookLoginService.AccessToken);
                FirebaseAuth.SignInWithCredential(credentials, new AuthDataResultHandler(OnAuthDataResult));
            }




        }
        //public static bool OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        //{
        //    if (requestCode == 1)
        //    {
        //        GoogleSignInResult result = Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
        //        if (result.IsSuccess)
        //        {
        //            GoogleSignInAccount account = result.SignInAccount;
        //            LoginWithFirebase(account);
        //        }
        //    }
        //    else
        //        FacebookCallbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode), data);
        //    return true;
        //}

        //private static void LoginWithFirebase(GoogleSignInAccount account)
        //{
        //    var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
        //    FirebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(FirebaseAuthEventsConsumer)
        //        .AddOnFailureListener(FirebaseAuthEventsConsumer);
        //}




    }

    /// <summary>
    /// https://firebase.google.com/docs/auth/android/facebook-login
    /// https://firebase.google.com/docs/android/setup/
    /// https://evgenyzborovsky.com/2018/03/09/using-native-facebook-login-button-in-xamarin-forms/
    /// </summary>
    class FacebookLoginService : Facebook.Services.FacebookLoginService
    {

        public override string AccessToken => "";

        public static void Init(/*FirebaseAuthEvents firebaseAuthEvents*/)
        {

            //FacebookLoginService.CallbackManager = CallbackManagerFactory.Create();

            CurrentFacebookLoginService = new FacebookLoginService(/*firebaseAuthEvents*/);



        }
        //public static ICallbackManager CallbackManager;
        //readonly MyAccessTokenTracker myAccessTokenTracker;
        public override Action<string, string> AccessTokenChanged { get; set; }

        public FacebookLoginService(/*FirebaseAuthEvents firebaseAuthEvents*/)
        {
            NSNotificationCenter.DefaultCenter.AddObserver(
             new NSString(global::Facebook.CoreKit.AccessToken.DidChangeNotification),
             async (n) =>
             {
                 var token = global::Facebook.CoreKit.AccessToken.CurrentAccessToken.TokenString;
                 var oldToken = (n.UserInfo[global::Facebook.CoreKit.AccessToken.OldTokenKey] as global::Facebook.CoreKit.AccessToken)?.TokenString;
                 var newToken = (n.UserInfo[global::Facebook.CoreKit.AccessToken.NewTokenKey] as global::Facebook.CoreKit.AccessToken)?.TokenString;
                 if (newToken != null)
                 {
                     //var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(newToken);
                     //await FirebaseAuthentication.FirebaseAuth.SignInWithCredentialAsync(credentials);
                 }
                 AccessTokenChanged?.Invoke(oldToken,newToken);
             });

            //myAccessTokenTracker = new MyAccessTokenTracker(this, firebaseAuthEvents);
            //// TODO: Stop tracking
            //myAccessTokenTracker.StartTracking();

            ////LoginManager.Instance.SetLoginBehavior(LoginBehavior.DeviceAuth)
            //var ss = this.AccessToken;
        }



        //public override string AccessToken => Xamarin.Facebook.AccessToken.CurrentAccessToken?.Token;

        public override void SignOut()
        {
            using (var loginManager = new LoginManager())
            {
                loginManager.LogOut();
            }

        }

        public override void SignIn()
        {
            using (var loginManager = new LoginManager())
            {
                loginManager.LogIn(new string[] { }, GetTopViewController(), new global::Facebook.LoginKit.LoginManagerLoginResultBlockHandler(LoginManagerLoginResultBlock));
            }
        }

        public static UIViewController GetTopViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            if (vc is UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }


        void LoginManagerLoginResultBlock(global::Facebook.LoginKit.LoginManagerLoginResult result, NSError error)
        {

            string oldToken = global::Facebook.CoreKit.AccessToken.OldTokenKey;
            string newToken = global::Facebook.CoreKit.AccessToken.NewTokenKey;

        }

    }

    //class MyAccessTokenTracker : AccessTokenTracker
    //{
    //    readonly Facebook.Services.FacebookLoginService facebookLoginService;
    //    readonly FirebaseAuthEvents FirebaseAuthEvents;
    //    public MyAccessTokenTracker(FacebookLoginService facebookLoginService, FirebaseAuthEvents firebaseAuthEvents)
    //    {
    //        this.facebookLoginService = facebookLoginService;
    //        FirebaseAuthEvents = firebaseAuthEvents;
    //    }

    //    protected override void OnCurrentAccessTokenChanged(AccessToken oldAccessToken, AccessToken currentAccessToken)
    //    {

    //        if (!string.IsNullOrWhiteSpace(FacebookLoginService.CurrentFacebookLoginService.AccessToken))
    //        {
    //            var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(FacebookLoginService.CurrentFacebookLoginService.AccessToken);
    //            FirebaseAuthentication.FirebaseAuth.SignInWithCredential(credentials)
    //                .AddOnCompleteListener(FirebaseAuthEvents)
    //                .AddOnSuccessListener(FirebaseAuthEvents)
    //                .AddOnFailureListener(FirebaseAuthEvents)
    //                .AddOnCanceledListener(FirebaseAuthEvents);
    //        }

    //        if (oldAccessToken != null && currentAccessToken == null)
    //            FirebaseAuthentication.FirebaseAuth.SignOut();


    //        facebookLoginService.AccessTokenChanged?.Invoke(oldAccessToken?.Token, currentAccessToken?.Token);
    //    }
    //}

    //class FirebaseAuthEvents : Java.Lang.Object, IOnSuccessListener, IOnCompleteListener, IOnFailureListener, IOnCanceledListener
    //{
    //    public void OnCanceled()
    //    {
    //        FirebaseAuthentication.OnCanceled();
    //    }

    //    public void OnComplete(Task task)
    //    {
    //        FirebaseAuthentication.OnComplete(task);
    //    }

    //    public void OnFailure(Java.Lang.Exception e)
    //    {
    //        FirebaseAuthentication.OnFailure(e);
    //    }

    //    public void OnSuccess(Java.Lang.Object result)
    //    {
    //        FirebaseAuthentication.OnSuccess(result);
    //    }
    //}



}
