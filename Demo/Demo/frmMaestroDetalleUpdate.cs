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
    public partial class frmMaestroDetalleUpdate : Form
    {
        private DataTable _dtSolicitud;
        private DataTable _dtDetalle;

        private DataSet _dsSolicitud;
        private DataSet _dsDetalle;
        private DataRow _currentRow;
        private String Accion=  "Add";
        String sCodSucursal= "JT01";
        String sUsuario = "jespinoza";

        public frmMaestroDetalleUpdate()
        {
            InitializeComponent();
            InicializarControles();
            //Obtener el Siguiente consecutivo de la solicitud"
            _dsSolicitud = SolicitudDAC.GetData(sCodSucursal, "");
            _dtSolicitud = _dsSolicitud.Tables[0];
            _currentRow = _dtSolicitud.NewRow();
            _currentRow["NumSolicitud"] = SolicitudDAC.GetNextConsecutivo(sCodSucursal);
            _currentRow["CodSucursal"] = sCodSucursal;
            _currentRow["FechaSolicitud"] = DateTime.Now;
            _currentRow["Estado"] = "INI";
            _currentRow["UsuarioSolicitud"] = sUsuario;


        }

        public frmMaestroDetalleUpdate(DataSet ds,DataRow dr)
        {
            InitializeComponent();
            InicializarControles();
            _dsSolicitud = ds;
            _currentRow = dr;
            Accion = "Edit";
        }

        public void UpdateControlsFromDataRow(DataRow row) {


            this.txtNumSolicitud.Text = row["NumSolicitud"].ToString();
            this.txtCodSucursal.Text = row["CodSucursal"].ToString();
            this.txtDescr.Text = row["Descripcion"].ToString();
            this.txtEstado.Text = row["Estado"].ToString();
            this.txtUsuario.Text = row["UsuarioSolicitud"].ToString();
            this.txtFecha.Text = row["FechaSolicitud"].ToString();
            this.slkupCategoria.EditValue = row["CodCategoria"].ToString();

            //Obtener los datos segun cabecera
            PopulateGrid();
        }

        private void PopulateGrid() {
            _dsDetalle = SolicitudDetalleDAC.GetData(_currentRow["CodSucursal"].ToString(), _currentRow["NumSolicitud"].ToString());
            _dtDetalle = _dsDetalle.Tables[0];
            this.dtgDetalle.DataSource = _dtDetalle;
        }

        private void ClearControls() {
            this.txtNumSolicitud.Text = "";
            this.txtCodSucursal.Text = "";
            this.txtDescr.Text = "";
            this.txtEstado.Text = "";
            this.txtUsuario.Text = "";
            this.txtFecha.Text = "";
            this.slkupCategoria.EditValue = null;
            //this.dtgDetalle.DataSource = null;
        }
        
        private void InicializarControles() {
            gridViewDetalle.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            gridViewDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
        }

        private void HabilitarControles(bool Activo) {
            this.layoutControls.Enabled = Activo;
            this.dtgDetalle.Enabled = Activo;
            this.btnAgregar.Enabled = !Activo;
            this.btnEditar.Enabled = !Activo;
            this.btnGuardar.Enabled = Activo;
            this.btnCancelar.Enabled = Activo;
            this.btnEliminar.Enabled = !Activo;
        }

        private void frmMaestroDetalleUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                HabilitarControles(true);
                UpdateControlsFromDataRow(_currentRow);

                //Configurar searchLookUp

                this.slkupArticulo.DataSource = ArticuloDAC.GetData("*").Tables["Articulo"];
                this.slkupArticulo.DisplayMember = "DESCRIPCION";
                this.slkupArticulo.ValueMember = "ARTICULO";
                this.slkupArticulo.NullText = " --- ---";

                Util.ConfigLookupEdit(this.slkupCategoria, CategoriaDAC.GetData().Tables["Categoria"], "Descripcion", "CodCategoria");
                Util.ConfigLookupEditSetViewColumns(this.slkupCategoria, "[{'ColumnCaption':'CodCategoria','ColumnField':'CodCategoria','width':30},{'ColumnCaption':'Descripcion','ColumnField':'Descripcion','width':70}]");

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridViewDetalle_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)e.Row;
              
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridViewDetalle_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["NumSolicitud"], _currentRow["NumSolicitud"]);
            view.SetRowCellValue(e.RowHandle, view.Columns["CodSucursal"], _currentRow["CodSucursal"]);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Accion = "Edit";
            if (_currentRow == null)
            {
                lblStatusBar.Caption = "Por favor seleccion un elemento";
                return;
            }
            else
                lblStatusBar.Caption = "";

            HabilitarControles(true);
            lblStatusBar.Caption = "Editando :" + _currentRow["NumSolicitud"].ToString();

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Accion=="Edit")
            {
                lblStatusBar.Caption = "Actualizando : " + _currentRow["NumSolicitud"].ToString();

                Application.DoEvents();
                _currentRow.BeginEdit();
                _currentRow["NumSolicitud"] = this.txtNumSolicitud.Text.Trim();
                _currentRow["CodSucursal"] = this.txtCodSucursal.Text.Trim();
                _currentRow["CodCategoria"] = this.slkupCategoria.EditValue;
                _currentRow["Descripcion"] = this.txtDescr.Text.Trim();
                _currentRow["Estado"] = this.txtEstado.Text.Trim();
                _currentRow["UsuarioSolicitud"] = this.txtUsuario.Text.Trim();
                _currentRow["FechaSolicitud"] = this.txtFecha.Text.Trim();
                
                _currentRow.EndEdit();

                DataSet _dsChanged = _dsSolicitud.GetChanges(DataRowState.Modified);

                bool okFlag = true;
                if (_dsChanged.HasErrors)
                {
                    okFlag = false;
                    string msg = "Error en la fila con el tipo Id";

                    foreach (DataTable tb in _dsChanged.Tables)
                    {
                        if (tb.HasErrors)
                        {
                            DataRow[] errosRow = tb.GetErrors();

                            foreach (DataRow dr in errosRow)
                            {
                                msg = msg + dr["Tipo"].ToString();
                            }
                        }
                    }

                    this.lblStatusBar.Caption = msg;
                }

                //Si no hay errores

                if (okFlag)
                {
                   
                    SolicitudDAC.oAdaptadorSolicitud.Update(_dsChanged, "Solicitud");
                    this.lblStatusBar.Caption = "Actualizado " + _currentRow["Numsolicitud"].ToString();
                    Application.DoEvents();

                    _dsSolicitud.AcceptChanges();
                    
                    PopulateGrid();
                    HabilitarControles(false);
                }
                else
                {
                    _dsSolicitud.RejectChanges();
                   

                }
            }
            else
            {
                //nuevo registro
                //_currentRow = _dtSolicitud.NewRow();
                _currentRow["NumSolicitud"] = this.txtNumSolicitud.Text.Trim();
                _currentRow["CodSucursal"] = this.txtCodSucursal.Text.Trim();
                _currentRow["CodCategoria"] = this.slkupCategoria.EditValue;
                _currentRow["Descripcion"] = this.txtDescr.Text.Trim();
                _currentRow["Estado"] = this.txtEstado.Text.Trim();
                _currentRow["UsuarioSolicitud"] = this.txtUsuario.Text.Trim();
                _currentRow["FechaSolicitud"] = this.txtFecha.Text.Trim();

                _dtSolicitud.Rows.Add(_currentRow);
                try
                {
                    ConnectionManager.BeginTran();
                    SolicitudDAC.SetTransactionToAdaptador(true);
                    SolicitudDetalleDAC.SetTransactionToAdaptador(true);

                    SolicitudDAC.oAdaptadorSolicitud.Update(_dsSolicitud, "Solicitud");
                    _dsSolicitud.AcceptChanges();

                    //Agregar el detalle
                    SolicitudDetalleDAC.oAdaptadorSolicitud.Update(_dsDetalle, "SolicitudDetalle");
                    _dsDetalle.AcceptChanges();

                    ConnectionManager.CommitTran();
                    SolicitudDAC.SetTransactionToAdaptador(false);
                    SolicitudDetalleDAC.SetTransactionToAdaptador(false);
                    
                    this.lblStatusBar.Caption = "Se ha ingresado un nuevo registro";
                    Application.DoEvents();
                    PopulateGrid();
                    HabilitarControles(false);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    _dsSolicitud.RejectChanges();
                    _dsDetalle.RejectChanges();
                   // _currentRow = null;
                    ConnectionManager.RollBackTran();
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            HabilitarControles(false);
            UpdateControlsFromDataRow(_currentRow);
            this.lblStatusBar.Caption = "";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_currentRow != null)
            {
                string msg = _currentRow["NumSolicitud"] + " eliminado..";
                _currentRow.Delete();

                try
                {
                    TipoDAC.oAdaptadorTipo.Update(_dsSolicitud, "Tipo");
                    _dsSolicitud.AcceptChanges();

                    PopulateGrid();
                    this.lblStatusBar.Caption = msg;
                    Application.DoEvents();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    _dsSolicitud.RejectChanges();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            HabilitarControles(true);
            ClearControls();
            _currentRow = null;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.dtgDetalle.DataSource;
            MessageBox.Show(dt.Rows.Count.ToString());
        }
    }
}
