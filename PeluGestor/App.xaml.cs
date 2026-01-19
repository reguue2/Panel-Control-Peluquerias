using System.Threading.Tasks;
using System.Windows;

namespace PeluGestor
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SplashWindow splash = new SplashWindow();
            splash.Show();

            await Task.Delay(2000);

            MainWindow main = new MainWindow();
            MainWindow = main;
            main.Show();

            splash.Close();
        }
    }
}
