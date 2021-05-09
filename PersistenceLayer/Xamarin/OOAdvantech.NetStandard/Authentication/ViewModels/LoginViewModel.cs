using System.Windows.Input;
using Xamarin.Forms;
using OOAdvantech.Authentication.Facebook.Services;

namespace OOAdvantech.Authentication.Facebook
{
    /// <MetaDataID>{3aa7ea9a-715f-44f7-817f-917eb9ec5c31}</MetaDataID>
    public class LoginViewModel
    {


        public ICommand OnFacebookLoginSuccessCmd { get; }
        public ICommand OnFacebookLoginErrorCmd { get; }
        public ICommand OnFacebookLoginCancelCmd { get; }
        public Command FacebookLogoutCmd { get; }

        public LoginViewModel()
        {
            FacebookLoginService.CurrentFacebookLoginService.AccessTokenChanged = (string oldToken, string newToken) => FacebookLogoutCmd.ChangeCanExecute();

            FacebookLogoutCmd = new Command(() =>
                FacebookLoginService.CurrentFacebookLoginService.SignOut(),
                () => true);

            OnFacebookLoginSuccessCmd = new Command<string>(
                (authToken) => DisplayAlert("Success", $"Authentication succeed: {authToken}"));

            OnFacebookLoginErrorCmd = new Command<string>(
                (err) => DisplayAlert("Error", $"Authentication failed: {err}"));

            OnFacebookLoginCancelCmd = new Command(
                () => DisplayAlert("Cancel", "Authentication cancelled by the user."));
        }

        void DisplayAlert(string title, string msg) =>
            (Application.Current).MainPage.DisplayAlert(title, msg, "OK");
    }
}
