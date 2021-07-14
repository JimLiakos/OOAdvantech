using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using OOAdvantech.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  OOAdvantech.Droid
{
    /// <MetaDataID>{34afcc85-a6c0-49a6-ba66-eeed9aa9d7ee}</MetaDataID>
    [Activity(Label = "SpeechActivity")]
    public class SpeechActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IRecognitionListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here Pressed
        }
        public event KeyUpPressedHandler KeyUpPressed;

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var res=base.OnKeyUp(keyCode, e);
            KeyUpPressed?.Invoke(keyCode, e);
            return res;

        }
        public override bool OnKeyDown([GeneratedEnum] Android.Views.Keycode keyCode, KeyEvent e)
        {

      
            return base.OnKeyDown(keyCode, e);
        }

        SpeechRecognizer _speechRecognizer;
        private Intent speechRecognizerIntent;

        public SpeechRecognizer speechRecognizer
        {
            get
            {
                if (_speechRecognizer == null)
                {
                    _speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
                    string sse = Java.Util.Locale.English.ToString();
                    speechRecognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, Java.Util.Locale.Default);
                    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraPartialResults, true);
                    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 4000);
                    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                    speechRecognizer.SetRecognitionListener(this);

                }
                return _speechRecognizer;
            }
        }

        internal void StopLitstener()
        {
            speechRecognizer.StopListening();
        }

        internal void StartLitstener()
        {
            speechRecognizer.StartListening(speechRecognizerIntent);
        }




        public void OnBeginningOfSpeech()
        {
        }

        public void OnBufferReceived(byte[] buffer)
        {
        }

        public void OnEndOfSpeech()
        {
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
        }

        public void OnEvent(int eventType, Bundle @params)
        {
        }

        public void OnPartialResults(Bundle partialResults)
        {
            var speechTexs = partialResults.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            var ss = speechTexs.ToList().Where(x => !string.IsNullOrWhiteSpace(x));
            foreach (var s in ss)
                System.Diagnostics.Debug.WriteLine(s);

        }

        public void OnReadyForSpeech(Bundle @params)
        {
        }

        public void OnResults(Bundle results)
        {
            var speechTexs = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            var ss = speechTexs.ToList();
            SpeechRecognized?.Invoke(speechTexs.ToList());

        }

        public void OnRmsChanged(float rmsdB)
        {
        }

        public event SpeechRecognizedHandle SpeechRecognized;
    }
}