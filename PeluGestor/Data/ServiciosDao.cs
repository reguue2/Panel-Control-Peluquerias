using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PeluGestor.Data
{
    public static class ServiciosDao
    {
        public static DataTable ObtenerTodos()
        {
            string sql = @"
                SELECT Id, PeluqueriaId AS [Peluqueria Id], Nombre, Descripcion, Precio, DuracionMin
                FROM dbo.Servicios
                ORDER BY PeluqueriaId;";

            return Db.Consulta(sql);
        }

        public static DataTable ObtenerPorPeluqueria(int peluqueriaId)
        {
            string sql = @"
                SELECT Id, PeluqueriaId, Nombre, Descripcion, Precio, DuracionMin
                FROM dbo.Servicios
                WHERE PeluqueriaId = @pid
                ORDER BY Nombre;";

            return Db.Consulta(sql, new SqlParameter("@pid", peluqueriaId));
        }

        public static DataTable Buscar(int peluqueriaId, string texto)
        {
            string sql = @"
                SELECT Id, PeluqueriaId, Nombre, Descripcion, Precio, DuracionMin
                FROM dbo.Servicios
                WHERE PeluqueriaId = @pid
                  AND Nombre LIKE @q
                ORDER BY Nombre;";

            return Db.Consulta(sql,
                new SqlParameter("@pid", peluqueriaId),
                new SqlParameter("@q", texto + "%"));
        }

        public static DataTable BuscarTodos(string texto)
        {
            string sql = @"
                SELECT Id, PeluqueriaId AS [Peluqueria Id], Nombre, Descripcion, Precio, DuracionMin
                FROM dbo.Servicios
                WHERE Nombre LIKE @q
                ORDER BY Nombre;";

            return Db.Consulta(sql, new SqlParameter("@q", texto + "%"));
        }

        public static int Insertar(int peluqueriaId, string nombre, string descripcion, decimal precio, int duracionMin)
        {
            string sql = @"
                INSERT INTO dbo.Servicios (PeluqueriaId, Nombre, Descripcion, Precio, DuracionMin)
                VALUES (@pid, @nombre, @desc, @precio, @dur);";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@pid", peluqueriaId),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@desc", descripcion),
                new SqlParameter("@precio", precio),
                new SqlParameter("@dur", duracionMin));
        }

        public static int Update(int id, string nombre, string? descripcion, decimal precio, int duracionMin)
        {
            string sql = @"
                UPDATE dbo.Servicios
                SET Nombre = @nombre,
                    Descripcion = @desc,
                    Precio = @precio,
                    DuracionMin = @dur
                WHERE Id = @id;";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@id", id),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@desc", (object?)descripcion ?? DBNull.Value),
                new SqlParameter("@precio", precio),
                new SqlParameter("@dur", duracionMin));
        }

        public static int Delete(int id)
        {
            string sql = @"DELETE FROM dbo.Servicios WHERE Id = @id;";
            return Db.EjecutarCRUD(sql, new SqlParameter("@id", id));
        }
    }
}
