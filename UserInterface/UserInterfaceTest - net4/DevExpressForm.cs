using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Collections;

namespace UserInterfaceTest
{
    /// <MetaDataID>{9e0de0b3-147e-4e39-af95-9a44d8496586}</MetaDataID>
    public partial class DevExpressForm : Form
    {
        /// <MetaDataID>{8025209a-1ebd-46fc-b31b-eaf8c24fd3a4}</MetaDataID>
        List<int> esen;
        /// <MetaDataID>{4b42c83a-d2e2-46d9-8a8f-0995b1943cb6}</MetaDataID>
        public DevExpressForm()
        {
            InitializeComponent();

            esen=new List<int>();
            esen.Add(0);
            //esen.Add(1);
            //esen.Add(2);
            //esen.Add(3);
            //esen.Add(4);
            //esen.Add(5);
            //esen.Add(6);
            //esen.Add(7);
            //esen.Add(8);
            //esen.Add(9);
          //  NumberLookUpEdit.DataSource = esen;
        }
        /// <MetaDataID>{076b4efc-590f-4311-b024-e34475649758}</MetaDataID>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


           ObjectStorage objectStorage = Form3.OpenStorage();

            string objectQuery = "#OQL: SELECT productPrice  " +
                              " FROM AbstractionsAndPersistency.ProductPrice productPrice  #";//WHERE productPrice.PriceList = @PriceList#";

            List<AbstractionsAndPersistency.IProductPrice> productPrices = new List<AbstractionsAndPersistency.IProductPrice>();
            StructureSet objectSet = objectStorage.Execute(objectQuery);
            AbstractionsAndPersistency.IProductPrice productPrice = null;
            int i = 0;
            foreach (StructureSet objectSetInstance in objectSet)
            {
                //productPrice = objectSet["productPrice"] as AbstractionsAndPersistency.IProductPrice;
                //productPrices.Add(productPrice);
              //  break;
            }
            //ProductPriceLookUpEdit.DataSource = productPrices;

        }
        /// <MetaDataID>{3dd5956e-ef11-478f-bcd4-a9dbcef963ee}</MetaDataID>
        protected override void OnClosed(EventArgs e)
        {
            //(ProductPriceLookUpEdit.DataSource as List<AbstractionsAndPersistency.IProductPrice>).Clear();
            //(gridControl1.DataSource as System.Collections.IList).Clear();
            //gridControl1.RefreshDataSource();
          
            base.OnClosed(e);
        }

        /// <MetaDataID>{dc97d6ee-7c78-4055-94a5-8c8aa9679a1a}</MetaDataID>
        private void Test_Click(object sender, EventArgs e)
        {
            esen.Add(1);
            esen.Add(2);
            esen.Add(3);
            esen.Add(4);
            esen.Add(5);
            esen.Add(6);
            esen.Add(7);
            esen.Add(8);
            esen.Add(9);
        }

        private void DevExpressForm_Load(object sender, EventArgs e)
        {

        }
    }
}
