using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.Json;

namespace OOAdvantech.Remoting.RestApi
{

    /// <MetaDataID>{ebd76e86-a049-47aa-8689-c2a642fa45e6}</MetaDataID>
    class FirebaseJWTAuth
    {
        /// <MetaDataID>{0020ea03-1900-4f6e-8274-36d7bff059bb}</MetaDataID>
        public string FirebaseId;
        /// <MetaDataID>{fe55e917-aa79-49ef-9db1-32343d1c64f3}</MetaDataID>
        private HttpClient Req;

        /// <MetaDataID>{b7509168-d59d-4ee8-a864-f2bdcf0c57e1}</MetaDataID>
        public FirebaseJWTAuth(string firebaseId)
        {
            FirebaseId = firebaseId;
            Req = new HttpClient();
            Req.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");
        }





        /// <MetaDataID>{c5e68ed6-5386-40ed-8852-f9b502e27cf2}</MetaDataID>
        public string Verify(string token)
        {
            //following instructions from https://firebase.google.com/docs/auth/admin/verify-id-tokens
            string hashChunk = token; //keep for hashing later on
            hashChunk = hashChunk.Substring(0, hashChunk.LastIndexOf('.'));
           // token = token.Replace('-', '+').Replace('_', '/'); //sanitize for base64 on C#
            string[] sections = token.Split('.'); //split into 3 sections according to JWT standards
            JwtHeader header = B64Json<JwtHeader>(sections[0]);

            if (header.alg != "RS256")
            {
                return null;
            }
            var response = Req.GetAsync("x509/securetoken@system.gserviceaccount.com").Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                string keyDictStr = responseContent.ReadAsStringAsync().Result;
                Dictionary<string, string> keyDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(keyDictStr);
                string keyStr = null;
                keyDict.TryGetValue(header.kid, out keyStr);
                if (keyStr == null)
                {
                    return null;
                }
                using (var rsaCrypto = CertFromPem(keyStr))
                {
                    using (var hasher = SHA256Managed.Create())
                    {
                        byte[] plainText = Encoding.UTF8.GetBytes(hashChunk);
                        byte[] hashed = hasher.ComputeHash(plainText);

                        byte[] challenge = SafeB64Decode(sections[2]);

                        RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsaCrypto);
                        rsaDeformatter.SetHashAlgorithm("SHA256");
                        if (!rsaDeformatter.VerifySignature(hashed, challenge))
                        {
                            return null;
                        }
                    }
                }

                JwtPayload payload = B64Json<JwtPayload>(sections[1]);
                long currentTime = EpochSec();
                //the if chain of death
                if (payload.aud != FirebaseId ||
                 payload.iss != "https://securetoken.google.com/" + FirebaseId ||
                 payload.iat >= currentTime ||
                 payload.exp <= currentTime)
                {
                    return null;
                }
                return payload.sub;
            }
            return null;
        }

        /// <MetaDataID>{ac2ef506-2680-429f-8e6b-5bd0f6864f44}</MetaDataID>
        private static RSACryptoServiceProvider CertFromPem(string pemKey)
        {
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(Encoding.UTF8.GetBytes(pemKey));
            return (RSACryptoServiceProvider)cert.PublicKey.Key;
        }

        /// <MetaDataID>{ab2a4b1a-924a-4605-8633-e8c9a822a470}</MetaDataID>
        private static byte[] SafeB64Decode(string encoded)
        {
            string encodedPad = encoded + new string('=', 4 - encoded.Length % 4);
            return Convert.FromBase64String(encodedPad);
        }
        /// <MetaDataID>{fae5278a-38fb-4e0e-81e9-58b0665ec301}</MetaDataID>
        private static string SafeB64DecodeStr(string encoded)
        {
            return Encoding.UTF8.GetString(SafeB64Decode(encoded));
        }
        /// <MetaDataID>{f428bf4a-e23b-415d-9e7f-fa2c06339aba}</MetaDataID>
        private static T B64Json<T>(string encoded)
        {
            string decoded = SafeB64DecodeStr(encoded);
            return JsonConvert.DeserializeObject<T>(decoded);
        }
        /// <MetaDataID>{5f97fc1f-799f-45a5-a9ad-a3418817c7aa}</MetaDataID>
        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <MetaDataID>{4042933d-507f-4f6f-b6e1-16e44c04e9c7}</MetaDataID>
        public static long EpochSec()
        {
            return (long)((DateTime.UtcNow - Jan1st1970).TotalSeconds);
        }
        private struct JwtHeader
        {
            public string alg;
            public string kid;
        }
        private struct JwtPayload
        {
            public long exp;
            public long iat;
            public string aud;
            public string iss;
            public string sub;
        }
    }
}

