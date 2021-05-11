using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.SchedulerControl.Events
{
    /// <summary>
    /// Represents the methods that will handle the PropertyChanged event of a Column, 
    /// or a Table's BeginSort and EndSort events
    /// </summary>
    public delegate void ColumnEventHandler(object sender, ColumnEventArgs e);


    /// <MetaDataID>{860fe23d-4457-42d6-baed-65b2b571946d}</MetaDataID>
    public class ColumnEventArgs
    {
        #region Class Data

        /// <summary>
        /// The Column that Raised the event
        /// </summary>
        private Column source;

        /// <summary>
        /// The index of the Column in the ColumnModel
        /// </summary>
        private int index;

        /// <summary>
        /// The old value of the property that changed
        /// </summary>
        private object oldValue;

        /// <summary>
        /// The type of event
        /// </summary>
        private ColumnEventType eventType;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ColumnEventArgs class with 
        /// the specified Column source, column index and event type
        /// </summary>
        /// <param name="source">The Column that Raised the event</param>
        /// <param name="eventType">The type of event</param>
        /// <param name="oldValue">The old value of the changed property</param>
        /// <MetaDataID>{7659647C-CD80-403D-ADB8-D6F836072620}</MetaDataID>
        public ColumnEventArgs(Column source, ColumnEventType eventType, object oldValue)
            : this(source, -1, eventType, oldValue)
        {

        }


        /// <summary>
        /// Initializes a new instance of the ColumnEventArgs class with 
        /// the specified Column source, column index and event type
        /// </summary>
        /// <param name="source">The Column that Raised the event</param>
        /// <param name="index">The index of the Column</param>
        /// <param name="eventType">The type of event</param>
        /// <param name="oldValue">The old value of the changed property</param>
        /// <MetaDataID>{6C19F2AA-E12A-4702-AF53-0D3A441AA5A5}</MetaDataID>
        public ColumnEventArgs(Column source, int index, ColumnEventType eventType, object oldValue)
            : base()
        {
            this.source = source;
            this.index = index;
            this.eventType = eventType;
            this.oldValue = oldValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Column that Raised the event
        /// </summary>
        public Column Column
        {
            get
            {
                return this.source;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="column"></param>
        /// <MetaDataID>{E1BCE01F-4D0E-48C2-A0C1-43469F4242EF}</MetaDataID>
        internal void SetColumn(Column column)
        {
            this.source = column;
        }


        /// <summary>
        /// Gets the index of the Column
        /// </summary>
        public int Index
        {
            get
            {
                return this.index;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <MetaDataID>{E41ED65D-6CE9-4236-8645-F44C55427A04}</MetaDataID>
        internal void SetIndex(int index)
        {
            this.index = index;
        }


        /// <summary>
        /// Gets the type of event
        /// </summary>
        public ColumnEventType EventType
        {
            get
            {
                return this.eventType;
            }
        }


        /// <summary>
        /// Gets the old value of the Columns changed property
        /// </summary>
        public object OldValue
        {
            get
            {
                return this.oldValue;
            }
        }

        #endregion
    }


    /// <summary>
    /// Specifies the type of event generated when the value of a 
    /// Column's property changes
    /// </summary>
    /// <MetaDataID>{6b624536-1410-4406-b192-294564b9549d}</MetaDataID>
    public enum ColumnEventType
    {
        /// <summary>
        /// Occurs when the Column's property change type is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Occurs when the value of a Column's Text property changes
        /// </summary>
        TextChanged = 1,

        /// <summary>
        /// Occurs when the value of a Column's Alignment property changes
        /// </summary>
        AlignmentChanged = 2,

        /// <summary>
        /// Occurs when the value of a Column's HeaderAlignment property changes
        /// </summary>
        HeaderAlignmentChanged = 3,

        /// <summary>
        /// Occurs when the value of a Column's Width property changes
        /// </summary>
        WidthChanged = 4,

        /// <summary>
        /// Occurs when the value of a Column's Visible property changes
        /// </summary>
        VisibleChanged = 5,

        /// <summary>
        /// Occurs when the value of a Column's Image property changes
        /// </summary>
        ImageChanged = 6,

        /// <summary>
        /// Occurs when the value of a Column's Format property changes
        /// </summary>
        FormatChanged = 7,

        /// <summary>
        /// Occurs when the value of a Column's ColumnState property changes
        /// </summary>
        StateChanged = 8,

        /// <summary>
        /// Occurs when the value of a Column's Renderer property changes
        /// </summary>
        RendererChanged = 9,

        /// <summary>
        /// Occurs when the value of a Column's Editor property changes
        /// </summary>
        EditorChanged = 10,

        /// <summary>
        /// Occurs when the value of a Column's Comparer property changes
        /// </summary>
        ComparerChanged = 11,

        /// <summary>
        /// Occurs when the value of a Column's Enabled property changes
        /// </summary>
        EnabledChanged = 12,

        /// <summary>
        /// Occurs when the value of a Column's Editable property changes
        /// </summary>
        EditableChanged = 13,

        /// <summary>
        /// Occurs when the value of a Column's Selectable property changes
        /// </summary>
        SelectableChanged = 14,

        /// <summary>
        /// Occurs when the value of a Column's Sortable property changes
        /// </summary>
        SortableChanged = 15,

        /// <summary>
        /// Occurs when the value of a Column's SortOrder property changes
        /// </summary>
        SortOrderChanged = 16,

        /// <summary>
        /// Occurs when the value of a Column's ToolTipText property changes
        /// </summary>
        ToolTipTextChanged = 17,

        /// <summary>
        /// Occurs when a Column is being sorted
        /// </summary>
        Sorting = 18,

        Type = 19
    }

}
