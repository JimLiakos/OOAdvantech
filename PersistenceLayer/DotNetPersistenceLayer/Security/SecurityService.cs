namespace OOAdvantech.Security
{
    /// <MetaDataID>{8d01dd42-b81c-4c4c-8481-b3de335cb38a}</MetaDataID>
    public static class SecurityService
    {
        /// <MetaDataID>{2ab56a1d-61e6-4029-af4d-c14a7f3dc9d9}</MetaDataID>
        public static void Login()
        {

        }

        /// <MetaDataID>{f9d05f18-9d5b-48ff-9519-5ee98eca20be}</MetaDataID>
        public static void Login(string userName, string password)
        {
        }
        /// <MetaDataID>{3a0d01b0-2c1c-4667-bf22-0e2f4ccd16b7}</MetaDataID>
        public static string CurrentUser
        {
          
            get
            {
#if !DeviceDotNet
                return System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
#else
                return Xamarin.Forms.Device.OS.ToString();
#endif
            }
        }
    }
}
