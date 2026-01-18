using System;
using System.Data;
using System.Globalization;
using System.Windows;
using PeluGestor.Data;

namespace PeluGestor.Dialogs
{
    public partial class ReservaDialog : Window
    {
        public string NombreCliente;
        public string Telefono;
        public DateTime Fecha;
        public TimeSpan Hora;
        public int ServicioId;
        public int PeluqueroId;

        private int peluqueriaId;

        public ReservaDialog(int peluqueriaId)
        {
            InitializeComponent();
            this.peluqueriaId = peluqueriaId;

            CargarCombos();

            DpFecha.SelectedDate = DateTime.Today;
            TxtHora.Text = "10:00";
        }

        public void CargarParaEditar(string cliente, string telefono, DateTime fecha, TimeSpan hora, int servicioId, int peluqueroId)
        {
            TxtCliente.Text = cliente;
            TxtTelefono.Text = telefono;
            DpFecha.SelectedDate = fecha.Date;
            TxtHora.Text = hora.ToString("hh\\:mm");

            CmbServicio.SelectedValue = servicioId;
            CmbPeluquero.SelectedValue = peluqueroId;
        }

        private void CargarCombos()
        {
            DataTable dtServicios = ServiciosDao.ObtenerPorPeluqueria(peluqueriaId);
            CmbServicio.ItemsSource = dtServicios.DefaultView;
            if (dtServicios.Rows.Count > 0)
                CmbServicio.SelectedIndex = 0;

            DataTable dtPeluqueros = PeluquerosDao.ObtenerPorPeluqueria(peluqueriaId);
            DataView dv = dtPeluqueros.DefaultView;
            dv.RowFilter = "Activo = true";
            CmbPeluquero.ItemsSource = dv;
            if (dv.Count > 0)
                CmbPeluquero.SelectedIndex = 0;
        }

        private void btnGuardar(object sender, RoutedEventArgs e)
        {
            string cliente;
            string tel;

            if (TxtCliente.Text == null)
                cliente = "";
            else
                cliente = TxtCliente.Text.Trim();

            if (TxtTelefono.Text == null)
                tel = "";
            else
                tel = TxtTelefono.Text.Trim();

            if (cliente == "")
            {
                MessageBox.Show(
                    "Cliente obligatorio.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TxtCliente.Focus();
                return;
            }

            if (tel == "")
            {
                MessageBox.Show(
                    "Telefono obligatorio.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TxtTelefono.Focus();
                return;
            }

            if (tel.Length != 9 || !long.TryParse(tel, out _))
            {
                MessageBox.Show(
                    "El telefono debe tener 9 digitos y contener solo numeros.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TxtTelefono.Focus();
                return;
            }

            if (DpFecha.SelectedDate == null)
            {
                MessageBox.Show(
                    "Fecha obligatoria.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            DateTime dt = DpFecha.SelectedDate.Value;

            TimeSpan hora;
            if (!TimeSpan.TryParseExact(
                TxtHora.Text == null ? "" : TxtHora.Text.Trim(),
                "hh\\:mm",
                CultureInfo.InvariantCulture,
                out hora))
            {
                MessageBox.Show(
                    "Hora invalida. Formato HH:MM (ej 10:30).",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TxtHora.Focus();
                return;
            }

            if (CmbServicio.SelectedValue == null)
            {
                MessageBox.Show(
                    "Selecciona un servicio.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (CmbPeluquero.SelectedValue == null)
            {
                MessageBox.Show(
                    "Selecciona un peluquero.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            NombreCliente = cliente;
            Telefono = tel;
            Fecha = dt.Date;
            Hora = hora;
            ServicioId = (int)CmbServicio.SelectedValue;
            PeluqueroId = (int)CmbPeluquero.SelectedValue;

            DialogResult = true;
            Close();
        }

        private void btnCancelar(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
