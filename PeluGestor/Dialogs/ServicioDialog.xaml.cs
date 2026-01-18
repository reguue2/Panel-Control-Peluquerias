using System.Windows;

namespace PeluGestor.Dialogs
{
    public partial class ServicioDialog : Window
    {
        public string Nombre;
        public string Descripcion;
        public decimal Precio;
        public int DuracionMin;

        public ServicioDialog()
        {
            InitializeComponent();

            TxtPrecio.Text = "0";
            TxtDuracion.Text = "30";
        }

        public void CargarParaEditar(string nombre, string descripcion, decimal precio, int duracionMin)
        {
            TxtNombre.Text = nombre;
            TxtDescripcion.Text = descripcion;
            TxtPrecio.Text = precio.ToString();
            TxtDuracion.Text = duracionMin.ToString();
        }

        private void btnGuardar(object sender, RoutedEventArgs e)
        {
            string nombre = TxtNombre.Text.Trim().ToString();

            if (nombre == "")
            {
                MessageBox.Show(
                    "El nombre es obligatorio.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                TxtNombre.Focus();
                return;
            }

            decimal precio = decimal.Parse(TxtPrecio.Text.Trim());

            if (precio < 0)
            {
                MessageBox.Show(
                    "Precio invalido. Usa formato 10.50",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                TxtPrecio.Focus(); 
                return;
            }

            int dur = int.Parse(TxtDuracion.Text.Trim());
            if (dur <= 0)
            {
                MessageBox.Show(
                    "Duracion invalida. Debe ser mayor que 0.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                TxtDuracion.Focus();
                return;
            }

            Nombre = nombre;

            if (TxtDescripcion.Text == null || TxtDescripcion.Text.Trim() == "")
                Descripcion = null;
            else
                Descripcion = TxtDescripcion.Text.Trim();

            Precio = precio;
            DuracionMin = dur;

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
