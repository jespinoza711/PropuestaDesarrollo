using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Demo
{
    public static class SolicitudDAC
    {
        public static SqlDataAdapter oAdaptadorSolicitud = InicializarAdaptador();

        private static SqlDataAdapter InicializarAdaptador() {
            String getSQL = "SELECT *  FROM fnica.solSolicitud where (codSucursal=@CodSucursal or @CodSucursal='*') and (NumSolicitud=@NumSolicitud or @NumSolicitud='*')";
            String InsertSQL = "INSERT INTO fnica.solSolicitud        ( NumSolicitud ,          CodSucursal ,          CodCategoria ,          Descripcion ,          Estado ,          UsuarioSolicitud ,          FechaSolicitud        ) VALUES  ( @NumSolicitud ,          @CodSucursal ,          @CodCategoria ,          @Descripcion ,          @Estado ,          @UsuarioSolicitud ,          @FechaSolicitud    )";
            String UpdateSQL = "UPDATE fnica.solSolicitud SET  CodCategoria = @CodCategoria,Descripcion = @Descripcion , Estado =@Estado WHERE NumSolicitud=@NumSolicitud and CodSucursal=@CodSucursal";
            String DeleteSQL = "DELETE  fnica.solSolicitud WHERE NumSolicitud=@NumSolicitud and CodSucursal=@CodSucursal";
         
            
            SqlDataAdapter oAdaptador = new SqlDataAdapter() {
                SelectCommand = new SqlCommand(getSQL, ConnectionManager.GetConnection()),
                        InsertCommand = new SqlCommand(InsertSQL,ConnectionManager.GetConnection()),
                        UpdateCommand = new SqlCommand(UpdateSQL, ConnectionManager.GetConnection()),
                        DeleteCommand = new SqlCommand(DeleteSQL, ConnectionManager.GetConnection())
            };

            //Paremetros Select 
            oAdaptador.SelectCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.SelectCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";


            //Paremetros Insert
            oAdaptador.InsertCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.InsertCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";
            oAdaptador.InsertCommand.Parameters.Add("@CodCategoria", SqlDbType.NChar).SourceColumn = "CodCategoria";
            oAdaptador.InsertCommand.Parameters.Add("@Descripcion", SqlDbType.NChar).SourceColumn = "Descripcion";
            oAdaptador.InsertCommand.Parameters.Add("@Estado", SqlDbType.NChar).SourceColumn = "Estado";
            oAdaptador.InsertCommand.Parameters.Add("@UsuarioSolicitud", SqlDbType.NChar).SourceColumn = "UsuarioSolicitud";
            oAdaptador.InsertCommand.Parameters.Add("@FechaSolicitud", SqlDbType.DateTime).SourceColumn = "FechaSolicitud";

            //Paremetros Update 
            oAdaptador.UpdateCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.UpdateCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";
            oAdaptador.UpdateCommand.Parameters.Add("@CodCategoria", SqlDbType.NChar).SourceColumn = "CodCategoria";
            oAdaptador.UpdateCommand.Parameters.Add("@Descripcion", SqlDbType.NChar).SourceColumn = "Descripcion";
            oAdaptador.UpdateCommand.Parameters.Add("@Estado", SqlDbType.NChar).SourceColumn = "Estado";

            
            
            //Paremetros Delete 
            oAdaptador.DeleteCommand.Parameters.Add("@CodSucursal", SqlDbType.NChar).SourceColumn = "CodSucursal";
            oAdaptador.DeleteCommand.Parameters.Add("@NumSolicitud", SqlDbType.NChar).SourceColumn = "NumSolicitud";


           
            return oAdaptador;
        }

   

        public static void SetTransactionToAdaptador(bool Activo) {
            oAdaptadorSolicitud.UpdateCommand.Transaction = (Activo) ?ConnectionManager.Tran :null;
            oAdaptadorSolicitud.DeleteCommand.Transaction = (Activo) ?ConnectionManager.Tran:null;
            oAdaptadorSolicitud.InsertCommand.Transaction = (Activo) ?ConnectionManager.Tran:null;
        }

        private static DataSet CreateDataSet()
        {
            DataSet DS = new DataSet();
            DataTable tipoTable = DS.Tables.Add("Solicitud");
            return DS;
        }

        public static DataSet GetData(String CodSucursal,String NumSolicitud) {
            DataSet DS = CreateDataSet();
            oAdaptadorSolicitud.SelectCommand.Parameters["@CodSucursal"].Value = CodSucursal;
            oAdaptadorSolicitud.SelectCommand.Parameters["@NumSolicitud"].Value = NumSolicitud;
            oAdaptadorSolicitud.Fill(DS.Tables["Solicitud"]);
            return DS;
        }

        public static String GetNextConsecutivo(String sCodSucursal) {
            DataSet DS = new DataSet();
            SqlCommand oCmd = new SqlCommand("SELECT fnica.fsolNextSolicitud(@CodSucursal) NumSolicitud",ConnectionManager.GetConnection());
            oCmd.Parameters.Add("@CodSucursal", SqlDbType.NVarChar).Value = sCodSucursal;
            SqlDataAdapter oAdaptador = new SqlDataAdapter(oCmd);
            
            oAdaptador.Fill(DS, "Consecutivo");
            return DS.Tables[0].Rows[0][0].ToString() ;
        }


        public static Task<DataSet> GetDataAsync(String CodSucursal, String NumSolicitud)
        {
            return Task.Factory.StartNew<DataSet>(() => {
                DataSet DS = CreateDataSet();
                oAdaptadorSolicitud.SelectCommand.Parameters["@CodSucursal"].Value = CodSucursal;
                oAdaptadorSolicitud.SelectCommand.Parameters["@NumSolicitud"].Value = NumSolicitud;
                oAdaptadorSolicitud.Fill(DS.Tables["Solicitud"]);
                return DS;
            });
           
            
        }

     

    }
}
