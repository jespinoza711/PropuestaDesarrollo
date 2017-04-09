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
    public partial class Demo : Form
    {
        
        private DataTable _dtTipo;
        private DataSet _dsTipo;
        DataRow currentRow;

        public Demo()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                HabilitarControles(false);
                _dsTipo = TipoDAC.GetData();
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
            this.txtTipo.Enabled = Activo;
            this.txtDescr.Enabled = Activo;
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
        
        private void btnEditar_Click(object sender, EventArgs e)
        {           
            if (currentRow == null)
            {
                lblMessage.Text = "Por favor seleccione un elemento de la Lista";
                return;
            }
            else
                lblMessage.Text = "";
            HabilitarControles(true);
            lblMessage.Text = "Editando el registro : " + currentRow["Descr"].ToString();   
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            SetCurrentRow();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            HabilitarControles(true);
            ClearControls();
            currentRow = null;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (currentRow != null)
            {
                lblMessage.Text = "Actualizando : " + currentRow["Descr"].ToString();

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

                    lblMessage.Text = msg;
                }

                //Si no hay errores

                if (okFlag)
                {
                    TipoDAC.oAdaptadorTipo.Update(_dsChanged, "Tipo");
                    lblMessage.Text = "Actualizado " + currentRow["Descr"].ToString();
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
            else { 
                //nuevo registro
                currentRow = _dtTipo.NewRow();
                currentRow["Tipo"] = this.txtTipo.Text.Trim();
                currentRow["Descr"] = this.txtDescr.Text.Trim();
                _dtTipo.Rows.Add(currentRow);
                try
                {
                    TipoDAC.oAdaptadorTipo.Update(_dsTipo, "Tipo");
                    _dsTipo.AcceptChanges();

                    lblMessage.Text = "Se ha ingresado un nuevo registro";
                    Application.DoEvents();
                    PopulateGrid();
                    ClearControls();
                    HabilitarControles(false);
                }
                catch (System.Data.SqlClient.SqlException ex) {
                    _dsTipo.RejectChanges();
                    currentRow = null;
                    MessageBox.Show(ex.Message);
                }
            }
            
        }
        
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (currentRow != null) {
                string msg = currentRow["Descr"] + " eliminado..";
                currentRow.Delete();

                try
                {
                    TipoDAC.oAdaptadorTipo.Update(_dsTipo, "Tipo");
                    _dsTipo.AcceptChanges();

                    PopulateGrid();
                    lblMessage.Text = msg;
                    Application.DoEvents();
                }
                catch (System.Data.SqlClient.SqlException ex) {
                    _dsTipo.RejectChanges();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            HabilitarControles(false);
            SetCurrentRow();
            this.lblMessage.Text = "";
        }
    }
}
