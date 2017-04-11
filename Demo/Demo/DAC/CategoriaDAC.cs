using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Demo
{
    public static class CategoriaDAC
    {
        public static SqlDataAdapter oAdaptadorSolicitud = InicializarAdaptador();

        private static SqlDataAdapter InicializarAdaptador()
        {
            String getSQL = "SELECT *  FROM fnica.SolCategoriaSolicitud";

            SqlDataAdapter oAdaptador = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection())
            };

            //Paremetros Insert
            


            return oAdaptador;
        }

        private static DataSet CreateDataSet()
        {
            DataSet DS = new DataSet();
            DataTable tipoTable = DS.Tables.Add("Categoria");
            return DS;
        }

        public static DataSet GetData()
        {
            DataSet DS = CreateDataSet();
            
            oAdaptadorSolicitud.Fill(DS.Tables["Categoria"]);
            return DS;
        }
    }
}
