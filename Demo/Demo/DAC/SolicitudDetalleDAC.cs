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
        public static SqlDataAdapter oAdaptador = InicializarAdaptador();

        private static SqlDataAdapter InicializarAdaptador()
        {
            String getSQL = "SELECT *  FROM fnica.solSolicitudDetalle where (codSucursal=@CodSucursal or @CodSucursal='*') and (NumSolicitud=@NumSolicitud or @NumSolicitud='*')" ;
            String InsertSQL = "INSERT INTO fnica.solSolicitudDetalle        ( NumSolicitud ,          Articulo ,          CodSucursal ,          CantidadSolicitada ,          CantidadAsignada ,          CantidadPendiente ,          FlgLogAdded        ) values ( @NumSolicitud ,          @Articulo ,          @CodSucursal ,          @CantidadSolicitada ,          @CantidadAsignada ,          @CantidadPendiente ,          @FlgLogAdded)";
            String UpdateSQL = "UPDATE fnica.solSolicitudDetalle SET  CantidadSolicitada=@CantidadSolicitada WHERE CodSucursal=@CodSucursal AND NumSolicitud=@NumSolicitud";
            String DeleteSQL = "DELETE FROM fnica.solSolicitudDetalle WHERE CodSucursal=@CodSucursal AND NumSolicitud=@NumSolicitud";

            SqlDataAdapter oAdaptador = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection()),
                InsertCommand = new SqlCommand(InsertSQL, ConnectionManager.GetConnection()),
                UpdateCommand = new SqlCommand(UpdateSQL, ConnectionManager.GetConnection()),
                DeleteCommand = new SqlCommand(DeleteSQL, ConnectionManager.GetConnection())
            };

            //Paremetros select
            oAdaptador.SelectCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.SelectCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";

            //Parametros UpdateSQL
            oAdaptador.UpdateCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.UpdateCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";
            oAdaptador.UpdateCommand.Parameters.Add("@CantidadSolicitada", SqlDbType.NChar).SourceColumn = "CantidadSolicitada";

            //Parametros Insert  
            oAdaptador.InsertCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.InsertCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";
            oAdaptador.InsertCommand.Parameters.Add("@Articulo", SqlDbType.NChar).SourceColumn = "Articulo";
            oAdaptador.InsertCommand.Parameters.Add("@CantidadSolicitada", SqlDbType.Decimal).SourceColumn = "CantidadSolicitada";
            oAdaptador.InsertCommand.Parameters.Add("@CantidadAsignada", SqlDbType.Decimal).SourceColumn = "CantidadAsignada";
            oAdaptador.InsertCommand.Parameters.Add("@CantidadPendiente", SqlDbType.Decimal).SourceColumn = "CantidadPendiente";
            oAdaptador.InsertCommand.Parameters.Add("@FlgLogAdded", SqlDbType.Bit).SourceColumn = "FlgLogAdded";


            //Paremetros Insert
            oAdaptador.DeleteCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.DeleteCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn ="NumSolicitud";
            

            return oAdaptador;
        }

        public static void SetTransactionToAdaptador(bool Activo)
        {
            oAdaptador.UpdateCommand.Transaction = (Activo) ? ConnectionManager.Tran : null;
            oAdaptador.DeleteCommand.Transaction = (Activo) ? ConnectionManager.Tran : null;
            oAdaptador.InsertCommand.Transaction = (Activo) ? ConnectionManager.Tran : null;
        }

        private static DataSet CreateDataSet()
        {
            DataSet DS = new DataSet();
            DataTable tipoTable = DS.Tables.Add("Data");
            return DS;
        }

        public static DataSet GetData(String CodSucursal,String NumSolicitud)
        {
            DataSet DS = CreateDataSet();
            oAdaptador.SelectCommand.Parameters["@CodSucursal"].Value = CodSucursal;
            oAdaptador.SelectCommand.Parameters["@NumSolicitud"].Value = NumSolicitud;
            oAdaptador.Fill(DS.Tables["Data"]);
            return DS;
        }


    }
}
