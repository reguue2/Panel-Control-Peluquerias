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
                    ISNULL(p.Nombre, 'Peluqueria no existe') AS Peluqueria,
                    r.ServicioId,
                    ISNULL(s.Nombre, 'Servicio no existe') AS Servicio,
                    r.PeluqueroId,
                    ISNULL(pe.Nombre, 'Sin asignar') AS Peluquero,
                    r.NombreCliente AS Cliente,
                    r.Telefono,
                    r.Fecha,
                    r.Hora,
                    r.Estado
                FROM dbo.Reservas r
                LEFT JOIN dbo.Peluquerias p ON p.Id = r.PeluqueriaId
                LEFT JOIN dbo.Servicios s ON s.Id = r.ServicioId
                LEFT JOIN dbo.Peluqueros pe ON pe.Id = r.PeluqueroId
                WHERE
                    (@pid = 0 OR r.PeluqueriaId = @pid)
                AND (@fecha IS NULL OR r.Fecha = @fecha)
                AND (@estado = '' OR r.Estado = @estado)
                ORDER BY r.Fecha, r.Hora;";

            var pPid = new SqlParameter("@pid", SqlDbType.Int) { Value = peluqueriaId };
            var pFecha = new SqlParameter("@fecha", SqlDbType.Date) { Value = (object?)fecha ?? DBNull.Value };
            var pEstado = new SqlParameter("@estado", SqlDbType.NVarChar, 20) { Value = estado ?? "" };

            return Db.Consulta(sql, pPid, pFecha, pEstado);
        }


        public static int Insertar(int peluqueriaId, int servicioId, int peluqueroId,
                                   string nombreCliente, string telefono,
                                   DateTime fecha, TimeSpan hora)
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

        public static int Update(int id, int servicioId, int peluqueroId,
                                 string nombreCliente, string telefono,
                                 DateTime fecha, TimeSpan hora)
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

        public static int Cancelar(int id)
        {
            string sql = @"UPDATE dbo.Reservas SET Estado = 'cancelada' WHERE Id = @id;";
            return Db.EjecutarCRUD(sql, new SqlParameter("@id", id));
        }
    }
}
