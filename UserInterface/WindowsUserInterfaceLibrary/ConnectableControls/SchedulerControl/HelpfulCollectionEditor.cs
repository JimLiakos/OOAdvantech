using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ConnectableControls.SchedulerControl
{
    /// <summary>
    /// A CollectionEditor that displays the help and command areas of its PropertyGrid
    /// </summary>
    /// <MetaDataID>{59a67887-3aab-46fd-b3d1-702c8707ea8b}</MetaDataID>
    public class HelpfulCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the HelpfulCollectionEditor class using 
        /// the specified collection type
        /// </summary>
        /// <param name="type">The type of the collection for this editor to edit</param>
        /// <MetaDataID>{4A52032F-7077-4372-923E-C2B955953B3A}</MetaDataID>
        public HelpfulCollectionEditor(Type type)
            : base(type)
        {

        }


        /// <summary>
        /// Creates a new form to display and edit the current collection
        /// </summary>
        /// <returns>An instance of CollectionEditor.CollectionForm to provide as the 
        /// user interface for editing the collection</returns>
        /// <MetaDataID>{7DC71936-A7A2-4A71-A1CE-AC0781EB9332}</MetaDataID>
        protected override CollectionEditor.CollectionForm CreateCollectionForm()
        {
            CollectionEditor.CollectionForm editor = base.CreateCollectionForm();

            foreach (Control control in editor.Controls)
            {
                //
                if (control is PropertyGrid)
                {
                    PropertyGrid grid = (PropertyGrid)control;

                    grid.HelpVisible = true;
                    grid.CommandsVisibleIfAvailable = true;
                }
            }

            return editor;
        }
    }
}
