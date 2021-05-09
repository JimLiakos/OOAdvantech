using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using OOAdvantech.WindowsUniversal;
using Windows.Media.SpeechSynthesis;
using Xamarin.Forms;

[assembly:Dependency(typeof(TextToSpeechImplementation))]
namespace OOAdvantech.WindowsUniversal
{
    /// <MetaDataID>{96b38f16-1686-4358-8114-9c2219775cb2}</MetaDataID>
    public class TextToSpeechImplementation : OOAdvantech.Speech.ITextToSpeech
    {
        public async void Speak(string text)
        {
            var mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();
            var stream = await synth.SynthesizeTextToStreamAsync(text);

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}

