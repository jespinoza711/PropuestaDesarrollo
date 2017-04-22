using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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
        private String Accion=  "NEW";
        String sCodSucursal= "JT01";
        String sUsuario = "jespinoza";
        String _tituloVentana = "Maestro de Solicitudes";

        public frmMaestroDetalleUpdate()
        {
            InitializeComponent();
            InicializarControles();
            //Obtener el Siguiente consecutivo de la solicitud"
            _dsSolicitud = SolicitudDAC.GetData(sCodSucursal, "");
            _dtSolicitud = _dsSolicitud.Tables[0];
            InicializarNuevoElemento();
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void InicializarNuevoElemento()
        {
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
            _dtSolicitud = ds.Tables[0];
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
            this.slkupCategoria.EditValue = row["CodCategoria"].ToString().Trim();

            //Obtener los datos segun cabecera
            PopulateGrid();
        }

        private void PopulateGrid() {
            _dsDetalle = SolicitudDetalleDAC.GetData(_currentRow["CodSucursal"].ToString(), _currentRow["NumSolicitud"].ToString());
            _dtDetalle = _dsDetalle.Tables[0];
            this.dtgDetalle.DataSource = _dtDetalle;
           // this.dtNavigator.DataSource = _dtDetalle;
        }

        private void ClearControls() {
            this.txtNumSolicitud.Text = "";
            this.txtCodSucursal.Text = "";
            this.txtDescr.Text = "";
            this.txtEstado.Text = "";
            this.txtUsuario.Text = "";
            this.txtFecha.Text = "";
            this.slkupCategoria.EditValue = null;
            this.dtgDetalle.DataSource = null;
        }
        
        private void InicializarControles() {
            gridViewDetalle.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            gridViewDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
        }

        
        private void HabilitarControles(bool Activo) {
            this.txtCodSucursal.ReadOnly = !Activo;
            this.txtDescr.ReadOnly = !Activo;
            this.txtEstado.ReadOnly = !Activo;
            this.txtFecha.ReadOnly = !Activo;
            this.txtNumSolicitud.ReadOnly = !Activo;
            this.txtUsuario.ReadOnly=!Activo;
            this.slkupCategoria.ReadOnly = !Activo;
            this.gridViewDetalle.OptionsBehavior.Editable =Activo;
            
            this.btnAgregar.Enabled = !Activo;
            this.btnEditar.Enabled = !Activo;
            this.btnGuardar.Enabled = Activo;
            this.btnCancelar.Enabled = Activo;
            this.btnEliminar.Enabled = !Activo;
        }

        private void EnlazarEventos()
        {
            this.btnAgregar.ItemClick += btnAgregar_ItemClick;
            this.btnEditar.ItemClick += btnEditar_ItemClick;
            this.btnEliminar.ItemClick += btnEliminar_ItemClick;
            this.btnGuardar.ItemClick += btnGuardar_ItemClick;
            this.btnCancelar.ItemClick += btnCancelar_ItemClick;
        }


        private void frmMaestroDetalleUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                HabilitarControles(true);

                //SetDefaultBehaviorControls();
                Util.SetDefaultBehaviorControls(this.gridViewDetalle,true, null, this.bar1, this.lblTitulo, this.panelTitulo, _tituloVentana, this);
                EnlazarEventos();

                this.gridViewDetalle.EditFormPrepared += gridViewDetalle_EditFormPrepared;
                this.gridViewDetalle.NewItemRowText = Util.constNewItemTextGrid;
                this.gridViewDetalle.ValidatingEditor += gridViewDetalle_ValidatingEditor;


                //Configurar searchLookUp

                this.slkupArticulo.DataSource = ArticuloDAC.GetData("*").Tables["Data"];
                this.slkupArticulo.DisplayMember = "DESCRIPCION";
                this.slkupArticulo.ValueMember = "ARTICULO";
                this.slkupArticulo.NullText = " --- ---";

                Util.ConfigLookupEdit(this.slkupCategoria, CategoriaDAC.GetData().Tables["Categoria"], "Descripcion", "CodCategoria");
                Util.ConfigLookupEditSetViewColumns(this.slkupCategoria, "[{'ColumnCaption':'CodCategoria','ColumnField':'CodCategoria','width':30},{'ColumnCaption':'Descripcion','ColumnField':'Descripcion','width':70}]");
                dtgDetalle.ProcessGridKey += dtgDetalle_ProcessGridKey;
                UpdateControlsFromDataRow(_currentRow);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        void gridViewDetalle_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {
            Control ctrl = Util.FindControl(e.Panel, "Update");
            if (ctrl != null)
                ctrl.Text = "Actualizar";
            ctrl = Util.FindControl(e.Panel, "Cancel");
            if (ctrl != null)
                ctrl.Text = "Cancelar";
        }

        void dtgDetalle_ProcessGridKey(object sender, KeyEventArgs e)
        {
            var grid = sender as GridControl;
            var view = grid.FocusedView as GridView;
            if (e.KeyData == Keys.Delete)
            {
                if (MessageBox.Show("Esta seguro que desea eliminar el elemento seleccionado?", "Maestro Detalle Update", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    view.DeleteSelectedRows();
                    e.Handled = true;
                }
                else
                    e.Handled = false;
            }
        }

        private void gridViewDetalle_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["NumSolicitud"], _currentRow["NumSolicitud"]);
            view.SetRowCellValue(e.RowHandle, view.Columns["CodSucursal"], _currentRow["CodSucursal"]);
        }



        private void btnAgregar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HabilitarControles(true);
            ClearControls();
            Accion = "New";
            InicializarNuevoElemento();
            UpdateControlsFromDataRow(_currentRow);
            
        }

        private void btnEditar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void btnGuardar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Accion == "Edit")
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

                DataSet _dsChanged = _dsSolicitud.GetChanges(DataRowState.Modified | DataRowState.Added);

                bool okFlag = true;
                if ( _dsChanged!=null && _dsChanged.HasErrors)
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
                                msg = msg + dr["NumSolicitud"].ToString();
                            }
                        }
                    }

                    this.lblStatusBar.Caption = msg;
                }


   

                //Si no hay errores

                if (okFlag )
                {

                    ConnectionManager.BeginTran();
                    SolicitudDAC.SetTransactionToAdaptador(true);
                    SolicitudDetalleDAC.SetTransactionToAdaptador(true);

                    SolicitudDAC.oAdaptador.Update(_dsChanged, "Data");
                    SolicitudDetalleDAC.oAdaptador.Update(_dsDetalle, "Data");
                    
                    this.lblStatusBar.Caption = "Actualizado " + _currentRow["Numsolicitud"].ToString();
                    Application.DoEvents();

                    _dsSolicitud.AcceptChanges();
                    _dsDetalle.AcceptChanges();

                    ConnectionManager.CommitTran();
                    SolicitudDAC.SetTransactionToAdaptador(false);
                    SolicitudDetalleDAC.SetTransactionToAdaptador(false);
                    
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

                    SolicitudDAC.oAdaptador.Update(_dsSolicitud, "Data");
                    _dsSolicitud.AcceptChanges();

                    //Agregar el detalle
                    SolicitudDetalleDAC.oAdaptador.Update(_dsDetalle, "Data");
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
                    ConnectionManager.RollBackTran();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCancelar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HabilitarControles(false);
            UpdateControlsFromDataRow(_currentRow);
            this.lblStatusBar.Caption = "";
        }

        private void btnEliminar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_currentRow != null)
            {
                if (MessageBox.Show("Esta seguro que desea eliminar el elemento: " + _currentRow["NumSolicitud"].ToString(), _tituloVentana, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    string msg = _currentRow["NumSolicitud"] + " eliminado..";
                    string sNumSolicitud = _currentRow["NumSolicitud"].ToString();
                    _currentRow.Delete();

                    try
                    {
                        ConnectionManager.BeginTran();
                        SolicitudDAC.SetTransactionToAdaptador(true);


                        System.Data.SqlClient.SqlCommand oCmd = new System.Data.SqlClient.SqlCommand("Delete from fnica.solSolicituddetalle where numsolicitud=@Solicitud", ConnectionManager.GetConnection(), ConnectionManager.Tran);
                        oCmd.Parameters.Add("@Solicitud", SqlDbType.NVarChar).Value = sNumSolicitud;
                        int i = oCmd.ExecuteNonQuery();

                        SolicitudDAC.oAdaptador.Update(_dsSolicitud, "Data");
                        _dsSolicitud.AcceptChanges();

                        
                        ConnectionManager.CommitTran();
                        SolicitudDAC.SetTransactionToAdaptador(false);
                        

                        PopulateGrid();
                        this.lblStatusBar.Caption = msg;
                        Application.DoEvents();
                        
                        this.Close();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        ConnectionManager.RollBackTran();
                        _dsSolicitud.RejectChanges();
                        this.lblStatusBar.Caption = "";
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }



        private void gridViewDetalle_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (this.gridViewDetalle.FocusedRowHandle == DevExpress.XtraGrid.GridControl.AutoFilterRowHandle)
                return;
            
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            DataView dataView = view.DataSource as DataView;
            System.Collections.IEnumerator en = dataView.GetEnumerator();
            
            en.Reset();

            string currentCode = e.Value.ToString();
            
            while (en.MoveNext()) {
                DataRowView row = en.Current as DataRowView;
                object colValue = row["Articulo"];
                if (colValue.ToString() == currentCode)
                {
                    e.ErrorText = "El elemento ya existe.";
                    e.Valid = false;
                    break;
                }
            } 
        }
    }
}
