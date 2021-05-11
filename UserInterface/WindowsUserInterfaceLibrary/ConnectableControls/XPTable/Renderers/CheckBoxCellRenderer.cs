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
    /// A CellRenderer that draws Cell contents as CheckBoxes
    /// </summary>
    /// <MetaDataID>{9E9C35AE-51FA-420A-9BEA-F8C989F08B54}</MetaDataID>
	public class CheckBoxCellRenderer : CellRenderer
	{
		#region Class Data
		
		/// <summary>
		/// The size of the checkbox
		/// </summary>
		private Size checkSize;

		/// <summary>
		/// Specifies whether any text contained in the Cell should be drawn
		/// </summary>
		private bool drawText;

		#endregion
		

		#region Constructor

        /// <summary>
        /// Initializes a new instance of the CheckBoxCellRenderer class with 
        /// default settings
        /// </summary>
        /// <MetaDataID>{9E8897D7-CD06-482B-B48F-A6106743A81E}</MetaDataID>
		public CheckBoxCellRenderer() : base()
		{
			this.checkSize = new Size(13, 13);
			this.drawText = true;
		}

		#endregion


		#region Methods

        /// <summary>
        /// Gets the Rectangle that specifies the Size and Location of 
        /// the check box contained in the current Cell
        /// </summary>
        /// <returns>A Rectangle that specifies the Size and Location of 
        /// the check box contained in the current Cell</returns>
        /// <MetaDataID>{7EA3B92B-C1D3-44C9-9D59-EB13BF24C306}</MetaDataID>
		protected Rectangle CalcCheckRect(RowAlignment rowAlignment, ColumnAlignment columnAlignment)
		{
			Rectangle checkRect = new Rectangle(this.ClientRectangle.Location, this.CheckSize);
			
			if (checkRect.Height > this.ClientRectangle.Height)
			{
				checkRect.Height = this.ClientRectangle.Height;
				checkRect.Width = checkRect.Height;
			}

			switch (rowAlignment)
			{
				case RowAlignment.Center:
				{
					checkRect.Y += (this.ClientRectangle.Height - checkRect.Height) / 2;

					break;
				}

				case RowAlignment.Bottom:
				{
					checkRect.Y = this.ClientRectangle.Bottom - checkRect.Height;

					break;
				}
			}

			if (!this.DrawText)
			{
				if (columnAlignment == ColumnAlignment.Center)
				{
					checkRect.X += (this.ClientRectangle.Width - checkRect.Width) / 2;
				}
				else if (columnAlignment == ColumnAlignment.Right)
				{
					checkRect.X = this.ClientRectangle.Right - checkRect.Width;
				}
			}

			return checkRect;
		}


        /// <summary>
        /// Gets the CheckBoxCellRenderer specific data used by the Renderer from 
        /// the specified Cell
        /// </summary>
        /// <param name="cell">The Cell to get the CheckBoxCellRenderer data for</param>
        /// <returns>The CheckBoxCellRenderer data for the specified Cell</returns>
        /// <MetaDataID>{D2953767-2CEC-47EF-998C-866B233DD4B9}</MetaDataID>
		protected CheckBoxRendererData GetCheckBoxRendererData(Cell cell)
		{
			object rendererData = this.GetRendererData(cell);

			if (rendererData == null || !(rendererData is CheckBoxRendererData))
			{
				if (cell.CheckState == CheckState.Unchecked)
				{
					rendererData = new CheckBoxRendererData(CheckBoxStates.UncheckedNormal);
				}
				else if (cell.CheckState == CheckState.Indeterminate && cell.ThreeState)
				{
					rendererData = new CheckBoxRendererData(CheckBoxStates.MixedNormal);
				}
				else 
				{
					rendererData = new CheckBoxRendererData(CheckBoxStates.CheckedNormal);
				}

				this.SetRendererData(cell, rendererData);
			}

			this.ValidateCheckState(cell, (CheckBoxRendererData) rendererData);

			return (CheckBoxRendererData) rendererData;
		}


        /// <summary>
        /// Corrects any differences between the check state of the specified Cell 
        /// and the check state in its rendererData
        /// </summary>
        /// <param name="cell">The Cell to chech</param>
        /// <param name="rendererData">The CheckBoxRendererData to check</param>
        /// <MetaDataID>{18CB5289-305B-4448-A5A6-DB1264A1129B}</MetaDataID>
		private void ValidateCheckState(Cell cell, CheckBoxRendererData rendererData)
		{
			switch (cell.CheckState)
			{
				case CheckState.Checked:
				{		
					if (rendererData.CheckState <= CheckBoxStates.UncheckedDisabled)
					{
						rendererData.CheckState |= (CheckBoxStates) 4;
					}
					else if (rendererData.CheckState >= CheckBoxStates.MixedNormal)
					{
						rendererData.CheckState -= (CheckBoxStates) 4;
					}
					
					break;
				}

				case CheckState.Indeterminate:
				{		
					if (rendererData.CheckState <= CheckBoxStates.UncheckedDisabled)
					{
						rendererData.CheckState |= (CheckBoxStates) 8;
					}
					else if (rendererData.CheckState <= CheckBoxStates.CheckedDisabled)
					{
						rendererData.CheckState |= (CheckBoxStates) 4;
					}
					
					break;
				}

				default:
				{
					if (rendererData.CheckState >= CheckBoxStates.MixedNormal)
					{
						rendererData.CheckState -= (CheckBoxStates) 8;
					}
					else if (rendererData.CheckState >= CheckBoxStates.CheckedNormal)
					{
						rendererData.CheckState -= (CheckBoxStates) 4;
					}
					
					break;
				}
			}
		}

		#endregion


		#region Properties

		/// <summary>
		/// Gets the size of the checkbox
		/// </summary>
		protected Size CheckSize
		{
			get
			{
				return this.checkSize;
			}
		}

		
		/// <summary>
		/// Gets or sets whether any text contained in the Cell should be drawn
		/// </summary>
		public bool DrawText
		{
			get
			{
				return this.drawText;
			}
		}

		#endregion


		#region Events

		#region Keys

        /// <summary>
        /// Raises the KeyDown event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{601BBA72-51D6-45B5-9C93-8D0BD9B1FCA7}</MetaDataID>
		public override void OnKeyDown(CellKeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyData == Keys.Space && e.Table.IsCellEditable(e.CellPos))
			{
				// get the renderer data
				CheckBoxRendererData rendererData = this.GetCheckBoxRendererData(e.Cell);

				//
				if (e.Cell.CheckState == CheckState.Checked)
				{
					rendererData.CheckState = CheckBoxStates.CheckedPressed;
				}
				else if (e.Cell.CheckState == CheckState.Indeterminate)
				{
					rendererData.CheckState = CheckBoxStates.MixedPressed;
				}
				else //if (e.Cell.CheckState == CheckState.Unchecked)
				{
					rendererData.CheckState = CheckBoxStates.UncheckedPressed;
				}

				e.Table.Invalidate(e.CellRect);
			}
		}


        /// <summary>
        /// Raises the KeyUp event
        /// </summary>
        /// <param name="e">A CellKeyEventArgs that contains the event data</param>
        /// <MetaDataID>{8078E65D-4B01-483F-AB2B-42543663AC3F}</MetaDataID>
		public override void OnKeyUp(CellKeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (e.KeyData == Keys.Space && e.Table.IsCellEditable(e.CellPos))
			{
				// get the renderer data
				CheckBoxRendererData rendererData = this.GetCheckBoxRendererData(e.Cell);

				//
				if (e.Cell.CheckState == CheckState.Checked)
				{
					if (!e.Cell.ThreeState || !(e.Table.ColumnModel.Columns[e.Column] is CheckBoxColumn) || 
						((CheckBoxColumn) e.Table.ColumnModel.Columns[e.Column]).CheckStyle == CheckBoxColumnStyle.RadioButton)
					{
						rendererData.CheckState = CheckBoxStates.UncheckedNormal;
						e.Cell.CheckState = CheckState.Unchecked;
                        e.Cell.Column.SetValue(e.Cell.Row, false);
					}
					else
					{
						rendererData.CheckState = CheckBoxStates.MixedNormal;
						e.Cell.CheckState = CheckState.Indeterminate;
					}
				}
				else if (e.Cell.CheckState == CheckState.Indeterminate)
				{
					rendererData.CheckState = CheckBoxStates.UncheckedNormal;
					e.Cell.CheckState = CheckState.Unchecked;
                    e.Cell.Column.SetValue(e.Cell.Row, false);
				}
				else //if (e.Cell.CheckState == CheckState.Unchecked)
				{
					rendererData.CheckState = CheckBoxStates.CheckedNormal;
					e.Cell.CheckState = CheckState.Checked;
                    e.Cell.Column.SetValue(e.Cell.Row, true);
				}

				e.Table.Invalidate(e.CellRect);
			}
		}

		#endregion

		#region Mouse

		#region MouseLeave

        /// <summary>
        /// Raises the MouseLeave event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{A914D04F-8BD6-48B1-9508-D28D1B877816}</MetaDataID>
		public override void OnMouseLeave(CellMouseEventArgs e)
		{
			base.OnMouseLeave(e);

			if (e.Table.IsCellEditable(e.CellPos))
			{
				// get the renderer data
				CheckBoxRendererData rendererData = this.GetCheckBoxRendererData(e.Cell);

				if (e.Cell.CheckState == CheckState.Checked)
				{
					if (rendererData.CheckState != CheckBoxStates.CheckedNormal)
					{
						rendererData.CheckState = CheckBoxStates.CheckedNormal;

						e.Table.Invalidate(e.CellRect);
					}
				}
				else if (e.Cell.CheckState == CheckState.Indeterminate)
				{
					if (rendererData.CheckState != CheckBoxStates.MixedNormal)
					{
						rendererData.CheckState = CheckBoxStates.MixedNormal;

						e.Table.Invalidate(e.CellRect);
					}
				}
				else //if (e.Cell.CheckState == CheckState.Unchecked)
				{
					if (rendererData.CheckState != CheckBoxStates.UncheckedNormal)
					{
						rendererData.CheckState = CheckBoxStates.UncheckedNormal;

						e.Table.Invalidate(e.CellRect);
					}
				}
			}
		}

		#endregion

		#region MouseUp

        /// <summary>
        /// Raises the MouseUp event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{5AD48824-D2AD-4A0C-82C3-6F3890BAC165}</MetaDataID>
		public override void OnMouseUp(CellMouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (e.Table.IsCellEditable(e.CellPos))
			{
				// get the renderer data
				CheckBoxRendererData rendererData = this.GetCheckBoxRendererData(e.Cell);

				if (this.CalcCheckRect(e.Table.TableModel.Rows[e.Row].Alignment, e.Table.ColumnModel.Columns[e.Column].Alignment).Contains(e.X, e.Y))
				{
					if (e.Button == MouseButtons.Left && e.Table.LastMouseDownCell.Row == e.Row && e.Table.LastMouseDownCell.Column == e.Column)
					{
						//
						if (e.Cell.CheckState == CheckState.Checked)
						{
							if (!e.Cell.ThreeState || !(e.Table.ColumnModel.Columns[e.Column] is CheckBoxColumn) || 
								((CheckBoxColumn) e.Table.ColumnModel.Columns[e.Column]).CheckStyle == CheckBoxColumnStyle.RadioButton)
							{
								rendererData.CheckState = CheckBoxStates.UncheckedHot;
								e.Cell.CheckState = CheckState.Unchecked;
                                e.Cell.Column.SetValue(e.Cell.Row, false);
							}
							else
							{
								rendererData.CheckState = CheckBoxStates.MixedHot;
								e.Cell.CheckState = CheckState.Indeterminate;
							}
						}
						else if (e.Cell.CheckState == CheckState.Indeterminate)
						{
							rendererData.CheckState = CheckBoxStates.UncheckedHot;
                            e.Cell.Column.SetValue(e.Cell.Row, false);
						}
						else //if (e.Cell.CheckState == CheckState.Unchecked)
						{
							rendererData.CheckState = CheckBoxStates.CheckedHot;
							e.Cell.CheckState = CheckState.Checked;
                            e.Cell.Column.SetValue(e.Cell.Row, true);
						}

						e.Table.Invalidate(e.CellRect);
					}
				}
			}
		}

		#endregion

		#region MouseDown

        /// <summary>
        /// Raises the MouseDown event
        /// </summary>
        /// <param name="e">A CellMouseEventArgs that contains the event data</param>
        /// <MetaDataID>{2DCABA2A-28FC-4616-AB9A-F4F225D9D4CE}</MetaDataID>
		public override void OnMouseDown(CellMouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Table.IsCellEditable(e.CellPos))
			{
				// get the renderer data
				CheckBoxRendererData rendererData = this.GetCheckBoxRendererData(e.Cell);

				if (this.CalcCheckRect(e.Table.TableModel.Rows[e.Row].Alignment, e.Table.ColumnModel.Columns[e.Column].Alignment).Contains(e.X, e.Y))
				{
					//
					if (e.Cell.CheckState == CheckState.Checked)
					{
						rendererData.CheckState = CheckBoxStates.CheckedPressed;
					}
					else if (e.Cell.CheckState == CheckState.Indeterminate)
					{
						rendererData.CheckState = CheckBoxStates.MixedPressed;
					}
					else //if (e.Cell.CheckState == CheckState.Unchecked)
					{
						rendererData.CheckState = CheckBoxStates.UncheckedPressed;
					}

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
        /// <MetaDataID>{29085C15-8AB2-4A13-A52B-2C15A3C03E06}</MetaDataID>
        public override void OnMouseMove(List.Events.CellMouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.Table.IsCellEditable(e.CellPos))
			{
				// get the renderer data
				CheckBoxRendererData rendererData = this.GetCheckBoxRendererData(e.Cell);

				if (this.CalcCheckRect(e.Table.TableModel.Rows[e.Row].Alignment, e.Table.ColumnModel.Columns[e.Column].Alignment).Contains(e.X, e.Y))
				{
					if (e.Cell.CheckState == CheckState.Checked)
					{
						if (rendererData.CheckState == CheckBoxStates.CheckedNormal)
						{
							if (e.Button == MouseButtons.Left && e.Row == e.Table.LastMouseDownCell.Row && e.Column == e.Table.LastMouseDownCell.Column)
							{
								rendererData.CheckState = CheckBoxStates.CheckedPressed;
							}
							else
							{
								rendererData.CheckState = CheckBoxStates.CheckedHot;
							}

							e.Table.Invalidate(e.CellRect);
						}
					}
					else if (e.Cell.CheckState == CheckState.Indeterminate)
					{
						if (rendererData.CheckState == CheckBoxStates.MixedNormal)
						{
							if (e.Button == MouseButtons.Left && e.Row == e.Table.LastMouseDownCell.Row && e.Column == e.Table.LastMouseDownCell.Column)
							{
								rendererData.CheckState = CheckBoxStates.MixedPressed;
							}
							else
							{
								rendererData.CheckState = CheckBoxStates.MixedHot;
							}

							e.Table.Invalidate(e.CellRect);
						}
					}
					else //if (e.Cell.CheckState == CheckState.Unchecked)
					{
						if (rendererData.CheckState == CheckBoxStates.UncheckedNormal)
						{
							if (e.Button == MouseButtons.Left && e.Row == e.Table.LastMouseDownCell.Row && e.Column == e.Table.LastMouseDownCell.Column)
							{
								rendererData.CheckState = CheckBoxStates.UncheckedPressed;
							}
							else
							{
								rendererData.CheckState = CheckBoxStates.UncheckedHot;
							}

							e.Table.Invalidate(e.CellRect);
						}
					}
				}
				else
				{
					if (e.Cell.CheckState == CheckState.Checked)
					{
						rendererData.CheckState = CheckBoxStates.CheckedNormal;
					}
					else if (e.Cell.CheckState == CheckState.Indeterminate)
					{
						rendererData.CheckState = CheckBoxStates.MixedNormal;
					}
					else //if (e.Cell.CheckState == CheckState.Unchecked)
					{
						rendererData.CheckState = CheckBoxStates.UncheckedNormal;
					}

					e.Table.Invalidate(e.CellRect);
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
        /// <MetaDataID>{216A895D-DAA5-474A-ADDD-BA923A593D35}</MetaDataID>
		public override void OnPaintCell(PaintCellEventArgs e)
		{
			if (e.Table.ColumnModel.Columns[e.Column] is CheckBoxColumn)
			{
				CheckBoxColumn column = (CheckBoxColumn) e.Table.ColumnModel.Columns[e.Column];

				this.checkSize = column.CheckSize;
				this.drawText = column.DrawText;
			}
			else
			{
				this.checkSize = new Size(13, 13);
				this.drawText = true;
			}
			
			base.OnPaintCell(e);
		}


        /// <summary>
        /// Raises the Paint event
        /// </summary>
        /// <param name="e">A PaintCellEventArgs that contains the event data</param>
        /// <MetaDataID>{334E5A45-94DD-4D56-AF19-A64A9B4C9896}</MetaDataID>
		protected override void OnPaint(PaintCellEventArgs e)
		{
			base.OnPaint(e);

			// don't bother if the Cell is null
			if (e.Cell == null)
			{
				return;
			}

			Rectangle checkRect = this.CalcCheckRect(this.LineAlignment, this.Alignment);

			CheckBoxStates state = this.GetCheckBoxRendererData(e.Cell).CheckState;

			if (!e.Enabled)
			{
				if (e.Cell.CheckState == CheckState.Checked)
				{
					state = CheckBoxStates.CheckedDisabled;
				}
				else if (e.Cell.CheckState == CheckState.Indeterminate)
				{
					state = CheckBoxStates.MixedDisabled;
				}
				else // if (e.Cell.CheckState == CheckState.Unchecked)
				{
					state = CheckBoxStates.UncheckedDisabled;
				}
			}
			
			if (e.Table.ColumnModel.Columns[e.Column] is CheckBoxColumn && 
				((CheckBoxColumn) e.Table.ColumnModel.Columns[e.Column]).CheckStyle != CheckBoxColumnStyle.CheckBox)
			{
				// remove any mixed states
				switch (state)
				{
					case CheckBoxStates.MixedNormal:
						state = CheckBoxStates.CheckedNormal;
						break;

					case CheckBoxStates.MixedHot:
						state = CheckBoxStates.CheckedHot;
						break;

					case CheckBoxStates.MixedPressed:
						state = CheckBoxStates.CheckedPressed;
						break;

					case CheckBoxStates.MixedDisabled:
						state = CheckBoxStates.CheckedDisabled;
						break;
				}
				
				ThemeManager.DrawRadioButton(e.Graphics, checkRect, (RadioButtonStates) state);
			}
			else
			{
				ThemeManager.DrawCheck(e.Graphics, checkRect, state);
			}

			if (this.DrawText)
			{
				string text = e.Cell.Text;

				if (text != null && text.Length != 0)
				{
					Rectangle textRect = this.ClientRectangle;
					textRect.X += checkRect.Width + 1;
					textRect.Width -= checkRect.Width + 1;

					if (e.Enabled)
					{
						e.Graphics.DrawString(e.Cell.Text, this.Font, this.ForeBrush, textRect, this.StringFormat);
					}
					else
					{
						e.Graphics.DrawString(e.Cell.Text, this.Font, this.GrayTextBrush, textRect, this.StringFormat);
					}
				}
			}
			
			if (e.Focused && e.Enabled)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
			}
		}

		#endregion

		#endregion
	}
}
