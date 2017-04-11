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
    public partial class frmMasterDetail : Form
    {

        private DataSet _dsSolicitud;
        
        public frmMasterDetail()
        {
            InitializeComponent();
        }

        private void frmMasterDetail_Load(object sender, EventArgs e)
        {
            try
            {
                _dsSolicitud = new DataSet();
                _dsSolicitud.Tables.Add(SolicitudDAC.GetData("*","*").Tables["Solicitud"].Copy());
                _dsSolicitud.Tables.Add(SolicitudDetalleDAC.GetData("*","*").Tables[0].Copy());
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateGrid()
        {


             // Establecer la relacion entre las dos tablas
            DataRelation relation = new DataRelation("SolicituSolicitudDetalle",
                new DataColumn[] { _dsSolicitud.Tables["Solicitud"].Columns["NumSolicitud"], _dsSolicitud.Tables["Solicitud"].Columns["CodSucursal"] },
                new DataColumn[] { _dsSolicitud.Tables["SolicitudDetalle"].Columns["NumSolicitud"], _dsSolicitud.Tables["SolicitudDetalle"].Columns["CodSucursal"] });
                
            _dsSolicitud.Relations.Add(relation);


            this.dtgSolicitud.DataSource = _dsSolicitud;
            this.dtgSolicitud.DataMember = "Solicitud";
   


        }

    
    }
}
