﻿using System;
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
    public partial class frmListado : Form
    {
        private DataTable _dtSolicitud;
        private DataSet _dsSolicitud;
        DataRow currentRow;
        const String _tituloVentana = "Listado de Solicitudes";


        //private void SetDefaultBehaviorControls()
        //{
        //    //Grid
        //    this.gridViewSolicitudes.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
        //    this.gridViewSolicitudes.OptionsBehavior.Editable = false;
        //    this.gridViewSolicitudes.OptionsSelection.EnableAppearanceFocusedRow = true;
        //    this.gridViewSolicitudes.OptionsFilter.DefaultFilterEditorView = DevExpress.XtraEditors.FilterEditorViewMode.TextAndVisual;
        //    this.gridViewSolicitudes.OptionsView.ShowAutoFilterRow = true;
        //    //Navegador
        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.Append.Enabled = false;
        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.Append.Visible = false;

        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;

        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.Remove.Enabled = false;
        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.Remove.Visible = false;

        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
        //    this.dtgSolicitudes.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
        //    this.dtgSolicitudes.EmbeddedNavigator.Enabled=true;

        //    //Barra Prinicpal
        //    this.bar1.OptionsBar.AllowQuickCustomization = false;

        //    //titulo
        //    this.lblTitulo.Font = new Font(lblTitulo.Font.FontFamily, 12f, FontStyle.Bold);
        //    this.lblTitulo.Size = new Size(panelTitulo.Size.Width / 2, panelTitulo.Size.Height / 2);
        //    this.lblTitulo.Top = (panelTitulo.Height / 2) - (lblTitulo.Height / 2);
        //    this.lblTitulo.Left = (panelTitulo.Width / 2) - (lblTitulo.Width / 2);
        //    this.lblTitulo.ForeColor = Color.DodgerBlue;
        //    this.lblTitulo.Text = _tituloVentana;

        //    ////Titulo e Icono de la ventana
        //    this.Text = _tituloVentana;
        //    this.Icon = Properties.Resources.Icon1;


        //}

        public frmListado()
        {
            InitializeComponent();
        }

        private void EnlazarEventos()
        {
            this.btnAgregar.ItemClick += btnAgregar_ItemClick;
            this.btnEditar.ItemClick += btnEditar_ItemClick;
            this.btnEliminar.ItemClick += btnEliminar_ItemClick;
            
        }


        private void frmListado_Load(object sender, EventArgs e)
        {
            try
            {
                HabilitarControles(false);
                _dsSolicitud = SolicitudDAC.GetDataAsync("*","*").Result;
                EnlazarEventos();
                Util.SetDefaultBehaviorControls(this.gridViewSolicitudes,false,this.dtgSolicitudes,this.bar1,lblTitulo,panelTitulo,_tituloVentana,this);

                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void PopulateGrid() {
            _dtSolicitud = _dsSolicitud.Tables[0];
            this.dtgSolicitudes.DataSource = _dtSolicitud;
        }

    
        private void HabilitarControles(bool Activo) {

            this.btnAgregar.Enabled = !Activo;
            this.btnEditar.Enabled = !Activo;
           
            this.btnEliminar.Enabled = !Activo;
            
        }

        private void SetCurrentRow() {
            int index = (int)this.gridViewSolicitudes.FocusedRowHandle;
            if (index >-1)
            {
                 currentRow = gridViewSolicitudes.GetDataRow(index);
                 //UpdateControlsFromCurrentRow(currentRow);
            }
        }

    
   

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            SetCurrentRow();
        }

   
        

        private void btnAgregar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HabilitarControles(true);
            
            currentRow = null;
        }

        private void btnEditar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            HabilitarControles(true);
            
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
                        TipoDAC.oAdaptador.Update(_dsSolicitud, "Data");
                        _dsSolicitud.AcceptChanges();

                        PopulateGrid();
                        //lblStatus.Caption = msg;
                        Application.DoEvents();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        _dsSolicitud.RejectChanges();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnAgregar_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmMaestroDetalleUpdate ofrmMaestro = new frmMaestroDetalleUpdate();
            ofrmMaestro.FormClosed += OfrmMaestro_FormClosed;
            ofrmMaestro.ShowDialog();
        }

        private void OfrmMaestro_FormClosed(object sender, FormClosedEventArgs e)
        {
            PopulateGrid();
        }

        private void btnEditar_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (currentRow != null)
            {
                frmMaestroDetalleUpdate ofrmMaestros = new frmMaestroDetalleUpdate(_dsSolicitud, currentRow);
                ofrmMaestros.ShowDialog();
            }
        }

        private void btnEliminar_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (currentRow != null)
            {
                if (MessageBox.Show("Esta seguro que desea eliminar el elemento: " + currentRow["NumSolicitud"].ToString(), _tituloVentana, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    string msg = currentRow["NumSolicitud"] + " eliminado..";
                    string sNumSolicitud = currentRow["NumSolicitud"].ToString();
                    currentRow.Delete();

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
                        //this.lblStatusBar.Caption = msg;
                        Application.DoEvents();

          
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        ConnectionManager.RollBackTran();
                        _dsSolicitud.RejectChanges();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
    
}
