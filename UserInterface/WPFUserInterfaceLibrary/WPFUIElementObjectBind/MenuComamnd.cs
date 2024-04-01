using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{b6d1ccd6-533c-456d-99cd-debc18634dca}</MetaDataID>
    public class MenuCommand : MarshalByRefObject, OOAdvantech.UserInterface.Runtime.IConnectedCommand, INotifyPropertyChanged
    {

        public void InsertMenucommand(int index, MenuCommand menuCommand)
        {
        }

       public object Tag { get; set; }

        string OOAdvantech.UserInterface.Runtime.IConnectedCommand.Name
        {
            get => "";
        }

        public void AddMenucommand(MenuCommand menuCommand)
        {
            _SubMenuCommands.Add(menuCommand);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubMenuCommands)));
        }
        public void RemoveMenucommand(MenuCommand menuCommand)
        {
            _SubMenuCommands.Remove(menuCommand);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubMenuCommands)));
        }
        List<MenuCommand> _SubMenuCommands = new List<MenuCommand>();
        public List<MenuCommand> SubMenuCommands { get => _SubMenuCommands.ToList(); set => _SubMenuCommands = value; }

        System.Windows.Input.ICommand _Command;
        public System.Windows.Input.ICommand Command
        {
            get
            {
                return _Command;
            }
            set
            {
                _Command = value;

            }
        }


        private List<MenuCommand> GetTreeSubCommands()
        {
            var subMenuCommands = SubMenuCommands.Where(x => x != null).ToList();
            foreach (MenuCommand menuCommand in subMenuCommands.ToList())
                subMenuCommands.AddRange(menuCommand.GetTreeSubCommands());

            return subMenuCommands;
        }

        string _ToolTipText;
        public string ToolTipText
        {
            get
            {
                return _ToolTipText;
            }
            set
            {
                _ToolTipText = value;
            }
        }

        string _Header;
        public string Header
        {
            get
            {
                return _Header;
            }
            set
            {
                _Header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Header)));
            }
        }

        System.Windows.Controls.Image _Icon;
        public System.Windows.Controls.Image Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
            }
        }

        System.Windows.Media.ImageSource _ImageSource;
        public System.Windows.Media.ImageSource ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                _ImageSource = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public override string ToString()
        {
            return Header;
        }

        public Visibility ToolTipVisibility
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ToolTipText))
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public string DataTemplateStaticResource { get; set; }

        OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection;

        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get => _UserInterfaceObjectConnection;
            set
            {
                _UserInterfaceObjectConnection = value;
                List<OOAdvantech.UserInterface.Runtime.IConnectedCommand> cmds = GetTreeSubCommands().Where(x=>x.UserInterfaceObjectConnection==null).Select(x => x.Command).OfType<OOAdvantech.UserInterface.Runtime.IConnectedCommand>().ToList();
                foreach (var connectedCommand in cmds)
                    connectedCommand.UserInterfaceObjectConnection = value;

            }
        }

        public object Parameter { get; set; }
    }


    /// <MetaDataID>{cf4c2a54-1a9e-45ed-ba66-3dc2a8be2cbc}</MetaDataID>
    public class ToolBarItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ButtonTemplate { get; set; }
        public DataTemplate SeparatorTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return SeparatorTemplate;
            if (item is MenuCommand)
            {
                if (!string.IsNullOrWhiteSpace((item as MenuCommand).DataTemplateStaticResource))
                {
                    DataTemplate template = (container as FrameworkElement).FindResource((item as MenuCommand).DataTemplateStaticResource) as DataTemplate;
                    if (template != null)
                        return template;
                }



            }
            return ButtonTemplate;

            //var toolBarItem = (ToolBarItemViewModel)item;
            //Debug.Assert(toolBarItem != null);
            //if (!toolBarItem.IsSeparator)
            //{
            //    return ButtonTemplate;
            //}
            //return SeparatorTemplate;
        }
    }


}
