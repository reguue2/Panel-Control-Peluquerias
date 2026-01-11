using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;

namespace PeluGestor.Data
{
    public static class Db
    {
        private const string ConexionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=PeluGestorBD;Integrated Security=True;TrustServerCertificate=True;";

        public static DataTable Consulta(string sql, params SqlParameter[] parameters)
        {
            try 
            {
                using var con = new SqlConnection(ConexionString);
                using var cmd = new SqlCommand(sql, con);

                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                using var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();

                con.Open(); 
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(
                    "Error al realizar la consulta a la base de datos.\n\n" + ex.Message,
                    "Error de base de datos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                return new DataTable();
            }
        }

        public static int EjecutarCRUD(string sql, params SqlParameter[] parameters)
        {
            try 
            {
                using var con = new SqlConnection(ConexionString);
                using var cmd = new SqlCommand(sql, con);

                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var p in parameters)
                    {
                        if (p.Value == null)
                            p.Value = DBNull.Value;
                    }
                    cmd.Parameters.AddRange(parameters);
                }

                con.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al ejecutar la operacion en la base de datos.\n\n" + ex.Message,
                    "Error de base de datos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                return -1; 
            }
        }
    }
}
