using System;
using System.Windows.Forms;
using System.Drawing;

namespace ConnectableControls.Menus.Docking
{
    
    delegate void MyDelegate(int i);

    public delegate void DragDropCommandEventHandler(object sender, ref ConnectableControls.Menus.MenuCommand MenuCommand, ConnectableControls.Menus.MenuCommand CommandContainer);
    public delegate void DragEnterEventHandler(object sender);

    /// <MetaDataID>{5d294022-55c5-4c5f-9fcf-e50adb20c0c2}</MetaDataID>
    public enum Side
    {
        None=0,
        Left=1,
        Right=2,
        Top=4,
        Bottom=8,
        All=15
    }
    /// <MetaDataID>{80E33DA9-D10B-4E8D-9531-BE69B24711AE}</MetaDataID>
    public class CommandBarDockingManager : IMessageFilter
    {
        /// <MetaDataID>{1712f72d-1b82-4409-87a2-a128d59f8eb3}</MetaDataID>
        public CommandBarDockingManager(Control hostContainerControl, Side side)
        {
            _HostContainerControl = hostContainerControl;
            DockingViewAreas =new System.Collections.ArrayList();
            Application.AddMessageFilter(this);

            if ((side &Side.Top)!=0)
            {
                TopDockingViewArea =new CommandBarDockingViewArea(this);
                TopDockingViewArea.Dock = DockStyle.Top;
                TopDockingViewArea.Height = 3;
                DockingViewAreas.Add(TopDockingViewArea);
                _HostContainerControl.Controls.Add(TopDockingViewArea);
                TopDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);
            }
            if ((side & Side.Left)!=0)
            {

                //TopDockingViewArea.BackColor=Color.Teal;
                LeftDockingViewArea =new CommandBarDockingViewArea(this);
                LeftDockingViewArea.Dock = DockStyle.Left;
                LeftDockingViewArea.Width = 3;
                _HostContainerControl.Controls.Add(LeftDockingViewArea);
                DockingViewAreas.Add(LeftDockingViewArea);
                LeftDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);

            }
            //LeftDockingViewArea.BackColor=Color.Teal;
            if ((side & Side.Right)!=0)
            {
                RightDockingViewArea =new CommandBarDockingViewArea(this);
                RightDockingViewArea.Dock = DockStyle.Right;
                RightDockingViewArea.Width = 3;
                _HostContainerControl.Controls.Add(RightDockingViewArea);
                DockingViewAreas.Add(RightDockingViewArea);
                RightDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);

            }
            //RightDockingViewArea.BackColor=Color.Teal;
            if ((side & Side.Bottom)!=0)
            {

                BottomRightDockingViewArea =new CommandBarDockingViewArea(this);
                BottomRightDockingViewArea.Dock = DockStyle.Bottom;
                BottomRightDockingViewArea.Height = 3;
                _HostContainerControl.Controls.Add(BottomRightDockingViewArea);
                DockingViewAreas.Add(BottomRightDockingViewArea);
                BottomRightDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);

            }
            //BottomRightDockingViewArea.BackColor=Color.Teal;


        }

        /// <MetaDataID>{7027FCE4-B04D-4572-9CE5-9EF89BC0C7F6}</MetaDataID>
        public CommandBarDockingViewArea TopDockingViewArea;
        /// <MetaDataID>{BFD69D0F-CE31-4BDC-ABD4-B004E963935B}</MetaDataID>
        public CommandBarDockingViewArea LeftDockingViewArea;
        /// <MetaDataID>{4CE6F288-CB4C-426A-BDAE-6F4D30C33178}</MetaDataID>
        public CommandBarDockingViewArea RightDockingViewArea;
        /// <MetaDataID>{A3C5FC9A-0F9A-4C75-A5C7-59B05082D947}</MetaDataID>
        public CommandBarDockingViewArea BottomRightDockingViewArea;
        /// <MetaDataID>{08A706E1-CC76-4EE7-9E5D-41C5A43977C1}</MetaDataID>
        System.Collections.ArrayList DockingViewAreas;

        /// <MetaDataID>{ADAB2433-3604-49E1-84D0-5E50FE725522}</MetaDataID>
        public event System.Windows.Forms.MouseEventHandler DragCommandMove;

        /// <MetaDataID>{17A0D98C-F829-407A-B7D6-474AA1F4F3A6}</MetaDataID>
        public event DragDropCommandEventHandler DragCommandDrop;
        /// <MetaDataID>{CD3137C4-D678-4656-8FFF-CE8B1045E6EA}</MetaDataID>
        public event DragEnterEventHandler DragEnter;


        /// <MetaDataID>{137C8B38-467D-493E-B26B-99EEA56DB041}</MetaDataID>
        private Control _HostContainerControl;
        ///// <MetaDataID>{80F2C716-E15A-43A6-B23C-3E5386B58661}</MetaDataID>
        //public Control HostContainerControl
        //{
        //    //set
        //    //{
        //    //    DockingViewAreas =new System.Collections.ArrayList();
        //    //    Application.AddMessageFilter(this);
        //    //    _HostContainerControl = value;
        //    //    TopDockingViewArea =new CommandBarDockingViewArea(this);
        //    //    TopDockingViewArea.Dock = DockStyle.Top;
        //    //    TopDockingViewArea.Height = 3;
        //    //    //TopDockingViewArea.BackColor=Color.Teal;
        //    //    LeftDockingViewArea =new CommandBarDockingViewArea(this);
        //    //    LeftDockingViewArea.Dock = DockStyle.Left;
        //    //    LeftDockingViewArea.Width = 3;
        //    //    //LeftDockingViewArea.BackColor=Color.Teal;
        //    //    RightDockingViewArea =new CommandBarDockingViewArea(this);
        //    //    RightDockingViewArea.Dock = DockStyle.Right;
        //    //    RightDockingViewArea.Width = 3;
        //    //    //RightDockingViewArea.BackColor=Color.Teal;
        //    //    BottomRightDockingViewArea =new CommandBarDockingViewArea(this);
        //    //    BottomRightDockingViewArea.Dock = DockStyle.Bottom;
        //    //    BottomRightDockingViewArea.Height = 3;
        //    //    //BottomRightDockingViewArea.BackColor=Color.Teal;


        //    //    _HostContainerControl.Controls.Add(TopDockingViewArea);
        //    //    _HostContainerControl.Controls.Add(LeftDockingViewArea);
        //    //    _HostContainerControl.Controls.Add(RightDockingViewArea);
        //    //    _HostContainerControl.Controls.Add(BottomRightDockingViewArea);
        //    //    DockingViewAreas.Add(TopDockingViewArea);
        //    //    DockingViewAreas.Add(LeftDockingViewArea);
        //    //    DockingViewAreas.Add(RightDockingViewArea);
        //    //    DockingViewAreas.Add(BottomRightDockingViewArea);
        //    //    TopDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);
        //    //    LeftDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);
        //    //    RightDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);
        //    //    BottomRightDockingViewArea.OnRightClick +=new EventHandler(OnCommandDockingAreaRightClick);

        //    //}

        //}
        /// <MetaDataID>{80E40D05-46B0-4F83-9841-544B728E4C65}</MetaDataID>
        private void OnCommandCollectionFormClosed(object sender, System.EventArgs e)
        {
            //ConnectableControls.Menus.CommandCollectionForm CommandCollectionForm=(ConnectableControls.Menus.CommandCollectionForm)sender;
            CommandCollectionForm.Closed -=new EventHandler(OnCommandCollectionFormClosed);
            CommandCollectionForm.DropCommand -=new EventHandler(OnDropCommand);
            CommandCollectionForm.DragCommandMove -=new MouseEventHandler(OnDragCommandMoveDispacher);
            CommandCollectionForm.DragCommandEnter -=new DragDropCommandEventHandler(OnDragCommandEnterDispacher);
            CommandCollectionForm = null;
            foreach (CommandBarDockingViewArea CurrDockingViewArea in DockingViewAreas)
            {
                foreach (Control CurrControl in CurrDockingViewArea.Controls)
                {
                    if (CurrControl is DockingCommandBar)
                        (CurrControl as DockingCommandBar).MenuControl.DesignMode = false;
                }
            }


        }
        /// <MetaDataID>{623A9389-5D99-4669-BEB8-00D424DCFC45}</MetaDataID>
        System.Collections.ArrayList GetChildControl(Control _Control)
        {
            System.Collections.ArrayList ChildControls =new System.Collections.ArrayList();
            if (_Control == null)
                return ChildControls;
            ChildControls.Add(_Control);
            foreach (Control CurrControl in _Control.Controls)
                ChildControls.AddRange(GetChildControl(CurrControl));
            return ChildControls;
        }
        /// <MetaDataID>{24919AC7-F737-479E-AA5C-9DEC086A9753}</MetaDataID>
        internal void OnDragCommandMoveDispacher(object sender, System.Windows.Forms.MouseEventArgs e)
        {





            if (DragCommandMove != null && DragEnterFlag)
            {
                //	System.Diagnostics.Debug.WriteLine("OnDragCommandMoveDispacher");
                //System.Diagnostics.Debug.WriteLine(Control.MousePosition );
                //_HostContainerControl.Cursor=CommandCollectionForm.Cursor;
                DragCommandMove(sender, e);
            }/**/

        }

        /// <MetaDataID>{749CD991-AEB5-4737-AAEF-4EE1586EF38E}</MetaDataID>
        ConnectableControls.Menus.MenuCommand _DragedCommand = null;
        /// <MetaDataID>{FB944453-6FEA-4A84-9B89-E280B74510FF}</MetaDataID>
        ConnectableControls.Menus.MenuCommand _CommandContainer = null;
        /// <MetaDataID>{764525F1-0A6F-4E64-8107-390CB84461C7}</MetaDataID>
        ConnectableControls.Menus.IMenuComandViewer _DragedCommandViewer = null;


        /// <MetaDataID>{C61E0848-8FDE-4553-B965-386FB34D5A7B}</MetaDataID>
        System.Collections.Hashtable ChildControls =new System.Collections.Hashtable();
        /// <MetaDataID>{37356459-CD36-422E-A2FB-206D481380ED}</MetaDataID>
        internal void OnDragCommandEnterDispacher(object sender, ref ConnectableControls.Menus.MenuCommand DragedCommand, ConnectableControls.Menus.MenuCommand CommandContainer)
        {

            _DragedCommandViewer = sender as ConnectableControls.Menus.IMenuComandViewer;
            _DragedCommand = DragedCommand;
            _CommandContainer = CommandContainer;

            // Menu command is draged now
            if (_DragedCommand != null)
                _DragedCommand.IsDraged = true;

            DragEnterFlag = true;
            ChildControls.Clear();
            foreach (Control CurrControl in GetChildControl(_HostContainerControl))
            {
                Cursor mCursor = CurrControl.Cursor;
                ChildControls.Add(CurrControl, mCursor);
            }
            foreach (Control CurrControl in GetChildControl(_HostContainerControl))
            {
                //Cursor mCursor=CurrControl.Cursor;
                //ChildControls.Add(CurrControl,mCursor);
                CurrControl.Cursor = _DragStartCursor;
                int hh = 0;
            }
            if (DragEnter != null)
                DragEnter(sender);
            //_HostContainerControl.Cursor=Cursors.Hand;

        }
        /// <MetaDataID>{1D15A320-DD0D-4341-81F5-F725D8D47B4E}</MetaDataID>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == (int)Win32.Msgs.WM_LBUTTONUP)
                if (DragEnterFlag)
                    OnDropCommand(this, null);
            return false;
        }
        /// <MetaDataID>{AF05252D-026B-425C-9039-1ACFBF4D8161}</MetaDataID>
        private Cursor _DragStartCursor = ConnectableControls.Menus.Common.ResourceHelper.LoadCursor(Type.GetType("ConnectableControls.Menus.CommandCollectionForm"),
                "ConnectableControls.Menu.Resources.DragAwayCommand.cur");


        /// <MetaDataID>{70B088F6-0369-4D4E-8F63-6BC90F5B1990}</MetaDataID>
        bool DragEnterFlag = false;
        /// <MetaDataID>{626E376C-C09D-4A80-BA8C-7B01460C4055}</MetaDataID>
        internal void OnDropCommand(object sender, EventArgs theEventArgs)
        {
            if (DragEnterFlag)
            {
                // Menu command is no more draged
                if (_DragedCommand != null)
                    _DragedCommand.IsDraged = false;


                foreach (System.Collections.DictionaryEntry CurrControlPair in ChildControls)
                    (CurrControlPair.Key as Control).Cursor = CurrControlPair.Value as Cursor;
                if(_HostContainerControl!=null)
                    _HostContainerControl.Cursor = Cursors.Default;

                DragEnterFlag = false;
                if (CommandCollectionForm != null)
                    CommandCollectionForm.Cursor = Cursors.Default;
                
                if (DragCommandDrop != null)
                {

                    DragCommandDrop(this, ref _DragedCommand, _CommandContainer);
                    if (_DragedCommand != null)
                    {
                        if (_CommandContainer != null)
                        {
                            _CommandContainer.RemoveSubCommand(_DragedCommand);
                            _DragedCommand.Delete();
                        }
                    }
                }
                if (_DragedCommandViewer != null)
                    _DragedCommandViewer.Refresh();

            }

        }
        /// <MetaDataID>{0E40A1E0-F034-46C5-A2CA-85EE535D08C3}</MetaDataID>
        ConnectableControls.Menus.CommandCollectionForm CommandCollectionForm = null;
        /// <MetaDataID>{57640886-AF40-41C5-8B0D-604EA2DBBB43}</MetaDataID>
        private void OnCommandDockingAreaRightClick(object sender, System.EventArgs e)
        {
            if (CommandCollectionForm == null)
            {
                CommandCollectionForm =new ConnectableControls.Menus.CommandCollectionForm();
                CommandCollectionForm.ShowInTaskbar = false;
                Control HostForm = _HostContainerControl;
                while (!(HostForm is Form) && HostForm != null)
                {
                    HostForm = HostForm.Parent;

                }
                CommandCollectionForm.Owner = HostForm as Form;
                //CommandCollectionForm.DragCommandMove+=new MouseEventHandler(OnDragCommandMove);

                CommandCollectionForm.Show();
                CommandCollectionForm.Closed +=new EventHandler(OnCommandCollectionFormClosed);
                CommandCollectionForm.DropCommand +=new EventHandler(OnDropCommand);
                CommandCollectionForm.DragCommandMove +=new MouseEventHandler(OnDragCommandMoveDispacher);
                CommandCollectionForm.DragCommandEnter +=new DragDropCommandEventHandler(OnDragCommandEnterDispacher);

                foreach (CommandBarDockingViewArea CurrDockingViewArea in DockingViewAreas)
                {
                    foreach (Control CurrControl in CurrDockingViewArea.Controls)
                    {
                        if (CurrControl is DockingCommandBar)
                            (CurrControl as DockingCommandBar).MenuControl.DesignMode = true;
                    }
                }
            }
        }
        /// <MetaDataID>{1f2f0f58-524e-46e8-8163-9c0bef6380de}</MetaDataID>
        static void AssignFloatingCommadBar(FloatingCommadBar floatingCommadBar, Control hostContainerControl)
        {
            Control rootControl = hostContainerControl;
            while (rootControl != null && rootControl.Parent != null)
                rootControl = rootControl.Parent;
            System.Collections.Generic.List<CommandBarDockingManager> managers =new System.Collections.Generic.List<CommandBarDockingManager>();
            if (rootControl == null)
                return;
            foreach (CommandBarDockingViewArea commandBarDockingViewArea in GetAllCommandBarDockingViewAreas(rootControl))
            {
                if (!managers.Contains(commandBarDockingViewArea.CommandBarDockingManager))
                    managers.Add(commandBarDockingViewArea.CommandBarDockingManager);
            }
            foreach (CommandBarDockingManager commandBarDockingManager in managers)
            {
                floatingCommadBar.Move +=new EventHandler(commandBarDockingManager.OnCommandBarMove);
            }

        }
        /// <MetaDataID>{12033d5c-eef4-4ef3-a879-130de3998211}</MetaDataID>
        static void RemoveFloatingCommadBarAssignments(FloatingCommadBar floatingCommadBar, Control hostContainerControl)
        {
            Control rootControl = hostContainerControl;
            while (rootControl != null && rootControl.Parent != null)
                rootControl = rootControl.Parent;
            System.Collections.Generic.List<CommandBarDockingManager> managers =new System.Collections.Generic.List<CommandBarDockingManager>();
            if (rootControl == null)
                return;
            foreach (CommandBarDockingViewArea commandBarDockingViewArea in GetAllCommandBarDockingViewAreas(rootControl))
            {
                if (!managers.Contains(commandBarDockingViewArea.CommandBarDockingManager))
                    managers.Add(commandBarDockingViewArea.CommandBarDockingManager);
            }
            foreach (CommandBarDockingManager commandBarDockingManager in managers)
            {
                floatingCommadBar.Move -=new EventHandler(commandBarDockingManager.OnCommandBarMove);
            }

        }

        /// <MetaDataID>{e9f8f404-a603-4ded-a4bd-a4472db672b6}</MetaDataID>
        static System.Collections.Generic.List<CommandBarDockingViewArea> GetAllCommandBarDockingViewAreas(Control control)
        {
            System.Collections.Generic.List<CommandBarDockingViewArea> commandBarDockingViewAreas = new System.Collections.Generic.List<CommandBarDockingViewArea>();
            if (control is CommandBarDockingViewArea)
            {
                commandBarDockingViewAreas.Add(control as CommandBarDockingViewArea);
                return commandBarDockingViewAreas;
            }

            foreach (Control innerControl in control.Controls)
                commandBarDockingViewAreas.AddRange(GetAllCommandBarDockingViewAreas(innerControl));
            return commandBarDockingViewAreas;

        }
        /// <MetaDataID>{8E183631-3F3B-4EA9-AE4A-977CEFF26950}</MetaDataID>
        private void OnCommandBarMove(object sender, System.EventArgs e)
        {
            if (sender is DockingCommandBar)
            {
                DockingCommandBar MovedCommandBar = sender as DockingCommandBar;
                if (MovedCommandBar.Parent is CommandBarDockingViewArea)
                {
                    CommandBarDockingViewArea mDockingViewArea = MovedCommandBar.Parent as CommandBarDockingViewArea;
                    Point mPoint = Control.MousePosition;
                    mPoint = mDockingViewArea.PointToClient(mPoint);

                    if ((mDockingViewArea.Dock == DockStyle.Top && mPoint.Y > mDockingViewArea.Height) ||
                        (mDockingViewArea.Dock == DockStyle.Left && mPoint.X > mDockingViewArea.Width ||
                        mDockingViewArea.Dock == DockStyle.Right && mPoint.X < 0 ||
                        mDockingViewArea.Dock == DockStyle.Bottom && mPoint.Y < 0))
                    {

                        mDockingViewArea.RemoveCommandBar(_mDockingCommandBar);
                        FloatingCommadBar mFloatingCommadBar =new FloatingCommadBar(_mDockingCommandBar, _HostContainerControl);

                        AssignFloatingCommadBar(mFloatingCommadBar, _HostContainerControl);

                        mFloatingCommadBar.Top = Control.MousePosition.Y - 1;
                        mFloatingCommadBar.Left = Control.MousePosition.X - 2;
                        if (_mDockingCommandBar.Focused)
                            mFloatingCommadBar.Focus();


                    }
                }
            }
            if (sender is FloatingCommadBar)
            {
                FloatingCommadBar mFloatingCommadBar = sender as FloatingCommadBar;




                foreach (CommandBarDockingViewArea CurrDockingViewArea in DockingViewAreas)
                {


                    Point mPoint = _HostContainerControl.PointToClient(Control.MousePosition);
                    //Point mPoint=CurrDockingViewArea.PointToClient(Control.MousePosition);
                    //	if(CurrDockingViewArea.Dock==DockStyle.Right)
                    {
                        //mPoint.X-=_HostContainerControl.Width;
                        //mPoint.X+=CurrDockingViewArea.Width;
                        //	System.Diagnostics.Debug.WriteLine("MouseX  : "+mPoint.X+ " MouseY : "+ mPoint.Y);
                        //System.Diagnostics.Debug.WriteLine("ouseX  : "+mmPoint.X+ " ouseY : "+ mmPoint.Y);
                    }
                    //	int lo=0;
                    //if(mPoint.X==0)
                    //	lo=3;

                    if (mPoint.X < CurrDockingViewArea.Right && mPoint.X > CurrDockingViewArea.Left &&
                        mPoint.Y < CurrDockingViewArea.Bottom && mPoint.Y > CurrDockingViewArea.Top)
                    {

                        DockingCommandBar mDockingCommandBar = mFloatingCommadBar.CommandBar;
                        mFloatingCommadBar.Controls.Remove(mDockingCommandBar);
                        mFloatingCommadBar.RemoveCommandBar(mDockingCommandBar);
                        RemoveFloatingCommadBarAssignments(mFloatingCommadBar, _HostContainerControl);
                        mDockingCommandBar.Dock = DockStyle.None;
                        CurrDockingViewArea.AddCommandBar(mDockingCommandBar);
                        mDockingCommandBar.Dock = CurrDockingViewArea.Dock;
                        mDockingCommandBar.Dock = DockStyle.None;
                        mPoint = CurrDockingViewArea.PointToClient(Control.MousePosition);
                        if (CurrDockingViewArea.Dock == DockStyle.Top || CurrDockingViewArea.Dock == DockStyle.Bottom)
                            mDockingCommandBar.Left = mPoint.X;
                        else
                            mDockingCommandBar.Top = mPoint.Y;


                    }
                }
            }
        }

        /// <MetaDataID>{0B4525BF-DFC4-4EEC-BB15-F829A2567CE2}</MetaDataID>
        DockingCommandBar _mDockingCommandBar;
        /// <MetaDataID>{8FDFB8AB-4899-45E8-AF94-11A1CCCFBA52}</MetaDataID>
        public void AddCommandBar(DockingCommandBar mDockingCommandBar)
        {
            _mDockingCommandBar = mDockingCommandBar;
            _mDockingCommandBar.Move +=new EventHandler(OnCommandBarMove);
            if (mDockingCommandBar.Dock == DockStyle.Top)
                TopDockingViewArea.AddCommandBar(mDockingCommandBar);
            if (mDockingCommandBar.Dock == DockStyle.Left)
                LeftDockingViewArea.AddCommandBar(mDockingCommandBar);
            if (mDockingCommandBar.Dock == DockStyle.Top)
                TopDockingViewArea.AddCommandBar(mDockingCommandBar);
            if (mDockingCommandBar.Dock == DockStyle.Bottom)
                BottomRightDockingViewArea.AddCommandBar(mDockingCommandBar);

            if (mDockingCommandBar.Dock == DockStyle.None)
            {
                FloatingCommadBar mFloatingCommadBar =new FloatingCommadBar(mDockingCommandBar, _HostContainerControl);
                AssignFloatingCommadBar(mFloatingCommadBar, _HostContainerControl);
                Rectangle mRectangle = _HostContainerControl.RectangleToScreen(_HostContainerControl.ClientRectangle);
                mFloatingCommadBar.Top = mRectangle.Top + 10;
                mFloatingCommadBar.Left = mRectangle.Left + 10;
            }
            mDockingCommandBar.RecalculateSize();
            DragCommandMove +=new MouseEventHandler(mDockingCommandBar.MenuControl.OnDragCommandMove);
            DragCommandDrop +=new ConnectableControls.Menus.Docking.DragDropCommandEventHandler(mDockingCommandBar.MenuControl.OnDragCommandDrop);
            DragEnter +=new DragEnterEventHandler(mDockingCommandBar.MenuControl.OnDragCommandEnter);

            mDockingCommandBar.MenuControl.DragCommandMove +=new MouseEventHandler(OnDragCommandMoveDispacher);
            mDockingCommandBar.MenuControl.DropCommand +=new EventHandler(OnDropCommand);
            mDockingCommandBar.MenuControl.CommandBarManager = this;



        }

    }

}
