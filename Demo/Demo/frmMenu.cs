using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmListado ofrmLst = new frmListado();
            ofrmLst.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Demo ofrmCatalogo = new Demo();
            ofrmCatalogo.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmMasterDetail ofrm = new frmMasterDetail();
            ofrm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MasterDetailSeparateGrid ofrm = new MasterDetailSeparateGrid();
            ofrm.ShowDialog();
        }
    }
}
