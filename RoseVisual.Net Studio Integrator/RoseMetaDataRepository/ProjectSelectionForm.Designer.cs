namespace RoseMetaDataRepository
{
    partial class ProjectSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectSelectionForm));
            this.SolutionLabel = new System.Windows.Forms.Label();
            this.ProjectLabel = new System.Windows.Forms.Label();
            this.SolutionFileLabel = new System.Windows.Forms.Label();
            this.ProjectFileLabel = new System.Windows.Forms.Label();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.Solutions = new ConnectableControls.ComboBox();
            this.SolutionFileName = new ConnectableControls.TextBox();
            this.Projects = new ConnectableControls.ComboBox();
            this.ProjectFileName = new ConnectableControls.TextBox();
            this.OpenSolution = new ConnectableControls.Button();
            this.SuspendLayout();
            // 
            // SolutionLabel
            // 
            this.SolutionLabel.Location = new System.Drawing.Point(2, 18);
            this.SolutionLabel.Name = "SolutionLabel";
            this.SolutionLabel.Size = new System.Drawing.Size(70, 22);
            this.SolutionLabel.TabIndex = 1;
            this.SolutionLabel.Text = "Solution ";
            this.SolutionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ProjectLabel
            // 
            this.ProjectLabel.Location = new System.Drawing.Point(2, 76);
            this.ProjectLabel.Name = "ProjectLabel";
            this.ProjectLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ProjectLabel.Size = new System.Drawing.Size(70, 22);
            this.ProjectLabel.TabIndex = 3;
            this.ProjectLabel.Text = "Project";
            this.ProjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SolutionFileLabel
            // 
            this.SolutionFileLabel.Location = new System.Drawing.Point(2, 47);
            this.SolutionFileLabel.Name = "SolutionFileLabel";
            this.SolutionFileLabel.Size = new System.Drawing.Size(70, 22);
            this.SolutionFileLabel.TabIndex = 4;
            this.SolutionFileLabel.Text = "Solution File";
            this.SolutionFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SolutionFileLabel.Click += new System.EventHandler(this.SolutionFileLabel_Click);
            // 
            // ProjectFileLabel
            // 
            this.ProjectFileLabel.AllowDrop = true;
            this.ProjectFileLabel.Location = new System.Drawing.Point(2, 106);
            this.ProjectFileLabel.Name = "ProjectFileLabel";
            this.ProjectFileLabel.Size = new System.Drawing.Size(70, 22);
            this.ProjectFileLabel.TabIndex = 6;
            this.ProjectFileLabel.Text = "Project File";
            this.ProjectFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "RoseMetaDataRepository.ComponentPresentation";
            this.Connection.ContainerControl = this;
            this.Connection.CreatePresentationObjectAnyway = false;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.IniateTransactionOnInstanceSet = false;
            this.Connection.MasterViewControlObject = null;
            this.Connection.Name = "Connection";
            this.Connection.RollbackOnExitWithoutAnswer = false;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.Connection.ViewControlObjectAssembly = null;
            this.Connection.ViewControlObjectType = "RoseMetaDataRepository.Component";
            // 
            // Solutions
            // 
            this.Solutions.AllowDrag = false;
            this.Solutions.AllowDropOperationCall = ((object)(resources.GetObject("Solutions.AllowDropOperationCall")));
            this.Solutions.AssignPresentationObjectType = "RoseMetaDataRepository.SolutionPresentation";
            this.Solutions.AutoDisable = true;
            this.Solutions.AutoInsert = false;
            this.Solutions.AutoSuggest = false;
            this.Solutions.ChooseFromEnum = false;
            this.Solutions.ConnectedObjectAutoUpdate = false;
            this.Solutions.DisplayMember = "Name";
            this.Solutions.DragDropOperationCall = ((object)(resources.GetObject("Solutions.DragDropOperationCall")));
            this.Solutions.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Solutions.EnableCondition = null;
            // 
            // 
            // 
            this.Solutions.EnableProperty.Path = null;
            this.Solutions.Enumeration = "";
            this.Solutions.FormattingEnabled = true;
            this.Solutions.InsertOperationCall = ((object)(resources.GetObject("Solutions.InsertOperationCall")));
            this.Solutions.IntegralHeight = false;
            this.Solutions.Location = new System.Drawing.Point(79, 21);
            this.Solutions.Name = "Solutions";
            this.Solutions.NullValueName = "";
            this.Solutions.OperationCall = ((object)(resources.GetObject("Solutions.OperationCall")));
            this.Solutions.Path = "Solution";
            this.Solutions.PreLoaded = true;
            this.Solutions.RemoveOperationCall = ((object)(resources.GetObject("Solutions.RemoveOperationCall")));
            this.Solutions.Size = new System.Drawing.Size(215, 21);
            this.Solutions.Sorted = true;
            this.Solutions.TabIndex = 7;
            this.Solutions.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Solutions.ViewControlObject = this.Connection;
            this.Solutions.WarnigMessageOnRemove = null;
            // 
            // SolutionFileName
            // 
            this.SolutionFileName.AllowDrag = false;
            this.SolutionFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SolutionFileName.AutoDisable = true;
            this.SolutionFileName.BackColor = System.Drawing.SystemColors.Control;
            this.SolutionFileName.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.SolutionFileName.EnableProperty.Path = null;
            this.SolutionFileName.Location = new System.Drawing.Point(79, 51);
            this.SolutionFileName.Name = "SolutionFileName";
            this.SolutionFileName.Path = "SolutionFileName";
            this.SolutionFileName.Size = new System.Drawing.Size(279, 20);
            this.SolutionFileName.TabIndex = 8;
            this.SolutionFileName.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.SolutionFileName.ViewControlObject = this.Connection;
            // 
            // Projects
            // 
            this.Projects.AllowDrag = false;
            this.Projects.AllowDropOperationCall = ((object)(resources.GetObject("Projects.AllowDropOperationCall")));
            this.Projects.AssignPresentationObjectType = "";
            this.Projects.AutoDisable = true;
            this.Projects.AutoInsert = false;
            this.Projects.AutoSuggest = false;
            this.Projects.ChooseFromEnum = false;
            this.Projects.ConnectedObjectAutoUpdate = false;
            this.Projects.DisplayMember = "Name";
            this.Projects.DragDropOperationCall = ((object)(resources.GetObject("Projects.DragDropOperationCall")));
            this.Projects.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Projects.EnableCondition = null;
            // 
            // 
            // 
            this.Projects.EnableProperty.Path = null;
            this.Projects.Enumeration = "";
            this.Projects.FormattingEnabled = true;
            this.Projects.InsertOperationCall = ((object)(resources.GetObject("Projects.InsertOperationCall")));
            this.Projects.IntegralHeight = false;
            this.Projects.Location = new System.Drawing.Point(79, 80);
            this.Projects.Name = "Projects";
            this.Projects.NullValueName = "";
            this.Projects.OperationCall = ((object)(resources.GetObject("Projects.OperationCall")));
            this.Projects.Path = "Project";
            this.Projects.PreLoaded = true;
            this.Projects.RemoveOperationCall = ((object)(resources.GetObject("Projects.RemoveOperationCall")));
            this.Projects.Size = new System.Drawing.Size(215, 21);
            this.Projects.Sorted = true;
            this.Projects.TabIndex = 9;
            this.Projects.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Projects.ViewControlObject = this.Connection;
            this.Projects.WarnigMessageOnRemove = null;
            // 
            // ProjectFileName
            // 
            this.ProjectFileName.AllowDrag = false;
            this.ProjectFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectFileName.AutoDisable = true;
            this.ProjectFileName.BackColor = System.Drawing.SystemColors.Control;
            this.ProjectFileName.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.ProjectFileName.EnableProperty.Path = null;
            this.ProjectFileName.Location = new System.Drawing.Point(79, 110);
            this.ProjectFileName.Name = "ProjectFileName";
            this.ProjectFileName.Path = "ProjectFileName";
            this.ProjectFileName.Size = new System.Drawing.Size(312, 20);
            this.ProjectFileName.TabIndex = 10;
            this.ProjectFileName.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.ProjectFileName.ViewControlObject = this.Connection;
            // 
            // OpenSolution
            // 
            this.OpenSolution.AllowDrag = false;
            this.OpenSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenSolution.BackColor = System.Drawing.SystemColors.Control;
            this.OpenSolution.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OpenSolution.BackgroundImage")));
            this.OpenSolution.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.OpenSolution.ConnectedObjectAutoUpdate = false;
            this.OpenSolution.ImageKey = "(none)";
            this.OpenSolution.Location = new System.Drawing.Point(362, 47);
            this.OpenSolution.Name = "OpenSolution";
            this.OpenSolution.OnClickOperationCall = ((object)(resources.GetObject("OpenSolution.OnClickOperationCall")));
            this.OpenSolution.Path = "";
            this.OpenSolution.SaveButton = false;
            this.OpenSolution.Size = new System.Drawing.Size(29, 22);
            this.OpenSolution.TabIndex = 11;
            // 
            // 
            // 
            this.OpenSolution.TextProperty.Path = null;
            this.OpenSolution.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.OpenSolution.UseVisualStyleBackColor = false;
            this.OpenSolution.Value = null;
            this.OpenSolution.ViewControlObject = this.Connection;
            // 
            // ProjectSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(406, 136);
            this.Controls.Add(this.OpenSolution);
            this.Controls.Add(this.ProjectFileName);
            this.Controls.Add(this.Projects);
            this.Controls.Add(this.SolutionFileName);
            this.Controls.Add(this.Solutions);
            this.Controls.Add(this.ProjectFileLabel);
            this.Controls.Add(this.SolutionFileLabel);
            this.Controls.Add(this.ProjectLabel);
            this.Controls.Add(this.SolutionLabel);
            this.Name = "ProjectSelectionForm";
            this.ShowInTaskbar = false;
            this.Text = "Select Visual Studio Code Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private System.Windows.Forms.Label SolutionLabel;
        private System.Windows.Forms.Label ProjectLabel;
        
        private System.Windows.Forms.Label SolutionFileLabel;


        private System.Windows.Forms.Label ProjectFileLabel;
        private ConnectableControls.ComboBox Solutions;
        private ConnectableControls.TextBox SolutionFileName;
        private ConnectableControls.ComboBox Projects;
        private ConnectableControls.TextBox ProjectFileName;
        public ConnectableControls.FormConnectionControl Connection;
        private ConnectableControls.Button OpenSolution;
    }
}