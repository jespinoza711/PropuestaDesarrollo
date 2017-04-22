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
        public static SqlDataAdapter oAdaptador = InicializarAdaptador();

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


        private static DataSet CreateDataSetTipo()
        {
            DataSet TipoDS = new DataSet();
            DataTable tipoTable = TipoDS.Tables.Add("Data");
            return TipoDS;
        }

        public static DataSet GetData() {
            DataSet TipoDS = CreateDataSetTipo();
            oAdaptador.Fill(TipoDS.Tables["Data"]);
            return TipoDS;
        }
    }
}
