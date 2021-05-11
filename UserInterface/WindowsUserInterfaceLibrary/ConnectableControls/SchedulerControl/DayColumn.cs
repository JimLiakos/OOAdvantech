using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ConnectableControls.SchedulerControl.Renderers;
using ConnectableControls.SchedulerControl.Events;
using System.Drawing;

namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{622aaaa2-2669-461e-8481-cb0ebce22b07}</MetaDataID>
    [DesignTimeVisible(false),ToolboxItem(false)]
    public class DayColumn : Column
    {
        #region internal Rectangle DetailDayViewRectange
        /// <MetaDataID>{c9a02a8f-61dc-4c45-9830-8f7e76a403b4}</MetaDataID>
        private Rectangle _DetailDayViewRectange;
        /// <MetaDataID>{d5cdc418-b10f-45ef-8af9-402f3e7303c5}</MetaDataID>
        /// <summary>Gets or sets the DetailView Rectangle in the header rect</summary>
        internal Rectangle DetailDayViewRectange
        {
            get
            {
                return _DetailDayViewRectange;
            }
            set
            {
                _DetailDayViewRectange = value;
            }
        }

        #endregion

        #region public int DayActionViewsCount
        /// <MetaDataID>{21c505e9-dce3-4a90-8080-3be61bfc229d}</MetaDataID>
        private int _DayActionViewsCount=0;
        /// <MetaDataID>{3a78e56a-954b-4f64-94c8-7866fdf28d1d}</MetaDataID>
        public int DayActionViewsCount
        {
            get
            {
                return _DayActionViewsCount;
            }
        }        

        #endregion

        #region public int VisibleDayActionsCount
        /// <summary>
        /// 
        /// </summary>
        /// <MetaDataID>{3ad94afa-401e-4a25-9ebe-8c25cbb9c342}</MetaDataID>
        [Browsable(false)]
        public int VisibleDayActionsCount
        {
            get
            {
                int tw = _ActionWidth + this.Parent.Table.ActionsPadding;
                //int lapadding=this.LongActionHeight+this._LongActionsPadding;
                int count = System.Convert.ToInt32(this.Width) / tw;// 
                //if ((this.Parent.DayRectangle.Width % tw) > 0)
                //{
                //    count++;
                //}

                return count;
            }
        }
        
        #endregion

        #region internal int ActionWidth
        /// <MetaDataID>{f7ba563a-8535-4328-b75f-5fc074067b9d}</MetaDataID>
        private int _ActionWidth;
        /// <MetaDataID>{8886d5cd-2399-4da7-8b8f-886e9a0fd645}</MetaDataID>
        internal int ActionWidth
        {
            get
            {
                //if (_ActionWidth == 0)
                //    return this.Parent.Table.MaxActionViewWidth;
                return _ActionWidth;
            }
        } 
        #endregion

        #region internal void ClearLongActionViewsAbovePos(Point point)
        /// <summary>
        /// Clears all the Long DayActions Above the specified position.Used mainly for repositioning fo that actions
        /// </summary>
        /// <param name="point"></param>
        /// <MetaDataID>{6614aa74-7e65-4be4-8b5c-32fa90cc1cf7}</MetaDataID>
        internal void ClearLongActionViewsAbovePos(Point point)
        {
            List<SchedulerActionView> viewsToDelete = new List<SchedulerActionView>();
            foreach (SchedulerActionView actv in this.ActionViews)
            {
                if (actv.IsLongActionView && actv.ActionRectangle.Y > point.Y)
                    viewsToDelete.Add(actv);
            }

            foreach (SchedulerActionView actv in viewsToDelete)
            {
                this.ActionViews.Remove(actv);
            }
            viewsToDelete.Clear();
        } 
        #endregion

        #region public Point GetPositionForAction(ISchedulerAction action)
        /// <summary>
        /// Returns the Position of the Specified Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <MetaDataID>{8edaf17e-1682-4997-acd0-474378fa42f6}</MetaDataID>
        public Point GetPositionForAction(ISchedulerAction action)
        {
            foreach (SchedulerActionView actv in this.ActionViews)
            {
                if (actv.Action == action)
                {
                    return new Point(actv.ActionRectangle.X, actv.ActionRectangle.Y);
                }
            }
            return new Point();
        }
        
        #endregion

        #region internal bool IncludesAction(ISchedulerAction action)
        /// <MetaDataID>{200de0e7-cc46-4a04-9c70-ce441e01feec}</MetaDataID>
        internal bool IncludesAction(ISchedulerAction action)
        {
            if (action.DateEnd.Date >= this.Date.Date && this.Date.Date >= action.DateStart.Date)
            {
                return true;
            }

            return false;
        } 
        #endregion

        #region public int GetYPositionForAction(ISchedulerAction action, out int new_height)
        /// <MetaDataID>{85ba6f2c-6814-4df5-b491-fc4ae0e6d158}</MetaDataID>
        /// <summary>Used to define the Long Action Rectangle.If the action has Y position the height of the 
        /// rectangle remains the same,otherwise the out Parameter has the height of the Rectangle needed</summary>
        public int GetYPositionForAction(ISchedulerAction action)//, out int new_height
        {
            //new_height = 0;
            //int longActionsCount = 0;
            foreach (SchedulerActionView actv in this.ActionViews)
            {
                if (actv.Action == action)
                {
                    //new_height = 0;
                    return actv.ActionRectangle.Y;
                }
                //if (actv.IsLongActionView)
                //    longActionsCount++;
            }
            //new_height = longActionsCount * this.ColumnModel.Table.LongActionHeight;
            return 0;
        } 
        #endregion

        #region public void UpdateLongActionView(ISchedulerAction action, Rectangle rect)
        /// <MetaDataID>{b3f3ce7e-ba21-4ec6-a61c-b9f2ba6f0594}</MetaDataID>
        public void UpdateLongActionView(ISchedulerAction action, Rectangle rect)
        {
            foreach (SchedulerActionView actview in this.ActionViews)
            {
                if (actview.Action == action)
                {
                    actview.ActionRectangle = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);

                    //Needed when changed from day action to LongAction
                    if (action.DateStart.Year == Date.Year &&
                                        action.DateStart.Month == Date.Month &&
                                        action.DateStart.Day == Date.Day)
                    {
                        actview.ViewState = LongActionViewState.Start;
                        //string nam = action.PerformerName.Substring(0, 4) + ".";
                        //if(!string.IsNullOrEmpty(action.PerformerLastName))
                        //    nam += action.PerformerLastName.Substring(0, 4);
                        string nam = action.PerformerName + " ";
                        nam += action.PerformerLastName;
                        actview.StringToDraw = nam;
                        break;
                    }

                    if (action.DateEnd.Year == Date.Year &&
                                        action.DateEnd.Month == Date.Month &&
                                        action.DateEnd.Day == Date.Day)
                    {
                        actview.ViewState = LongActionViewState.End;
                        actview.StringToDraw = string.Empty;
                        break;
                    }


                    if (action.DateStart.Year == Date.Year &&
                                        action.DateStart.Month == Date.Month &&(action.DateStart.Day < Date.Day||action.DateEnd.Day>Date.Day))
                    {
                        actview.ViewState = LongActionViewState.Middle;
                        actview.StringToDraw = string.Empty;                        
                    }
                    
                    break;
                }
            }
        } 
        #endregion

        #region public void UpdateActionViewRect(ISchedulerAction action, Rectangle action_rect)
        /// <MetaDataID>{e31ba2c9-4ddd-49b3-a7c5-06fb70828ac1}</MetaDataID>
        public void UpdateActionViewRect(ISchedulerAction action, Rectangle action_rect)
        {
            //Check to see if action changed from longaction to dayaction
            SchedulerActionView ractview = null;
            foreach (SchedulerActionView actview in this.ActionViews)
            {
                if (actview.IsLongActionView)
                {
                    continue;
                }
                if (actview.Action == action && ractview == null)
                {
                    ractview = actview;
                    break;
                    //if (ractview.ActionRectangle.Height == this.Parent.Table.LongActionHeight)
                    //{
                    //    _DayActionsCount++;
                    //    if (string.IsNullOrEmpty(ractview.StringToDraw))
                    //    {
                    //        string nam = action.PerformerName + " ";
                    //        nam += action.PerformerLastName;
                    //        ractview.StringToDraw = nam;
                    //    }
                    //    break;
                    //}
                }
            }

            if (ractview == null)
                return;

            int width = this.GetWidthOfDayActions();
            if (width == 0)
                return;

            ractview.ActionRectangle = new Rectangle(action_rect.X, action_rect.Y, width, action_rect.Height);
            ractview.ActionRectangle.Offset(ractview.RectOffsetX, 0);     
        } 
        #endregion

        /// <summary>
        /// Updates the string to draw in the views of the action
        /// </summary>
        /// <param name="action"></param>
        internal void UpdateActionViews(ISchedulerAction action)
        {
            foreach (SchedulerActionView acview in ActionViews)
            {
                if (acview.Action == action)
                {
                    string nam = action.PerformerName + " ";
                    nam += action.PerformerLastName;
                    acview.StringToDraw = nam;
                }
            }
        }

        /// <MetaDataID>{877306f9-1bbe-42ce-ad7a-76e730798618}</MetaDataID>
        internal decimal NumOfActionsThatCanBeDrawn
        {
            get
            {
                return System.Decimal.Round(this.Width /this.Parent.Table.MinActionViewWidth );
            }
        }

        /// <MetaDataID>{26cc9537-443e-4749-8489-7776151e53bc}</MetaDataID>
        internal void ClearDayViews()
        {
            List<SchedulerActionView> views = new List<SchedulerActionView>(_ActionViews);
            foreach (SchedulerActionView acview in views)
            {
                if (!acview.IsLongActionView)
                    _ActionViews.Remove(acview);
            }
            this._DayActionViewsCount = 0;
        }

        #region public void DeleteGarbageViewForAction(ISchedulerAction action, bool islongaction)
        /// <MetaDataID>{7b23f242-8e83-41ff-936d-7c184458c646}</MetaDataID>
        /// <summary>Deletes the garbage view/s for action that changed date duration</summary>
        public void DeleteGarbageViewForAction(ISchedulerAction action, bool islongaction)
        {
            if (islongaction)
            {
                foreach (SchedulerActionView actview in this.ActionViews)
                {
                    if (!actview.IsLongActionView)
                        continue;
                    if (actview.Action == action)
                    {
                        this.ActionViews.Remove(actview);
                        if (this.Parent.Table.FocusedActionViews.Count != 0 &&
                            this.Parent.Table.FocusedActionViews.Contains(actview))
                            this.Parent.Table.FocusedActionViews.Remove(actview);

                        return;
                    }
                }
            }
            else
            {
                foreach (SchedulerActionView actview in this.ActionViews)
                {
                    if (actview.IsLongActionView)
                        continue;
                    if (actview.Action == action)
                    {
                        this.ActionViews.Remove(actview);
                        if (this.Parent.Table.FocusedActionViews.Count != 0 &&
                            this.Parent.Table.FocusedActionViews.Contains(actview))
                            this.Parent.Table.FocusedActionViews.Remove(actview);

                        return;
                    }
                }
            }
        } 
        #endregion

        #region public bool ContainsActionViewForAction(ISchedulerAction action,bool islongaction)
        /// <summary>
        /// returns true if this DayColumn contains the action otherwise false
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <MetaDataID>{ff7192db-b632-4fa1-a17b-490d4a61881f}</MetaDataID>
        public bool ContainsActionViewForAction(ISchedulerAction action,bool islongaction)
        {
            if (islongaction)
            {
                foreach (SchedulerActionView actview in this.ActionViews)
                {
                    if (!actview.IsLongActionView)
                        continue;
                    if (actview.Action == action)
                        return true;
                }
            }
            else
            {
                foreach (SchedulerActionView actview in this.ActionViews)
                {
                    if (actview.IsLongActionView)
                        continue;
                    if (actview.Action == action)
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region public void OnPaintActions(ActionRenderer renderer, Graphics e)

        /// <MetaDataID>{d3649788-5f46-42e5-a432-d829d7cca646}</MetaDataID>
        public void OnPaintActions(ActionRenderer renderer, Rectangle cell_display_area, Graphics e)
        {
            int xpos = System.Convert.ToInt32(this.Left) + this.Parent.Table.ActionsPadding + this.Parent.Table.BorderWidth;

           
            foreach (SchedulerActionView acview in this.ActionViews)
            {
                if (!acview.IsLongActionView)
                {
                    if (this.Parent.Table.TableView == TableViewState.DayView)
                    {
                        int leftActionIndex = this.Parent.Table.LeftDayActionIndex;

                        if (acview.Index >= leftActionIndex && acview.Index < Math.Min(DayActionViewsCount, leftActionIndex + VisibleDayActionsCount))
                        {                            
                            acview.ActionRectangle.X = xpos;

                            PaintActionEventArgs paarg = new PaintActionEventArgs(e, acview);
                            Rectangle columncliprect = new Rectangle(System.Convert.ToInt32(this.X), cell_display_area.Y, System.Convert.ToInt32(this.Width), cell_display_area.Height);
                            e.Clip = new Region(columncliprect);
                            paarg.CellDisplayArea = cell_display_area;
                            renderer.OnPaintAction(paarg);
                            e.ResetClip();
                            xpos += this.ActionWidth + this.Parent.Table.ActionsPadding;
                        }
                    }
                    else
                    {                        
                        PaintActionEventArgs paarg = new PaintActionEventArgs(e, acview);
                        Rectangle columncliprect = new Rectangle(System.Convert.ToInt32(this.X), cell_display_area.Y, System.Convert.ToInt32(this.Width), cell_display_area.Height);
                        e.Clip = new Region(columncliprect);
                        paarg.CellDisplayArea = cell_display_area;
                        renderer.OnPaintAction(paarg);
                        e.ResetClip();                        
                    }
                }
            }                  
        } 
        #endregion

        #region public void OnPaintLongActions(ActionRenderer renderer, Rectangle cell_display_area, Graphics e, int action_index, int yposition)
        /// <MetaDataID>{abe3ca9c-bf81-4c0a-a032-699e56194523}</MetaDataID>
        public void OnPaintLongActions(ActionRenderer renderer, Rectangle cell_display_area, Graphics e, int action_index, int yposition)
        {
            foreach (SchedulerActionView acview in this.ActionViews)
            {
                if (acview.IsLongActionView && acview.Index == action_index)
                {
                    acview.ActionRectangle.Y = yposition;
                    PaintActionEventArgs paarg = new PaintActionEventArgs(e, acview);//this.Parent.Table.Left + 
                    Rectangle colrect = new Rectangle(this.Parent.Table._TimeColumnWidth, this.Parent.Table.HeaderHeight, this.Parent.Table.Width, this.Parent.Table.LongActionsRectHeight);
                    e.Clip = new Region(colrect);
                    paarg.CellDisplayArea = cell_display_area;
                    renderer.OnPaintAction(paarg);
                    e.ResetClip();
                }
            }
        } 
        #endregion

        #region public void AddSchedulerActionView(SchedulerActionView action_view)
        /// <MetaDataID>{02dafef2-92de-4490-a048-ba8d59c66e2f}</MetaDataID>
        public void AddSchedulerActionView(SchedulerActionView action_view)
        {
            this._ActionViews.Add(action_view);
            action_view.StartWeek = this.StartsWeek;
            action_view.EndWeek = this.EndsWeek;
            if (!action_view.IsLongActionView)
            {                
                action_view.Index = _DayActionViewsCount;
                _DayActionViewsCount++;
                //FindOffsetForView1(action_view);
                UpdateActionsWidth();
            }
            //if (action_view.IsLongActionView)
            //    this._ActionViews.Add(action_view);
            //else
            //{
            //    bool found = FindOffsetForView(action_view);
            //    this._ActionViews.Add(action_view);
            //    if (!found)
            //    {
            //        UpdateActionsWidth();
            //        action_view.Index = _DayActionsCount;
            //        _DayActionsCount++;
            //    }
            //}            
        } 
        #endregion

        #region TODO: These 2 functions will be used to examine for overlapping
        /// <MetaDataID>{4c356a1a-c5d1-4b9f-85bf-593e89a72538}</MetaDataID>
        //List<int> Offsets = new List<int>();

        /// <MetaDataID>{ec1d0d09-786d-4842-84db-4011d48a2e64}</MetaDataID>
        //private bool FindOffsetForView(SchedulerActionView aview)
        //{
        //    bool viewGotOffeset = false;
        //    if (Offsets.Count == 0)
        //        Offsets.Add(0);

        //    Rectangle arect = new Rectangle(aview.ActionRectangle.X, aview.ActionRectangle.Y, ActionWidth, aview.ActionRectangle.Height);
        //    aview.ActionRectangle = arect;
        //    foreach (int offset in Offsets)
        //    {
        //        if (!ActionViewOverlapsInOffset(aview, offset))
        //        {
        //            aview.RectOffsetX = offset;
        //            viewGotOffeset = true;
        //            break;
        //        }
        //    }
        //    return viewGotOffeset;
        //}

        ///// <MetaDataID>{e11b7ae8-1913-481c-986e-bd0f80ed3a13}</MetaDataID>
        //internal bool ActionViewOverlapsInOffset(SchedulerActionView action_view, int offset)
        //{
        //    foreach (SchedulerActionView aview in this.ActionViews)
        //    {
        //        if (!aview.IsLongActionView && aview.RectOffsetX == offset)
        //        {
        //            if (aview.Action.TimeStart.Hour <= action_view.Action.TimeStart.Hour &&
        //                aview.Action.TimeEnd.Hour >= action_view.Action.TimeStart.Hour)
        //            {
        //                //action_view.Overlaps = true;
        //                //TotalOverlappingActions++;
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //} 
        #endregion

        #region public void UpdateActionsWidth()
        /// <MetaDataID>{373c9049-ea08-43ef-ab70-fa407b91e3a6}</MetaDataID>
        public void UpdateActionsWidth()
        {
            _ActionWidth = this.GetWidthOfDayActions();
            if (_ActionWidth == 0)
                return;

            int i = 1;
            int prev_action_pos = 0;
            foreach (SchedulerActionView aview in this.ActionViews)
            {
                if (!aview.IsLongActionView)
                {
                    if (i == 1)//!aview.Overlaps
                    {
                        Rectangle arect = new Rectangle(aview.ActionRectangle.X, aview.ActionRectangle.Y, _ActionWidth, aview.ActionRectangle.Height);
                        aview.ActionRectangle = arect;
                        i++;
                        continue;
                    }
                    else
                    {
                        Rectangle arect = new Rectangle(aview.ActionRectangle.X, aview.ActionRectangle.Y, _ActionWidth, aview.ActionRectangle.Height);
                        aview.ActionRectangle = arect;                        
                       
                        prev_action_pos += this.Parent.Table.ActionsPadding + _ActionWidth;
                        aview.RectOffsetX = prev_action_pos;
                        //Offsets.Add(prev_action_pos);

                        if (prev_action_pos + _ActionWidth > this.Width && this.Parent.Table.TableView == TableViewState.DayView)
                        {
                            this.Parent.Table.ShowDetailDayScroll = true;
                        }
                    }
                }
            }
        }
        
        #endregion

        #region private int GetWidthOfDayActions()
        /// <MetaDataID>{a302a08a-8cfb-47d2-b96b-91a670ae1aa7}</MetaDataID>
        private int GetWidthOfDayActions()
        {
            //if (_ActionViews.Count == 0)
            //    return 0;

            //int dayactions = 0;

            //foreach (SchedulerActionView actview in this.ActionViews)
            //{
            //    if (actview.IsLongActionView)
            //    {
            //        continue;
            //    }

            //    dayactions++;
            //}

            if (DayActionViewsCount == 0)//
                return 0;

            //int actionsforwidth = 0;
            //if (TotalOverlappingActions == 0)
            //    actionsforwidth = 1;
            //else
            //    actionsforwidth = TotalOverlappingActions;
            

            decimal cw = this.Width - this.Parent.Table.ActionsPadding;
            int width = System.Convert.ToInt32(cw / DayActionViewsCount);//_DayActionViewsCount
            width -= this.Parent.Table.ActionsPadding*2;
            if (width < this.Parent.Table.MinActionViewWidth)
                width = this.Parent.Table.MinActionViewWidth;

            if (width > this.Parent.Table.MaxActionViewWidth)
                width = this.Parent.Table.MaxActionViewWidth;

            //if (dayactions == 1)
                //width -= this.Parent.Table.ActionsPadding;


            return width;
        } 
        #endregion       

        #region public override string GetDefaultRendererName()
        // <summary>
        /// Gets a string that specifies the name of the Column's default CellRenderer
        /// </summary>
        /// <returns>A string that specifies the name of the Column's default 
        /// CellRenderer</returns>
        /// <MetaDataID>{2CBCE7B1-0448-40B2-9C6A-0F12338711AD}</MetaDataID>
        public override string GetDefaultRendererName()
        {
            return "TEXT";
        } 
        #endregion

        #region public override ICellRenderer CreateDefaultRenderer()
        /// <summary>
        /// Gets the Column's default CellRenderer
        /// </summary>
        /// <returns>The Column's default CellRenderer</returns>
        /// <MetaDataID>{75354A93-299B-495C-AC37-1FA7CCA71C42}</MetaDataID>
        public override ICellRenderer CreateDefaultRenderer()
        {
            return new TextCellRenderer();
        } 
        #endregion

        #region Constructors
        /// <MetaDataID>{6b6eca47-fa0e-4cb7-9377-40777b9b2d7c}</MetaDataID>
        public DayColumn(string text)
            : base(text)
        {

        }

        /// <MetaDataID>{321ed23b-a434-4bc8-911e-955e244d02f7}</MetaDataID>
        public DayColumn(string text, int width)
            : base(text, width)
        {

        }

        /// <MetaDataID>{c3b8c26c-09da-4fed-8963-a1f6d47e6183}</MetaDataID>
        public DayColumn(string text, DateTime date)
            : base(text)
        {
            Date = date;
            //DateTime ac_start_d=new DateTime(2010,2,24);
            //DateTime ac_end_d=new DateTime(2010,2,24);
            //DateTime ac_start_time=new DateTime(2010,2,24,9,17,30);
            //DateTime ac_end_time=new DateTime(2010,2,24,12,25,30);

            //Action action = new Action(ac_start_d, ac_end_d, ac_start_time, ac_end_time);
            //_Actions.Add(action);
        } 
        #endregion

        #region public DateTime Date
        /// <MetaDataID>{3e58ceca-197f-4eb8-bc6b-e0cba80c60d8}</MetaDataID>
        private DateTime _Date;
        /// <MetaDataID>{068700f7-9328-4a34-af2a-d58d20569af1}</MetaDataID>
        public DateTime Date
        {
            get
            {
                return _Date;
            }
            private set
            {
                _Date = value;
            }
        } 
        #endregion

        //internal int TotalOverlappingActions = 0;
        /// <MetaDataID>{9f0f70e9-a813-4834-9e43-c38689eed904}</MetaDataID>
        internal bool StartsWeek = false;
        /// <MetaDataID>{130a30d0-d3d3-467e-86ec-ee4743502902}</MetaDataID>
        internal bool EndsWeek = false;

        #region public List<SchedulerActionView> ActionViews
        /// <MetaDataID>{4c263875-6734-463d-b36d-4b24723eb687}</MetaDataID>
        private List<SchedulerActionView> _ActionViews = new List<SchedulerActionView>();
        /// <MetaDataID>{146cda77-e77a-488b-8fdc-bcc065854af7}</MetaDataID>
        public List<SchedulerActionView> ActionViews
        {
            get
            {
                return _ActionViews;
            }
        } 
        #endregion
    }
}
