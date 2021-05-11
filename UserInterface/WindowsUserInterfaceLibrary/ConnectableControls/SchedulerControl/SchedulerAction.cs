using System;
using System.Drawing;
namespace ConnectableControls.SchedulerControl
{
    /// <MetaDataID>{6ec1c74a-2527-4f28-8cc1-355f47e30d38}</MetaDataID>
    public class SchedulerActionView
    {        
        #region internal bool IsLongActionView
        internal bool IsLongActionView
        {
            get
            {
                if (Action.DateStart.Date < Action.DateEnd.Date)
                    return true;
                //if (Action.DateStart.Year <= Action.DateEnd.Year &&
                //    Action.DateStart.Month <= Action.DateEnd.Month &&
                //    Action.DateStart.Day < Action.DateEnd.Day)
                //    return true;

                return false;
            }
        }
        
        #endregion

        #region internal bool Contains(int x, int y)
        internal bool Contains(int x, int y)
        {
            if (ActionRectangle.Contains(x, y))
                return true;
            return false;
        } 
        #endregion

        #region internal SchedulerActionView(ISchedulerAction action, Rectangle action_rect)
        internal SchedulerActionView(ISchedulerAction action, Rectangle action_rect)
        {
            Action = action;
            ActionRectangle = action_rect;
        } 
        #endregion

        #region internal string Description
        internal string Description
        {
            get
            {
                return Action.Description;
            }
        }
        
        #endregion

        /// <summary>
        /// Used to control the drawing for long actions to the start and end of the week
        /// </summary>
        internal bool StartWeek = false;
        internal bool EndWeek = false;
        internal string StringToDraw;
        internal int Index;
        internal int RectOffsetX = 0;
        internal ISchedulerAction Action;
        internal Rectangle ActionRectangle;
        internal bool Focus = false;
        internal LongActionViewState ViewState = LongActionViewState.None;        
    }

    /// <MetaDataID>{75870104-465c-4774-8c5a-fdbd96f1de76}</MetaDataID>
    public interface ISchedulerAction
    {
        /// <MetaDataID>{cce45483-a23f-458e-a70a-71c89dba1234}</MetaDataID>
        string Description { get; set; }
        /// <MetaDataID>{e7d813b3-6a04-4284-ad62-1ae224cc0e13}</MetaDataID>
        DateTime DateStart { get; set; }
        /// <MetaDataID>{abe65a42-bc7c-43ac-8fe6-27cd26c36857}</MetaDataID>
        DateTime DateEnd { get; set; }
        /// <MetaDataID>{309a018d-dceb-4db9-a2c8-533919bba90d}</MetaDataID>
        DateTime TimeStart { get; set; }
        /// <MetaDataID>{9ed1da78-8502-44b7-80bd-fc43f0442af4}</MetaDataID>
        DateTime TimeEnd { get; set; }
        /// <MetaDataID>{0187c66f-4539-43c7-a8c5-18c5a66d5969}</MetaDataID>
        string PerformerName { get; set; }
        /// <MetaDataID>{1f13fd5a-d49a-46b6-9033-be39ab33f846}</MetaDataID>
        string PerformerLastName { get; set; }
        /// <MetaDataID>{2f07220d-35d5-4249-8751-2724e255bc7a}</MetaDataID>
        ActionState ActionState { get; set; }        
    }

    //public class Action : ISchedulerAction
    //{
    //    public bool ContainsDate(DateTime date)
    //    {
    //        if (date.Year >= DateStart.Year && date.Month >= DateStart.Month && date.Day >= DateStart.Day &&
    //            DateEnd.Year >= date.Year && DateEnd.Month >= date.Month && DateEnd.Day >= date.Day)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    #region public bool ContainsDate(DateTime date, int hour, int startmin, int endmin)
    //    public bool ContainsDate(DateTime date, int hour, int startmin, int endmin)
    //    {
    //        if (date.Year >= DateStart.Year && date.Month >= DateStart.Month && date.Day >= DateStart.Day &&
    //            DateEnd.Year >= date.Year && DateEnd.Month >= date.Month && DateEnd.Day >= date.Day)
    //        {
    //            //if (date.Year > DateStart.Year && date.Month > DateStart.Month && date.Day > DateStart.Day)
    //            //    return true;

    //            //if (date.Year == DateStart.Year && date.Month == DateStart.Month && date.Day == DateStart.Day)
    //            if (DateEnd.Day == date.Day)
    //            {
    //                //2 columns draw.It was in the begining when the long actions was drawn in vertical 
    //                if (DateEnd.Day > DateStart.Day)
    //                {
    //                    if (TimeEnd.Hour > hour)
    //                        return true;
    //                    if (TimeEnd.Hour == hour && TimeEnd.Minute >= startmin)
    //                        return true;
    //                    return false;
    //                }
    //                else
    //                {
    //                    if (TimeEnd.Hour >= hour && hour >= TimeStart.Hour)
    //                    {
    //                        if (hour == TimeStart.Hour && startmin >= TimeStart.Minute)
    //                            return true;

    //                        if (TimeEnd.Hour == hour && TimeEnd.Minute >= startmin)
    //                            return true;

    //                        if (TimeEnd.Hour > hour)
    //                            return true;

    //                        return false;
    //                    }
    //                }
    //                return false;
    //            }
    //            else
    //            {
    //                if (date.Day > DateStart.Day)
    //                    return true;

    //                if (date.Day == DateStart.Day)
    //                {
    //                    //find the start
    //                    if (hour == TimeStart.Hour && startmin <= TimeStart.Minute && TimeStart.Minute <= endmin)
    //                    {
    //                        startf = true;
    //                        return true;
    //                    }
    //                    if (hour == TimeStart.Hour && startf)
    //                        return true;

    //                    if (hour > TimeStart.Hour)
    //                        return true;
    //                    return false;
    //                }
    //            }
    //            return true;
    //        }
    //        //if (TimeEnd.Hour >= hour)
    //        //{
    //        //    if (TimeEnd.Hour > hour)
    //        //        return true;
    //        //    else if (TimeEnd.Minute >= startmin && TimeEnd.Minute <= endmin)
    //        //        return true;
    //        //    return false;
    //        //}
    //        return false;
    //    } 
    //    #endregion
        

    //    public Action(DateTime fdate,DateTime tdate,DateTime ftime,DateTime ttime)
    //    {
    //        DateStart = fdate;
    //        DateEnd = tdate;
    //        TimeStart = ftime;
    //        TimeEnd = ttime;
    //    }

    //    public string Description;
    //    private bool startf = false;
    //    public DateTime TimeStart;
    //    public DateTime TimeEnd;

    //    #region ISchedulerAction Members

    //    private DateTime _DateStart;
    //    public DateTime DateStart
    //    {
    //        get
    //        {
    //            return _DateStart;
    //        }
    //        set
    //        {
    //            _DateStart = value;
    //        }
    //    }

    //    public DateTime _DateEnd;
    //    public DateTime DateEnd
    //    {
    //        get
    //        {
    //            return _DateEnd;
    //        }
    //        set
    //        {
    //            _DateEnd = value;
    //        }
    //    }                      

    //    string ISchedulerAction.Description
    //    {
    //        get
    //        {
    //            return this.Description;
    //        }
    //        set
    //        {
    //            this.Description = value;
    //        }
    //    }

    //    public ISchedulerPerformer Performer
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    #endregion
    //}
    
    /// <MetaDataID>{983627ba-043f-437c-a39e-a005df0ff9f6}</MetaDataID>
    public enum LongActionViewState
    {
        None,
        Start,
        Middle,
        End
    }
}