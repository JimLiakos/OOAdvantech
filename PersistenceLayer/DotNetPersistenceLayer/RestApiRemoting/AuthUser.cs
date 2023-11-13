
using OOAdvantech.Json.Linq;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;



#if !DeviceDotNet
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Web.Script.Serialization;
#endif

namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{72f73b6d-a2e3-48cf-858e-fe6dfe67c782}</MetaDataID>
    public class AuthUser
    {
        /// <MetaDataID>{9171b279-c37b-4056-8f7c-11e2e93442da}</MetaDataID>
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


        /// <MetaDataID>{e61fa8eb-91be-4a85-a431-9b10727e568a}</MetaDataID>
        static Dictionary<string, string> Cx509Data;

#if !DeviceDotNet

        /// <MetaDataID>{7c8d195d-94e0-4b21-9f63-7d78fdd660d5}</MetaDataID>
        static JwtSecurityToken Validate(string authToken)
        {

            HttpClient client = new HttpClient();

            string encodedJwt = authToken;
            // 1. Get Google signing keys
            client.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");


            //var task = response.Content.ReadAsStringAsync();
            //task.Wait();
            //var responseDataString = task.Result;
            Stopwatch timer = new Stopwatch();
            timer.Start();

            var task = client.GetAsync(
              "x509/securetoken@system.gserviceaccount.com");
            if (!task.Wait(System.TimeSpan.FromSeconds(9)))
            {
                if (!task.Wait(Binding.DefaultBinding.SendTimeout))
                {
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                }
            }


            HttpResponseMessage response = task.Result;
            if (!response.IsSuccessStatusCode) { return null; }

            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            var x509DataTask = response.Content.ReadAsStringAsync();
            x509DataTask.Wait();

            timer.Stop();
            if (timer.ElapsedMilliseconds>100)
            {

            }
            if (!x509DataTask.Wait(System.TimeSpan.FromSeconds(9)))
            {
                if (!x509DataTask.Wait(Binding.DefaultBinding.SendTimeout))
                {
                    throw new System.TimeoutException(string.Format("SendTimeout {0} expired", Binding.DefaultBinding.SendTimeout));
                }

            }

            var x509Data = JSserializer.Deserialize<Dictionary<string, string>>(x509DataTask.Result);

            if (Cx509Data == null)
                Cx509Data = x509Data;

            foreach (var dictionaryEntry in x509Data)
            {
                string dictionaryData = null;
                if (Cx509Data.TryGetValue(dictionaryEntry.Key, out dictionaryData))
                {
                    if (dictionaryData != dictionaryEntry.Value)
                    {

                    }
                }
                else
                {

                }
            }
            Cx509Data = x509Data;

            SecurityKey[] keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();
            // 2. Configure validation parameters
            string FirebaseProjectId = Authentication.FirebaseProjectId;

            if (string.IsNullOrWhiteSpace(FirebaseProjectId))
            {
                throw new Exception("Empty FirebaseProjectId. Initialize with 'OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase'");
            }
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = "https://securetoken.google.com/" + FirebaseProjectId,
                ValidAudience = FirebaseProjectId,
                IssuerSigningKeys = keys,
            };
            // 3. Use JwtSecurityTokenHandler to validate signature, issuer, audience and lifetime
            var handler = new JwtSecurityTokenHandler();
            SecurityToken token;
            try
            {
                ClaimsPrincipal principal = handler.ValidateToken(encodedJwt, parameters, out token);
                var jwt = (JwtSecurityToken)token;
                // 4.Validate signature algorithm and other applicable valdiations
                if (jwt.Header.Alg != SecurityAlgorithms.RsaSha256)
                {
                    throw new SecurityTokenInvalidSignatureException(
                      "The token is not signed with the expected algorithm.");
                }
                return jwt;
            }
            catch (Exception error)
            {

                throw;
            }
        }

        /// <MetaDataID>{af714dd6-3278-4a2c-8b4a-c0a7d0134ecf}</MetaDataID>
        static SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }

#endif
        /// <MetaDataID>{dc8b5103-ea81-40bb-97d0-3a59ff766059}</MetaDataID>
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        /// <MetaDataID>{6ea4ebf2-c232-465c-92cd-76413220df7c}</MetaDataID>
        /// <summary>
        /// Is the number of seconds that have elapsed since January 1, 1970 (midnight UTC/GMT)
        /// </summary>
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

