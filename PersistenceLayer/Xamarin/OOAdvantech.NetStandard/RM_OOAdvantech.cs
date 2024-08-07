//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OOAdvantech.Remoting.Proxies
{
    using System;
    
    
    public sealed class Pr_IServerSessionPart : OOAdvantech.Remoting.MarshalByRefObject, OOAdvantech.Remoting.IServerSessionPart, OOAdvantech.Remoting.RestApi.ITransparentProxy
    {
        
        private OOAdvantech.Remoting.RestApi.Proxy Proxy;
        
        public OOAdvantech.Remoting.IServerSessionPart Org;
        
public event OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle Reconnected
            {
                add
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "add_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
                remove
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "remove_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
            }
        
        public Pr_IServerSessionPart(OOAdvantech.Remoting.RestApi.Proxy proxy)
        {
            this.Proxy = proxy;
        }
        
        // The Width property for the object.
        public System.Guid ServerProcessIdentity
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "get_ServerProcessIdentity", args, argsTypes);
                return this.Proxy.GetValue<System.Guid>(retValue);
            }
        }
        
        public OOAdvantech.Remoting.IProxy GetProxy()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "GetProxy", args, argsTypes);
            return this.Proxy.GetValue<OOAdvantech.Remoting.IProxy>(retValue);
        }
        
        public void ClientProcessTerminates()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "ClientProcessTerminates", args, argsTypes);
        }
        
        public object GetLifetimeService()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "GetLifetimeService", args, argsTypes);
            return this.Proxy.GetValue<object>(retValue);
        }
        
        public void Update(ref System.Collections.Generic.List<OOAdvantech.Remoting.ExtObjectUri> jastCreatedProxies, System.Collections.Generic.List<string> collectedProxies, out System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.Remoting.EventInfoData, System.Collections.Generic.List<object>>> pendingEvents)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = jastCreatedProxies;
            argsTypes[0] = typeof(System.Collections.Generic.List<OOAdvantech.Remoting.ExtObjectUri>);
            args[1] = collectedProxies;
            argsTypes[1] = typeof(System.Collections.Generic.List<string>);
            argsTypes[2] = typeof(System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.Remoting.EventInfoData, System.Collections.Generic.List<object>>>);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Update", args, argsTypes);
            jastCreatedProxies = this.Proxy.GetValue<System.Collections.Generic.List<OOAdvantech.Remoting.ExtObjectUri>>(args[0]);
            pendingEvents = this.Proxy.GetValue<System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<OOAdvantech.Remoting.EventInfoData, System.Collections.Generic.List<object>>>>(args[2]);
        }
        
        public void Subscribe(OOAdvantech.Remoting.ExtObjectUri eventPublisherUri, OOAdvantech.Remoting.EventInfoData eventInfo)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = eventPublisherUri;
            argsTypes[0] = typeof(OOAdvantech.Remoting.ExtObjectUri);
            args[1] = eventInfo;
            argsTypes[1] = typeof(OOAdvantech.Remoting.EventInfoData);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Subscribe", args, argsTypes);
        }
        
        public void ButchSubscribe(System.Collections.Generic.List<OOAdvantech.Remoting.EventSubscription> subscriptions)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = subscriptions;
            argsTypes[0] = typeof(System.Collections.Generic.List<OOAdvantech.Remoting.EventSubscription>);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "ButchSubscribe", args, argsTypes);
        }
        
        public void Subscribe(System.Collections.Generic.List<OOAdvantech.Remoting.RemoteEventSubscription> eventSubscriptions)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = eventSubscriptions;
            argsTypes[0] = typeof(System.Collections.Generic.List<OOAdvantech.Remoting.RemoteEventSubscription>);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Subscribe", args, argsTypes);
        }
        
        public void Subscribe(System.Collections.Generic.List<OOAdvantech.Remoting.RemoteEventSubscription> eventSubscriptions, OOAdvantech.Remoting.IRemoteEventHandler remoteEventHandler)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = eventSubscriptions;
            argsTypes[0] = typeof(System.Collections.Generic.List<OOAdvantech.Remoting.RemoteEventSubscription>);
            args[1] = remoteEventHandler;
            argsTypes[1] = typeof(OOAdvantech.Remoting.IRemoteEventHandler);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Subscribe", args, argsTypes);
        }
        
        public void Subscribe(OOAdvantech.Remoting.ExtObjectUri eventPublisherUri, OOAdvantech.Remoting.EventInfoData eventInfo, OOAdvantech.Remoting.IRemoteEventHandler clientSessionPart)
        {
            object[] args = new object[3];
            System.Type[] argsTypes = new System.Type[3];
            args[0] = eventPublisherUri;
            argsTypes[0] = typeof(OOAdvantech.Remoting.ExtObjectUri);
            args[1] = eventInfo;
            argsTypes[1] = typeof(OOAdvantech.Remoting.EventInfoData);
            args[2] = clientSessionPart;
            argsTypes[2] = typeof(OOAdvantech.Remoting.IRemoteEventHandler);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Subscribe", args, argsTypes);
        }
        
        public void Subscribe(System.Collections.Generic.List<OOAdvantech.Remoting.EventSubscription> subscriptions, OOAdvantech.Remoting.IRemoteEventHandler clientSessionPart)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = subscriptions;
            argsTypes[0] = typeof(System.Collections.Generic.List<OOAdvantech.Remoting.EventSubscription>);
            args[1] = clientSessionPart;
            argsTypes[1] = typeof(OOAdvantech.Remoting.IRemoteEventHandler);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Subscribe", args, argsTypes);
        }
        
        public void Unsubscribe(System.Collections.Generic.List<OOAdvantech.Remoting.RemoteEventSubscription> eventSubscriptions)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = eventSubscriptions;
            argsTypes[0] = typeof(System.Collections.Generic.List<OOAdvantech.Remoting.RemoteEventSubscription>);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Unsubscribe", args, argsTypes);
        }
        
        public void Unsubscribe(OOAdvantech.Remoting.ExtObjectUri eventPublisherUri, OOAdvantech.Remoting.EventInfoData eventInfo)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = eventPublisherUri;
            argsTypes[0] = typeof(OOAdvantech.Remoting.ExtObjectUri);
            args[1] = eventInfo;
            argsTypes[1] = typeof(OOAdvantech.Remoting.EventInfoData);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "Unsubscribe", args, argsTypes);
        }
        
        public object GetObject(OOAdvantech.Remoting.ExtObjectUri extObjectUri)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = extObjectUri;
            argsTypes[0] = typeof(OOAdvantech.Remoting.ExtObjectUri);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "GetObject", args, argsTypes);
            return this.Proxy.GetValue<object>(retValue);
        }
        
        public OOAdvantech.Remoting.MarshalByRefObject GetObjectFromUri(OOAdvantech.Remoting.ExtObjectUri extObjectUri)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = extObjectUri;
            argsTypes[0] = typeof(OOAdvantech.Remoting.ExtObjectUri);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.IServerSessionPart), "GetObjectFromUri", args, argsTypes);
            return this.Proxy.GetValue<OOAdvantech.Remoting.MarshalByRefObject>(retValue);
        }
    }
}
namespace OOAdvantech.Remoting.RestApi.Proxies
{
    using System;
    
    
    public sealed class Pr_IRemotingServer : OOAdvantech.Remoting.MarshalByRefObject, OOAdvantech.Remoting.RestApi.IRemotingServer, OOAdvantech.Remoting.RestApi.ITransparentProxy
    {
        
