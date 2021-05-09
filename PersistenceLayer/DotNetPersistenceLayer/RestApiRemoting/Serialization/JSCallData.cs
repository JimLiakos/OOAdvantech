using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;

namespace OOAdvantech.Remoting.RestApi.EmbeddedBrowser
{
    /// <summary>
    /// Asynchronous java script call data
    /// used for calls from web browser controls
    /// </summary>
    /// <MetaDataID>{a8ce6c3d-8105-445e-84e4-5605b8b22069}</MetaDataID>
    public class JSCallData
    {
        /// <summary>
        /// Defines the Identity of call from web browser controls.
        /// </summary>
        public int CallID { get; set; }

        /// <summary>
        /// Defines the args of java script call web browser controls.
        /// </summary>
        public string Args { get; set; }


        /// <summary>
        /// Check  serialized call string for async call.
        /// </summary>
        /// <param name="serializedCall"></param>
        /// <returns></returns>
        public static bool IsAsyncCall(string serializedCall)
        {
            int nPos = serializedCall.IndexOf("\"CallID\":");
            if (nPos >= 0 && nPos < 7)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Desirialize async call data
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// Returns java script call object 
        /// </returns>
        public static JSCallData GetJSCallData(string value)
        {
            return JsonConvert.DeserializeObject<JSCallData>(value);
        }
    }



}
