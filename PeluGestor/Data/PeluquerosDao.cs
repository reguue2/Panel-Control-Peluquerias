using System.Data;
using Microsoft.Data.SqlClient;

namespace PeluGestor.Data
{
    public static class PeluquerosDao
    {
        public static DataTable ObtenerTodos()
        {
            string sql = @"
                SELECT 
                    pe.Id,
                    p.Nombre AS Peluqueria,
                    pe.Nombre,
                    pe.Activo
                FROM dbo.Peluqueros pe
                JOIN dbo.Peluquerias p
                    ON pe.PeluqueriaId = p.Id
                ORDER BY p.Nombre, pe.Nombre;";

            return Db.Consulta(sql);
        }

        public static DataTable ObtenerPorPeluqueria(int peluqueriaId)
        {
            string sql = @"
                SELECT 
                    pe.Id,
                    pe.Nombre,
                    pe.Activo
                FROM dbo.Peluqueros pe
                WHERE pe.PeluqueriaId = @pid
                ORDER BY pe.Nombre;";

            return Db.Consulta(sql, new SqlParameter("@pid", peluqueriaId));
        }

        public static DataTable Buscar(int peluqueriaId, string texto)
        {
            string sql = @"
                SELECT 
                    pe.Id,
                    pe.Nombre,
                    pe.Activo
                FROM dbo.Peluqueros pe
                WHERE pe.PeluqueriaId = @pid
                  AND pe.Nombre LIKE @q
                ORDER BY pe.Nombre;";

            return Db.Consulta(
                sql,
                new SqlParameter("@pid", peluqueriaId),
                new SqlParameter("@q", texto + "%")
            );
        }

        public static DataTable BuscarTodos(string texto)
        {
            string sql = @"
                SELECT 
                    pe.Id,
                    p.Nombre AS Peluqueria,
                    pe.Nombre,
                    pe.Activo
                FROM dbo.Peluqueros pe
                JOIN dbo.Peluquerias p
                    ON pe.PeluqueriaId = p.Id
                WHERE pe.Nombre LIKE @q
                ORDER BY p.Nombre, pe.Nombre;";

            return Db.Consulta(sql, new SqlParameter("@q", texto + "%"));
        }

        public static int Insertar(int peluqueriaId, string nombre, bool activo)
        {
            string sql = @"
                INSERT INTO dbo.Peluqueros (PeluqueriaId, Nombre, Activo)
                VALUES (@pid, @nombre, @activo);";

            return Db.EjecutarCRUD(
                sql,
                new SqlParameter("@pid", peluqueriaId),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@activo", activo)
            );
        }

        public static int Update(int id, string nombre, bool activo)
        {
            string sql = @"
                UPDATE dbo.Peluqueros
                SET Nombre = @nombre,
                    Activo = @activo
                WHERE Id = @id;";

            return Db.EjecutarCRUD(
                sql,
                new SqlParameter("@id", id),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@activo", activo)
            );
        }

        public static int Delete(int id)
        {
            string sql = @"DELETE FROM dbo.Peluqueros WHERE Id = @id;";
            return Db.EjecutarCRUD(sql, new SqlParameter("@id", id));
        }
    }
}
