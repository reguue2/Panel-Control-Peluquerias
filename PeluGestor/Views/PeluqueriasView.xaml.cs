using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using PeluGestor.Data;
using PeluGestor.Dialogs;

namespace PeluGestor.Views
{
    public partial class PeluqueriasView : UserControl
    {
        private DataTable dt = new DataTable();

        public PeluqueriasView()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            dt = PeluqueriasDao.ObtenerTodo();
            Grid.ItemsSource = dt.DefaultView;
        }

        private DataRowView FilaSeleccionada()
        {
            return Grid.SelectedItem as DataRowView;
        }

        private void btnAnadir(object sender, RoutedEventArgs e)
        {
            PeluqueriaDialog dlg = new PeluqueriaDialog();
            dlg.Owner = Window.GetWindow(this);

            if (dlg.ShowDialog() == true)
            {
                int res = PeluqueriasDao.Insertar( 
                    dlg.Nombre,
                    dlg.Direccion,
                    dlg.Telefono,
                    dlg.Horario,
                    dlg.DiasCerrados
                );

                if (res <= 0)
                {
                    MessageBox.Show(
                        "No se pudo insertar la peluqueria.",
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
                    "Seleccione una peluqueria para editar.",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            string nombre = row["Nombre"].ToString();
            string direccion = row["Direccion"].ToString();
            string telefono = row["Telefono"].ToString();
            string horario = row["Horario"].ToString();
            string dias = row["DiasCerrados"].ToString();

            PeluqueriaDialog dlg = new PeluqueriaDialog();
            dlg.Owner = Window.GetWindow(this);
            dlg.CargarParaEditar(id, nombre, direccion, telefono, horario, dias);

            if (dlg.ShowDialog() == true)
            {
                int res = PeluqueriasDao.Update( 
                    id,
                    dlg.Nombre,
                    dlg.Direccion,
                    dlg.Telefono,
                    dlg.Horario,
                    dlg.DiasCerrados
                );

                if (res <= 0)
                {
                    MessageBox.Show(
                        "No se pudo actualizar la peluqueria.",
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
                    "Seleccione una peluqueria para eliminar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            string nombre = row["Nombre"].ToString();

            MessageBoxResult confirm = MessageBox.Show( "Eliminar la peluqueria '" + nombre + "'?", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            int res = PeluqueriasDao.Delete(id);

            if (res <= 0)
            {
                MessageBox.Show(
                    "No se pudo eliminar la peluqueria.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            CargarDatos();
        }

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string q = TxtBuscar.Text.Trim().ToString();

            if (q == "")
            {
                CargarDatos();
                return;
            }

            dt = PeluqueriasDao.BuscarPorNombre(q);
            Grid.ItemsSource = dt.DefaultView;
        }

        private void QuitarColumnas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                e.Cancel = true;
            }

            if (e.PropertyName == "DiasCerrados")
            {
                e.Column.Header = "Dias cerrados";
            }
        }
    }
}
