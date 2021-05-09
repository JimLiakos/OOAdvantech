using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UmlElementLink
{
    /// <MetaDataID>{0b830df9-3a9a-467b-ba22-766f0571ec98}</MetaDataID>
  public partial class ChooseReferenceForm : Form
  {
    public ChooseReferenceForm()
    {
      InitializeComponent();
      
    }

    public ChooseReferenceForm(IEnumerable<string> items)
      : this()
    {
      listBox1.Items.AddRange(items.ToArray());
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    void listBox1_DoubleClick(object sender, System.EventArgs e)
    {
      this.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.Close();
    }

    public string Selection
    {
      get
      {
        return listBox1.SelectedItem as string;
      }
    }
  }
}
