namespace VSMetadataRepositoryBrowser.RDBMSMapping
{
    partial class ProjectView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{000bae14-f39b-4340-81f1-b7f85599d1fa}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{399b861b-100d-4d46-a625-cbad022aed40}</MetaDataID>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        /// <MetaDataID>{cbca63e2-3837-4f40-82b3-1e0453496bd0}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectView));
            this.ProjectTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.MappingContext = new DevExpress.XtraTab.XtraTabPage();
            this.MappingContextList = new ConnectableControls.List.ListView();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectTabControl)).BeginInit();
            this.ProjectTabControl.SuspendLayout();
            this.MappingContext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MappingContextList)).BeginInit();
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.ViewControlObjectAssembly = "CodeMetaDataRepository";
            this.Connection.ViewControlObjectType = "OOAdvantech.CodeMetaDataRepository.Project";
            // 
            // ProjectTabControl
            // 
            this.ProjectTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectTabControl.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.ProjectTabControl.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.ProjectTabControl.Location = new System.Drawing.Point(20, 16);
            this.ProjectTabControl.Name = "ProjectTabControl";
            this.ProjectTabControl.SelectedTabPage = this.MappingContext;
            this.ProjectTabControl.Size = new System.Drawing.Size(589, 272);
            this.ProjectTabControl.TabIndex = 0;
            this.ProjectTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.MappingContext,
            this.xtraTabPage2});
            // 
            // MappingContext
            // 
            this.MappingContext.Controls.Add(this.MappingContextList);
            this.MappingContext.Name = "MappingContext";
            this.MappingContext.Size = new System.Drawing.Size(486, 265);
            this.MappingContext.Text = "Mapping Context";
            // 
            // MappingContextList
            // 
            this.MappingContextList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MappingContextList.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.MappingContextList.EnabledProperty.Path = null;
            this.MappingContextList.FocusedBackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.MappingContextList.ListConnection.AllowDrag = false;
            this.MappingContextList.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.MappingContextList.ListConnection.AssignPresentationObjectType = "";
            this.MappingContextList.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.MappingContextList.ListConnection.ConnectedObjectAutoUpdate = false;
            this.MappingContextList.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.MappingContextList.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.MappingContextList.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.MappingContextList.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.MappingContextList.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.MappingContextList.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.MappingContextList.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.MappingContextList.ListConnection.Path = "RDBMSMappingContexts";
            this.MappingContextList.ListConnection.SelectionMember = null;
            this.MappingContextList.ListConnection.ViewControlObject = this.Connection;
            this.MappingContextList.Location = new System.Drawing.Point(25, 23);
            this.MappingContextList.Name = "MappingContextList";
            this.MappingContextList.RowHeight = 15;
            this.MappingContextList.Size = new System.Drawing.Size(436, 218);
            this.MappingContextList.TabIndex = 1;
            this.MappingContextList.Text = "ContextsList";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(486, 265);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // ProjectView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ProjectTabControl);
            this.Name = "ProjectView";
            this.Size = new System.Drawing.Size(628, 305);
            ((System.ComponentModel.ISupportInitialize)(this.ProjectTabControl)).EndInit();
            this.ProjectTabControl.ResumeLayout(false);
            this.MappingContext.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MappingContextList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl ProjectTabControl;
        private DevExpress.XtraTab.XtraTabPage MappingContext;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private ConnectableControls.List.ListView MappingContextList;



    }
}
