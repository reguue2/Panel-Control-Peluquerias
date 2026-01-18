using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
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
                CmbPeluqueria.SelectedValue = 0;
                DpFecha.SelectedDate = null;
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

            DataRow filaTodas = dtPelus.NewRow();
            filaTodas["Id"] = 0;
            filaTodas["Nombre"] = "Todas";
            dtPelus.Rows.InsertAt(filaTodas, 0);

            CmbPeluqueria.ItemsSource = dtPelus.DefaultView;
        }

        private int PeluqueriaId()
        {
            if (CmbPeluqueria.SelectedValue == null)
                return 0;

            return Convert.ToInt32(CmbPeluqueria.SelectedValue);
        }

        private DateTime? FechaSeleccionada()
        {
            return DpFecha.SelectedDate;
        }

        private string EstadoSeleccionado()
        {
            if (CmbEstado.SelectedItem is ComboBoxItem item)
            {
                string txt = item.Content.ToString();
                if (txt == "Todos") return "";
                return txt;
            }
            return "";
        }

        private void CargarDatos()
        {
            if (Grid == null) return;

            int pid = PeluqueriaId();
            DateTime? fecha = FechaSeleccionada();
            string estado = EstadoSeleccionado();

            dt = ReservasDao.Filtrar(pid, fecha, estado);
            Grid.ItemsSource = dt.DefaultView;
        }

        private DataRowView FilaSeleccionada()
        {
            return Grid.SelectedItem as DataRowView;
        }

        private void CmbPeluqueria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDatos();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }

        private void DpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDatos();
        }

        private void CmbEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarDatos();
        }

        private void btnNueva(object sender, RoutedEventArgs e)
        {
            int pid = PeluqueriaId();

            if (pid == 0)
            {
                MessageBox.Show(
                    "Seleccione una peluqueria concreta para crear una reserva.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ReservaDialog dlg = new ReservaDialog(pid);
            dlg.Owner = Window.GetWindow(this);

            if (dlg.ShowDialog() == true)
            {
                if (dlg.Fecha.Date < DateTime.Today)
                {
                    MessageBox.Show(
                        "No se pueden crear reservas en fechas pasadas.",
                        "Aviso",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

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
                        "No se pudo insertar la reserva.",
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

            if (row["Estado"].ToString() == "cancelada")
            {
                MessageBox.Show(
                    "No se puede editar una reserva que esta cancelada.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (Convert.ToDateTime(row["Fecha"]).Date < DateTime.Today)
            {
                MessageBox.Show(
                    "No se pueden editar reservas de fechas pasadas.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            int pid = Convert.ToInt32(row["PeluqueriaId"]);

            ReservaDialog dlg = new ReservaDialog(pid);
            dlg.Owner = Window.GetWindow(this);
            dlg.CargarParaEditar(
                row["NombreCliente"].ToString(),
                row["Telefono"].ToString(),
                Convert.ToDateTime(row["Fecha"]),
                (TimeSpan)row["Hora"],
                Convert.ToInt32(row["ServicioId"]),
                Convert.ToInt32(row["PeluqueroId"])
            );

            if (dlg.ShowDialog() == true)
            {
                if (dlg.Fecha.Date < DateTime.Today)
                {
                    MessageBox.Show(
                        "No se pueden editar reservas en fechas pasadas.",
                        "Aviso",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

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
                        "No se pudo actualizar la reserva.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                CargarDatos();
            }
        }

        private void btnCancelar(object sender, RoutedEventArgs e)
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

            if (row["Estado"].ToString() == "cancelada")
            {
                MessageBox.Show(
                    "La reserva ya esta cancelada.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (Convert.ToDateTime(row["Fecha"]).Date < DateTime.Today)
            {
                MessageBox.Show(
                    "No se puede cancelar una reserva de una fecha pasada.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            MessageBoxResult res = MessageBox.Show(
                "Desea cancelar esta reserva?",
                "Confirmacion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (res != MessageBoxResult.Yes) return;

            ReservasDao.Cancelar(Convert.ToInt32(row["Id"]));
            CargarDatos();
        }

        private void QuitarColumnas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id" ||
                e.PropertyName == "PeluqueriaId" ||
                e.PropertyName == "ServicioId" ||
                e.PropertyName == "PeluqueroId")
            {
                e.Cancel = true;
            }

            if (e.PropertyName == "Peluqueria")
                e.Column.Width = 180;

            if (e.PropertyName == "Servicio")
                e.Column.Width = 160;

            if (e.PropertyName == "Peluquero")
            {
                e.Column.Width = 140;
                var col = e.Column as DataGridTextColumn;
                if (col != null)
                    col.Binding.TargetNullValue = "Sin asignar";
            }

            if (e.PropertyName == "Cliente")
                e.Column.Width = 160;

            if (e.PropertyName == "Telefono")
                e.Column.Width = 120;

            if (e.PropertyName == "Fecha")
            {
                e.Column.Width = 110;
                var col = e.Column as DataGridTextColumn;
                if (col != null)
                    col.Binding.StringFormat = "dd/MM/yyyy";
            }

            if (e.PropertyName == "Hora")
            {
                e.Column.Width = 80;
                var col = e.Column as DataGridTextColumn;
                if (col != null)
                    col.Binding.StringFormat = @"hh\:mm";
            }

            if (e.PropertyName == "Estado")
                e.Column.Width = 110;
        }
    }
}
