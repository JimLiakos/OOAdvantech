using System;

using System.Collections.Generic;

using System.Linq;
using System.Net.Http;
using System.Text;


#if !DeviceDotNet
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
#endif
namespace OOAdvantech.Remoting.RestApi
{

    /// <MetaDataID>{2a0208ce-644a-45ae-801c-6e0d7b867380}</MetaDataID>
    [MetaDataRepository.HttpVisible]
    public interface IDeviceAuthentication
    {
        [MetaDataRepository.GenerateEventConsumerProxy]
        event EventHandler<DeviceAuthentication> SignOutRequest;

    }

    /// <MetaDataID>{44ed95c5-3794-4f1a-bec3-3664e0b7c529}</MetaDataID>
    [MetaDataRepository.HttpVisible]
    public class DeviceAuthentication : MonoStateClass, IDeviceAuthentication
    {

        public DeviceAuthentication()
        {

        }
        static System.Threading.Timer Timer;

        static DeviceAuthentication()
        {

            Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTimer), null, 500, 500);
        }
        static void OnTimer(object state)
        {
            if (_AuthUser != null)
            {
                if (_AuthUser.ExpirationTime < DateTime.Now)
                {

                }
                if (((_AuthUser.ExpirationTime - DateTime.Now).TotalMinutes < 2))
                {

                }
            }

        }



        public static DeviceAuthentication Current
        {
            get
            {
                return DeviceAuthentication.GetInstance(typeof(DeviceAuthentication), true) as DeviceAuthentication;
            }
        }

        public static event EventHandler<AuthUser> AuthStateChanged;
        event EventHandler<DeviceAuthentication> _SignOutRequest;

        
        public event EventHandler<DeviceAuthentication> SignOutRequest
        {
            add
            {
                _SignOutRequest += value;
            }
            remove
            {
                _SignOutRequest -= value;
            }
        }

        public void RaiseSignOutRequest()
        {
            _SignOutRequest?.Invoke(this, this);
        }



        public bool AuthIDTokenChanged(string idToken, DateTime expirationTime, OOAdvantech.Authentication.AuthUser authUserData)
        {
            if (IDToken != idToken)
            {
                _UnInitialized = true;
                if (string.IsNullOrWhiteSpace(idToken))
                {
                    _AuthUser = null;
                    IDToken = idToken;
                    AuthStateChanged?.Invoke(this, _AuthUser);
                    return true;
                }
                else
                {
                    _AuthUser = GetAuthData(idToken);
                    if (_AuthUser == null && authUserData != null)
                    {
                        _AuthUser = new AuthUser()
                        {
                            AuthToken = idToken,
                            ExpirationTime = expirationTime,
                            Email = authUserData.Email,
                            Email_Verified = authUserData.IsEmailVerified,
                            Firebase_Sign_in_Provider = authUserData.ProviderId,
                            Name = authUserData.DisplayName,
                            Picture = authUserData.PhotoUrl,
                            User_ID = authUserData.Uid
                        };
                    }

                    IDToken = idToken;
                    _UnInitialized = false;
                    AuthStateChanged?.Invoke(this, _AuthUser);

                    return true;

                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(IDToken) && _AuthUser == null)
                {
                    _AuthUser = GetAuthData(idToken);
                    if (_AuthUser == null && authUserData != null)
                    {
                        _AuthUser = new AuthUser()
                        {
                            AuthToken = idToken,
                            ExpirationTime = expirationTime,
                            Email = authUserData.Email,
                            Email_Verified = authUserData.IsEmailVerified,
                            Firebase_Sign_in_Provider = authUserData.ProviderId,
                            Name = authUserData.DisplayName,
                            Picture = authUserData.PhotoUrl,
                            User_ID = authUserData.Uid
                        };
                        AuthStateChanged?.Invoke(this, _AuthUser);
                    }
                }

            }
            return true;
            return true;
        }

        /// <MetaDataID>{34df0860-c61e-41c6-80cc-8c5a3b1d9d65}</MetaDataID>
        public static string IDToken;


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public DateTime IdTokenExpirationTime
        {
            get
            {
                if (_AuthUser != null)
                    return _AuthUser.ExpirationTime.ToUniversalTime();
                else
                    return DateTime.Now;
            }
        }

        /// <MetaDataID>{4d9077fe-5041-4ef2-9bcc-fd1d5008970e}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public bool AuthIDTokenChanged(string idToken, OOAdvantech.Authentication.AuthUser authUserData)
        {
            //System.Reflection.di


            if (IDToken != idToken)
            {
                _UnInitialized = true;

                if (string.IsNullOrWhiteSpace(idToken))
                {
                    _AuthUser = null;
                    IDToken = idToken;
                    AuthStateChanged?.Invoke(this, _AuthUser);
                    return true;
                }
                else
                {

                    _AuthUser = GetAuthData(idToken);
                    if (_AuthUser == null && authUserData != null)
                    {
                        _AuthUser = new AuthUser()
                        {
                            AuthToken = idToken,
                            Email = authUserData.Email,
                            Email_Verified = authUserData.IsEmailVerified,
                            Firebase_Sign_in_Provider = authUserData.ProviderId,
                            Name = authUserData.DisplayName,
                            Picture = authUserData.PhotoUrl,
                            User_ID = authUserData.Uid
                        };
                    }

                    IDToken = idToken;
                    _UnInitialized = false;
                    AuthStateChanged?.Invoke(this, _AuthUser);

                    return true;

                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(IDToken) && _AuthUser == null)
                {
                    _AuthUser = GetAuthData(idToken);
                    if (_AuthUser == null && authUserData != null)
                    {
                        _AuthUser = new AuthUser()
                        {
                            AuthToken = idToken,
                            Email = authUserData.Email,
                            Email_Verified = authUserData.IsEmailVerified,
                            Firebase_Sign_in_Provider = authUserData.ProviderId,
                            Name = authUserData.DisplayName,
                            Picture = authUserData.PhotoUrl,
                            User_ID = authUserData.Uid
                        };
                        AuthStateChanged?.Invoke(this, _AuthUser);
                    }
                }

            }
            return true;
        }
        internal void InternalAuthIDTokenChanged(string idToken, AuthUser authUser)
        {

            IDToken = idToken;
            if (!string.IsNullOrWhiteSpace(idToken) && authUser != null)
                _AuthUser = authUser;
            else
                _AuthUser = authUser;

        }

