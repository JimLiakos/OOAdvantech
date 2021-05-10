using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using OOAdvantech.Transactions;
using OOAdvantech.UserInterface.Runtime;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{dc38e50a-fc07-4837-a07f-2b764573a77e}</MetaDataID>
    public class CommandBinding : System.Windows.Input.CommandBinding
    {

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Input.CommandBinding class by
        //     using the specified System.Windows.Input.ICommand and the specified System.Windows.Input.CommandBinding.Executed
        //     event handler.
        //
        // Parameters:
        //   command:
        //     The command to base the new System.Windows.Input.RoutedCommand on.
        //
        //   executed:
        //     The handler for the System.Windows.Input.CommandBinding.Executed event on the
        //     new System.Windows.Input.RoutedCommand.
        public CommandBinding(ICommand command, ExecutedRoutedEventHandler executed)
            : base(command, new CommandConsumer(executed).Command_Executed)
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Input.CommandBinding class by
        //     using the specified System.Windows.Input.ICommand and the specified System.Windows.Input.CommandBinding.Executed
        //     and System.Windows.Input.CommandBinding.CanExecute event handlers.
        //
        // Parameters:
        //   command:
        //     The command to base the new System.Windows.Input.RoutedCommand on.
        //
        //   executed:
        //     The handler for the System.Windows.Input.CommandBinding.Executed event on the
        //     new System.Windows.Input.RoutedCommand.
        //
        //   canExecute:
        //     The handler for the System.Windows.Input.CommandBinding.CanExecute event on the
        //     new System.Windows.Input.RoutedCommand.
        public CommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute)
            : base(command, new CommandConsumer(executed).Command_Executed, new CommandConsumer(canExecute).Command_Enabled)
        {

        }



        //private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        //{

        //}

        ///// <MetaDataID>{1b5d5f8f-6649-490c-bd2b-025199a018a5}</MetaDataID>
        //private void Command_Enabled(object sender, CanExecuteRoutedEventArgs e)
        //{

        //}


    }

    /// <MetaDataID>{2fdfb06a-cc2d-4973-9d49-9b3cd5985d90}</MetaDataID>
    class CommandConsumer
    {

        ExecutedRoutedEventHandler Executed;
        CanExecuteRoutedEventHandler CanExecute;
        internal CommandConsumer(ExecutedRoutedEventHandler executed)
        {
            Executed = executed;
        }
        internal CommandConsumer(CanExecuteRoutedEventHandler canExecute)
        {
            CanExecute = canExecute;
        }
        public void CallExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Executed(sender, e);
            }
            catch (Exception error)
            {

            }
        }
        internal void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if ((sender as FrameworkElement).GetObjectContext() is ObjectContext)
                    {
                        (sender as FrameworkElement).GetObjectContext().FormObjectConnection.Invoke(this, GetType().GetMethod("CallExecuted"), new object[2] { sender, e });
                    }
                    else if (UIProxy.GetUIProxy((sender as FrameworkElement).DataContext) != null)
                    {
                        UIProxy UIProxy = UIProxy.GetUIProxy((sender as FrameworkElement).DataContext);
                        if (UIProxy != null && UIProxy.UserInterfaceObjectConnection != null)
                            UIProxy.UserInterfaceObjectConnection.Invoke(this, GetType().GetMethod("CallExecuted"), new object[2] { sender, e });
                        else
                        {
                            ///TODO: να ενημερωθεί ο developer μεσω diagnostic.debug ...
                        }
                    }
                    stateTransition.Consistent = true;
                }


            }
            else
                CallExecuted(sender, e);

        }

        /// <MetaDataID>{1b5d5f8f-6649-490c-bd2b-025199a018a5}</MetaDataID>
        internal void Command_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    if ((sender as FrameworkElement).GetObjectContext() is ObjectContext)
                    {
                        if ((sender as FrameworkElement).GetObjectContext().FormObjectConnection.State == ViewControlObjectState.DesigneMode)
                            return;

                        if((sender as FrameworkElement).GetObjectContext().FormObjectConnection.State==ViewControlObjectState.Passive)
                        {
                            e.CanExecute = false;
                            return;
                        }

                       (sender as FrameworkElement).GetObjectContext().FormObjectConnection.Invoke(this, GetType().GetMethod("CallCanExecute"), new object[2] { sender, e });
                    }
                    else if (UIProxy.GetUIProxy((sender as FrameworkElement).DataContext) != null)
                    {
                        UIProxy UIProxy = UIProxy.GetUIProxy((sender as FrameworkElement).DataContext);
                        if (UIProxy != null && UIProxy.UserInterfaceObjectConnection != null)
                            UIProxy.UserInterfaceObjectConnection.Invoke(this, GetType().GetMethod("CallCanExecute"), new object[2] { sender, e });
                        else
                        {
                            ///TODO: να ενημερωθεί ο developer μεσω diagnostic.debug ...
                        }
                    }
                    stateTransition.Consistent = true;
                }

            }
            else
                CallCanExecute(sender, e);
        }

        public void CallCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                CanExecute(sender, e);
            }
            catch (Exception error)
            {


            }
        }

    }


    /// <MetaDataID>{8b949794-fc7e-4ebc-96a4-a57b0bfa80f8}</MetaDataID>
    public class RelayCommand : ICommand, IConnectedCommand
    {
        /// <MetaDataID>{b7f14da1-981e-4530-9f1f-7afd18bad9fe}</MetaDataID>
        private Action<object> execute;
        /// <MetaDataID>{134bc83e-60ac-49bb-bac8-2357c72af346}</MetaDataID>
        private Func<object, bool> canExecute;

        event EventHandler _CanExecuteChanged;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                _CanExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                _CanExecuteChanged -= value;
            }
        }
        public void Refresh()
        {
            _CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        }

        /// <MetaDataID>{f7adda9b-ee52-427f-aa5f-4cc4d00c0b8e}</MetaDataID>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <MetaDataID>{37bf5e7b-4059-40c5-8bed-81d36430af29}</MetaDataID>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
                return true;
            else
            {
                if (UserInterfaceObjectConnection != null)
                {


                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        if (UserInterfaceObjectConnection.State == ViewControlObjectState.Passive)
                            return false;
                        return (bool)UserInterfaceObjectConnection.Invoke(this, GetType().GetMethod("CallCanExecute"), new object[1] { parameter }, TransactionOption.Supported);
                    }

                }
                else
                    return this.canExecute(parameter);
            }
        }


        /// <MetaDataID>{a359a6a4-bb51-4166-931c-f187acd2f346}</MetaDataID>
        public void Execute(object parameter)
        {
            if (UserInterfaceObjectConnection != null)
                UserInterfaceObjectConnection.Invoke(this, GetType().GetMethod("CallExecute"), new object[1] { parameter }, TransactionOption.Supported);
            else
                this.execute(parameter);
        }


        string IConnectedCommand.Name
        {
            get => "";
        }


        /// <MetaDataID>{0eb56279-5107-4828-a354-3d532024ac90}</MetaDataID>
        UserInterfaceObjectConnection _UserInterfaceObjectConnection;
        /// <MetaDataID>{4ab250b6-8fc9-41ad-8493-695e4045be10}</MetaDataID>
        public UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;
            }
            set
            {
                _UserInterfaceObjectConnection = value;
            }
        }

        /// <MetaDataID>{29f7eedd-5036-4cbd-90e1-d96598492920}</MetaDataID>
        public void CallExecute(object parameter)
        {

            try
            {
                this.execute(parameter);
            }
            catch (Exception error)
            {
            }
        }

        public bool CallCanExecute(object parameter)
        {

            try
            {
                return this.canExecute(parameter);
            }
            catch (Exception error)
            {
                return false;
            }
        }
    }

    /// <MetaDataID>{b5cb83fb-ebdc-45d4-ae49-3b1aa88996c6}</MetaDataID>
    public class RoutedCommand : System.Windows.Input.RoutedCommand, IConnectedCommand
    {



        /// <MetaDataID>{b7f14da1-981e-4530-9f1f-7afd18bad9fe}</MetaDataID>
        private Action<object> execute;
        /// <MetaDataID>{134bc83e-60ac-49bb-bac8-2357c72af346}</MetaDataID>
        private Func<object, bool> canExecute;

        event EventHandler _CanExecuteChanged;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                _CanExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                _CanExecuteChanged -= value;
            }
        }
        public void Refresh()
        {
            _CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        }
        public readonly string CommandName;


        string IConnectedCommand.Name
        {
            get => CommandName;
        }



        /// <MetaDataID>{f7adda9b-ee52-427f-aa5f-4cc4d00c0b8e}</MetaDataID>
        public RoutedCommand(Action<object> execute, Func<object, bool> canExecute = null, string name = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            CommandName = name;
        }

        public RoutedCommand(IList<InputGesture> inputGestures, Action<object> execute, Func<object, bool> canExecute = null, string name = null)
        {
            CommandName = name;
            this.execute = execute;
            this.canExecute = canExecute;
            foreach (var inputGesture in inputGestures)
            {
                this.InputGestures.Add(inputGesture);
            }
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                Execute(e.Parameter);
                stateTransition.Consistent = true;
            }

        }
        private void Command_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                e.CanExecute = CanExecute(e.Parameter);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{37bf5e7b-4059-40c5-8bed-81d36430af29}</MetaDataID>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
                return true;
            else
            {
                if (UserInterfaceObjectConnection != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        if (UserInterfaceObjectConnection.State == ViewControlObjectState.Passive)
                            return false;
                        return (bool)UserInterfaceObjectConnection.Invoke(this, GetType().GetMethod("CallCanExecute"), new object[1] { parameter }, TransactionOption.Supported);
                    }
                }
                else
                    return this.canExecute(parameter);
            }
        }


        /// <MetaDataID>{a359a6a4-bb51-4166-931c-f187acd2f346}</MetaDataID>
        public void Execute(object parameter)
        {
            if (UserInterfaceObjectConnection != null)
                UserInterfaceObjectConnection.Invoke(this, GetType().GetMethod("CallExecute"), new object[1] { parameter }, TransactionOption.Supported);
            else
                this.execute(parameter);
        }


        /// <MetaDataID>{0eb56279-5107-4828-a354-3d532024ac90}</MetaDataID>
        UserInterfaceObjectConnection _UserInterfaceObjectConnection;
        /// <MetaDataID>{4ab250b6-8fc9-41ad-8493-695e4045be10}</MetaDataID>
        public UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return _UserInterfaceObjectConnection;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(CommandName))
                {

                }
                if (_UserInterfaceObjectConnection != value)
                {
                    _UserInterfaceObjectConnection = value;
                    if (_UserInterfaceObjectConnection.ContainerControl is UIElement && Window.GetWindow((_UserInterfaceObjectConnection.ContainerControl as UIElement)) != null)
                        Window.GetWindow((_UserInterfaceObjectConnection.ContainerControl as UIElement)).CommandBindings.Add(new WPFUIElementObjectBind.CommandBinding(this, Command_Executed, Command_Enabled));
                    else
                    {
                        if (!(_UserInterfaceObjectConnection.ContainerControl as FrameworkElement).IsLoaded)
                            (_UserInterfaceObjectConnection.ContainerControl as FrameworkElement).Loaded += RoutedCommand_Loaded;
                    }
                }
            }
        }

        private void RoutedCommand_Loaded(object sender, RoutedEventArgs e)
        {
            if (_UserInterfaceObjectConnection.ContainerControl is UIElement && Window.GetWindow((_UserInterfaceObjectConnection.ContainerControl as UIElement)) != null)
                Window.GetWindow((_UserInterfaceObjectConnection.ContainerControl as UIElement)).CommandBindings.Add(new WPFUIElementObjectBind.CommandBinding(this, Command_Executed, Command_Enabled));

        }

        /// <MetaDataID>{29f7eedd-5036-4cbd-90e1-d96598492920}</MetaDataID>
        public void CallExecute(object parameter)
        {

            try
            {
                this.execute(parameter);
            }
            catch (Exception error)
            {
            }
        }

        public bool CallCanExecute(object parameter)
        {

            try
            {
                return this.canExecute(parameter);
            }
            catch (Exception error)
            {
                return false;
            }
        }
    }
}
