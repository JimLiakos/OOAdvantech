using OOAdvantech.Transactions;
namespace OOAdvantech.UserInterface
{
    /// <MetaDataID>{94f32ffd-ab9e-4497-b40d-66ae24e1310c}</MetaDataID>
    public class ImageColumn : OOAdvantech.UserInterface.Column
    {

        /// <exclude>Excluded</exclude>
        private ImageBoxSizeMode _SizeMode;

        /// <MetaDataID>{8f18cf08-3a76-4ca3-ad49-edb78164ce4a}</MetaDataID>
        [MetaDataRepository.PersistentMember("_SizeMode")]
        [MetaDataRepository.BackwardCompatibilityID("+2")]
        public ImageBoxSizeMode SizeMode
        { 
            get
            {
                return _SizeMode;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _SizeMode = value;
                    stateTransition.Consistent = true;
                }
                
            }
        }

        /// <exclude>Excluded</exclude>
        private bool _DrawText;

        /// <MetaDataID>{0971e5c2-e04a-4b2b-9e20-5486dc47c766}</MetaDataID>
        [MetaDataRepository.PersistentMember("_DrawText")]
        [MetaDataRepository.BackwardCompatibilityID("+1")]
        public bool DrawText
        {
            get
            {
                return _DrawText;
            }
            set
            {
                if (_DrawText != value)
                {
                    using (Transactions.ObjectStateTransition stateTransition = new Transactions.ObjectStateTransition(this))
                    {
                        _DrawText = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
     
  
         /// <MetaDataID>{bc15997d-f090-4ca3-bfea-4ca40485a439}</MetaDataID>
        public ImageColumn()
        {

        }
        /// <MetaDataID>{1fc4ddc9-25b0-40c2-8921-c23125cb7717}</MetaDataID>
        public ImageColumn(Column copyColumn)
            : base(copyColumn)
        {

        }
    }


    // Summary:
    //     Specifies how an image is positioned within a System.Windows.Forms.PictureBox.

    /// <MetaDataID>{94f32ffd-ab9e-4497-b40d-66ae24e1310c}</MetaDataID>
    public enum ImageBoxSizeMode
    {
        // Summary:
        //     The image is placed in the upper-left corner of the System.Windows.Forms.PictureBox.
        //     The image is clipped if it is larger than the System.Windows.Forms.PictureBox
        //     it is contained in.
        Normal = 0,
        //
        // Summary:
        //     The image within the System.Windows.Forms.PictureBox is stretched or shrunk
        //     to fit the size of the System.Windows.Forms.PictureBox.
        StretchImage = 1,
        //
        // Summary:
        //     The System.Windows.Forms.PictureBox is sized equal to the size of the image
        //     that it contains.
        AutoSize = 2,
        //
        // Summary:
        //     The image is displayed in the center if the System.Windows.Forms.PictureBox
        //     is larger than the image. If the image is larger than the System.Windows.Forms.PictureBox,
        //     the picture is placed in the center of the System.Windows.Forms.PictureBox
        //     and the outside edges are clipped.
        CenterImage = 3,
        //
        // Summary:
        //     The size of the image is increased or decreased maintaining the size ratio.
        Zoom = 4,
    }
}