#if !DeviceDotNet
        /// <MetaDataID>{a83e82f9-70e5-4e68-b3e0-2795d447f9d3}</MetaDataID>
        public static AuthUser GetAuthUserFromToken(string authToken)
        {
            AuthUser authUser;
            authUser = new AuthUser();

            AuthUser mauthUser = null;
            //var handler = new JwtSecurityTokenHandler();
            //JwtSecurityToken tokenS = handler.ReadToken(authToken) as JwtSecurityToken;
            if (Authentication.OAuth!=null)
            {
                Stopwatch timer = Stopwatch.StartNew();
                timer.Start();
                var VerifyIdTokenTask = Authentication.OAuth.VerifyIdToken(authToken);
                VerifyIdTokenTask.Wait();

                timer.Stop();
                var elapsed = timer.ElapsedMilliseconds;
                mauthUser= VerifyIdTokenTask.Result;
               // return mauthUser;
            }
            JwtSecurityToken tokenS = Validate(authToken);
            //System.Diagnostics.Debug.WriteLine(OOAdvantech.Json.JsonConvert.SerializeObject(tokenS));
            // var validToken= handler.ValidateToken.ValidateToken(tokenS);

            //dontwait4waiter
            //var firebaseJWTAuth = new FirebaseJWTAuth("demomicroneme");
            //var firebaseJWTAuth = new FirebaseJWTAuth("dontwait4waiter");
            //string res = firebaseJWTAuth.Verify(authToken);




            string kid = tokenS.Header["kid"] as string;

            string iat = (from claim in tokenS.Claims where claim.Type == "iat" select claim.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(iat))
                authUser.IssuedAt = FromUnixTime(int.Parse(iat)).ToLocalTime();

            string exp = (from claim in tokenS.Claims where claim.Type == "exp" select claim.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(exp))
                authUser.ExpirationTime = FromUnixTime(int.Parse(exp)).ToLocalTime();


            string auth_time = (from claim in tokenS.Claims where claim.Type == "auth_time" select claim.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(auth_time))
                authUser.Auth_Time = FromUnixTime(int.Parse(auth_time)).ToLocalTime();

            authUser.Audience = (from claim in tokenS.Claims where claim.Type == "aud" select claim.Value).FirstOrDefault();
            authUser.Email = (from claim in tokenS.Claims where claim.Type == "email" select claim.Value).FirstOrDefault();

            if ((from claim in tokenS.Claims where claim.Type == "email_verified" select claim.Value).FirstOrDefault() != null)
                authUser.Email_Verified = bool.Parse((from claim in tokenS.Claims where claim.Type == "email_verified" select claim.Value).FirstOrDefault());
            authUser.Iss = (from claim in tokenS.Claims where claim.Type == "iss" select claim.Value).FirstOrDefault();
            authUser.Name = (from claim in tokenS.Claims where claim.Type == "name" select claim.Value).FirstOrDefault();
            authUser.Picture = (from claim in tokenS.Claims where claim.Type == "picture" select claim.Value).FirstOrDefault();
            authUser.Subject = (from claim in tokenS.Claims where claim.Type == "sub" select claim.Value).FirstOrDefault();
            authUser.User_ID = (from claim in tokenS.Claims where claim.Type == "user_id" select claim.Value).FirstOrDefault();

            var firebaseAttributes = (from claim in tokenS.Claims where claim.Type == "firebase" select JObject.Parse(claim.Value)).FirstOrDefault();

            authUser.Firebase_Sign_in_Provider = (from fireBaseProperty in firebaseAttributes.Properties()
                                                  where fireBaseProperty.Name == "sign_in_provider"
                                                  select (fireBaseProperty.Value as JValue).Value).FirstOrDefault() as string;

            authUser.AuthToken = authToken;
            return authUser;
        }

#endif

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

        public static IOAuth OAuth { get; private set; }

        static internal string FirebaseProjectId
        {
            get
            {

                return _FirebaseProjectId;
            }
        }
        public static void InitializeFirebase(string FirebaseProjectId, IOAuth oAuth=null)
        {
            _FirebaseProjectId = FirebaseProjectId;

            OAuth=oAuth;
        }

    }

    public interface IOAuth
    {
        Task<AuthUser> VerifyIdToken(string authToken);
    }

}
