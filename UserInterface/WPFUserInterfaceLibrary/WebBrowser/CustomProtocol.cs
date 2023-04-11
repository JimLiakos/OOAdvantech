using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenWebBrowser
{
    /// <MetaDataID>{0c184e96-aecf-4d6e-b738-7c942bcc37f9}</MetaDataID>
    public class CustomProtocolSchemeHandler : ResourceHandler
    {
        // Specifies where you bundled app resides.
        // Basically path to your index.html
        private string frontendFolderPath;

        WebBrowserOverlay WebBrowserOverlay;
        public CustomProtocolSchemeHandler(WebBrowserOverlay webBrowserOverlay)
        {
            WebBrowserOverlay = webBrowserOverlay;
            frontendFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./bundle/");
        }
         
        // Process request and craft response.
        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {



            using (callback)
            {
                var uri = new Uri(request.Url);
                string fileExtension = null;
                CustomProtocolResponse customProtocolResponse = WebBrowserOverlay.OnProcessRequest(uri);


                var fileName = uri.AbsolutePath;
                fileExtension = Path.GetExtension(fileName);

                if (customProtocolResponse.Stream != null)
                {
                    MimeType = GetMimeType(fileExtension);
                    Stream = customProtocolResponse.Stream;
                    callback.Continue();
                }
                else
                    callback.Cancel();
            }



            return CefReturnValue.ContinueAsync;
        }
    }

    /// <MetaDataID>{ce500ee4-ddaf-4a0f-8e1e-766949c36942}</MetaDataID>
    public class CustomProtocolSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "customscheme";

        public CustomProtocolSchemeHandlerFactory()
        {

        }

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new CustomProtocolSchemeHandler(WebBrowserOverlay.GetWebBrowserOverlay(browser));
        }
    }
}