#if DeviceDotNet
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public OOAdvantech.Authentication.IAuth Auth
        {
            get
            {
                return OOAdvantech.Authentication.OOAdvantechAuth.Auth;
            }
        }
#endif


        static bool _UnInitialized;
        public static bool UnInitialized
        {
            get
            {
                return _UnInitialized;
            }
        }

        static AuthUser _AuthUser;
        static public AuthUser AuthUser
        {
            get
            {
                return _AuthUser;
            }
        }


#if DeviceDotNet
        public static void SignedIn(AuthUser authUser)
        {
            _AuthUser = authUser;
            AuthStateChanged?.Invoke(null, _AuthUser);
        }
        public static void SignedOut()
        {
            _AuthUser = null;

            AuthStateChanged?.Invoke(null, _AuthUser);

        }
#endif

        internal AuthUser GetAuthData(string authToken)
        {

#if !DeviceDotNet

            JwtSecurityToken tokenS = Validate(authToken);

            _AuthUser = new AuthUser();

            string kid = tokenS.Header["kid"] as string;

            string iat = (from claim in tokenS.Claims where claim.Type == "iat" select claim.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(iat))
                _AuthUser.IssuedAt = FromUnixTime(int.Parse(iat)).ToLocalTime();

            string exp = (from claim in tokenS.Claims where claim.Type == "exp" select claim.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(exp))
                _AuthUser.ExpirationTime = FromUnixTime(int.Parse(exp)).ToLocalTime();


            string auth_time = (from claim in tokenS.Claims where claim.Type == "auth_time" select claim.Value).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(auth_time))
                _AuthUser.Auth_Time = FromUnixTime(int.Parse(auth_time)).ToLocalTime();

            _AuthUser.Audience = (from claim in tokenS.Claims where claim.Type == "aud" select claim.Value).FirstOrDefault();
            _AuthUser.Email = (from claim in tokenS.Claims where claim.Type == "email" select claim.Value).FirstOrDefault();

            _AuthUser.Email_Verified = bool.Parse((from claim in tokenS.Claims where claim.Type == "email_verified" select claim.Value).FirstOrDefault());
            _AuthUser.Iss = (from claim in tokenS.Claims where claim.Type == "iss" select claim.Value).FirstOrDefault();
            _AuthUser.Name = (from claim in tokenS.Claims where claim.Type == "name" select claim.Value).FirstOrDefault();
            _AuthUser.Picture = (from claim in tokenS.Claims where claim.Type == "picture" select claim.Value).FirstOrDefault();
            _AuthUser.Subject = (from claim in tokenS.Claims where claim.Type == "sub" select claim.Value).FirstOrDefault();
            _AuthUser.User_ID = (from claim in tokenS.Claims where claim.Type == "user_id" select claim.Value).FirstOrDefault();

            var firebaseAttributes = (from claim in tokenS.Claims where claim.Type == "firebase" select JObject.Parse(claim.Value)).FirstOrDefault();

            _AuthUser.Firebase_Sign_in_Provider = (from fireBaseProperty in firebaseAttributes.Properties()
                                                   where fireBaseProperty.Name == "sign_in_provider"
                                                   select (fireBaseProperty.Value as JValue).Value).FirstOrDefault() as string;

            _AuthUser.AuthToken = authToken;


            if (_AuthUser.Audience != "demomicroneme" ||
            _AuthUser.Iss != "https://securetoken.google.com/" + "demomicroneme" ||
            _AuthUser.IssuedAt >= System.DateTime.Now ||
            _AuthUser.ExpirationTime <= System.DateTime.Now)
            {
                return null;
            }
            else
                return _AuthUser;
