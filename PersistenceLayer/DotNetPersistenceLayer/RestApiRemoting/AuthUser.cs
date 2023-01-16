using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{72f73b6d-a2e3-48cf-858e-fe6dfe67c782}</MetaDataID>
    public class AuthUser
    {
        public AuthUser()
        {
            //123
        }

        /// <summary>
        ///  The "iss" (issuer) claim identifies the principal that issued the
        ///  JWT.  The processing of this claim is generally application specific.
        ///  The "iss" value is a case-sensitive string containing a StringOrURI
        //   value.  Use of this claim is OPTIONAL.
        /// </summary>
        public string Iss { get; set; }
        /// <MetaDataID>{f58fee1b-7b4a-452a-a841-a971a76f54ac}</MetaDataID>
        public string Name { get; set; }

        /// <MetaDataID>{ae90bc8a-8a94-4712-a6de-f86231fa5a91}</MetaDataID>
        public string Picture { get; set; }

        /// <summary>
        ///  The "aud" (audience) claim identifies the recipients that the JWT is
        ///  intended for.  Each principal intended to process the JWT MUST
        ///  identify itself with a value in the audience claim.If the principal
        ///  processing the claim does not identify itself with a value in the
        ///  "aud" claim when this claim is present, then the JWT MUST be
        ///  rejected.In the general case, the "aud" value is an array of case-
        ///  sensitive strings, each containing a StringOrURI value.In the
        ///  special case when the JWT has one audience, the "aud" value MAY be a
        ///  single case-sensitive string containing a StringOrURI value.The
        ///  interpretation of audience values is generally application specific.
        ///  Use of this claim is OPTIONAL.
        /// </summary>
        /// <MetaDataID>{556af901-16a3-4f47-9fa7-1071fb7bdd76}</MetaDataID>
        public string Audience { get; set; }

        /// <summary>
        /// Time when the End-User authentication occurred
        /// </summary>
        /// <MetaDataID>{5a877566-471c-4b73-8afd-5320e5358c98}</MetaDataID>
        public DateTime Auth_Time { get; set; }

        /// <MetaDataID>{4678d405-aa61-47a7-bd0a-efad761c74c8}</MetaDataID>
        public string User_ID { get; set; }

        /// <summary>
        /// The "sub" (subject) claim identifies the principal that is the
        /// subject of the JWT.The claims in a JWT are normally statements
        /// about the subject.The subject value MUST either be scoped to be
        /// locally unique in the context of the issuer or be globally unique.
        /// The processing of this claim is generally application specific.The
        /// "sub" value is a case-sensitive string containing a StringOrURI
        /// value.Use of this claim is OPTIONAL.
        ///  </summary>
        /// <MetaDataID>{ef411975-2d77-4c88-bbf1-22c1a7ab4afc}</MetaDataID>
        public string Subject { get; set; }

        /// <summary>
        /// The "iat" (issued at) claim identifies the time at which the JWT was
        /// issued.This claim can be used to determine the age of the JWT.Its
        /// value MUST be a number containing a NumericDate value.  Use of this
        /// claim is OPTIONAL.
        ///  </summary>
        /// <MetaDataID>{6b72e4ad-a4eb-4b87-8b23-8c096509c7c5}</MetaDataID>
        public DateTime IssuedAt { get; set; }

        /// <summary>
        /// The "exp" (expiration time) claim identifies the expiration time on
        /// or after which the JWT MUST NOT be accepted for processing.The
        /// processing of the "exp" claim requires that the current date/time
        /// MUST be before the expiration date/time listed in the "exp" claim.
        ///  </summary>
        /// <MetaDataID>{bc1dc873-561c-4019-a2f2-03ed9bb54f46}</MetaDataID>
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        /// <MetaDataID>{b5a0bc29-a93b-4b95-b491-a1659ceb5f29}</MetaDataID>
        public string Email { get; set; }

        /// <MetaDataID>{e88a2323-91b4-4474-88a9-d5f3d7064976}</MetaDataID>
        public bool Email_Verified { get; set; }

        /// <MetaDataID>{4a1060d8-7edb-42bb-8e17-f6f6e2732136}</MetaDataID>
        public string Firebase_Sign_in_Provider { get; set; }


        /// <MetaDataID>{4fa04e99-9c39-41a0-b19c-422bf2518058}</MetaDataID>
        public string AuthToken { get; set; }


    }




    ///// <MetaDataID>{0d247309-92fb-4d62-953b-15ddfd6b7d78}</MetaDataID>
    //public class AuthUserData
    //{
    //    public string PhoneNumber { get; set; }
    //    public string PhotoURL;
    //    public string DisplayName;
    //    public string Email;
    //    public bool EmailVerified;
    //    public string Uid;
    //    public string ProviderId;
    //}

    /// <MetaDataID>{a3c80482-edbe-4a1e-a514-3210ec880729}</MetaDataID>
    public class Authentication
    {

        static string _FirebaseProjectId;
        static internal string FirebaseProjectId
        {
            get
            {
                return _FirebaseProjectId;
            }
        }
        public static void InitializeFirebase(string FirebaseProjectId)
        {
            _FirebaseProjectId = FirebaseProjectId;
        }
    }

}