        private OOAdvantech.Remoting.RestApi.Proxy Proxy;
        
        public OOAdvantech.Remoting.RestApi.IRemotingServer Org;
        
public event OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle Reconnected
            {
                add
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "add_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
                remove
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "remove_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
            }
        
        public Pr_IRemotingServer(OOAdvantech.Remoting.RestApi.Proxy proxy)
        {
            this.Proxy = proxy;
        }
        
        public OOAdvantech.Remoting.IProxy GetProxy()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "GetProxy", args, argsTypes);
            return this.Proxy.GetValue<OOAdvantech.Remoting.IProxy>(retValue);
        }
        
        public object CreateInstance(string TypeFullName, string assemblyData, System.Type[] paramsTypes, object[] ctorParams)
        {
            object[] args = new object[4];
            System.Type[] argsTypes = new System.Type[4];
            args[0] = TypeFullName;
            argsTypes[0] = typeof(string);
            args[1] = assemblyData;
            argsTypes[1] = typeof(string);
            args[2] = paramsTypes;
            argsTypes[2] = typeof(System.Type[]);
            args[3] = ctorParams;
            argsTypes[3] = typeof(object[]);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.IRemotingServer), "CreateInstance", args, argsTypes);
            return this.Proxy.GetValue<object>(retValue);
        }
        
        public OOAdvantech.Remoting.MarshalByRefObject RefreshCacheData(OOAdvantech.Remoting.MarshalByRefObject obj)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = obj;
            argsTypes[0] = typeof(OOAdvantech.Remoting.MarshalByRefObject);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.IRemotingServer), "RefreshCacheData", args, argsTypes);
            return this.Proxy.GetValue<OOAdvantech.Remoting.MarshalByRefObject>(retValue);
        }
        
        public OOAdvantech.Remoting.MarshalByRefObject GetPersistentObject(string persistentUri)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = persistentUri;
            argsTypes[0] = typeof(string);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.IRemotingServer), "GetPersistentObject", args, argsTypes);
            return this.Proxy.GetValue<OOAdvantech.Remoting.MarshalByRefObject>(retValue);
        }
    }
    
    public sealed class CNSPr_IDeviceAuthentication_SignOutRequest : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object sender, OOAdvantech.Remoting.RestApi.DeviceAuthentication e)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = sender;
            argsTypes[0] = typeof(object);
            args[1] = e;
            argsTypes[1] = typeof(OOAdvantech.Remoting.RestApi.DeviceAuthentication);
            object retValue = this.Invoke(typeof(System.EventHandler<OOAdvantech.Remoting.RestApi.DeviceAuthentication>), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new System.EventHandler<OOAdvantech.Remoting.RestApi.DeviceAuthentication>(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new System.EventHandler<OOAdvantech.Remoting.RestApi.DeviceAuthentication>(this.Invoke));
        }
    }
}
namespace OOAdvantech.Authentication.Proxies
{
    using System;
    
    
    public sealed class Pr_IAuth : OOAdvantech.Remoting.MarshalByRefObject, OOAdvantech.Authentication.IAuth, OOAdvantech.Remoting.RestApi.ITransparentProxy
    {
        
