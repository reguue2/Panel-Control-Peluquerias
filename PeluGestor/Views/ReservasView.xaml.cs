using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using PeluGestor.Data;
using PeluGestor.Dialogs;

namespace PeluGestor.Views
{
    public partial class ReservasView : UserControl
    {
        private DataTable dt = new DataTable();

        public ReservasView()
        {
            InitializeComponent();

            try 
            {
                CargarPeluquerias();
                DpFecha.SelectedDate = null;
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar las reservas.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CargarPeluquerias()
        {
            DataTable dtPelus = PeluqueriasDao.ObtenerTodo();
            CmbPeluqueria.ItemsSource = dtPelus.DefaultView;
        }

        private int PeluqueriaId()
        {
            if (CmbPeluqueria.SelectedValue == null)
                return -1;

            return Convert.ToInt32(CmbPeluqueria.SelectedValue);
        }

        private string Estado()
        {
            ComboBoxItem item = CmbEstado.SelectedItem as ComboBoxItem;

            if (item == null) return "";

            string val = item.Content.ToString().Trim();
            if (val == "Todos")
                return "";
            else
                return val;
        }

        private DateTime? Fecha()
        {
            if (DpFecha.SelectedDate == null)
                return null;

            return DpFecha.SelectedDate.Value.Date;
        }

        private DataRowView FilaSeleccionada()
        {
            return Grid.SelectedItem as DataRowView;
        }

        private void CargarDatos()
        {
            int pid = PeluqueriaId();

            if (pid <= 0) 
            {
                Grid.ItemsSource = null;
                return;
            }

            DateTime? fecha = Fecha();

            dt = ReservasDao.Filtrar(pid, fecha, Estado());
            Grid.ItemsSource = dt.DefaultView;
        }

        private void CmbPeluqueria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            CargarDatos();
        }

        private void DpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            CargarDatos();
        }

        private void CmbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            CargarDatos();
        }

        private void btnNueva(object sender, RoutedEventArgs e)
        {
            int pid = PeluqueriaId();

            if (pid <= 0)
            {
                MessageBox.Show(
                    "Seleccione una peluqueria primero.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ReservaDialog dlg = new ReservaDialog(pid);
            dlg.Owner = Window.GetWindow(this);

            if (dlg.ShowDialog() == true)
            {
                int res = ReservasDao.Insertar( 
                    pid,
                    dlg.ServicioId,
                    dlg.PeluqueroId,
                    dlg.NombreCliente,
                    dlg.Telefono,
                    dlg.Fecha,
                    dlg.Hora
                );

                if (res <= 0)
                {
                    MessageBox.Show(
                        "No se puede guardar: el peluquero ya tiene una reserva en esa fecha y hora.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                CargarDatos();
            }
        }

        private void btnEditar(object sender, RoutedEventArgs e)
        {
            DataRowView row = FilaSeleccionada();
            if (row == null)
            {
                MessageBox.Show(
                    "Seleccione una reserva para editar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            int pid = Convert.ToInt32(row["PeluqueriaId"]);
            int servicioId = Convert.ToInt32(row["ServicioId"]);
            int peluqueroId = Convert.ToInt32(row["PeluqueroId"]);
            string cliente = row["NombreCliente"].ToString();
            string tel = row["Telefono"].ToString();
            DateTime fecha = Convert.ToDateTime(row["Fecha"]).Date;
            TimeSpan hora = (TimeSpan)row["Hora"];

            ReservaDialog dlg = new ReservaDialog(pid);
            dlg.Owner = Window.GetWindow(this);
            dlg.CargarParaEditar(cliente, tel, fecha, hora, servicioId, peluqueroId);

            if (dlg.ShowDialog() == true)
            {
                int res = ReservasDao.Update(
                    id,
                    dlg.ServicioId,
                    dlg.PeluqueroId,
                    dlg.NombreCliente,
                    dlg.Telefono,
                    dlg.Fecha,
                    dlg.Hora
                );

                if (res <= 0) 
                {
                    MessageBox.Show(
                        "No se puede guardar: el peluquero ya tiene una reserva en esa fecha y hora.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                CargarDatos();
            }
        }

        private void btnEliminar(object sender, RoutedEventArgs e)
        {
            DataRowView row = FilaSeleccionada();

            if (row == null)
            {
                MessageBox.Show(
                    "Seleccione una reserva para cancelar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            ;

            int id = Convert.ToInt32(row["Id"]);
            string estado = row["Estado"].ToString();

            if (estado == "cancelada")
            {
                MessageBox.Show(
                    "La reserva ya esta cancelada.",
                    "Info",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            MessageBoxResult res = MessageBox.Show(
                "Marcar la reserva como cancelada?",
                "Confirmacion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (res != MessageBoxResult.Yes) return;

            int r = ReservasDao.Delete(id);

            if (r <= 0)
            {
                MessageBox.Show(
                    "No se pudo cancelar la reserva.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            CargarDatos();
        }

        private void QuitarColumnas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id" || e.PropertyName == "PeluqueriaId" || e.PropertyName == "ServicioId" || e.PropertyName == "PeluqueroId" || e.PropertyName == "PeluqueriaNombre")
            {
                e.Cancel = true;
            }

            if (e.PropertyName == "ServicioNombre")
            {
                e.Column.Header = "Servicio";
            }

            if (e.PropertyName == "PeluqueroNombre")
            {
                e.Column.Header = "Peluquero";
            }

            if (e.PropertyName == "NombreCliente")
            {
                e.Column.Header = "Nombre";
            }

            if (e.PropertyName == "Fecha")
            {
                e.Column.Header = "Fecha";
                if (e.Column is DataGridTextColumn col)
                {
                    col.Binding.StringFormat = "dd/MM/yyyy";
                }
            }
        }
    }
}
