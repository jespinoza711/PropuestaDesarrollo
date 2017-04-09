namespace Demo
{
    partial class frmMasterDetail
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
            this.dtgSolicitud = new DevExpress.XtraGrid.GridControl();
            this.gridSolicitud = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtgSolicitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSolicitud)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgSolicitud
            // 
            this.dtgSolicitud.Location = new System.Drawing.Point(45, 79);
            this.dtgSolicitud.MainView = this.gridSolicitud;
            this.dtgSolicitud.Name = "dtgSolicitud";
            this.dtgSolicitud.Size = new System.Drawing.Size(966, 780);
            this.dtgSolicitud.TabIndex = 0;
            this.dtgSolicitud.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridSolicitud});
            // 
            // gridSolicitud
            // 
            this.gridSolicitud.GridControl = this.dtgSolicitud;
            this.gridSolicitud.Name = "gridSolicitud";
            this.gridSolicitud.OptionsView.ShowAutoFilterRow = true;
            this.gridSolicitud.OptionsView.ShowGroupPanel = false;
            // 
            // frmMasterDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 889);
            this.Controls.Add(this.dtgSolicitud);
            this.Name = "frmMasterDetail";
            this.Text = "frmMasterDetail";
            this.Load += new System.EventHandler(this.frmMasterDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgSolicitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSolicitud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl dtgSolicitud;
        private DevExpress.XtraGrid.Views.Grid.GridView gridSolicitud;
    }
}