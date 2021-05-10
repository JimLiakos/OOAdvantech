using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace StyleableWindow
{
    /// <MetaDataID>{4d269b6c-394f-425e-903b-d11134875c35}</MetaDataID>
    public class ComboBoxEx : ComboBox
    {
        //PopUpFooter

        public System.Windows.Controls.ContentControl PopUpFooter
        {
            get { return (System.Windows.Controls.ContentControl)GetValue(PopUpFooterProperty); }
            set { SetValue(PopUpFooterProperty, value); }
        }

        public static readonly DependencyProperty PopUpFooterProperty =DependencyProperty.Register("PopUpFooter", typeof(ContentControl), typeof(ComboBoxEx), new UIPropertyMetadata(null));
    }
}
