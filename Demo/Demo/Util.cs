using DevExpress.XtraEditors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
     
    public static class Util
    {
        //constantes
        public const String constNewItemTextGrid="Click para agregar un nuevo elemento";
    

        public static void ConfigLookupEdit(SearchLookUpEdit lkupControl, Object DataSource,
                                         String DisplayMember, String ValueMember, int WinPopupWidth = 250, int WinPopupHeigth = 200)
        {
            lkupControl.Properties.DataSource = DataSource;
            lkupControl.Properties.PopupFormSize = new Size(WinPopupWidth, WinPopupHeigth);
            lkupControl.Properties.ShowClearButton = false;
            lkupControl.Properties.DisplayMember = DisplayMember;
            lkupControl.Properties.ValueMember = ValueMember;
            lkupControl.Properties.NullText = "--- ---";
            lkupControl.Properties.View.BestFitColumns();
            lkupControl.Properties.NullValuePrompt = "--- ---";
        }

        public static void ConfigLookupEditSetViewColumns(SearchLookUpEdit lkupControl, String Columns)
        {
            dynamic oColumns = JArray.Parse(Columns);
            DevExpress.XtraGrid.Views.Grid.GridView lookupView = new DevExpress.XtraGrid.Views.Grid.GridView();
            bool AutoWidth = false;
            int count = 0;
            foreach (dynamic ele in oColumns)
            {
                DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
                col.Caption = ele.ColumnCaption;
                col.FieldName = ele.ColumnField;
                col.MinWidth = 10;
                col.Visible = true;

                col.VisibleIndex = count;
                
                if (ele.width != null)
                    col.Width = Convert.ToInt32(((Convert.ToDouble(lkupControl.Properties.PopupFormSize.Width) - 27) * Convert.ToDouble(ele.width)) / 100);
                else
                    AutoWidth = true;
                
                col.OptionsColumn.AllowSize = true;

                lookupView.Columns.Add(col);

                count++;
            }

            lookupView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            lookupView.OptionsLayout.StoreAllOptions = true;
            lookupView.BestFitMaxRowCount = -1;
            lookupView.OptionsFilter.ColumnFilterPopupRowCount = 10;
            lookupView.OptionsFilter.DefaultFilterEditorView = DevExpress.XtraEditors.FilterEditorViewMode.VisualAndText;
            lookupView.OptionsFind.FindDelay = 100;
            lookupView.OptionsFind.SearchInPreview = true;
            lookupView.OptionsView.ColumnAutoWidth = AutoWidth;
            lookupView.OptionsFind.ShowClearButton = false;
            lookupView.OptionsLayout.Columns.AddNewColumns = false;
            lookupView.OptionsSelection.EnableAppearanceFocusedCell = false;
            lookupView.OptionsView.ShowGroupPanel = false;
            lkupControl.Properties.View = lookupView;
        }


        public static Control FindControl(this Control root, string text)
        {
            if (root == null) throw new ArgumentNullException("root");
            foreach (Control child in root.Controls)
            {
                if (child.Text == text) return child;
                Control found = FindControl(child, text);
                if (found != null) return found;
            }
            return null;
        }


        public static void SetDefaultBehaviorControls(DevExpress.XtraGrid.Views.Grid.GridView pGridView, bool pEditable,
                                                            DevExpress.XtraGrid.GridControl pGrid,
                                                                     DevExpress.XtraBars.Bar pBar,
                                                   DevExpress.XtraEditors.LabelControl plblTitulo,
                                                 DevExpress.XtraEditors.PanelControl pPanelTitulo,
                                                                            String _tituloVentana,
                                                                   System.Windows.Forms.Form pForm)
        {
            //Grid
            pGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            pGridView.OptionsBehavior.Editable = pEditable;
            pGridView.OptionsSelection.EnableAppearanceFocusedRow = true;
            pGridView.OptionsFilter.DefaultFilterEditorView = DevExpress.XtraEditors.FilterEditorViewMode.TextAndVisual;
            pGridView.OptionsView.ShowAutoFilterRow = true;
            //Navegador
            if (pGrid != null)
            {
                pGrid.EmbeddedNavigator.Buttons.Append.Enabled = false;
                pGrid.EmbeddedNavigator.Buttons.Append.Visible = false;

                pGrid.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
                pGrid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;

                pGrid.EmbeddedNavigator.Buttons.Remove.Enabled = false;
                pGrid.EmbeddedNavigator.Buttons.Remove.Visible = false;

                pGrid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
                pGrid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
                pGrid.EmbeddedNavigator.Enabled = true;
            }
            //Barra Prinicpal
            pBar.OptionsBar.AllowQuickCustomization = false;

            //titulo
            plblTitulo.Font = new Font(plblTitulo.Font.FontFamily, 12f, FontStyle.Bold);
            plblTitulo.Size = new Size(pPanelTitulo.Size.Width / 2, pPanelTitulo.Size.Height / 2);
            plblTitulo.Top = (pPanelTitulo.Height / 2) - (plblTitulo.Height / 2);
            plblTitulo.Left = (pPanelTitulo.Width / 2) - (plblTitulo.Width / 2);
            plblTitulo.ForeColor = Color.DodgerBlue;
            plblTitulo.Text = _tituloVentana;

            ////Titulo e Icono de la ventana
            pForm.Text = _tituloVentana;
            pForm.Icon = Properties.Resources.Icon1;


        }
    }
}
