using System.Data;
using Microsoft.Data.SqlClient;

namespace PeluGestor.Data
{
    public static class PeluqueriasDao
    {
        public static DataTable ObtenerTodo()
        {
            string sql = @"
                SELECT Id, Nombre, Direccion, Telefono, Horario, DiasCerrados AS [Dias Cerrados]
                FROM dbo.Peluquerias
                ORDER BY Nombre;";
            return Db.Consulta(sql);
        }

        public static DataTable BuscarPorNombre(string texto)
        {
            string sql = @"
                SELECT Id, Nombre, Direccion, Telefono, Horario, DiasCerrados AS [Dias Cerrados]
                FROM dbo.Peluquerias
                WHERE Nombre LIKE @q
                ORDER BY Nombre;";

            return Db.Consulta(sql, new SqlParameter("@q", texto + "%"));
        }

        public static int Insertar(string nombre, string direccion, string telefono, string horario, string diasCerrados)
        {
            string sql = @"
                INSERT INTO dbo.Peluquerias (Nombre, Direccion, Telefono, Horario, DiasCerrados)
                VALUES (@Nombre, @Direccion, @Telefono, @Horario, @DiasCerrados);";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@Direccion", direccion),
                new SqlParameter("@Telefono", telefono),
                new SqlParameter("@Horario", horario),
                new SqlParameter("@DiasCerrados", diasCerrados));
        }

        public static int Update(int id, string nombre, string direccion, string telefono, string horario, string diasCerrados)
        {
            string sql = @"
                UPDATE dbo.Peluquerias
                SET Nombre = @Nombre,
                    Direccion = @Direccion,
                    Telefono = @Telefono,
                    Horario = @Horario,
                    DiasCerrados = @DiasCerrados
                WHERE Id = @Id;";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@Direccion", direccion),
                new SqlParameter("@Telefono", telefono),
                new SqlParameter("@Horario", horario),
                new SqlParameter("@DiasCerrados", diasCerrados));
        }

        public static int Delete(int id)
        {
            string sql = @"DELETE FROM dbo.Peluquerias WHERE Id = @Id;";
            return Db.EjecutarCRUD(sql, new SqlParameter("@Id", id));
        }
    }
}
