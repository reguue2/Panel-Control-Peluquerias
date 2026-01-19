using System.Threading.Tasks;
using System.Windows;

namespace PeluGestor
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
        }

        public async Task MostrarAsync()
        {
            Show();
            await Task.Delay(500);
            Close();
        }
    }
}
