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
    public partial class MasterDetailSeparateGrid : Form
    {
        private DataTable _dtSolicitud;
        private DataTable _dtSolicitudDetalle;
        
        DataRow CurrentRowParent;

        public MasterDetailSeparateGrid()
        {
            InitializeComponent();
        }

        private void MasterDetailSeparateGrid_Load(object sender, EventArgs e)
        {
            try
            {
                _dtSolicitud = SolicitudDAC.GetData().Tables["Solicitud"];
                _dtSolicitudDetalle = SolicitudDetalleDAC.GetData().Tables[0];
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void PopulateGrid()
        {
            this.dtgSolicitud.DataSource = _dtSolicitud;
      


        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            String IdSolicitud = (String)gridSolicitud.GetFocusedRowCellValue("NumSolicitud");

            this.dtgSolicitudDetalle.DataSource = _dtSolicitudDetalle.AsEnumerable().Where(
                                                    row => row.Field<String>("NumSolicitud") == IdSolicitud).CopyToDataTable();

            SetCurrentParentRow();
        }

        private void SetCurrentParentRow()
        {
            int index = (int)this.gridSolicitud.FocusedRowHandle;
            if (index > -1)
            {
                CurrentRowParent = gridSolicitud.GetDataRow(index);
                
            }
        }
    }
}
