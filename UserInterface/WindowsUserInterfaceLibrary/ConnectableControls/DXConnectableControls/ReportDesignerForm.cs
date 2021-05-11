using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraReports.UserDesigner;
using System.Runtime.InteropServices;

namespace DXConnectableControls.XtraReports.Design
{
    /// <MetaDataID>{9e27dae8-6f18-4bd1-a69b-ad579e39550f}</MetaDataID>
    public partial class ReportDesignForm : XRDesignFormEx
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        DXConnectableControls.XtraReports.UI.Report _Report;

        public void NewReport()
        {
            _Report = new DXConnectableControls.XtraReports.UI.Report();
            DevExpress.XtraReports.UI.DetailBand detail = new DevExpress.XtraReports.UI.DetailBand();
            DevExpress.XtraReports.UI.PageHeaderBand pageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            DevExpress.XtraReports.UI.PageFooterBand pageFooter = new DevExpress.XtraReports.UI.PageFooterBand();

            pageFooter.Height = 30;
            pageHeader.Height = 30;
            _Report.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
                                                detail,
                                                pageHeader,
                                                pageFooter});

            if (IsWindow(Handle))
            {
                DesignPanel.CloseReport();
                DesignPanel.OpenReport(_Report);
            }

        }



        protected override void OnLoad(EventArgs e)
        {
            if (_Report != null)
            {
                DesignPanel.CloseReport();
                DesignPanel.OpenReport(_Report);
                
            }
            base.OnLoad(e);
        }


        internal static System.Collections.Generic.Dictionary<DevExpress.XtraReports.UI.XtraReport, string> ReportFileNames = new Dictionary<DevExpress.XtraReports.UI.XtraReport, string>();
        public ReportDesignForm()
        {
            DesignPanel.AddCommandHandler(new CommandHandler(DesignPanel));
        }
        public UI.Report Report
        {
            get
            {
                return DesignPanel.Report as UI.Report;
            }
        }

        public new void OpenReport(string fileName)
        {

            if (DesignPanel.Report != null && ReportFileNames.ContainsKey(DesignPanel.Report))
                ReportFileNames.Remove(DesignPanel.Report);
            DXConnectableControls.XtraReports.UI.Report report = DXConnectableControls.XtraReports.UI.Report.FromFile(fileName, true);
            ReportFileNames[report] = fileName;
            OpenReport(report);
        }
        protected override void OnClosed(EventArgs e)
        {
            if (DesignPanel.Report != null && ReportFileNames.ContainsKey(DesignPanel.Report))
                ReportFileNames.Remove(DesignPanel.Report);

            base.OnClosed(e);
        }

        #region private void InitializeComponent()
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // xrDesignPanel
            // 
            this.xrDesignPanel.Location = new System.Drawing.Point(92, 76);
            this.xrDesignPanel.Size = new System.Drawing.Size(346, 379);
            // 
            // ReportDesignForm
            // 
            this.ClientSize = new System.Drawing.Size(688, 478);
            this.Name = "ReportDesignForm";
            this.Controls.SetChildIndex(this.xrDesignPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.xrDesignPanel)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }



    /// <MetaDataID>{92243759-d259-46cc-a85c-2e52733b6a02}</MetaDataID>
    public class CommandHandler : ICommandHandler
    {
        XRDesignPanel DesignPanel;

        public CommandHandler(XRDesignPanel panel)
        {
            this.DesignPanel = panel;
        }

        public virtual void HandleCommand(ReportCommand command, object[] args, ref bool handled)
        {
            switch (command)
            {
                case ReportCommand.NewReport:
                case ReportCommand.NewReportWizard:
                    {

                        DXConnectableControls.XtraReports.UI.Report report = new DXConnectableControls.XtraReports.UI.Report();
                        DevExpress.XtraReports.UI.DetailBand detail = new DevExpress.XtraReports.UI.DetailBand();
                        DevExpress.XtraReports.UI.PageHeaderBand pageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
                        DevExpress.XtraReports.UI.PageFooterBand pageFooter = new DevExpress.XtraReports.UI.PageFooterBand();

                        pageFooter.Height = 30;
                        pageHeader.Height = 30;
                        report.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
                                                detail,
                                                pageHeader,
                                                pageFooter});
                        DesignPanel.OpenReport(report);

                    }
                    break;
                case ReportCommand.SaveFileAs:
                    {
                        SaveFileAs();
                        break;
                    }
                case ReportCommand.OpenFile:
                    {
                        OpenFile();
                        break;
                    }
                case ReportCommand.SaveFile:
                case ReportCommand.Closing:
                    Save();
                    break;
                case ReportCommand.ShowPreviewTab:
                case ReportCommand.ShowHTMLViewTab:

                    break;


                default:
                    handled = false;
                    break;
            }
            if (!CanHandleCommand(command)) return;

            // Save a report.
            // Save();

            // Set handled to true to avoid the standard saving procedure to be called.
            handled = true;
        }

        public virtual bool CanHandleCommand(ReportCommand command)
        {
            // This handler is used for SaveFile, SaveFileAs and Closing commands.
            return command == ReportCommand.NewReport ||
                command == ReportCommand.NewReportWizard ||
                command == ReportCommand.SaveFile ||
                command == ReportCommand.SaveFileAs ||
                command == ReportCommand.Closing ||
                command == ReportCommand.OpenFile;
        }

        #region private void Save()
        private void Save()
        {
            if (DesignPanel.ReportState == ReportState.Saved)
                return;
            if (DesignPanel.Report != null && ReportDesignForm.ReportFileNames.ContainsKey(DesignPanel.Report))
            {

                DesignPanel.Report.SaveLayout(ReportDesignForm.ReportFileNames[DesignPanel.Report]);

                // Prevent the "Report has been changed" dialog from being shown.
                DesignPanel.ReportState = ReportState.Saved;

            }
            else
            {
                SaveFileAs();
            }
        }
        #endregion

        #region private void SaveFileAs()
        private void SaveFileAs()
        {
            _SaveFileDialog = new SaveFileDialog();
            _SaveFileDialog.DefaultExt = "repx";
            _SaveFileDialog.Filter = "Report files(*.repx)|*.repx";
            if (_SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DesignPanel.Report.SaveLayout(_SaveFileDialog.FileName);//"c:\\report1.repx"
                if (DesignPanel.Report is DXConnectableControls.XtraReports.UI.Report)
                    (DesignPanel.Report as DXConnectableControls.XtraReports.UI.Report).FileName = _SaveFileDialog.FileName;
                ReportDesignForm.ReportFileNames[DesignPanel.Report] = _SaveFileDialog.FileName;// "c:\\report1.repx";
                // Prevent the "Report has been changed" dialog from being shown.
                DesignPanel.ReportState = ReportState.Saved;
                _SaveFileDialog.Dispose();
            }
        }
        #endregion

        private void OpenFile()
        {
            _OpenFileDialog = new OpenFileDialog();
            _OpenFileDialog.DefaultExt = "repx";
            _OpenFileDialog.Filter = "Report files(*.repx)|*.repx";
            if (_OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                DXConnectableControls.XtraReports.UI.Report report = DXConnectableControls.XtraReports.UI.Report.FromFile(_OpenFileDialog.FileName, true) as DXConnectableControls.XtraReports.UI.Report;
                DesignPanel.OpenReport(report);
                ReportDesignForm.ReportFileNames[DesignPanel.Report] = _OpenFileDialog.FileName;
                _OpenFileDialog.Dispose();
            }
        }

        private SaveFileDialog _SaveFileDialog;
        private OpenFileDialog _OpenFileDialog;
    }

}
