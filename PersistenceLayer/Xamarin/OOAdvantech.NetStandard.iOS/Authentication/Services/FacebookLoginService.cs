using System;
using Facebook.CoreKit;
using Facebook.LoginKit;

using Foundation;
using UIKit;
using System.Linq;
using Firebase.Auth;
using System.Threading.Tasks;
using Google.SignIn;
using Xamarin.Forms;

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


        static Firebase.Auth.Auth _FirebaseAuth;
        private static UIViewController _viewController;

        public static Firebase.Auth.Auth FirebaseAuth
        {
            get
            {

                if (_FirebaseAuth == null)
                {
                    
                    _FirebaseAuth = Firebase.Auth.Auth.DefaultInstance;
                    if (_FirebaseAuth != null)
                    {
                        _FirebaseAuth.AddAuthStateDidChangeListener(new Firebase.Auth.AuthStateDidChangeListenerHandler(AuthStateDidChangeListener));

                        _FirebaseAuth.AddIdTokenDidChangeListener(new Firebase.Auth.IdTokenDidChangeListenerHandler(AuthStateDidChangeListener));
                    }
                }
                return _FirebaseAuth;
            }
        }
        static async void IdTokenDidChangeListener(Firebase.Auth.Auth auth, Firebase.Auth.User user)
        {

        }
        static async void AuthStateDidChangeListener(Firebase.Auth.Auth auth, Firebase.Auth.User user)
        {
            try
            {
                if (user != null)
                {
                    string authToken = await user.GetIdTokenAsync(false);

                    string providerId = null;
                    if (user.ProviderData.Where(x => x.ProviderId == "facebook.com").Count() > 0)
                        providerId = "facebook.com";
                    else if (user.ProviderData.Where(x => x.ProviderId == "google.com").Count() > 0)
                        providerId = "google.com";


                    //Remoting.RestApi.DeviceAuthentication.AuthUser
                    var authUser = new Remoting.RestApi.AuthUser()
                    {
                        AuthToken = authToken,
                        Email = FirebaseAuth.CurrentUser.Email,
                        Name = FirebaseAuth.CurrentUser.DisplayName,

                        Firebase_Sign_in_Provider = providerId,// user.Providers[FirebaseAuth.CurrentUser.Providers.Count - 1],

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
            try
            {
                FacebookLoginService.CurrentFacebookLoginService.SignIn();
            }
            catch (Exception ex)
            {

            }
            


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
            try
            {
                var user = SignIn.SharedInstance.CurrentUser;
                
                if (UIApplication.SharedApplication != null)
                {
                   
                 
                    SignIn.SharedInstance.SignInUser();
                }

                
                
            }
            catch (Exception ex)
            {
               
            }

        }

        private static void DidSignIn(object value, GoogleUser user, NSError error)
        {
            
        }

        internal static void GoogleSignOut()
        {
            try
            {

                Google.SignIn.SignIn.SharedInstance.SignOutUser();
            }
            catch (Exception ex)
            {

            }
        }

        internal static void SignOut()
        {


            if (FirebaseAuth != null && FirebaseAuth.CurrentUser != null)
            {
                if (OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser.Firebase_Sign_in_Provider.ToLower() == "google.com")
                    GoogleSignOut();

                if (OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser.Firebase_Sign_in_Provider.ToLower() == "facebook.com")
                {
                    try
                    {

                        FacebookLoginService.CurrentFacebookLoginService.SignOut();
                    }
                    catch (Exception ex)
                    {

                    }

                }


                //FirebaseAuth.SignOut();
            }
        }

        static void OnAuthDataResult(Firebase.Auth.AuthDataResult authResult, NSError error)
        {

        }
        public static void Init(string googleAuthWebClientID)
        {
            if (SignIn.SharedInstance != null)
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }
                _viewController = vc;
                var currentUser = SignIn.SharedInstance.CurrentUser;
                SignIn.SharedInstance.PresentingViewController = _viewController;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    // called every 1 second
                    if (currentUser!= SignIn.SharedInstance.CurrentUser)
                    {
                        currentUser = SignIn.SharedInstance.CurrentUser;
                        if (currentUser != null)
                        {
                            if(currentUser.Authentication?.IdToken!=null)
                            {
                                var credentials = Firebase.Auth.GoogleAuthProvider.GetCredential(currentUser.Authentication?.IdToken, currentUser.Authentication?.IdToken);
                                FirebaseAuth.SignInWithCredential(credentials, new Firebase.Auth.AuthDataResultHandler(OnAuthDataResult));

                            }
                        }

                    }
                    
                    // do stuff here

                    return true; // return true to repeat counting, false to stop timer
                });
            }
            


            SignIn.SharedInstance.ClientId = googleAuthWebClientID;
            //FirebaseApp.InitializeApp(firebaseOptions);
            FacebookLoginService.Init(/*FirebaseAuthEventsConsumer*/);
            if (!string.IsNullOrWhiteSpace(FacebookLoginService.CurrentFacebookLoginService.AccessToken))
            {
                var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(FacebookLoginService.CurrentFacebookLoginService.AccessToken);
                FirebaseAuth.SignInWithCredential(credentials, new Firebase.Auth.AuthDataResultHandler(OnAuthDataResult));
            }

            


        }

        private static void Google_SignedIn(object sender, SignInDelegateEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Google_SignedIn");
        }

        internal static void SendPasswordResetEmail(string email)
        {
            FirebaseAuth.SendPasswordReset(email,null);
        }

        internal static Task<string> EmailSignUp(string email, string password)
        {
            return System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    await FirebaseAuth.CreateUserAsync(email, password);
                    return null;
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            });
        }

        internal static Task<string> EmailSignIn(string email, string password)
        {
            return System.Threading.Tasks.Task<string>.Run(async () =>
            {
                try
                {
                    await FirebaseAuth.SignInWithPasswordAsync(email, password);
                    return null;
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            });

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
                 string token = null;
                 if(global::Facebook.CoreKit.AccessToken.CurrentAccessToken!=null)
                  token= global::Facebook.CoreKit.AccessToken.CurrentAccessToken.TokenString;

                 var oldToken = (n.UserInfo[global::Facebook.CoreKit.AccessToken.OldTokenKey] as global::Facebook.CoreKit.AccessToken)?.TokenString;
                 var newToken = (n.UserInfo[global::Facebook.CoreKit.AccessToken.NewTokenKey] as global::Facebook.CoreKit.AccessToken)?.TokenString;
                 if (newToken != null)
                 {
                     //var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(newToken);
                     //await FirebaseAuthentication.FirebaseAuth.SignInWithCredentialAsync(credentials);
                 }
                 if (token != null)
                 {
                     AuthCredential credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(token);
                     if (credentials != null)
                     {
                         FirebaseAuthentication.FirebaseAuth.SignInWithCredential(credentials, null);
                     }
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
                try
                {
                    string token = null;
                    var to = global::Facebook.CoreKit.AccessToken.CurrentAccessToken;
                    if (to != null)
                    {
                        token = to.TokenString;
                    }
                    loginManager.LogOut();

                    to = global::Facebook.CoreKit.AccessToken.CurrentAccessToken;
                   
                    if(to!=null)
                    {
                        token = to.TokenString;
                    }
                }
                catch (Exception ex)
                {

                }
                
            }

        }

        public override void SignIn()
        {
            using (var loginManager = new LoginManager())
            {
                try
                {
                    loginManager.LogIn(new string[] { }, GetTopViewController(), new global::Facebook.LoginKit.LoginManagerLoginResultBlockHandler(LoginManagerLoginResultBlock));
                }
                catch (Exception ex)
                {

                }
                
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

    public class SignInDelegate : Google.SignIn.SignInDelegate
    {
        public SignInDelegate()
        {

        }
        public override void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            
        }
        public override void DidDisconnect(SignIn signIn, GoogleUser user, NSError error)
        {
            base.DidDisconnect(signIn, user, error);
        }
    }


}
