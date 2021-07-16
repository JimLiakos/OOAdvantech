﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.PersistenceLayer
{

    /// <MetaDataID>{46b72d6f-6335-443d-9fb9-c1a0074fb902}</MetaDataID>
    public partial class StorageServerInstanceLocatorEx
    {

        static StorageLocatorEx StorageLocator = null;
        public static void SetStorageInstanceLocationServerUrl(string storageMetadataGetFullUrl)
        {
            if (StorageLocator == null)
            {
                StorageLocator = new StorageLocatorEx(storageMetadataGetFullUrl);
                StorageServerInstanceLocator.AddStorageLocatorExtender(new StorageLocatorEx(storageMetadataGetFullUrl));
            }
            else
                StorageLocator.StorageMetadataGetFullUrl = storageMetadataGetFullUrl;

        }

        /// <MetaDataID>{ee181986-a08a-4804-a6e2-8a62f77af9d9}</MetaDataID>
        class StorageLocatorEx : OOAdvantech.PersistenceLayer.IStorageLocatorEx
        {
            internal string StorageMetadataGetFullUrl;
            public StorageLocatorEx(string storageMetadataGetFullUrl)
            {
                StorageMetadataGetFullUrl = storageMetadataGetFullUrl;
            }
            public OOAdvantech.MetaDataRepository.StorageMetaData GetSorageMetaData(string storageIdentity)
            {
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                OOAdvantech.PersistenceLayer.StoragesClient storagesClient = new OOAdvantech.PersistenceLayer.StoragesClient(httpClient);
                storagesClient.StorageMetadataGetFullUrl = StorageMetadataGetFullUrl;
                var task = storagesClient.GetAsync(storageIdentity);
                if (task.Wait(TimeSpan.FromSeconds(2)))
                    return task.Result;
                else
                    return new OOAdvantech.MetaDataRepository.StorageMetaData();
            }
        }
    }

    /// <MetaDataID>{ab9393f5-7ef1-4843-ba2c-1dae553ad4b2}</MetaDataID>
    public partial class StoragesClient
    {

        private string _baseUrl = "http://192.168.2.5:8090";
        private System.Net.Http.HttpClient _httpClient;
        private System.Lazy<OOAdvantech.Json.JsonSerializerSettings> _settings;

        public StoragesClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
            _settings = new System.Lazy<OOAdvantech.Json.JsonSerializerSettings>(() =>
            {
                var settings = new OOAdvantech.Json.JsonSerializerSettings();
                UpdateJsonSerializerSettings(settings);
                return settings;
            });
        }

        public string StorageMetadataGetFullUrl { get; set; }
        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        protected OOAdvantech.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

        partial void UpdateJsonSerializerSettings(OOAdvantech.Json.JsonSerializerSettings settings);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

        /// <summary>get storage meta data</summary>
        /// <returns>OK</returns>
        /// <exception cref="SwaggerException">A server side error occurred.</exception>
        public System.Threading.Tasks.Task<MetaDataRepository.StorageMetaData> GetAsync(string storageIdentity)
        {
            return GetAsync(storageIdentity, System.Threading.CancellationToken.None);
        }

        /// <summary>get storage meta data</summary>
        /// <returns>OK</returns>
        /// <exception cref="SwaggerException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async System.Threading.Tasks.Task<MetaDataRepository.StorageMetaData> GetAsync(string storageIdentity, System.Threading.CancellationToken cancellationToken)
        {
            if (storageIdentity == null)
                throw new System.ArgumentNullException("storageIdentity");

            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Storages?");
            urlBuilder_.Append("storageIdentity=").Append(System.Uri.EscapeDataString(ConvertToString(storageIdentity, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            urlBuilder_.Length--;
            if (!string.IsNullOrWhiteSpace(StorageMetadataGetFullUrl))
            {
                urlBuilder_ = new StringBuilder();
                urlBuilder_.Append(StorageMetadataGetFullUrl);
                urlBuilder_.Append("?");
                urlBuilder_.Append("storageIdentity=").Append(System.Uri.EscapeDataString(ConvertToString(storageIdentity, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
                urlBuilder_.Length--;
            }

            var client_ = _httpClient;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result_ = default(MetaDataRepository.StorageMetaData);
                            try
                            {
                                result_ = OOAdvantech.Json.JsonConvert.DeserializeObject<MetaDataRepository.StorageMetaData>(responseData_, _settings.Value);
                                return result_;
                            }
                            catch (System.Exception exception_)
                            {
                                throw new SwaggerException("Could not deserialize the response body.", (int)response_.StatusCode, responseData_, headers_, exception_);
                            }
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new SwaggerException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default(MetaDataRepository.StorageMetaData);
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        /// <returns>OK</returns>
        /// <exception cref="SwaggerException">A server side error occurred.</exception>
        public System.Threading.Tasks.Task<string> PostAsync(MetaDataRepository.StorageMetaData storageMetaData)
        {
            return PostAsync(storageMetaData, System.Threading.CancellationToken.None);
        }

        public System.Threading.Tasks.Task<string> PostAsync(Storage storage, bool multipleObjectContext)
        {
            var storageMetaData = new OOAdvantech.MetaDataRepository.StorageMetaData()
            {
                StorageName = storage.StorageName,
                StorageLocation = storage.StorageLocation,
                StorageType = storage.StorageType,
                StorageIdentity = storage.StorageIdentity,
                MultipleObjectContext = multipleObjectContext
            };
            return PostAsync(storageMetaData);
        }

        /// <returns>OK</returns>
        /// <exception cref="SwaggerException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async System.Threading.Tasks.Task<string> PostAsync(MetaDataRepository.StorageMetaData storageMetaData, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Storages");

            var client_ = _httpClient;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    var content_ = new System.Net.Http.StringContent(OOAdvantech.Json.JsonConvert.SerializeObject(storageMetaData, _settings.Value));
                    content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    PrepareRequest(client_, request_, urlBuilder_);
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);
                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var result_ = default(string);
                            try
                            {
                                result_ = OOAdvantech.Json.JsonConvert.DeserializeObject<string>(responseData_, _settings.Value);
                                return result_;
                            }
                            catch (System.Exception exception_)
                            {
                                throw new SwaggerException("Could not deserialize the response body.", (int)response_.StatusCode, responseData_, headers_, exception_);
                            }
                        }
                        else
                        if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new SwaggerException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }

                        return default(string);
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }

        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is System.Enum)
            {
                string name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    //if (field != null)
                    //{
                    //    var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                    //        as System.Runtime.Serialization.EnumMemberAttribute;
                    //    if (attribute != null)
                    //    {
                    //        return attribute.Value;
                    //    }
                    //}
                }
            }
            else if (value is bool)
            {
                return System.Convert.ToString(value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return System.Convert.ToBase64String((byte[])value);
            }
            else if (value != null && value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            return System.Convert.ToString(value, cultureInfo);
        }


        [System.CodeDom.Compiler.GeneratedCode("NSwag", "12.0.15.0 (NJsonSchema v9.13.22.0 (Newtonsoft.Json v11.0.0.0))")]
        public partial class SwaggerException : System.Exception
        {
            public int StatusCode { get; private set; }

            public string Response { get; private set; }

            public System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

            public SwaggerException(string message, int statusCode, string response, System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
                : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
            {
                StatusCode = statusCode;
                Response = response;
                Headers = headers;
            }

            public override string ToString()
            {
                return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
            }
        }

      
    }

}