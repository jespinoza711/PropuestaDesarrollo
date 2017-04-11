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
            String getSQL = "SELECT *  FROM fnica.solSolicitudDetalle where (codSucursal=@CodSucursal or @CodSucursal='*') and (NumSolicitud=@NumSolicitud or @NumSolicitud='*')" ;
            String InsertSQL = "INSERT INTO fnica.solSolicitudDetalle        ( NumSolicitud ,          Articulo ,          CodSucursal ,          CantidadSolicitada ,          CantidadAsignada ,          CantidadPendiente ,          FlgLogAdded        ) values ( @NumSolicitud ,          @Articulo ,          @CodSucursal ,          @CantidadSolicitada ,          @CantidadAsignada ,          @CantidadPendiente ,          @FlgLogAdded)";
            String UpdateSQL = "UPDATE fnica.solSolicitudDetalle SET  CantidadAsignada=@CantidadAsignada WHERE CodSucursal=@CodSucursal AND NumSolicitud=@NumSolicitud";
            String DeleteSQL = "DELETE FROM fnica.solSolicitudDetalle WHERE CodSucursal=@CodSucursal AND NumSolicitud=@NumSolicitud";

            SqlDataAdapter oAdaptador = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection()),
                InsertCommand = new SqlCommand(InsertSQL, ConnectionManager.GetConnection()),
                UpdateCommand = new SqlCommand(UpdateSQL, ConnectionManager.GetConnection()),
                DeleteCommand = new SqlCommand(DeleteSQL, ConnectionManager.GetConnection())
            };

            //Paremetros Insert
            oAdaptador.SelectCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.SelectCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";

            //Parametros UpdateSQL
            oAdaptador.UpdateCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.UpdateCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";
            oAdaptador.UpdateCommand.Parameters.Add("@CantidadSolicitada", SqlDbType.NChar).SourceColumn = "CantidadSolicitada";

            //Parametros UpdateSQL  
            oAdaptador.InsertCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.InsertCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";
            oAdaptador.InsertCommand.Parameters.Add("@Articulo", SqlDbType.NChar).SourceColumn = "Articulo";
            oAdaptador.InsertCommand.Parameters.Add("@CantidadSolicitada", SqlDbType.Decimal).SourceColumn = "CantidadSolicitada";
            oAdaptador.InsertCommand.Parameters.Add("@CantidadAsignada", SqlDbType.Decimal).SourceColumn = "CantidadAsignada";
            oAdaptador.InsertCommand.Parameters.Add("@CantidadPendiente", SqlDbType.Decimal).SourceColumn = "CantidadPendiente";
            oAdaptador.InsertCommand.Parameters.Add("@FlgLogAdded", SqlDbType.Bit).SourceColumn = "FlgLogAdded";


            //Paremetros Insert
            oAdaptador.DeleteCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.DeleteCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";

            return oAdaptador;
        }

        public static void SetTransactionToAdaptador(bool Activo)
        {
            oAdaptadorSolicitud.UpdateCommand.Transaction = (Activo) ? ConnectionManager.Tran : null;
            oAdaptadorSolicitud.DeleteCommand.Transaction = (Activo) ? ConnectionManager.Tran : null;
            oAdaptadorSolicitud.InsertCommand.Transaction = (Activo) ? ConnectionManager.Tran : null;
        }

        private static DataSet CreateDataSet()
        {
            DataSet DS = new DataSet();
            DataTable tipoTable = DS.Tables.Add("SolicitudDetalle");
            return DS;
        }

        public static DataSet GetData(String CodSucursal,String NumSolicitud)
        {
            DataSet DS = CreateDataSet();
            oAdaptadorSolicitud.SelectCommand.Parameters["@CodSucursal"].Value = CodSucursal;
            oAdaptadorSolicitud.SelectCommand.Parameters["@NumSolicitud"].Value = NumSolicitud;
            oAdaptadorSolicitud.Fill(DS.Tables["SolicitudDetalle"]);
            return DS;
        }


    }
}
