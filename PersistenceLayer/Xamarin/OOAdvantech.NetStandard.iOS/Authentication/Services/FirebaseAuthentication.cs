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
        public static TaskCompletionSource<bool> GoogleCompletionSource;
        public static TaskCompletionSource<bool> FacebookCompletionSource;
        

        // private static UIViewController _viewController;

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

                    string providerId = "email";
                    if (user.ProviderData.Where(x => x.ProviderId == "facebook.com").Count() > 0)
                        providerId = "facebook.com";
                    else if (user.ProviderData.Where(x => x.ProviderId == "google.com").Count() > 0)
                        providerId = "google.com";
                    else if (user.ProviderData.Where(x => x.ProviderId == "apple.com").Count() > 0)
                        providerId = "apple.com";




                    //Remoting.RestApi.DeviceAuthentication.AuthUser
                    var authUser = new Remoting.RestApi.AuthUser()
                    {
                        AuthToken = authToken,
                        Email = FirebaseAuth.CurrentUser.Email,
                        Name = FirebaseAuth.CurrentUser.DisplayName,

                        Firebase_Sign_in_Provider = providerId,// user.Providers[FirebaseAuth.CurrentUser.Providers.Count - 1],

                        User_ID = FirebaseAuth.CurrentUser.Uid,
                        Picture = FirebaseAuth.CurrentUser.PhotoUrl?.ToString()
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

        internal static Task<bool> FacebookSignIn()
        {
            FacebookCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                FacebookLoginService.CurrentFacebookLoginService.SignIn();
                SignIn.SharedInstance.SignInUser();
                return FacebookCompletionSource.Task;
            }
            catch (Exception ex)
            {
            }
            return Task.FromResult(false);
            //Xamarin.Facebook.Login.LoginManager.Instance.LogIn(Xamarin.Essentials.Platform.CurrentActivity, new string[] { });
        }

        internal static async Task<bool> AppleSignIn()
        {
            try
            {
                AppleSignInService appleSignInService = new AppleSignInService();
                var appleAccount= await appleSignInService.SignInAsync();
                return appleAccount!=null;
                
            }
            catch (Exception ex)
            {
            }
            return false;
            //Xamarin.Facebook.Login.LoginManager.Instance.LogIn(Xamarin.Essentials.Platform.CurrentActivity, new string[] { });
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

        internal static System.Threading.Tasks.Task<bool> GoogleSignIn()
        {
            GoogleCompletionSource = new System.Threading.Tasks.TaskCompletionSource<bool>();
            try
            {
                var user = SignIn.SharedInstance.CurrentUser;
                if (UIApplication.SharedApplication != null)
                {
                    SignIn.SharedInstance.SignInUser();
                    return GoogleCompletionSource.Task;
                }
            }
            catch (Exception ex)
            {
            }
            return System.Threading.Tasks.Task<bool>.FromResult(false);


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
                if (OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser.Firebase_Sign_in_Provider.ToLower() == "emal")
                {
                    NSError error;
                    FirebaseAuth.SignOut(out error);
                }

                //FirebaseAuth.SignOut();
            }
        }

        static void OnAuthDataResult(Firebase.Auth.AuthDataResult authResult, NSError error)
        {

        }
        public static void Init(string googleAuthWebClientID)
        {

            #region Google provider initialze

            if (SignIn.SharedInstance != null)
            {
                SignIn.SharedInstance.ClientId = googleAuthWebClientID;

                var currentUser = SignIn.SharedInstance.CurrentUser;
                SignIn.SharedInstance.PresentingViewController = FirebaseAuthentication.GetTopViewController();
                Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {
                    // called every 1 second
                    if (currentUser != SignIn.SharedInstance.CurrentUser)
                    {
                        currentUser = SignIn.SharedInstance.CurrentUser;
                        if (currentUser != null)
                        {
                            if (currentUser.Authentication?.IdToken != null)
                            {
                                var credentials = Firebase.Auth.GoogleAuthProvider.GetCredential(currentUser.Authentication?.IdToken, currentUser.Authentication?.IdToken);
                                FirebaseAuth.SignInWithCredential(credentials, new Firebase.Auth.AuthDataResultHandler(OnAuthDataResult));

                                if (GoogleCompletionSource != null)
                                    GoogleCompletionSource.SetResult(true);
                            }
                        }
                    }
                    // do stuff here

                    return true; // return true to repeat counting, false to stop timer
                });
            }
            #endregion




            #region  Facebook provider initialization

            FacebookLoginService.Init();
            if (!string.IsNullOrWhiteSpace(FacebookLoginService.CurrentFacebookLoginService.AccessToken))
            {
                var credentials = Firebase.Auth.FacebookAuthProvider.GetCredential(FacebookLoginService.CurrentFacebookLoginService.AccessToken);
                FirebaseAuth.SignInWithCredential(credentials, new Firebase.Auth.AuthDataResultHandler(OnAuthDataResult));
            }

            #endregion



        }

        private static void Google_SignedIn(object sender, SignInDelegateEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Google_SignedIn");
        }

        internal static void SendPasswordResetEmail(string email)
        {
            FirebaseAuth.SendPasswordReset(email, null);
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
                    AuthDataResult authDataResult = await FirebaseAuth.SignInWithPasswordAsync(email, password);
                    return null;
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            });

        }

        internal static void FacebookSignInCompletted(bool signedIn)
        {
            if (FacebookCompletionSource != null)
                FacebookCompletionSource.SetResult(signedIn);

            FacebookCompletionSource = null;


        }
    }






}
