using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UserInterfaceTest
{

    /// <MetaDataID>{7a4f740f-1e84-4a6b-baa1-85a0f454f35e}</MetaDataID>
    public partial class Form2 : Form
    {
        static int AutoIncrement = 0;
        string text = "OrderForm";
        public Form2()
        {
            AutoIncrement++;
            text = text + AutoIncrement.ToString();


            InitializeComponent();
            Text = text; 
        }
        public override string Text
        {
            get
            {
                return text;
            }
            set
            {
                base.Text = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    
    }
}