using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
//NOT BEING USED CAN BE Deleted!!!
namespace ModellingTool
{
    public partial class frmMIDIParent : Form
    {
        public frmMIDIParent()
        {
            InitializeComponent();
        }



        private void loadData_Click(object sender, EventArgs e)
        {

            BindingSource bs = new BindingSource();
            DataSet ds = new DataSet();//create a new dataset


            frmModellingTool frmdata = new frmModellingTool();//create a new instance of the loading input data input form
            frmdata.MdiParent = this;//connect it to the parent form..
            frmdata.HookUpData(ds);//link the initial data in the set up to the form
            frmdata.StartPosition = FormStartPosition.CenterScreen;
            frmdata.Show();


        }



        private void exitMDIParent_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }

}
