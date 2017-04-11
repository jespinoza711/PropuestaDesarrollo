using DevExpress.XtraEditors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public static class Util
    {
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

    }
}
