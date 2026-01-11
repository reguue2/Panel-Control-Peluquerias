using System.Windows;
using PeluGestor.Views;

namespace PeluGestor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainContent.Content = new PeluqueriasView();
        }

        private void Peluquerias_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PeluqueriasView();
        }

        private void Peluqueros_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PeluquerosView();
        }

        private void Servicios_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ServiciosView();
        }

        private void Reservas_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReservasView();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea salir de la aplicacion?", "Salir", MessageBoxButton.YesNo,  MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void AcercaDe_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("PeluGestor\nAplicacion de gestion de peluquerias\n\nProyecto de Desarrollo de Interfaces", "Acerca de", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
