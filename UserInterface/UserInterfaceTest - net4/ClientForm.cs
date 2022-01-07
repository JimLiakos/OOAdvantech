using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UserInterfaceTest
{
    /// <MetaDataID>{c4855e4d-322e-4181-9fd7-22ed653f583c}</MetaDataID>
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        ConnectableControls.Menus.MenuControl topMenu=new ConnectableControls.Menus.MenuControl();
        private void ClientForm_Load(object sender, EventArgs e)
        {
            ConnectableControls.Menus.MenuCommand mainMenu = new ConnectableControls.Menus.MenuCommand("Menu");
            topMenu.MenuCommands.Add(mainMenu);
            ConnectableControls.Menus.MenuCommand menuCommand = new ConnectableControls.Menus.MenuCommand("ButtonColumn");
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("CheckBoxColumn");
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("ColorColumn");
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("ComboBoxColumn");
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("DateTimeColumn");
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("ImageColumn");
            mainMenu.MenuCommands.Add(menuCommand);

            Controls.Add(topMenu);
            topMenu.Dock = DockStyle.Top;
        }
    }
}