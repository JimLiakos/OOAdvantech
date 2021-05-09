using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OOAdvantech.Droid;
using OOAdvantech.Speech;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplementation))]
namespace OOAdvantech.Droid
{
    /// <MetaDataID>{4539ea60-0600-4f12-8ec4-0a888a0c45b4}</MetaDataID>
    public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, Android.Speech.Tts.TextToSpeech.IOnInitListener
    {
        Android.Speech.Tts.TextToSpeech speaker;
        string toSpeak;


        float SpeechRate = 0.5f;

        public void Speak(string text)
        {
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new Android.Speech.Tts.TextToSpeech(Forms.Context, this);
                //Locale loc = new Locale("el_GR");
                //var ll = speaker.IsLanguageAvailable(loc);
                //speaker.SetSpeechRate(SpeechRate);
            }
            else
            {
                //SpeechRate += 0.2f;
                //speaker.SetSpeechRate(SpeechRate);
                speaker.Speak(toSpeak, Android.Speech.Tts.QueueMode.Flush, null, null);
            }
        }

        #region IOnInitListener implementation
        public void OnInit(Android.Speech.Tts.OperationResult status)
        {
            if (status.Equals(Android.Speech.Tts.OperationResult.Success))
            {
                speaker.Speak(toSpeak, Android.Speech.Tts.QueueMode.Flush, null, null);
            }
        }
        #endregion
    }
}