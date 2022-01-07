namespace UserInterfaceTest
{
    partial class ClientUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientUserControl));
            this.ClientSearchBox = new ConnectableControls.SearchTextBox();
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.ViewControlObjectType = "AbstractionsAndPersistency.IClient";
            // 
            // ClientSearchBox
            // 
            this.ClientSearchBox.AllowDrag = false;
            this.ClientSearchBox.AllowDropOperationCall = ((object)(resources.GetObject("ClientSearchBox.AllowDropOperationCall")));
            this.ClientSearchBox.AssignPresentationObjectType = "";
            this.ClientSearchBox.AutoDisable = true;
            this.ClientSearchBox.ConnectedObjectAutoUpdate = false;
            this.ClientSearchBox.DisplayMember = "Name";
            this.ClientSearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClientSearchBox.DragDropOperationCall = ((object)(resources.GetObject("ClientSearchBox.DragDropOperationCall")));
            this.ClientSearchBox.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ClientSearchBox.DropDownHeight = 106;
            // 
            // 
            // 
            this.ClientSearchBox.EnableProperty.Path = null;
            this.ClientSearchBox.InsertOperationCall = ((object)(resources.GetObject("ClientSearchBox.InsertOperationCall")));
            this.ClientSearchBox.Location = new System.Drawing.Point(0, 0);
            this.ClientSearchBox.Name = "ClientSearchBox";
            this.ClientSearchBox.OperationCall = ((object)(resources.GetObject("ClientSearchBox.OperationCall")));
            this.ClientSearchBox.Path = "(ViewControlObject)";
            this.ClientSearchBox.RemoveOperationCall = ((object)(resources.GetObject("ClientSearchBox.RemoveOperationCall")));
            this.ClientSearchBox.Size = new System.Drawing.Size(167, 19);
            this.ClientSearchBox.TabIndex = 0;
            this.ClientSearchBox.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.ClientSearchBox.ViewControlObject = this.Connection;
            // 
            // ClientUserControl
            // 
            this.Controls.Add(this.ClientSearchBox);
            this.Name = "ClientUserControl";
            this.Size = new System.Drawing.Size(167, 24);
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectableControls.SearchTextBox ClientSearchBox;

    }
}
