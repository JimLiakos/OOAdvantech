﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ConnectableControls.SchedulerControl.RowDesign;
using ConnectableControls.SchedulerControl.Events;
using System.Diagnostics;

namespace ConnectableControls.SchedulerControl.Renderers
{
    /// <MetaDataID>{861a0c6e-5f4b-43a8-ad2d-32996e11a55f}</MetaDataID>
    public abstract class CellRenderer : Renderer, ICellRenderer
    {
        #region Class Data

        /// <summary>
        /// A string that specifies how a Cells contents are formatted
        /// </summary>
        private string format;

        /// <summary>
        /// The Brush used to draw disabled text
        /// </summary>
        private SolidBrush grayTextBrush;

        /// <summary>
        /// The amount of padding for the cell being rendered
        /// </summary>
        private CellPadding padding;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the CellRenderer class with default settings
        /// </summary>
        /// <MetaDataID>{4526AB6A-E2C8-4856-85CC-B8633914C24C}</MetaDataID>
        protected CellRenderer()
            : base()
        {
            this.format = "";

            this.grayTextBrush = new SolidBrush(SystemColors.GrayText);
            this.padding = CellPadding.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases the unmanaged resources used by the Renderer and 
        /// optionally releases the managed resources
        /// </summary>
        /// <MetaDataID>{C8274516-75CC-4390-9410-957C17C50EF9}</MetaDataID>
        public override void Dispose()
        {
            base.Dispose();

            if (this.grayTextBrush != null)
            {
                this.grayTextBrush.Dispose();
                this.grayTextBrush = null;
            }
        }


        /// <summary>
        /// Gets the renderer specific data used by the Renderer from 
        /// the specified Cell
        /// </summary>
        /// <param name="cell">The Cell to get the renderer data for</param>
        /// <returns>The renderer data for the specified Cell</returns>
        /// <MetaDataID>{DDB5001A-7696-43AE-A7B8-FA7BA5D3F81F}</MetaDataID>
        protected object GetRendererData(Cell cell)
        {
            return cell.RendererData;
        }


        /// <summary>
        /// Sets the specified renderer specific data used by the Renderer for 
        /// the specified Cell
        /// </summary>
        /// <param name="cell">The Cell for which the data is to be stored</param>
        /// <param name="value">The renderer specific data to be stored</param>
        /// <MetaDataID>{AAF2289A-34C5-4A3C-964B-633A559B1249}</MetaDataID>
        protected void SetRendererData(Cell cell, object value)
        {
            cell.RendererData = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Overrides Renderer.ClientRectangle
        /// </summary>
        public override Rectangle ClientRectangle
        {
            get
            {
                Rectangle client = new Rectangle(this.Bounds.Location, this.Bounds.Size);

                // take borders into account
                client.Width -= Renderer.BorderWidth;
                client.Height -= Renderer.BorderWidth;

                // take cell padding into account
                client.X += this.Padding.Left + 1;
                client.Y += this.Padding.Top;
                client.Width -= this.Padding.Left + this.Padding.Right + 1;
                client.Height -= this.Padding.Top + this.Padding.Bottom;

                return client;
            }
        }


        /// <summary>
        /// Gets or sets the string that specifies how a Cells contents are formatted
        /// </summary>
        protected string Format
        {
            get
            {
                return this.format;
            }

            set
            {
                this.format = value;
            }
        }


        /// <summary>
        /// Gets the Brush used to draw disabled text
        /// </summary>
        protected Brush GrayTextBrush
        {
            get
            {
                return this.grayTextBrush;
            }
        }


        /// <summary>
        /// Gets or sets the amount of padding around the Cell being rendered
        /// </summary>
        protected CellPadding Padding
        {
            get
            {
                return this.padding;
            }

            set
            {
                this.padding = value;
            }
        }

        #endregion

        #region Events

        #region Focus

        /// <summary>
        /// Raises the GotFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{82886AA0-6903-429F-A0B9-61BC138A3D0D}</MetaDataID>
        public virtual void OnGotFocus(CellFocusEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }

            e.Table.Invalidate(e.CellRect);
        }


        /// <summary>
        /// Raises the LostFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{822E9C88-F778-47C5-A23F-08FD5AEFF8CD}</MetaDataID>
        public virtual void OnLostFocus(CellFocusEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }

            e.Table.Invalidate(e.CellRect);
        }

        #endregion

        #region Keys

