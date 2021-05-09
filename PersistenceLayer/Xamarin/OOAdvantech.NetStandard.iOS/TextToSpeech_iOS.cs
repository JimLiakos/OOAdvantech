using System;
using AVFoundation;
using OOAdvantech.iOS;
using OOAdvantech.Speech;

[assembly: Xamarin.Forms.Dependency (typeof (TextToSpeechImplementation))]
namespace OOAdvantech.iOS
{
	
	public class TextToSpeechImplementation : ITextToSpeech
	{
		public TextToSpeechImplementation (){ }

		public void Speak(string text){
			var speechSynthesizer = new AVSpeechSynthesizer ();
			var speechUtterance = new AVSpeechUtterance (text) {
				Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
				Voice = AVSpeechSynthesisVoice.FromLanguage ("en-US"),
				Volume = 0.5f,
				PitchMultiplier = 1.0f
			};

			speechSynthesizer.SpeakUtterance (speechUtterance);
		}
	}
}

