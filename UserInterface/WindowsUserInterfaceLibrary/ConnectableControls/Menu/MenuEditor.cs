using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConnectableControls.Menus
{
    /// <MetaDataID>{ab75440a-2151-4f47-93b4-f29f42ac2013}</MetaDataID>
    public partial class MenuEditor : Form
    {
        public MenuEditor()
        {
            
            InitializeComponent();
            topMenu.EditMenu += new EventHandler(EditMenu);
            ConnectableControls.Menus.MenuCommand mainMenu = new ConnectableControls.Menus.MenuCommand("Menu");
            topMenu.MenuCommands.Add(mainMenu);
            ConnectableControls.Menus.MenuCommand menuCommand = new ConnectableControls.Menus.MenuCommand("ButtonColumn");
            menuCommand.Click += new EventHandler(OnMenuClick);
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("CheckBoxColumn");
            menuCommand.Click += new EventHandler(OnMenuClick);
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("ColorColumn");
            menuCommand.Click += new EventHandler(OnMenuClick);
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("ComboBoxColumn");
            menuCommand.Click += new EventHandler(OnMenuClick);
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("DateTimeColumn");
            menuCommand.Click += new EventHandler(OnMenuClick);
            mainMenu.MenuCommands.Add(menuCommand);
            menuCommand = new ConnectableControls.Menus.MenuCommand("ImageColumn");
            menuCommand.Click += new EventHandler(OnMenuClick);
            mainMenu.MenuCommands.Add(menuCommand);
            MenuDesigner.Controls.Add(topMenu);
            topMenu.Dock = DockStyle.Top;
            topMenu.DesignMode = true;


        }
        public MenuEditor(ConnectableControls.Menus.MenuCommand menuCommand)
        {

            InitializeComponent();
            
            topMenu.MenuCommands.Add(menuCommand);
            topMenu.Dock = DockStyle.Top;
            topMenu.DesignMode = true;
            MenuDesigner.Controls.Add(topMenu);
            topMenu.EditMenu += new EventHandler(EditMenu);
            if (menuCommand.MenuCommands.Count == 0)
                menuCommand.MenuCommands.Add(new CreateMenuCommand("Type Here"));
            

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (topMenu.MenuCommands.Count > 0)
                topMenu.MenuCommands[0].RemoveCreateMenuCommands();


        }

            ConnectableControls.Menus.MenuControl topMenu = new ConnectableControls.Menus.MenuControl(true);
        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

 
        }

        void EditMenu(object sender, EventArgs e)
        {
            //MenuPropertyGrid.SelectedObject = null;
            if (!(sender is ConnectableControls.Menus.CreateMenuCommand))
                MenuPropertyGrid.SelectedObject = sender as ConnectableControls.Menus.MenuCommand;
        }

        void OnMenuClick(object sender, EventArgs e)
        {


            
        }

        private void MenuPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MenuPropertyGrid.Refresh();
            

        }
    }
}