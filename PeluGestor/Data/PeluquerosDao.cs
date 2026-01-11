using System.Data;
using Microsoft.Data.SqlClient;

namespace PeluGestor.Data
{
    public static class PeluquerosDao
    {
        public static DataTable ObtenerPorPeluqueria(int peluqueriaId)
        {
            string sql = @"
                SELECT Id, Nombre, Activo
                FROM dbo.Peluqueros
                WHERE PeluqueriaId = @pid
                ORDER BY Nombre;";

            return Db.Consulta(sql, new SqlParameter("@pid", peluqueriaId));
        }

        public static DataTable Buscar(int peluqueriaId, string texto)
        {
            string sql = @"
                SELECT Id, Nombre, Activo
                FROM dbo.Peluqueros
                WHERE PeluqueriaId = @pid
                  AND Nombre LIKE @q
                ORDER BY Nombre;";

            return Db.Consulta(sql,
                new SqlParameter("@pid", peluqueriaId),
                new SqlParameter("@q", texto + "%"));
        }

        public static int Insertar(int peluqueriaId, string nombre, bool activo)
        {
            string sql = @"
                INSERT INTO dbo.Peluqueros (PeluqueriaId, Nombre, Activo)
                VALUES (@pid, @nombre, @activo);";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@pid", peluqueriaId),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@activo", activo));
        }

        public static int Update(int id, string nombre, bool activo)
        {
            string sql = @"
                UPDATE dbo.Peluqueros
                SET Nombre = @nombre,
                    Activo = @activo
                WHERE Id = @id;";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@id", id),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@activo", activo));
        }

        public static int Delete(int id)
        {
            string sql = @"DELETE FROM dbo.Peluqueros WHERE Id = @id;";
            return Db.EjecutarCRUD(sql, new SqlParameter("@id", id));
        }
    }
}
