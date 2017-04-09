namespace Demo
{
    partial class MasterDetailSeparateGrid
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtgSolicitudDetalle = new DevExpress.XtraGrid.GridControl();
            this.gridSolicitudDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dtgSolicitud = new DevExpress.XtraGrid.GridControl();
            this.gridSolicitud = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSolicitudDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSolicitudDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSolicitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSolicitud)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgSolicitudDetalle
            // 
            this.dtgSolicitudDetalle.Location = new System.Drawing.Point(27, 555);
            this.dtgSolicitudDetalle.MainView = this.gridSolicitudDetalle;
            this.dtgSolicitudDetalle.Name = "dtgSolicitudDetalle";
            this.dtgSolicitudDetalle.Size = new System.Drawing.Size(966, 272);
            this.dtgSolicitudDetalle.TabIndex = 3;
            this.dtgSolicitudDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridSolicitudDetalle});
            // 
            // gridSolicitudDetalle
            // 
            this.gridSolicitudDetalle.GridControl = this.dtgSolicitudDetalle;
            this.gridSolicitudDetalle.Name = "gridSolicitudDetalle";
            this.gridSolicitudDetalle.OptionsView.ShowAutoFilterRow = true;
            this.gridSolicitudDetalle.OptionsView.ShowGroupPanel = false;
            // 
            // dtgSolicitud
            // 
            this.dtgSolicitud.Location = new System.Drawing.Point(41, 53);
            this.dtgSolicitud.MainView = this.gridSolicitud;
            this.dtgSolicitud.Name = "dtgSolicitud";
            this.dtgSolicitud.Size = new System.Drawing.Size(966, 476);
            this.dtgSolicitud.TabIndex = 2;
            this.dtgSolicitud.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridSolicitud});
            // 
            // gridSolicitud
            // 
            this.gridSolicitud.GridControl = this.dtgSolicitud;
            this.gridSolicitud.Name = "gridSolicitud";
            this.gridSolicitud.OptionsView.ShowAutoFilterRow = true;
            this.gridSolicitud.OptionsView.ShowGroupPanel = false;
            this.gridSolicitud.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // MasterDetailSeparateGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 897);
            this.Controls.Add(this.dtgSolicitudDetalle);
            this.Controls.Add(this.dtgSolicitud);
            this.Name = "MasterDetailSeparateGrid";
            this.Text = "MasterDetailSeparateGrid";
            this.Load += new System.EventHandler(this.MasterDetailSeparateGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgSolicitudDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSolicitudDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSolicitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSolicitud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl dtgSolicitudDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gridSolicitudDetalle;
        private DevExpress.XtraGrid.GridControl dtgSolicitud;
        private DevExpress.XtraGrid.Views.Grid.GridView gridSolicitud;
    }
}