using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Demo
{
    public static class SolicitudDetalleDAC
    {
        public static SqlDataAdapter oAdaptadorSolicitud = InicializarAdaptador();

        private static SqlDataAdapter InicializarAdaptador()
        {
            String getSQL = "SELECT *  FROM fnica.solSolicitudDetalle";

            SqlDataAdapter oAdaptador = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection())
            };

            return oAdaptador;
        }

        private static DataSet CreateDataSet()
        {
            DataSet DS = new DataSet();
            DataTable tipoTable = DS.Tables.Add("SolicitudDetalle");
            return DS;
        }

        public static DataSet GetData()
        {
            DataSet DS = CreateDataSet();
            oAdaptadorSolicitud.Fill(DS.Tables["SolicitudDetalle"]);
            return DS;
        }
    }
}
