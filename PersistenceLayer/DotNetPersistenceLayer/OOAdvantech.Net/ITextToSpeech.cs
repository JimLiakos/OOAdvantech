using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Speech
{
    public interface ITextToSpeech
    {
        void Speak(string text);
    }
}