#else
            return null;
#endif
        }

#if !DeviceDotNet
        private JwtSecurityToken Validate(string authToken)
        {
            HttpClient client = new HttpClient();

            string encodedJwt = authToken;
            // 1. Get Google signing keys
            client.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");


            //var task = response.Content.ReadAsStringAsync();
            //task.Wait();
            //var responseDataString = task.Result;

            var task = client.GetAsync(
              "x509/securetoken@system.gserviceaccount.com");
            task.Wait();
            HttpResponseMessage response = task.Result;
            if (!response.IsSuccessStatusCode) { return null; }

            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
            var x509DataTask = response.Content.ReadAsStringAsync();
            x509DataTask.Wait();

            var x509Data = JSserializer.Deserialize<Dictionary<string, string>>(x509DataTask.Result);


            SecurityKey[] keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();
            // 2. Configure validation parameters
            string FirebaseProjectId = Authentication.FirebaseProjectId;
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = "https://securetoken.google.com/" + FirebaseProjectId,
                ValidAudience = FirebaseProjectId,
                IssuerSigningKeys = keys,
            };
            // 3. Use JwtSecurityTokenHandler to validate signature, issuer, audience and lifetime
            var handler = new JwtSecurityTokenHandler();
            SecurityToken token;
            System.Security.Claims.ClaimsPrincipal principal = handler.ValidateToken(encodedJwt, parameters, out token);
            var jwt = (JwtSecurityToken)token;
            // 4.Validate signature algorithm and other applicable valdiations
            if (jwt.Header.Alg != SecurityAlgorithms.RsaSha256)
            {
                throw new SecurityTokenInvalidSignatureException(
                  "The token is not signed with the expected algorithm.");
            }
            return jwt;
        }
        SecurityKey CreateSecurityKeyFromPublicKey(string data)
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
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
