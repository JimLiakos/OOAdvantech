using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using OOAdvantech.UserInterface.Runtime;

namespace StorageManagmentStudio
{
    public class NewUserViewModel: PresentationObject<OOAdvantech.Security.User>
    {
        public NewUserViewModel(OOAdvantech.Security.User user)
            : base(user)
        {

            PrincipalContext domainContext = new PrincipalContext(ContextType.Machine, Environment.MachineName);
            
            _PrincipalContexts.Add(domainContext);
            CurrentPrincipalContext = domainContext;
          
        }
        public String UserName
        {
            get
            {
                return RealObject.Name;
            }
            set
            {
                RealObject.Name = value;
            }
        }



        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        bool _IsWindowsAythentication=true;
        public bool IsWindowsAythentication
        {
            get
            {
                return _IsWindowsAythentication;
            }
            set
            {
                _IsWindowsAythentication = value;
                if (ObjectChangeState != null)
                    ObjectChangeState(this, null);

            }
        }

        public PrincipalContext CurrentPrincipalContext
        {
            get;
            set;
        }
        List<PrincipalContext> _PrincipalContexts=new List<PrincipalContext>();
        public List<PrincipalContext> PrincipalContexts
        {
            get
            {
                return _PrincipalContexts;
            }

        }

        UserPrincipal _SelectedWindowsUser;
        public UserPrincipal SelectedWindowsUser
        {
            get
            {
                return _SelectedWindowsUser;
            }
            set
            {
                _SelectedWindowsUser = value;

                //System.Security.Principal.SecurityIdentifier(sid)
                
                    
                if (_SelectedWindowsUser != null)
                {
                    System.Diagnostics.Debug.WriteLine(OOAdvantech.Transactions.Transaction.Current.LocalTransactionUri);
                    //UserPrincipal user =
                    //UserPrincipal.FindByIdentity(CurrentPrincipalContext,
                    //           IdentityType.Sid, (_SelectedWindowsUser.Sid.ToString()));

                    RealObject.Name = _SelectedWindowsUser.Name;
                    RealObject.Sid = _SelectedWindowsUser.Sid.ToString(); ;
                    if (ObjectChangeState != null)
                        ObjectChangeState(this, "UserName");

                }
            }

        }

        public List<UserPrincipal> Users
        {
            get
            {
                //Create a "user object" in the context
                UserPrincipal user = new UserPrincipal(CurrentPrincipalContext);

                ////Specify the search parameters
                //user.Name = "ji*";

                //Create the searcher
                //pass (our) user object
                PrincipalSearcher pS = new PrincipalSearcher();
                pS.QueryFilter = user;

                //Perform the search
                PrincipalSearchResult<Principal> results = pS.FindAll();
                return results.OfType<UserPrincipal>().ToList();
                //If necessary, request more details
                //Principal pc = results.ToList()[0];
            }
        }


        public bool IsStorageServerAythentication
        {
            get
            {
                return !_IsWindowsAythentication;
            }

            set
            {
                _IsWindowsAythentication = !value;

                if(ObjectChangeState!=null)
                    ObjectChangeState(this, null);
            }
        }


    }
}
