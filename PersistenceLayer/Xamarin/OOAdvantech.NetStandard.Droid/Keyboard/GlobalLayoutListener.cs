using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;

using Object = Java.Lang.Object;

namespace OOAdvantech.Droid.KeyboardService
{
    /// <MetaDataID>{c5f90273-2b29-465d-8229-475cb2256387}</MetaDataID>
    internal class GlobalLayoutListener : Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private static InputMethodManager _inputManager;
        //private readonly SoftwareKeyboardService _softwareKeyboardService;

        private static void ObtainInputManager()
        {
            //_inputManager = (InputMethodManager)TinyIoCContainer.Current.Resolve<Activity>()
            //    .GetSystemService(Context.InputMethodService);
        }

        public GlobalLayoutListener()
        {
            _inputManager = (InputMethodManager)Xamarin.Essentials.Platform.CurrentActivity.GetSystemService(Android.Content.Context.InputMethodService);
            //_softwareKeyboardService = softwareKeyboardService;
            //ObtainInputManager();
        }
        KeybordStatus keybordStatus = KeybordStatus.KeyboardHiden;
        public void OnGlobalLayout()
        {
            if (_inputManager.Handle == IntPtr.Zero)
            {
                ObtainInputManager();
            }
            try
            {
                if (_inputManager.IsAcceptingText)
                {
                    if (keybordStatus == KeybordStatus.KeyboardHiden)
                    {
                        DeviceOOAdvantechCore.KeyboordChangeState(KeybordStatus.KeyboardVisible);
                        keybordStatus = KeybordStatus.KeyboardVisible;
                    }
                    //_softwareKeyboardService.InvokeKeyboardShow(new SoftwareKeyboardEventArgs(true));
                }
                else
                {
                    if (keybordStatus != KeybordStatus.KeyboardHiden)
                    {
                        DeviceOOAdvantechCore.KeyboordChangeState(KeybordStatus.KeyboardHiden);
                        keybordStatus = KeybordStatus.KeyboardHiden;
                    }

                    //_softwareKeyboardService.InvokeKeyboardHide(new SoftwareKeyboardEventArgs(false));
                }
            }
            catch (Exception error)
            {


            }
        }
    }




}