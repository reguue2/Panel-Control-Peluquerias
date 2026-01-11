using System.Windows;

namespace PeluGestor.Dialogs
{
    public partial class PeluqueroDialog : Window
    {
        public string Nombre;
        public bool Activo;

        public PeluqueroDialog()
        {
            InitializeComponent();
            ChkActivo.IsChecked = true;
            Activo = true;
        }

        public void CargarParaEditar(string nombre, bool activo)
        {
            TxtNombre.Text = nombre;
            ChkActivo.IsChecked = activo;
            Activo = activo;
        }

        private void btnGuardar(object sender, RoutedEventArgs e)
        {
            string nombre = TxtNombre.Text.Trim().ToString();

            if (nombre == "")
            {
                MessageBox.Show(
                    "El nombre del peluquero es obligatorio.",
                    "Validacion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                TxtNombre.Focus();
                return;
            }

            Nombre = nombre;
            Activo = ChkActivo.IsChecked == true;

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
