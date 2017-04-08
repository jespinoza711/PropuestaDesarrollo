using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Demo
{
    public static class TipoDAC
    {
        public static SqlDataAdapter oAdaptadorTipo = InicializarAdaptador();

        private static SqlDataAdapter InicializarAdaptador() {
            String getSQL = "SELECT *  FROM fnica.fafTIPO";
            String InsertSQL = "INSERT INTO fnica.fafTIPO      ( Tipo, Descr )VALUES  (@Tipo,  @Descr  )";
            String UpdateSQL =  "Update fnica.fafTIPO  set Descr= @Descr  where Tipo=@Tipo";
            String DeleteSQL = "Delete from fnica.fafTipo where Tipo=@Tipo";

            SqlDataAdapter oAdaptador = new SqlDataAdapter() { 
                        SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection()), 
                        InsertCommand = new SqlCommand(InsertSQL, ConnectionManager.GetConnection()),
                        UpdateCommand = new SqlCommand(UpdateSQL, ConnectionManager.GetConnection()),
                        DeleteCommand = new SqlCommand(DeleteSQL, ConnectionManager.GetConnection())
            };

            //Paremetros Insert
            oAdaptador.InsertCommand.Parameters.Add("@Tipo", SqlDbType.NChar).SourceColumn = "Tipo";
            oAdaptador.InsertCommand.Parameters.Add("@Descr", SqlDbType.NChar).SourceColumn = "Descr";
            
            //Parametros Update
            oAdaptador.UpdateCommand.Parameters.Add("@Tipo", SqlDbType.NVarChar).SourceColumn = "Tipo";
            oAdaptador.UpdateCommand.Parameters.Add("@Descr", SqlDbType.NChar).SourceColumn = "Descr";

            //Parametros delete
            oAdaptador.DeleteCommand.Parameters.Add("@Tipo", SqlDbType.NChar).SourceColumn = "Tipo";
            return oAdaptador;
        }

        private static void Update(DataSet DS) {
            bool OkFlag = true;
            if (DS.HasErrors) {
                OkFlag = false;
                String sMsg = "Error en la fila con el tipo: ";
                foreach (DataTable theTable in DS.Tables)
                {
                    // If any table has errors, find out which rows
                    if (theTable.HasErrors)
                    {
                        // Get the rows with errors
                        DataRow[] errorRows = theTable.GetErrors();

                        // iterate through the errors and correct
                        // (in our case, just identify)
                        foreach (DataRow theRow in errorRows)
                        {
                            sMsg = sMsg + theRow["CustomerID"];
                        }
                    }
                }
                throw new Exception( sMsg);
                }
            

            if (OkFlag)
            {
                // Update the database on second dataset
                oAdaptadorTipo.Update(DS,"Tipo");

                // Inform the user
                //lblMessage.Text = "Updated " +  targetRow["CompanyName"];
                //Application.DoEvents();

                // Accept the changes and repopulate the list box
                DS.AcceptChanges();
                //PopulateListBox();
            }
            else
            {   // If we had errors, reject the changes
                DS.RejectChanges();
            }
        }

        private static void DefineTipoSchema(DataTable table)
        {
            DataColumn cTipo = table.Columns.Add("Tipo", typeof(String));
            cTipo.AllowDBNull = false;
            table.PrimaryKey = new DataColumn[] { cTipo };

            DataColumn cDescr = table.Columns.Add("Descr", typeof(String));
            cDescr.MaxLength = 50;
        }

        

        private static DataSet CreateDataSetTipo()
        {
            DataSet TipoDS = new DataSet();
            DataTable tipoTable = TipoDS.Tables.Add("Tipo");
           // DefineTipoSchema(tipoTable);
            return TipoDS;
        }

        public static DataTable GetData() {
            DataSet TipoDS = CreateDataSetTipo();
            //TipoDS.EnforceConstraints = false;
            oAdaptadorTipo.Fill(TipoDS.Tables["Tipo"]);
           //TipoDS.EnforceConstraints = true;
            return TipoDS.Tables[0];
        }
    }
}