        private OOAdvantech.Remoting.RestApi.Proxy Proxy;
        
        public OOAdvantech.Authentication.IAuth Org;
        
public event OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle Reconnected
            {
                add
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "add_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
                remove
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "remove_Reconnected",new object[] {value} , new Type[] { typeof(OOAdvantech.Remoting.RestApi.ProxyRecconectedHandle)});
                }
            }
public event OOAdvantech.Authentication.AuthStateChangeHandler AuthStateChange
            {
                add
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "add_AuthStateChange",new object[] {value} , new Type[] { typeof(OOAdvantech.Authentication.AuthStateChangeHandler)});
                }
                remove
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "remove_AuthStateChange",new object[] {value} , new Type[] { typeof(OOAdvantech.Authentication.AuthStateChangeHandler)});
                }
            }
public event OOAdvantech.Authentication.IdTokenChangeHandler IdTokenChange
            {
                add
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "add_IdTokenChange",new object[] {value} , new Type[] { typeof(OOAdvantech.Authentication.IdTokenChangeHandler)});
                }
                remove
                {
                    this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "remove_IdTokenChange",new object[] {value} , new Type[] { typeof(OOAdvantech.Authentication.IdTokenChangeHandler)});
                }
            }
        
        public Pr_IAuth(OOAdvantech.Remoting.RestApi.Proxy proxy)
        {
            this.Proxy = proxy;
        }
        
        // The Width property for the object.
        public OOAdvantech.Authentication.IAuthUser CurrentUser
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "get_CurrentUser", args, argsTypes);
                return this.Proxy.GetValue<OOAdvantech.Authentication.IAuthUser>(retValue);
            }
        }
        
        // The Width property for the object.
        public System.Collections.Generic.List<OOAdvantech.Authentication.SignInProvider> Providers
        {
            get
            {
                object[] args = new object[0];
                System.Type[] argsTypes = new System.Type[0];
                object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "get_Providers", args, argsTypes);
                return this.Proxy.GetValue<System.Collections.Generic.List<OOAdvantech.Authentication.SignInProvider>>(retValue);
            }
        }
        
        public OOAdvantech.Remoting.IProxy GetProxy()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Remoting.RestApi.ITransparentProxy), "GetProxy", args, argsTypes);
            return this.Proxy.GetValue<OOAdvantech.Remoting.IProxy>(retValue);
        }
        
        public System.Threading.Tasks.Task<string> GetIdToken()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "GetIdToken", args, argsTypes);
            return this.Proxy.GetValue<System.Threading.Tasks.Task<string>>(retValue);
        }
        
        public System.Threading.Tasks.Task<bool> SignInWith(OOAdvantech.Authentication.SignInProvider provider)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = provider;
            argsTypes[0] = typeof(OOAdvantech.Authentication.SignInProvider);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "SignInWith", args, argsTypes);
            return this.Proxy.GetValue<System.Threading.Tasks.Task<bool>>(retValue);
        }
        
        public System.Threading.Tasks.Task<string> EmailSignIn(string email, string password)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = email;
            argsTypes[0] = typeof(string);
            args[1] = password;
            argsTypes[1] = typeof(string);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "EmailSignIn", args, argsTypes);
            return this.Proxy.GetValue<System.Threading.Tasks.Task<string>>(retValue);
        }
        
        public System.Threading.Tasks.Task<string> EmailSignUp(string email, string password)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = email;
            argsTypes[0] = typeof(string);
            args[1] = password;
            argsTypes[1] = typeof(string);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "EmailSignUp", args, argsTypes);
            return this.Proxy.GetValue<System.Threading.Tasks.Task<string>>(retValue);
        }
        
        public System.Threading.Tasks.Task SendPasswordResetEmail(string email)
        {
            object[] args = new object[1];
            System.Type[] argsTypes = new System.Type[1];
            args[0] = email;
            argsTypes[0] = typeof(string);
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "SendPasswordResetEmail", args, argsTypes);
            return this.Proxy.GetValue<System.Threading.Tasks.Task>(retValue);
        }
        
        public void SignOut()
        {
            object[] args = new object[0];
            System.Type[] argsTypes = new System.Type[0];
            object retValue = this.Proxy.Invoke(typeof(OOAdvantech.Authentication.IAuth), "SignOut", args, argsTypes);
        }
    }
    
    public sealed class CNSPr_IAuth_AuthStateChange : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object sender, OOAdvantech.Authentication.AuthStateEventArgs authArgs)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = sender;
            argsTypes[0] = typeof(object);
            args[1] = authArgs;
            argsTypes[1] = typeof(OOAdvantech.Authentication.AuthStateEventArgs);
            object retValue = this.Invoke(typeof(OOAdvantech.Authentication.AuthStateChangeHandler), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.Authentication.AuthStateChangeHandler(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.Authentication.AuthStateChangeHandler(this.Invoke));
        }
    }
    
    public sealed class CNSPr_IAuth_IdTokenChange : OOAdvantech.Remoting.EventConsumerHandler
    {
        
        public void Invoke(object sender, OOAdvantech.Authentication.IdTokenEventArgs idTokenArgs)
        {
            object[] args = new object[2];
            System.Type[] argsTypes = new System.Type[2];
            args[0] = sender;
            argsTypes[0] = typeof(object);
            args[1] = idTokenArgs;
            argsTypes[1] = typeof(OOAdvantech.Authentication.IdTokenEventArgs);
            object retValue = this.Invoke(typeof(OOAdvantech.Authentication.IdTokenChangeHandler), "Invoke", args, argsTypes);
        }
        
        public override void AddEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(target, new OOAdvantech.Authentication.IdTokenChangeHandler(this.Invoke));
        }
        
        public override void RemoveEventHandler(object target, System.Reflection.EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(target, new OOAdvantech.Authentication.IdTokenChangeHandler(this.Invoke));
        }
    }
}
