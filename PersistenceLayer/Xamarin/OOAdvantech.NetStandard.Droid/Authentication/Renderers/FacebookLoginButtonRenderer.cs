using Android.Content;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OOAdvantech.Authentication.Facebook;
using OOAdvantech.Authentication.Facebook.Droid;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace OOAdvantech.Authentication.Facebook.Droid
{
    /// <MetaDataID>{b46ded91-f0db-4042-b1ef-a06f83a6410d}</MetaDataID>
    public class FacebookLoginButtonRenderer : ViewRenderer
    {
        Context ctx;
        bool disposed;

        public FacebookLoginButtonRenderer(Context ctx) : base(ctx)
        {
            this.ctx = ctx;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            if (Control == null)
            {
                var fbLoginBtnView = e.NewElement as FacebookLoginButton;
                var fbLoginbBtnCtrl = new Xamarin.Facebook.Login.Widget.LoginButton(ctx)
                {
                    LoginBehavior = LoginBehavior.NativeWithFallback,
                };

                fbLoginbBtnCtrl.SetPermissions(fbLoginBtnView.Permissions);
                fbLoginbBtnCtrl.RegisterCallback(OOAdvantech.Authentication.Droid.FacebookLoginService.CallbackManager, new MyFacebookCallback(this.Element as FacebookLoginButton));

                SetNativeControl(fbLoginbBtnCtrl);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                if (this.Control != null)
                {
                    (this.Control as Xamarin.Facebook.Login.Widget.LoginButton).UnregisterCallback(Authentication.Droid.FacebookLoginService.CallbackManager);
                    this.Control.Dispose();
                }
                this.disposed = true;
            }
            base.Dispose(disposing);
        }

        class MyFacebookCallback : Java.Lang.Object, IFacebookCallback
        {
            FacebookLoginButton view;

            public MyFacebookCallback(FacebookLoginButton view)
            {
                this.view = view;
            }

            public void OnCancel() =>
                view.OnCancel?.Execute(null);

            public void OnError(FacebookException fbException) =>
                view.OnError?.Execute(fbException.Message);

            public void OnSuccess(Java.Lang.Object result) =>
                view.OnSuccess?.Execute(((LoginResult)result).AccessToken.Token);

        }
    }
}
