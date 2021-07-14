using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OOAdvantech.Droid;
using OOAdvantech.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextImplementation))]

namespace OOAdvantech.Droid
{

    /// <MetaDataID>{21b7ab9f-e1a9-4dba-bfce-4c9eeb40571b}</MetaDataID>
    public class SpeechToTextImplementation : ISpeechText
    {




        public event SpeechRecognizedHandle SpeechRecognized;
        /// <MetaDataID>{c16d862e-8df8-470e-9e98-f50b8e266d2b}</MetaDataID>
        public SpeechToTextImplementation()
        {
            MainActivity = Xamarin.Forms.Forms.Context as SpeechActivity;
            MainActivity.SpeechRecognized += MainActivity_SpeechRecognized;
            MainActivity.KeyUpPressed += MainActivity_KeyUpPressed;
        }

        /// <MetaDataID>{a461816d-093c-4b03-a703-b3bb4bffa564}</MetaDataID>
        private bool MainActivity_KeyUpPressed([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.VolumeUp)
                this.StartSpeechToText();
            return true;
        }

        /// <MetaDataID>{0c6ea934-d9f5-4e7b-be10-f11f08ff6820}</MetaDataID>
        private void MainActivity_SpeechRecognized(List<string> speechTexts)
        {
            start = !start;
            SpeechRecognized?.Invoke(speechTexts);

        }

        /// <MetaDataID>{4b5ffc2c-013f-4418-b379-c769fe162afe}</MetaDataID>
        public SpeechActivity MainActivity { get; }

        /// <MetaDataID>{89cc073e-4faf-49b0-b4ef-c4f868a48dd4}</MetaDataID>
        bool start;
        /// <MetaDataID>{7abc832e-f3fa-484c-a9ef-7319029b9170}</MetaDataID>
        public void StartSpeechToText()
        {
            start = !start;
            if (start)
                MainActivity.StartLitstener();
            else
                MainActivity.StopLitstener();
        }

        /// <MetaDataID>{56faa985-3e5f-4b19-9963-adf8d4aefe22}</MetaDataID>
        public void StopSpeechToText()
        {
        }

    }
    public delegate bool KeyUpPressedHandler([GeneratedEnum] Android.Views.Keycode keyCode, KeyEvent e);
}