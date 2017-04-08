using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Demo
{
    public static class ArticuloDAC
    {
        public static SqlDataAdapter oAdaptadorTipoData = CreateTipoAdapter();

        public static SqlDataReader GetArticuloInformation(){
            SqlDataReader reader;
            string Sql = "Select *  from fnica.articulo";
            using (SqlCommand command = new SqlCommand(Sql,ConnectionManager.GetConnection())){
                reader = command.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.CloseConnection);
            }
            return reader;
        }

        private static SqlDataAdapter CreateTipoAdapter() {
            String getSQL = "SELECT *  FROM fnica.fafTIPO";
            String InsertSQL = "INSERT INTO fnica.fafTIPO      ( Tipo, Descr )VALUES  (@Tipo,  @Descr  )";
            String DeleteSQL = "Delete from fnica.fafTipo where Tipo=@Tipo";

            SqlDataAdapter oAdaptador = new SqlDataAdapter();
            oAdaptador.SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection());
            oAdaptador.InsertCommand = new SqlCommand(InsertSQL, ConnectionManager.GetConnection());
            oAdaptador.InsertCommand.Parameters.Add("@Tipo", SqlDbType.NChar).SourceColumn = "Tipo";
            oAdaptador.InsertCommand.Parameters.Add("@Descr", SqlDbType.NChar).SourceColumn = "Descr";
            
            oAdaptador.DeleteCommand = new SqlCommand(DeleteSQL, ConnectionManager.GetConnection());
            oAdaptador.DeleteCommand.Parameters.Add("@Tipo", SqlDbType.NChar).SourceColumn = "Tipo";
            return oAdaptador;
        }

        private static void DefineTipoSchema(DataTable table) {
            DataColumn cTipo = table.Columns.Add("Tipo", typeof(String));
            cTipo.AllowDBNull = false;
            table.PrimaryKey = new DataColumn[] { cTipo };

            DataColumn cDescr = table.Columns.Add("Descr", typeof(String));
            cDescr.MaxLength = 50;

        }

        private static DataSet CreateDataSetTipo() {
            DataSet TipoDS = new DataSet();
            DataTable tipoTable = TipoDS.Tables.Add("Tipo");
            DefineTipoSchema(tipoTable);
            return TipoDS;
        }

        public static DataSet GetData() {
            DataSet TipoDS = CreateDataSetTipo();
            TipoDS.EnforceConstraints = false;
            oAdaptadorTipoData.Fill(TipoDS.Tables["Tipo"]);
            TipoDS.EnforceConstraints = true;
            return TipoDS;
        }
    }
}
