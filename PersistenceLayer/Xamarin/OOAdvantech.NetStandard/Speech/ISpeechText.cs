using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.Speech
{
    public delegate void SpeechRecognizedHandle(List<string> speechTexts);

    /// <MetaDataID>{b0280fb2-5056-4f3f-a311-fe7d7eec03a7}</MetaDataID>
    public interface ISpeechText
    {
        void StartSpeechToText();
        void StopSpeechToText();

        event SpeechRecognizedHandle SpeechRecognized;
    }
}
