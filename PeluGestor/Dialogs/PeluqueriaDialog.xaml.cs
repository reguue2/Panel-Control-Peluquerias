using System.Windows;

namespace PeluGestor.Dialogs
{
    public partial class PeluqueriaDialog : Window
    {
        public int Id;
        public string Nombre;
        public string Direccion;
        public string Telefono;
        public string Horario;
        public string DiasCerrados;

        public PeluqueriaDialog()
        {
            InitializeComponent();
        }

        public void CargarParaEditar(int id, string nombre, string direccion, string telefono, string horario, string dias)
        {
            Id = id;
            TxtNombre.Text = nombre;
            TxtDireccion.Text = direccion;
            TxtTelefono.Text = telefono;
            TxtHorario.Text = horario;
            TxtDias.Text = dias;
        }

        private void btnGuardar(object sender, RoutedEventArgs e)
        {
            if (TxtNombre.Text == null || TxtNombre.Text.Trim() == "")
            {
                MessageBox.Show(
                    "El nombre es obligatorio.",
                    "Validacion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                TxtNombre.Focus();
                return;
            }

            Nombre = TxtNombre.Text.Trim();
            Direccion = TxtDireccion.Text.Trim();
            Telefono = TxtTelefono.Text.Trim();
            Horario =TxtHorario.Text.Trim();
            DiasCerrados = TxtDias.Text.Trim();

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
