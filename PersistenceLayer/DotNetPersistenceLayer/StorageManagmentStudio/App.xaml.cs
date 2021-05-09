using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using OOAdvantech.UserInterface.Runtime;
using Microsoft.Data.ConnectionUI;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;
using System.Security.Principal;
using System.Security.Principal;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace StorageManagmentStudio
{
    public class Users
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool isMapped { get; set; }
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <MetaDataID>{dcd84091-a2eb-429b-ad31-9e6bbe22d79a}</MetaDataID>
    public partial class App : Application
    {
        /// <MetaDataID>{67463595-ce46-4750-ad36-6171c7554f45}</MetaDataID>
        protected override void OnStartup(StartupEventArgs e)
        {



            OOAdvantech.Transactions.Transaction.PreActivateDTCConnection();

            //try
            //{

            //    try
            //    {

            //        Domain dd = Domain.GetCurrentDomain();
            //        //foreach(Domain domain in Forest.GetCurrentForest().Domains)
            //        //{

            //        //}

            //        int tyt = Domain.GetCurrentDomain().DomainControllers.Count;

            //        PrincipalContext ddomainContext = new PrincipalContext(ContextType.Domain, "10.0.0.5");


            //        using (DirectoryEntry dec = new DirectoryEntry("LDAP://ArionServer"))
            //        {
            //            using (DirectorySearcher adSearch = new DirectorySearcher(dec))
            //            {
            //                adSearch.Filter = "(&(&(objectClass=user)(objectClass=person)))";
                            
            //                var resultCollection = adSearch.FindAll();

            //              //  adSearch.Filter = "(sAMAccountName=someuser)";
            //                foreach(System.DirectoryServices.SearchResult adSearchResult in adSearch.FindAll())
            //                {
            //                    var dentry = adSearchResult.GetDirectoryEntry();
                                
            //                    string dem = adSearchResult.Path;

            //                    List<string> props = new List<string>();
            //                    foreach (string prop in adSearchResult.Properties.PropertyNames)
            //                    {
            //                        props.Add(prop);
            //                    }

            //                }
            //            }
            //        }


            //        List<Users> lstADUsers = new List<Users>();
            //        string DomainPath = "LDAP://DC=xxxx,DC=com";
            //        DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
            //        DirectorySearcher search = new DirectorySearcher(searchRoot);
            //        search.Filter = "(&(objectClass=user)(objectCategory=person))";
            //        search.PropertiesToLoad.Add("samaccountname");
            //        search.PropertiesToLoad.Add("mail");
            //        search.PropertiesToLoad.Add("usergroup");
            //        search.PropertiesToLoad.Add("displayname");//first name
            //        SearchResult result;
            //        SearchResultCollection resultCol = search.FindAll();
            //        if (resultCol != null)
            //        {
            //            for (int counter = 0; counter < resultCol.Count; counter++)
            //            {
            //                string UserNameEmailString = string.Empty;
            //                result = resultCol[counter];
            //                if (result.Properties.Contains("samaccountname") &&
            //                         result.Properties.Contains("mail") &&
            //                    result.Properties.Contains("displayname"))
            //                {
            //                    Users objSurveyUsers = new Users();
            //                    objSurveyUsers.Email = (String)result.Properties["mail"][0] +
            //                      "^" + (String)result.Properties["displayname"][0];
            //                    objSurveyUsers.UserName = (String)result.Properties["samaccountname"][0];
            //                    objSurveyUsers.DisplayName = (String)result.Properties["displayname"][0];
            //                    lstADUsers.Add(objSurveyUsers);
            //                }
            //            }
            //        }


            //    }
            //    catch (Exception ex)
            //    {

            //    }


            //    PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, "10.0.0.5");

            //    //PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, Environment.UserDomainName, Environment.UserDomainName + @"\admin", "arion123");

            //    //Create a "user object" in the context
            //    UserPrincipal user = new UserPrincipal(domainContext);

            //    ////Specify the search parameters
            //    //user.Name = "ji*";

            //    //Create the searcher
            //    //pass (our) user object
            //    PrincipalSearcher pS = new PrincipalSearcher();
            //    pS.QueryFilter = user;

            //    //Perform the search
            //    PrincipalSearchResult<Principal> results = pS.FindAll();

            //    //If necessary, request more details
            //    Principal pc = results.ToList()[0];
            //    DirectoryEntry de = (DirectoryEntry)pc.GetUnderlyingObject();

            //}
            //catch (Exception error)
            //{


            //}





            //<ConnectableCtrl:ViewControlObject ViewControlObjectAssembly="StorageManagmentStudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" ViewControlObjectType="StorageManagmentStudio.StorageServersManager" Name="Connection" TransactionOption="Required" RollbackOnExitWithoutAnswer="True"></ConnectableCtrl:ViewControlObject>
            base.OnStartup(e);
        }

        public static SecurityIdentifier GetComputerSid()
        {
            return new SecurityIdentifier((byte[])new DirectoryEntry(string.Format("WinNT://{0},Computer", "Yiannis")).Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid;
        }
    }





}
