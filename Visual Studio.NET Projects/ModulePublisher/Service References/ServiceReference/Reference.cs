﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModulePublisher.ServiceReference
{


    /// <MetaDataID>{5b966473-cef5-431f-919f-59d080dda6f2}</MetaDataID>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ServiceReference.IModulePublisher")]
    public interface IModulePublisher
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IModulePublisher/ExecudeModulePublishCommand", ReplyAction = "http://tempuri.org/IModulePublisher/ExecudeModulePublishCommandResponse")]
        int ExecudeModulePublishCommand(string fileName, string arguments);
    }

    /// <MetaDataID>{06c7dddf-cee8-4559-8f25-388ae5bfffcf}</MetaDataID>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IModulePublisherChannel : ModulePublisher.ServiceReference.IModulePublisher, System.ServiceModel.IClientChannel
    {
    }

    /// <MetaDataID>{cef74146-1045-4706-8ccc-b59f5f8eefc9}</MetaDataID>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ModulePublisherClient : System.ServiceModel.ClientBase<ModulePublisher.ServiceReference.IModulePublisher>, ModulePublisher.ServiceReference.IModulePublisher
    {

        public ModulePublisherClient()
        {
        }

        public ModulePublisherClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public ModulePublisherClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ModulePublisherClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ModulePublisherClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public int ExecudeModulePublishCommand(string fileName, string arguments)
        {
            return base.Channel.ExecudeModulePublishCommand(fileName, arguments);
        }
    }
}
