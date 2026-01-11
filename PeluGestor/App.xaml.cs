using System.Windows;

namespace PeluGestor
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var main = new MainWindow();
            MainWindow = main;
            main.Show();
        }
    }
}
