using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace PublishModuleService
{
    public partial class ServiceTask : ServiceBase
    {
        public ServiceTask()
        {
            InitializeComponent();
        }
        ServiceHost ModulePublisherServiceHost = null;
        protected override void OnStart(string[] args)
        {
             
            try
            {
                bool Block = false;
                while (Block)
                {
                    System.Threading.Thread.Sleep(2000);
                }

                
                //Base Address for StudentService
                Uri httpBaseAddress = new Uri("http://localhost:4321/ModulePublisherService");
                
                //Instantiate ServiceHost
                ModulePublisherServiceHost = new ServiceHost(typeof(ModulePublisher.ModulePublisherService),
                    httpBaseAddress);
 
                //Add Endpoint to Host
                ModulePublisherServiceHost.AddServiceEndpoint(typeof(ModulePublisher.IModulePublisher), 
                                                        new WSHttpBinding(), "");            
 
                //Metadata Exchange
                ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
                serviceBehavior.HttpGetEnabled = true;
                ModulePublisherServiceHost.Description.Behaviors.Add(serviceBehavior);

                //Open
                ModulePublisherServiceHost.Open();
                
                
            }

            catch (Exception ex)
            {
                ModulePublisherServiceHost = null;
                
            }
        }

        protected override void OnStop()
        {
        }
    }
}
