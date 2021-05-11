/*
 * Copyright © 2005, Mathew Hall
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 *
 *    - Redistributions of source code must retain the above copyright notice, 
 *      this list of conditions and the following disclaimer.
 * 
 *    - Redistributions in binary form must reproduce the above copyright notice, 
 *      this list of conditions and the following disclaimer in the documentation 
 *      and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE.
 */


using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using ConnectableControls.List.Events;
using ConnectableControls.List.Models;
using ConnectableControls.List.Themes;


namespace ConnectableControls.List.Renderers
{
    /// <summary>
    /// A CellRenderer that draws Cell contents as Buttons
    /// </summary>
    /// <MetaDataID>{F4E95B78-107E-467B-B285-AEEFE7AF3CFB}</MetaDataID>
	public class ButtonCellRenderer : CellRenderer
	{
		#region Class Data

		/// <summary>
		/// Specifies the alignment of the Image displayed on the button
		/// </summary>
		private ContentAlignment imageAlignment;

		#endregion
		
		
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the ButtonCellRenderer class with 
        /// default settings
        /// </summary>
        /// <MetaDataID>{7437CAD8-AC44-4732-8EA6-EA8104DAB742}</MetaDataID>
		public ButtonCellRenderer() : base()
		{
			this.imageAlignment = ContentAlignment.MiddleCenter;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets the ButtonCellRenderer specific data used by the Renderer from 
        /// the specified Cell
        /// </summary>
        /// <param name="cell">The Cell to get the ButtonCellRenderer data for</param>
        /// <returns>The ButtonCellRenderer data for the specified Cell</returns>
        /// <MetaDataID>{AF24DC96-6CB9-47FE-8D29-90321E7F10E5}</MetaDataID>
		protected ButtonRendererData GetButtonRendererData(Cell cell)
		{
			object rendererData = this.GetRendererData(cell);

			if (rendererData == null || !(rendererData is ButtonRendererData))
			{
				rendererData = new ButtonRendererData();

				this.SetRendererData(cell, rendererData);
			}

			return (ButtonRendererData) rendererData;
		}


        /// <summary>
        /// Returns a Rectangle that specifies the size and location of the button
        /// </summary>
        /// <returns>A Rectangle that specifies the size and location of the button</returns>
        /// <MetaDataID>{E876E0A7-DF0F-49D9-A682-462C2D6002B9}</MetaDataID>
		protected virtual Rectangle CalcButtonBounds()
		{
			return this.ClientRectangle;
		}


        /// <summary>
        /// Returns a Rectangle that specifies the size and location of the buttons Image
        /// </summary>
        /// <param name="image">The buttons image</param>
        /// <param name="imageAlignment">The alignment of the image</param>
        /// <returns>A Rectangle that specifies the size and location of the buttons Image</returns>
        /// <MetaDataID>{7EA3C929-3B8B-4423-A06A-9A4A2243FFBD}</MetaDataID>
		protected Rectangle CalcImageRect(Image image, ContentAlignment imageAlignment)
		{
			Rectangle imageRect = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y, image.Width, image.Height);

			switch (imageAlignment)
			{
				case ContentAlignment.TopCenter:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.BottomCenter:
				{
					imageRect.X += (this.ClientRectangle.Width - image.Width) / 2;

					break;
				}

				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
				{
					imageRect.X = this.ClientRectangle.Right - image.Width;

					break;
				}
			}

			switch (imageAlignment)
			{
				case ContentAlignment.TopLeft:
				case ContentAlignment.TopCenter:
				case ContentAlignment.TopRight:
				{
					imageRect.Y += 2;

					break;
				}
				
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.MiddleCenter:
				case ContentAlignment.MiddleRight:
				{
					imageRect.Y += (this.ClientRectangle.Height - image.Height) / 2;

					break;
				}

				case ContentAlignment.BottomLeft:
				case ContentAlignment.BottomCenter:
				case ContentAlignment.BottomRight:
				{
					imageRect.Y = this.ClientRectangle.Bottom - image.Height - 2;

					break;
				}
			}

			return imageRect;
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets or sets the alignment of the Image displayed on the buttons
		/// </summary>
		public ContentAlignment ImageAlignment
		{
			get
			{
				return this.imageAlignment;
			}

			set
			{
				this.imageAlignment = value;
			}
		}

		#endregion


		#region Events

		#region Focus

        /// <summary>
        /// Raises the GotFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{846FF07F-295C-42DE-AA98-54F8C9ED29A0}</MetaDataID>
		public override void OnGotFocus(CellFocusEventArgs e)
		{
			base.OnGotFocus(e);

			// get the table to redraw the cell
			e.Table.Invalidate(e.CellRect);
		}


        /// <summary>
        /// Raises the LostFocus event
        /// </summary>
        /// <param name="e">A CellFocusEventArgs that contains the event data</param>
        /// <MetaDataID>{880D3B25-BFB9-47A8-A007-B1DDC632B3EF}</MetaDataID>
		public override void OnLostFocus(CellFocusEventArgs e)
		{
			base.OnLostFocus(e);

			// get the table to redraw the cell
			e.Table.Invalidate(e.CellRect);
		}

		#endregion

		#region Keys

        /// <summary>
        /// Raises the KeyDown event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{16D39BF5-2867-402F-A1A9-DB2BFCDE4C5C}</MetaDataID>
		public override void OnKeyDown(CellKeyEventArgs e)
		{
			base.OnKeyDown(e);

			// get the button renderer data
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);

			// 
			if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
			{
				rendererData.ButtonState = PushButtonStates.Pressed;

				e.Table.Invalidate(e.CellRect);
			}
		}


