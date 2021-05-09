using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;


namespace TransactionCoordinator
{
    /// <MetaDataID>{e495ea5a-9dad-45d6-894a-cb3049b9c838}</MetaDataID>
    public partial class TransactionCoordinatorService : ServiceBase
    {
        public TransactionCoordinatorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);
            //9050
            //System.Windows.Forms.MessageBox.Show("in OnStart");
            bool Block = true;

            //while (Block)
            //{
            //    System.Threading.Thread.Sleep(1000);
            //}


            //   throw new Exception("mitsos");
            try
            {
               System.Reflection.FieldInfo fieldInfo= typeof(OOAdvantech.Transactions.Transaction).Assembly.GetType("OOAdvantech.Transactions.Sinks.ClientSink").GetField("DisableTransactionMarshalling",System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
               fieldInfo.SetValue(null, true);
               fieldInfo = typeof(OOAdvantech.Transactions.Transaction).Assembly.GetType("OOAdvantech.Transactions.Sinks.ServerSink").GetField("DisableTransactionMarshalling", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
               fieldInfo.SetValue(null, true);




                //OOAdvantech.Transactions.Sinks.ClientSink.DisableTransactionMarshalling = true;
                //OOAdvantech.Transactions.Sinks.ServerSink.DisableTransactionMarshalling = true;

                string ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + "TransactionCoordinatorService.config";
                if (!System.IO.File.Exists(ConfigFileName))
                {
                    System.Xml.XmlDocument XmlDocument = new System.Xml.XmlDocument();
                    XmlDocument.LoadXml("<configuration><system.runtime.remoting><customErrors mode=\"off\"/></system.runtime.remoting></configuration>");
                    XmlDocument.Save(ConfigFileName);
                }
                System.Runtime.Remoting.RemotingConfiguration.Configure(ConfigFileName,false);

                System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                System.Runtime.Remoting.RemotingConfiguration.CustomErrorsEnabled(false);

                #region Org Setting

                OOAdvantech.Remoting.RemotingServices.RegisterSecureTcpChannel(9050, true);
                //OOAdvantech.Remoting.RemotingServices.RegisterTcpServerChannel( "tcp_free", 9051, false, false);
                OOAdvantech.Remoting.RemotingServices.RegisterIpcClientChannel();
                OOAdvantech.Remoting.RemotingServices.ImpersonateToInitSessionUser = true;

                #endregion


                #region Org Setting

                
                #endregion


                //OOAdvantech.Remoting.RemotingServices.RegisterChannel("PIDTransactions",true);// + System.Diagnostics.Process.GetCurrentProcess().Id.ToString(), true);//regiser ipc


                System.ServiceProcess.ServiceController DTC = new System.ServiceProcess.ServiceController("MSDTC");
               if(DTC.Status==ServiceControllerStatus.Stopped)
                   DTC.Start();


                //System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseTime = TimeSpan.FromSeconds(20);
                //System.Runtime.Remoting.Lifetime.LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(10);
                //System.Runtime.Remoting.Lifetime.LifetimeServices.SponsorshipTimeout = TimeSpan.FromSeconds(10);/**/
            }
            catch (System.Exception Error)
            {

                //TODO prone γεμισει με message το log file τοτε παράγει exception
                if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                    System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "PersistencySystem";
                System.Diagnostics.Debug.WriteLine(
                    Error.Message + Error.StackTrace);
                //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                throw Error;


                int hh = 0;

            }

        }

        void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Exception error = e.ExceptionObject as System.Exception;
            if (error != null)
            {
                if (!System.Diagnostics.EventLog.SourceExists("TransactionSystem", "."))
                {
                    System.Diagnostics.EventLog.CreateEventSource("TransactionSystem", "OOTransactionSystem");
                }

                //TODO γεμισει με message το log file τοτε παράγει exception
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "TransactionSystem";
                System.Diagnostics.Debug.WriteLine(
                    error.Message + error.StackTrace);
                //TODO να διαχειρίζωμε σωστά το log file μηπως και κάνει overflow
                myLog.WriteEntry(error.Message + error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                System.Exception InerError = error.InnerException;
                while (InerError != null)
                {
                    System.Diagnostics.Debug.WriteLine(
                        InerError.Message + InerError.StackTrace);
                    myLog.WriteEntry(InerError.Message + InerError.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                    InerError = InerError.InnerException;
                }
            }

            
        }

        protected override void OnStop()
        {
            AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(UnhandledException);
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
