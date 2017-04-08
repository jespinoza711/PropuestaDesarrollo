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
    public partial class Form1 : Form
    {
        private DataTable TipoDS;
        DataRow currentRow;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                TipoDS = TipoDAC.GetData();
                DataTable TipoTable = TipoDS;
                this.dataGridView1.DataSource = TipoTable;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