        /// <summary>
        /// Raises the KeyUp event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{24842393-0CAA-42BF-8948-B61E1FF6D40E}</MetaDataID>
		public override void OnKeyUp(CellKeyEventArgs e)
		{
			base.OnKeyUp(e);

			// get the button renderer data
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);

			// 
			if (e.KeyData == Keys.Enter || e.KeyData == Keys.Space)
			{
				rendererData.ButtonState = PushButtonStates.Normal;

				e.Table.Invalidate(e.CellRect);
				e.Table.OnCellButtonClicked(new CellButtonEventArgs(e.Cell, e.Column, e.Row));
			}
		}

		#endregion

		#region Mouse

		#region MouseEnter

        /// <summary>
        /// Raises the MouseEnter event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{03468C1A-69AC-43AC-968D-2278EA53C6E6}</MetaDataID>
		public override void OnMouseEnter(CellMouseEventArgs e)
		{
			base.OnMouseEnter(e);

			// get the button renderer data
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);

			// if the mouse is inside the button, make sure it is "hot"
			if (this.CalcButtonBounds().Contains(e.X, e.Y))
			{
				if (rendererData.ButtonState != PushButtonStates.Hot)
				{
					rendererData.ButtonState = PushButtonStates.Hot;

					e.Table.Invalidate(e.CellRect);
				}
			}
				// the mouse isn't inside the button, so it is in its normal state
			else
			{
				if (rendererData.ButtonState != PushButtonStates.Normal)
				{
					rendererData.ButtonState = PushButtonStates.Normal;

					e.Table.Invalidate(e.CellRect);
				}
			}
		}

		#endregion

		#region MouseLeave

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{95028781-8EBF-41C7-A89F-8B63F6233A72}</MetaDataID>
		public override void OnMouseLeave(CellMouseEventArgs e)
		{
			base.OnMouseLeave(e);

			// get the button renderer data
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);

			// make sure the button is in its normal state
			if (rendererData.ButtonState != PushButtonStates.Normal)
			{
				rendererData.ButtonState = PushButtonStates.Normal;

				e.Table.Invalidate(e.CellRect);
			}
		}

		#endregion

		#region MouseUp

        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{2A9A4BEE-CA89-470B-84E8-77A1C95F065A}</MetaDataID>
		public override void OnMouseUp(CellMouseEventArgs e)
		{
			base.OnMouseUp(e);

			// get the button renderer data
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);

			// check for the left mouse button
			if (e.Button == MouseButtons.Left)
			{
				Rectangle buttonRect = this.CalcButtonBounds();
				
				// if the mouse pointer is over the button, make sure 
				// the button is "hot"
				if (buttonRect.Contains(e.X, e.Y))
				{
					rendererData.ButtonState = PushButtonStates.Hot;

					e.Table.Invalidate(e.CellRect);

					// check if the click started inside the button.  if 
					// it did, Raise the tables CellButtonClicked event
					if (buttonRect.Contains(rendererData.ClickPoint))
					{
						e.Table.OnCellButtonClicked(new CellButtonEventArgs(e.Cell, e.Column, e.Row));
					}
				}
				else
				{
					// the mouse was released somewhere outside of the button, 
					// so make set the button back to its normal state
					if (rendererData.ButtonState != PushButtonStates.Normal)
					{
						rendererData.ButtonState = PushButtonStates.Normal;

						e.Table.Invalidate(e.CellRect);
					}
				}
			}

			// reset the click point
			rendererData.ClickPoint = Point.Empty;
		}

		#endregion

		#region MouseDown

        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{D5C9E411-310B-42F0-A3CF-8CBC38F67047}</MetaDataID>
		public override void OnMouseDown(CellMouseEventArgs e)
		{
			base.OnMouseDown(e);

			// get the button renderer data
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);

			// check if the left mouse button is pressed
			if (e.Button == MouseButtons.Left)
			{
				// record where the click started
				rendererData.ClickPoint = new Point(e.X, e.Y);
				
				// if the click was inside the button, set the button state to pressed
				if (this.CalcButtonBounds().Contains(rendererData.ClickPoint))
				{
					rendererData.ButtonState = PushButtonStates.Pressed;

					e.Table.Invalidate(e.CellRect);
				}
			}
		}

		#endregion

		#region MouseMove

        /// <summary>
        /// Raises the MouseMove event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{3B5846BC-16FB-4F78-9302-3C21A3D3DBA0}</MetaDataID>
		public override void OnMouseMove(CellMouseEventArgs e)
		{
			base.OnMouseMove(e);

			// get the button renderer data
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);

			Rectangle buttonRect = this.CalcButtonBounds();
			
			// check if the left mouse button is pressed
			if (e.Button == MouseButtons.Left)
			{
				// check if the mouse press originated in the button area
				if (buttonRect.Contains(rendererData.ClickPoint))
				{
					// check if the mouse is currently in the button
					if (buttonRect.Contains(e.X, e.Y))
					{
						// make sure the button is pressed
						if (rendererData.ButtonState != PushButtonStates.Pressed)
						{
							rendererData.ButtonState = PushButtonStates.Pressed;

							e.Table.Invalidate(e.CellRect);
						}
					}
					else
					{
						// the mouse isn't inside the button so make sure it is "hot"
						if (rendererData.ButtonState != PushButtonStates.Hot)
						{
							rendererData.ButtonState = PushButtonStates.Hot;

							e.Table.Invalidate(e.CellRect);
						}
					}
				}
			}
			else
			{
				// check if the mouse is currently in the button
				if (buttonRect.Contains(e.X, e.Y))
				{
					// the mouse is inside the button so make sure it is "hot"
					if (rendererData.ButtonState != PushButtonStates.Hot)
					{
						rendererData.ButtonState = PushButtonStates.Hot;

						e.Table.Invalidate(e.CellRect);
					}
				}
				else
				{
					// not inside the button so make sure it is in its normal state
					if (rendererData.ButtonState != PushButtonStates.Normal)
					{
						rendererData.ButtonState = PushButtonStates.Normal;

						e.Table.Invalidate(e.CellRect);
					}
				}
			}
		}

		#endregion

		#endregion

		#region Paint

        /// <summary>
        /// Raises the PaintCell event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{733FBC51-EB11-4427-A631-D396D34E6217}</MetaDataID>
		public override void OnPaintCell(PaintCellEventArgs e)
		{
			if (e.Table.ColumnModel.Columns[e.Column] is ButtonColumn)
			{
				this.ImageAlignment = ((ButtonColumn) e.Table.ColumnModel.Columns[e.Column]).ImageAlignment;
			}
			else
			{
				this.ImageAlignment = ContentAlignment.MiddleLeft;
			}
			
			base.OnPaintCell(e);
		}


        /// <summary>
        /// Raises the PaintBackground event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{7B40D4CC-6A2E-423B-A097-7D1838B17123}</MetaDataID>
		protected override void OnPaintBackground(PaintCellEventArgs e)
		{
			base.OnPaintBackground(e);

			// don't bother going any further if the Cell is null 
			if (e.Cell == null)
			{
				return;
			}

			// get the button state
			ButtonRendererData rendererData = this.GetButtonRendererData(e.Cell);
			PushButtonStates state = rendererData.ButtonState;

			// if the cell has focus and is in its normal state, 
			// make the button look like a default button
			if (state == PushButtonStates.Normal && e.Focused)
			{
				state = PushButtonStates.Default;
			}

			// if the table is not enabled, make sure the button is disabled
			if (!e.Enabled)
			{
				state = PushButtonStates.Disabled;
			}

			// draw the button
			ThemeManager.DrawButton(e.Graphics, this.CalcButtonBounds(), state);
		}


        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{B5D8E5D9-F192-4B25-9E81-1DA71BC8E61E}</MetaDataID>
		protected override void OnPaint(PaintCellEventArgs e)
		{
			base.OnPaint(e);

			// don't bother going any further if the Cell is null 
			if (e.Cell == null)
			{
				return;
			}

			Rectangle textRect = this.CalcButtonBounds();
			textRect.Inflate(-4, -2);

			if (e.Cell.Image != null)
			{
				Rectangle imageRect = this.CalcImageRect(e.Cell.Image, this.ImageAlignment);

				if (this.GetButtonRendererData(e.Cell).ButtonState == PushButtonStates.Pressed && !ThemeManager.VisualStylesEnabled)
				{
					imageRect.X += 1;
					imageRect.Y += 1;
				}
				
				this.DrawImage(e.Graphics, e.Cell.Image, imageRect, e.Enabled);
			}

			// draw the text
			if (e.Cell.Text != null && e.Cell.Text.Length != 0)
			{
				if (e.Enabled)
				{
					if (!ThemeManager.VisualStylesEnabled && this.GetButtonRendererData(e.Cell).ButtonState == PushButtonStates.Pressed)
					{
						textRect.X += 1;
						textRect.Y += 1;
					}

					// if the cell or the row it is in is selected 
					// our forecolor will be the selection forecolor.
					// we'll ignore this and reset our forecolor to 
					// that of the cell being rendered
					if (e.Selected)
					{
						this.ForeColor = e.Cell.ForeColor;
					}
					
					e.Graphics.DrawString(e.Cell.Text, this.Font, this.ForeBrush, textRect, this.StringFormat);
				}
				else
				{
					e.Graphics.DrawString(e.Cell.Text, this.Font, this.GrayTextBrush, textRect, this.StringFormat);
				}
			}

			// draw focus
			if (e.Focused && e.Enabled)
			{
				Rectangle focusRect = this.CalcButtonBounds();
				
				if (ThemeManager.VisualStylesEnabled)
				{
					focusRect.Inflate(-3, -3);

					if (this.GetButtonRendererData(e.Cell).ButtonState != PushButtonStates.Pressed)
					{
						ControlPaint.DrawFocusRectangle(e.Graphics, focusRect);
					}
				}
				else
				{
					focusRect.Inflate(-4, -4);

					ControlPaint.DrawFocusRectangle(e.Graphics, focusRect);
				}
			}
		}


        /// <summary>
        /// Draws the Image displayed on the button
        /// </summary>
        /// <param name="g">The Graphics to draw on</param>
        /// <param name="image">The Image to draw</param>
        /// <param name="imageRect">A Rectangle that specifies the location 
        /// of the Image</param>
        /// <param name="enabled">Specifies whether the Image should be drawn 
        /// in an enabled state</param>
        /// <MetaDataID>{58560E8D-7866-4025-A1ED-1CC8F650FA99}</MetaDataID>
		protected void DrawImage(Graphics g, Image image, Rectangle imageRect, bool enabled)
		{
			if (enabled)
			{
				g.DrawImageUnscaled(image, imageRect);
			}
			else
			{
				ControlPaint.DrawImageDisabled(g, image, imageRect.X, imageRect.Y, this.BackColor);
			}
		}

		#endregion

		#endregion
	}
}
