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


    /// <MetaDataID>{e318efe1-dc3e-4ced-9c11-f8155b36444e}</MetaDataID>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ServiceReference.IModulePublisher")]
    public interface IModulePublisher
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IModulePublisher/ExecudeModulePublishCommand", ReplyAction = "http://tempuri.org/IModulePublisher/ExecudeModulePublishCommandResponse")]
        int ExecudeModulePublishCommand(string fileName, string arguments);
    }

    /// <MetaDataID>{33ee5e3c-598b-4519-a5ca-2a64b1beae2b}</MetaDataID>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IModulePublisherChannel : ModulePublisher.ServiceReference.IModulePublisher, System.ServiceModel.IClientChannel
    {
    }

    /// <MetaDataID>{cc18a213-7205-4b38-865b-8d9119508587}</MetaDataID>
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