        /// <summary>
        /// Raises the KeyDown event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{B4E5BE42-FFBB-4C1D-B650-6D2E3E5AAF03}</MetaDataID>
        public virtual void OnKeyDown(CellKeyEventArgs e)
        {

        }


        /// <summary>
        /// Raises the KeyUp event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{AD4FB785-3368-4D01-BCA3-49BC8CE3CDDF}</MetaDataID>
        public virtual void OnKeyUp(CellKeyEventArgs e)
        {

        }

        #endregion

        #region Mouse

        #region MouseEnter

        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{B105413D-7A85-4EE5-8132-E4336667B859}</MetaDataID>
        public virtual void OnMouseEnter(CellMouseEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }

            bool tooltipActive = e.Table.ToolTip.Active;

            if (tooltipActive)
            {
                e.Table.ToolTip.Active = false;
            }

            e.Table.ResetMouseEventArgs();

            e.Table.ToolTip.SetToolTip(e.Table, e.Cell.ToolTipText);

            if (tooltipActive)
            {
                e.Table.ToolTip.Active = true;
            }
        }

        #endregion

        #region MouseLeave

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{0D15C527-B50A-41DF-A146-CD757C809A27}</MetaDataID>
        public virtual void OnMouseLeave(CellMouseEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }
        }

        #endregion

        #region MouseUp

        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{02C8866E-E9B7-4DD6-9FBC-2D55B5456604}</MetaDataID>
        public virtual void OnMouseUp(CellMouseEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }
        }

        #endregion

        #region MouseDown

        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{8DA73F9F-FCFA-4BA6-A1E1-C8034168DB3D}</MetaDataID>
        public virtual void OnMouseDown(CellMouseEventArgs e)
        {
            //if (!e.Table.Focused)
            //{

            //    if (!(e.Table.IsEditing && e.Table.EditingCell == e.CellPos && e.Table.EditingCellEditor is IEditorUsesRendererButtons))
            //    {
            //        e.Table.Focus();
            //    }
            //}

            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }
        }

        #endregion

        #region MouseMove

        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{422AC5AE-8560-4287-BC2F-4D2E166E36CF}</MetaDataID>
        public virtual void OnMouseMove(CellMouseEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }
        }

        #endregion

        #region Click

        /// <summary>
        /// Raises the Click event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{0E9BDB79-3D54-4064-A504-296454930221}</MetaDataID>
        public virtual void OnClick(CellMouseEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }
            //if (e.Table.EditStartAction == EditStartAction.SingleClick &&
            //    e.Table.IsCellEditable(e.CellPos) &&
            //    !e.Cell.Row.IsSelectedOnLastClick)
            //{
            //    e.Cell.Row.IsSelectedOnLastClick = true;
            //    if (e.Button != MouseButtons.Right)
            //        return;
            //}
            //if (e.Table.EditStartAction == EditStartAction.SingleClick &&
            //    e.Table.IsCellEditable(e.CellPos) &&
            //    e.Cell.Row.IsSelectedOnLastClick && e.Button == MouseButtons.Left)
            //{
            //    e.Table.EditCell(e.CellPos);
            //    return;
            //}
            //if (e.Button == MouseButtons.Right)
            //{
            //    Column column = e.Table.ColumnModel.Columns[e.Column];
            //    if (column.Menu.MenuCommands.Count > 0)
            //    {
            //        ConnectableControls.Menus.PopupMenu popupMenu = new ConnectableControls.Menus.PopupMenu(false);

            //        int returnDir = 0;

            //        popupMenu.TrackPopup(
            //            Control.MousePosition,
            //            Control.MousePosition,
            //            ConnectableControls.Menus.Common.Direction.Horizontal,
            //            column.Menu,
            //            0,
            //            ConnectableControls.Menus.GapPosition.None, false, null, false, ref returnDir);

            //    }

            //}
        }


        /// <summary>
        /// Raises the DoubleClick event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{D644B2D6-6307-4499-BE60-1372D2019AAC}</MetaDataID>
        public virtual void OnDoubleClick(CellMouseEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell == null)
            {
                this.Padding = CellPadding.Empty;
            }
            else
            {
                this.Padding = e.Cell.Padding;
            }

            //if (e.Table.EditStartAction == EditStartAction.DoubleClick && e.Table.IsCellEditable(e.CellPos))
            //{
            //    e.Table.EditCell(e.CellPos);
            //}
        }

        #endregion

        #endregion

        #region Paint

        /// <summary>
        /// Raises the PaintCell event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{49350F27-6D70-4D17-A013-B54E5F023DF2}</MetaDataID>
        public virtual void OnPaintCell(PaintCellEventArgs e)
        {
            this.Bounds = e.CellRect;

            if (e.Cell != null)
            {
                this.Padding = e.Cell.Padding;

                this.Alignment = e.Table.ColumnModel.Columns[e.Column].Alignment;
                this.LineAlignment = e.Table.TableModel.TotalRows[e.Row].Alignment;

                this.Format = e.Table.ColumnModel.Columns[e.Column].Format;

                this.Font = e.Cell.Font;
            }
            else
            {
                this.Padding = CellPadding.Empty;

                this.Alignment = ColumnAlignment.Left;
                this.LineAlignment = RowAlignment.Center;

                this.Format = "";

                this.Font = null;
            }

            // if the font is null, use the default font
            if (this.Font == null)
            {
                this.Font = Control.DefaultFont;
            }

            // paint the Cells background
            this.OnPaintBackground(e);

            // paint the Cells foreground
            this.OnPaint(e);
        }


        /// <summary>
        /// Raises the PaintBackground event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{7196C890-BDA5-4966-901E-0503B8E0706C}</MetaDataID>
        protected virtual void OnPaintBackground(PaintCellEventArgs e)
        {
            if (e.Selected && (e.Table.Focused || e.Table.IsEditing))//(!e.Table.HideSelection || (e.Table.HideSelection && (e.Table.Focused || e.Table.IsEditing))))
            {
                Debug.WriteLine("Selected cell");
                if (e.Table.Focused || e.Table.IsEditing)
                {
                    this.ForeColor = e.Table.SelectedCellForeColor;
                    this.BackColor = e.Table.SelectedCellBackColor;
                }
                else
                {
                    this.BackColor = e.Table.UnfocusedSelectedCellBackColor;
                    this.ForeColor = e.Table.UnfocusedSelectedCellForeColor;
                }

                if (this.BackColor.A != 0)
                {
                    e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
                }
            }
            //else
            //{
            //    this.ForeColor = e.Cell != null ? e.Cell.ForeColor : Color.Black;

            //    if (!e.Sorted || (e.Sorted && e.Table.SortedColumnBackColor.A < 255))
            //    {
            //        if (e.Cell != null)
            //        {
            //            if (e.Cell.BackColor.A < 255)
            //            {
            //                if (e.Row % 2 == 1)
            //                {
            //                    if (e.Table.AlternatingRowColor.A != 0)
            //                    {
            //                        this.BackColor = e.Table.AlternatingRowColor;
            //                        e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
            //                    }
            //                }

            //                this.BackColor = e.Cell.BackColor;
            //                if (e.Cell.BackColor.A != 0)
            //                {
            //                    e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
            //                }
            //            }
            //            else
            //            {
            //                this.BackColor = e.Cell.BackColor;
            //                if (e.Cell.BackColor.A != 0)
            //                {
            //                    e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (e.Row % 2 == 1)
            //            {
            //                if (e.Table.AlternatingRowColor.A != 0)
            //                {
            //                    this.BackColor = e.Table.AlternatingRowColor;
            //                    e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
            //                }
            //            }
            //        }

            //        if (e.Sorted)
            //        {
            //            this.BackColor = e.Table.SortedColumnBackColor;
            //            if (e.Table.SortedColumnBackColor.A != 0)
            //            {
            //                e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        this.BackColor = e.Table.SortedColumnBackColor;
            //        e.Graphics.FillRectangle(this.BackBrush, e.CellRect);
            //    }
            //}
        }


        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{28EC42A3-EF73-495A-9747-58589081ED80}</MetaDataID>
        protected virtual void OnPaint(PaintCellEventArgs e)
        {

        }


        /// <summary>
        /// Raises the PaintBorder event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <param name="pen">The pen used to draw the border</param>
        /// <MetaDataID>{65B722C9-6711-4578-99A2-97A1538168E2}</MetaDataID>
        protected virtual void OnPaintBorder(PaintCellEventArgs e, Pen pen)
        {
            // bottom
            e.Graphics.DrawLine(pen, e.CellRect.Left, e.CellRect.Bottom, e.CellRect.Right, e.CellRect.Bottom);

            // right
            e.Graphics.DrawLine(pen, e.CellRect.Right, e.CellRect.Top, e.CellRect.Right, e.CellRect.Bottom);
        }

        #endregion

        #endregion
    }    
}
