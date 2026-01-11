using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PeluGestor.Data
{
    public static class ReservasDao
    {
        public static DataTable Filtrar(int peluqueriaId, DateTime? fecha, string estado)
        {
            string sql = @"
                SELECT
                    r.Id,
                    r.PeluqueriaId,
                    p.Nombre AS PeluqueriaNombre,
                    r.ServicioId,
                    s.Nombre AS ServicioNombre,
                    r.PeluqueroId,
                    pe.Nombre AS PeluqueroNombre,
                    r.NombreCliente,
                    r.Telefono,
                    r.Fecha,
                    r.Hora,
                    r.Estado
                FROM dbo.Reservas r
                JOIN dbo.Peluquerias p ON p.Id = r.PeluqueriaId
                JOIN dbo.Servicios s ON s.Id = r.ServicioId
                JOIN dbo.Peluqueros pe ON pe.Id = r.PeluqueroId
                WHERE r.PeluqueriaId = @pid
                  AND (@fecha IS NULL OR r.Fecha = @fecha)
                  AND (@estado = '' OR r.Estado = @estado)
                ORDER BY r.Hora;";

            if (fecha == null)
            {
                return Db.Consulta(sql,
                    new SqlParameter("@pid", peluqueriaId),
                    new SqlParameter("@fecha", DBNull.Value),
                    new SqlParameter("@estado", estado));
            }
            else
            {
                return Db.Consulta(sql,
                    new SqlParameter("@pid", peluqueriaId),
                    new SqlParameter("@fecha", fecha),
                    new SqlParameter("@estado", estado));
            }
        }

        public static int Insertar(int peluqueriaId, int servicioId, int peluqueroId, string nombreCliente, string telefono, DateTime fecha, TimeSpan hora)
        {
            string sql = @"
                INSERT INTO dbo.Reservas
                (PeluqueriaId, ServicioId, PeluqueroId, NombreCliente, Telefono, Fecha, Hora, Estado)
                VALUES
                (@pid, @sid, @peid, @cliente, @tel, @fecha, @hora, 'confirmada');";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@pid", peluqueriaId),
                new SqlParameter("@sid", servicioId),
                new SqlParameter("@peid", peluqueroId),
                new SqlParameter("@cliente", nombreCliente),
                new SqlParameter("@tel", telefono),
                new SqlParameter("@fecha", fecha.Date),
                new SqlParameter("@hora", hora));
        }

        public static int Update(int id, int servicioId, int peluqueroId, string nombreCliente, string telefono, DateTime fecha, TimeSpan hora)
        {
            string sql = @"
                UPDATE dbo.Reservas
                SET ServicioId = @sid,
                    PeluqueroId = @peid,
                    NombreCliente = @cliente,
                    Telefono = @tel,
                    Fecha = @fecha,
                    Hora = @hora
                WHERE Id = @id;";

            return Db.EjecutarCRUD(sql,
                new SqlParameter("@id", id),
                new SqlParameter("@sid", servicioId),
                new SqlParameter("@peid", peluqueroId),
                new SqlParameter("@cliente", nombreCliente),
                new SqlParameter("@tel", telefono),
                new SqlParameter("@fecha", fecha.Date),
                new SqlParameter("@hora", hora));
        }

        public static int Delete(int id)
        {
            string sql = @"UPDATE dbo.Reservas SET Estado = 'cancelada' WHERE Id = @id;";
            return Db.EjecutarCRUD(sql, new SqlParameter("@id", id));
        }
    }
}
