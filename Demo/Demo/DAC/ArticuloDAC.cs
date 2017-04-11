using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public static class ArticuloDAC
    {
        public static SqlDataAdapter oAdaptadorSolicitud = InicializarAdaptador();

        private static SqlDataAdapter InicializarAdaptador()
        {
            String getSQL = "SELECT *  FROM fnica.Articulo where (Articulo=@Articulo OR  @Articulo='*')";

            SqlDataAdapter oAdaptador = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection())
            };

            //Paremetros Insert
            oAdaptador.SelectCommand.Parameters.Add("@Articulo", SqlDbType.NChar).SourceColumn = "Articulo";
            



            return oAdaptador;
        }

        private static DataSet CreateDataSet()
        {
            DataSet DS = new DataSet();
            DataTable tipoTable = DS.Tables.Add("Articulo");
            return DS;
        }

        public static DataSet GetData(String Articulo)
        {
            DataSet DS = CreateDataSet();
            oAdaptadorSolicitud.SelectCommand.Parameters["@Articulo"].Value = Articulo;
            oAdaptadorSolicitud.Fill(DS.Tables["Articulo"]);
            return DS;
        }
    }
}
