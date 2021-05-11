using System;
using System.Collections.Generic;
using System.Text;
using ConnectableControls.SchedulerControl.Events;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{455d43fd-5032-47ba-b773-6d974eaf0e90}</MetaDataID>
    public class ActionRenderer : Renderer
    {
        /// <MetaDataID>{f174eaee-d528-4976-9adf-bdffe295ccd4}</MetaDataID>
        public virtual void OnActionMouseDown(ActionMouseEventArgs amea)
        {
            //GraphicsPath path;
            //if (amea.ActionView.IsLongActionView)
            //    path = this.GetLongPath(this.ClientRectangle, this.Bounds.Width / 10, amea.ActionView);
            //else
            //    path = this.GetPath(this.ClientRectangle, 8);  
          
            
        }

        /// <MetaDataID>{c4190e84-64ec-4674-8169-c3b79adea4a9}</MetaDataID>
        public ActionRenderer()
        {

        }

        #region public virtual void OnPaintAction(PaintActionEventArgs e)
        /// <MetaDataID>{2e0ec213-5b22-4a3b-a4a9-09eb3f954c0f}</MetaDataID>
        public virtual void OnPaintAction(PaintActionEventArgs e)
        {
            if (e == null)
                return;

            this.Bounds = e.ActionRectangle;
            this.Font = e.Font;
            this._DisplayArea = e.CellDisplayArea;

            //_ActionDescription = e.ActionView.Description;
            if (this.Font == null)
            {
                this.Font = Control.DefaultFont;
            }

            // paint the Cells background
            this.OnPaintBackground(e);

            // paint the Cells foreground
            this.OnPaint(e);
        } 
        #endregion

        #region protected virtual void OnPaintBackground(PaintActionEventArgs e)
        /// <MetaDataID>{f3badd1c-aaa5-4e19-9e74-80e301d48427}</MetaDataID>
        protected virtual void OnPaintBackground(PaintActionEventArgs e)
        {
            try
            {
                GraphicsPath path;
                GraphicsPath shadow;
                Rectangle shadowrect = new Rectangle(this.ClientRectangle.X + 2, this.ClientRectangle.Y + 2, this.ClientRectangle.Width, this.ClientRectangle.Height);

                if (e.ActionView.IsLongActionView)
                {
                    path = this.GetLongPath(this.ClientRectangle, this.Bounds.Width / 10, e.ActionView);
                    shadow = this.GetLongPath(shadowrect, this.Bounds.Width / 10, e.ActionView);
                }
                else
                {
                    if (this.ClientRectangle.X == 0 || this.ClientRectangle.Y == 0)
                        return;
                    path = this.GetPath(this.ClientRectangle, 8, false);
                    shadow = this.GetPath(shadowrect, 8, true);
                }

                using (PathGradientBrush sbrush = new PathGradientBrush(shadow))
                {
                    sbrush.SurroundColors = new Color[] { Color.LightSlateGray };                    
                    e.Graphics.FillPath(sbrush, shadow);
                }

                using (PathGradientBrush pbrush = new PathGradientBrush(path))
                {
                    if (e.ActionView.Focus)
                    {
                        pbrush.SurroundColors = new Color[] { SystemColors.Highlight };
                        pbrush.CenterColor = SystemColors.Highlight;
                    }
                    else
                    {
                        switch (e.ActionView.Action.ActionState)
                        {
                            case ActionState.Proposed:
                                {
                                    pbrush.CenterColor = SystemColors.Control;
                                    pbrush.SurroundColors = new Color[] { SystemColors.Control };
                                    break;
                                }
                            case ActionState.Started:
                                {
                                    pbrush.CenterColor = Color.LightGreen;
                                    pbrush.SurroundColors = new Color[] { Color.LightGreen };
                                    break;
                                }
                            case ActionState.Completed:
                                {
                                    pbrush.CenterColor = Color.Green;
                                    pbrush.SurroundColors = new Color[] { Color.Green };
                                    break;
                                }
                            case ActionState.Suspended:
                                {
                                    pbrush.CenterColor = Color.White;
                                    pbrush.SurroundColors = new Color[] { Color.Red };
                                    break;
                                }
                            case ActionState.Abandoned:
                                {
                                    pbrush.CenterColor = Color.Red;
                                    pbrush.SurroundColors = new Color[] { Color.Red };
                                    break;
                                }
                            default:
                                break;
                        }
                    }

                    e.Graphics.FillPath(pbrush, path);//Brushes.LightSkyBlue
                }
                using (Pen pen = new Pen(Color.Black))
                {
                    //if (e.ActionView.Focus)
                    //    pen.Width = 1.7f;
                    e.Graphics.DrawPath(pen, path);
                }

                path.Dispose();
                shadow.Dispose();               
            }
            catch (Exception ex)
            {


            }
        } 
        #endregion

        #region protected virtual void OnPaint(PaintActionEventArgs e)
        /// <MetaDataID>{61d88b5d-0fb5-4e3e-bf0b-84ddb8d467b2}</MetaDataID>
        protected virtual void OnPaint(PaintActionEventArgs e)
        {
            if (this.ClientRectangle.X == 0 || this.ClientRectangle.Y==0)
                return;
            RectangleF rF = new RectangleF(this.ClientRectangle.Location, this.ClientRectangle.Size);
            if (!e.ActionView.IsLongActionView)
                this.StringFormat.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.NoWrap;
            else
                this.StringFormat.FormatFlags = StringFormatFlags.NoWrap;
            e.Graphics.DrawString(e.ActionView.StringToDraw, this.Font, Brushes.Black, rF, this.StringFormat);//_ActionDescription

        } 
        #endregion

        #region private GraphicsPath GetLongPath(Rectangle rect, int radious, SchedulerActionView ac_view)
        /// <MetaDataID>{21c81992-4271-47bd-990a-12f91e419fe2}</MetaDataID>
        private GraphicsPath GetLongPath(Rectangle rect, int radious, SchedulerActionView ac_view)
        {
            int rad = radious;
            Rectangle r = new Rectangle(rect.Location, new Size(rad, rad));
            GraphicsPath path = new GraphicsPath();

            if (ac_view.ViewState == LongActionViewState.Start)
            {
                //bottom, line
                r.Y = rect.Top + rect.Height;
                if(ac_view.EndWeek)
                    r.X = rect.Left + rect.Width ;
                else
                    r.X = rect.Left + rect.Width + 2;

                path.AddLine(r.X, r.Y, rect.Left + rad, rect.Bottom);

                ////bottom left arc
                r.X = rect.Left;
                r.Y = rect.Top + rect.Height - rad;
                path.AddArc(r, 90, 90);
                //Point lb = new Point(r.X, r.Y);

                //topleft
                r.Y = rect.Top;
                path.AddArc(r, 180, 90);
                //upper line
                if (ac_view.EndWeek)
                    path.AddLine(r.Left + rad, r.Top, rect.Right , rect.Top);
                else
                    path.AddLine(r.Left + rad, r.Top, rect.Right + 2, rect.Top);
                //Point lt = new Point(r.Left, rect.Top + rad);

                //path.AddLine(lt, lb);

                //path.StartFigure();
            }
            else if (ac_view.ViewState == LongActionViewState.End)
            {
                //upper line
                if (ac_view.StartWeek)
                    path.AddLine(r.Left, rect.Top, rect.Right - rad, rect.Top);
                else
                    path.AddLine(r.Left - 3, rect.Top, rect.Right - rad, rect.Top);

                //top right
                r.X = rect.Right - rad;
                path.AddArc(r, 270, 90);

                //path.AddLine(r.Left + rad, rect.Bottom, rect.Right - rad, rect.Bottom);                                

                //bottom right
                r.Y = rect.Bottom - rad;
                path.AddArc(r, 0, 90);
                //bottom line
                r.Y = rect.Bottom;
                if (ac_view.StartWeek)
                    path.AddLine(r.X, r.Y, rect.Left, rect.Bottom);
                else
                    path.AddLine(r.X, r.Y, rect.Left - 3, rect.Bottom);

            }
            else if (ac_view.ViewState == LongActionViewState.Middle)
            {
                if (ac_view.StartWeek)
                    path.AddLine(r.X, r.Y, rect.Right + 3, rect.Top);
                else
                {
                    if (ac_view.EndWeek)
                        path.AddLine(r.X - 3, r.Y, rect.Right, rect.Top);
                    else
                        path.AddLine(r.X - 3, r.Y, rect.Right + 3, rect.Top);
                }

                if (ac_view.EndWeek)
                    r.X = rect.Right;
                else
                    r.X = rect.Right + 3;

                r.Y = rect.Bottom;
                if (ac_view.StartWeek)
                    path.AddLine(r.X, r.Y, rect.Left, rect.Bottom);
                else
                    path.AddLine(r.X, r.Y, rect.Left - 3, rect.Bottom);
            }

            return path;
        } 
        #endregion

        #region private GraphicsPath GetPath(Rectangle rect, int radious)
        /// <summary>
        /// Gets the path for day actions
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radious"></param>
        /// <returns></returns>
        /// <MetaDataID>{74db1beb-0492-4dc0-95fa-b2b8804f622d}</MetaDataID>
        private GraphicsPath GetPath(Rectangle rect, int radious, bool shadow)
        {
            int rad = radious;
            Rectangle r = new Rectangle(rect.Location, new Size(rad, rad));
            GraphicsPath path = new GraphicsPath();

            if (_DisplayArea.Y < rect.Location.Y)
            {
                //topleft
                path.AddArc(r, 180, 90);

                //top right
                r.X = rect.Right - rad;
                path.AddArc(r, 270, 90);

                //bottom right
                r.Y = rect.Bottom - rad;
                path.AddArc(r, 0, 90);

                //bottom left
                r.X = rect.Left;
                path.AddArc(r, 90, 90);

                path.CloseFigure();


            }
            else
            {
                if (_DisplayArea.Bottom > rect.Bottom || _DisplayArea.Y > rect.Y)
                {
                    if (shadow)
                        r = new Rectangle(rect.Right, _DisplayArea.Y+2, rad, rad);
                    else
                        r = new Rectangle(rect.Right, _DisplayArea.Y, rad, rad);

                    //right vertical line
                    path.AddLine(r.Location.X, r.Location.Y, r.Location.X, r.Location.Y + (rect.Height - rad- 4 ));//

                    r.Y = r.Location.Y + (rect.Height - rad- 4 );//
                    r.X = rect.X + rect.Width - rad;
                    path.AddArc(r, 0, 90);

                    r.Y = r.Location.Y + rad;
                    //r.Y = r.Location.Y + rect.Height;
                    path.AddLine(r.X, r.Y, rect.X+rad-4 , r.Y);

                    r.X = rect.X;
                    r.Y = rect.Y + rect.Height - rad + 2;
                    path.AddArc(r, 90, 90);

                    //r.X = rect.X;
                    //r.Y = rect.Bottom - rad;

                    path.AddLine(r.X, r.Y, rect.X, _DisplayArea.Y);
                }
            }
            return path;
        } 
        #endregion

        #region public override void Dispose()
        /// <MetaDataID>{8e0094e2-7c70-4592-a43c-91c32920e3f7}</MetaDataID>
        public override void Dispose()
        {
            base.Dispose();
        } 
        #endregion

        #region public override System.Drawing.Rectangle ClientRectangle
        /// <MetaDataID>{6036d349-668f-46f1-b8ac-548db61fb663}</MetaDataID>
        public override System.Drawing.Rectangle ClientRectangle
        {
            get
            {
                Rectangle client = new Rectangle(this.Bounds.Location, this.Bounds.Size);

                //client.Width -= 5;
                //client.Height -= 2;

                //client.X += 5;
                //client.Y += 2;

                return client;
            }
        } 
        #endregion

        //private string _ActionDescription;
        //private bool _Focused = false;
        /// <MetaDataID>{f8a98ad1-45b0-40fc-84b6-5f2f42e66e6a}</MetaDataID>
        private Rectangle _DisplayArea;
    }
}
