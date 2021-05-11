using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.SchedulerControl
{
    /// <summary>
    /// Provides a user interface that can edit collections of Columns 
    /// at design time
    /// </summary>
    /// <MetaDataID>{58063579-b417-4993-8694-ae9450874d3a}</MetaDataID>
    public class ColumnCollectionEditor : HelpfulCollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the HelpfulCollectionEditor class using 
        /// the specified collection type
        /// </summary>
        /// <param name="type">The type of the collection for this editor to edit</param>
        /// <MetaDataID>{4A52032F-7077-4372-923E-C2B955953B3A}</MetaDataID>
        public ColumnCollectionEditor(Type type)
            : base(type)
        {

        }
    }
}
