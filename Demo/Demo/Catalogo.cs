using DevExpress.XtraGrid.Views.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

namespace Demo
{
    public partial class Demo : Form
    {
        
        private DataTable _dtTipo;
        private DataSet _dsTipo;
        DataRow currentRow;
        const String _tituloVentana = "Catalogo de Tipo";

        public Demo()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }


        private void EnlazarEventos() {
            this.btnAgregar.ItemClick += btnAgregar_ItemClick;
            this.btnEditar.ItemClick += btnEditar_ItemClick;
            this.btnEliminar.ItemClick += btnEliminar_ItemClick;
            this.btnGuardar.ItemClick += btnGuardar_ItemClick;
            this.btnCancelar.ItemClick += btnCancelar_ItemClick;
            this.gridView1.FocusedRowChanged += gridView1_FocusedRowChanged;
        }

     
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                HabilitarControles(false);
                _dsTipo = TipoDAC.GetData();

                Util.SetDefaultBehaviorControls(this.gridView1,false,this.dtgTipo,bar3,lblTitulo,panelTitulo,_tituloVentana,this);
                EnlazarEventos();

                PopulateGrid();        
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateGrid() {
            _dtTipo = _dsTipo.Tables[0];
            this.dtgTipo.DataSource = _dtTipo;
        }

        private void ClearControls() {
            this.txtTipo.Text = "";
            this.txtDescr.Text = "";
        }

        private void HabilitarControles(bool Activo) {
            this.txtTipo.ReadOnly = !Activo;
            this.txtDescr.ReadOnly = !Activo;
            this.dtgTipo.Enabled = !Activo;

            this.btnAgregar.Enabled = !Activo;
            this.btnEditar.Enabled = !Activo;
            this.btnGuardar.Enabled = Activo;
            this.btnCancelar.Enabled = Activo;
            this.btnEliminar.Enabled = !Activo;
            
        }

        private void SetCurrentRow() {
            int index = (int)this.gridView1.FocusedRowHandle;
            if (index >-1)
            {
                 currentRow = gridView1.GetDataRow(index);
                 UpdateControlsFromCurrentRow(currentRow);
            }
        }

        private void UpdateControlsFromCurrentRow(DataRow Row) {
            this.txtTipo.Text = Row["Tipo"].ToString();
            this.txtDescr.Text = Row["Descr"].ToString();
        }
        
   

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            SetCurrentRow();
        }

   
        

        private void btnAgregar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HabilitarControles(true);
            ClearControls();
            currentRow = null;
        }

        private void btnEditar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (currentRow == null)
            {
                lblStatus.Caption = "Por favor seleccione un elemento de la Lista";
                return;
            }
            else
                lblStatus.Caption = "";
            HabilitarControles(true);
            lblStatus.Caption= "Editando el registro : " + currentRow["Descr"].ToString();   
        }

        private void btnGuardar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (currentRow != null)
            {
                lblStatus.Caption = "Actualizando : " + currentRow["Descr"].ToString();

                Application.DoEvents();
                currentRow.BeginEdit();

                currentRow["Tipo"] = this.txtTipo.Text.Trim();
                currentRow["Descr"] = this.txtDescr.Text.Trim();

                currentRow.EndEdit();

                DataSet _dsChanged = _dsTipo.GetChanges(DataRowState.Modified);

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

                    lblStatus.Caption = msg;
                }

                //Si no hay errores

                if (okFlag)
                {
                    TipoDAC.oAdaptador.Update(_dsChanged, "Data");
                    lblStatus.Caption = "Actualizado " + currentRow["Descr"].ToString();
                    Application.DoEvents();

                    _dsTipo.AcceptChanges();
                    PopulateGrid();
                    HabilitarControles(false);
                }
                else
                {
                    _dsTipo.RejectChanges();

                }
            }
            else
            {
                //nuevo registro
                currentRow = _dtTipo.NewRow();
                currentRow["Tipo"] = this.txtTipo.Text.Trim();
                currentRow["Descr"] = this.txtDescr.Text.Trim();
                _dtTipo.Rows.Add(currentRow);
                try
                {
                    TipoDAC.oAdaptador.Update(_dsTipo, "Data");
                    _dsTipo.AcceptChanges();

                    lblStatus.Caption = "Se ha ingresado un nuevo registro";
                    Application.DoEvents();
                    PopulateGrid();
                    ClearControls();
                    HabilitarControles(false);
                    ColumnView view = this.gridView1;
                    view.MoveLast();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    _dsTipo.RejectChanges();
                    currentRow = null;
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void btnCancelar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HabilitarControles(false);
            SetCurrentRow();
            lblStatus.Caption = "";
        }

        private void btnEliminar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (currentRow != null)
            {
                string msg = currentRow["Descr"] + " eliminado..";

                if (MessageBox.Show("Esta seguro que desea eliminar el elemento: " + currentRow["Descr"].ToString(), _tituloVentana, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    currentRow.Delete();

                    try
                    {
                        
                        TipoDAC.oAdaptador.Update(_dsTipo, "Data");
                        _dsTipo.AcceptChanges();

                        PopulateGrid();
                        lblStatus.Caption = msg;
                        Application.DoEvents();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        _dsTipo.RejectChanges();
                        lblStatus.Caption = "";
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
