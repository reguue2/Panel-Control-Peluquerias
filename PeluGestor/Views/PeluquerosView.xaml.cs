using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using PeluGestor.Data;
using PeluGestor.Dialogs;

namespace PeluGestor.Views
{
    public partial class PeluquerosView : UserControl
    {
        private DataTable dt = new DataTable();

        public PeluquerosView()
        {
            InitializeComponent();
            try
            {
                CargarPeluquerias();
                CmbPeluqueria.SelectedValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar las peluquerias.\n\n" + ex.Message,
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
                return -1;

            return Convert.ToInt32(CmbPeluqueria.SelectedValue);
        }

        private void CargarDatos()
        {
            int pid = PeluqueriaId();

            if (pid == 0)
            {
                dt = PeluquerosDao.ObtenerTodos();
            }
            else
            {
                dt = PeluquerosDao.ObtenerPorPeluqueria(pid);
            }

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

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            int pid = PeluqueriaId();
            if (pid < 0) return;

            string q = TxtBuscar.Text.Trim();

            if (q == "")
            {
                CargarDatos();
                return;
            }

            if (pid == 0)
            {
                dt = PeluquerosDao.BuscarTodos(q);
            }
            else
            {
                dt = PeluquerosDao.Buscar(pid, q);
            }

            Grid.ItemsSource = dt.DefaultView;
        }

        private void btnNueva(object sender, RoutedEventArgs e)
        {
            int pid = PeluqueriaId();

            if (pid <= 0) 
            {
                MessageBox.Show(
                    "Seleccione una peluqueria primero.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            PeluqueroDialog dlg = new PeluqueroDialog();
            dlg.Owner = Window.GetWindow(this);

            if (dlg.ShowDialog() == true)
            {
                int res = PeluquerosDao.Insertar(pid, dlg.Nombre, dlg.Activo);

                if (res <= 0)
                {
                    MessageBox.Show(
                        "No se pudo insertar el peluquero.",
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
                    "Seleccione un peluquero para editar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            string nombre = row["Nombre"].ToString();
            bool activo = Convert.ToBoolean(row["Activo"]);

            PeluqueroDialog dlg = new PeluqueroDialog();
            dlg.Owner = Window.GetWindow(this);
            dlg.CargarParaEditar(nombre, activo);

            if (dlg.ShowDialog() == true)
            {
                int res = PeluquerosDao.Update(id, dlg.Nombre, dlg.Activo);

                if (res <= 0)
                {
                    MessageBox.Show(
                        "No se pudo actualizar el peluquero.",
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
                    "Seleccione un peluquero para eliminar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);

            MessageBoxResult res = MessageBox.Show("Eliminar peluquero?\nSi tiene reservas, no se permitira.", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (res != MessageBoxResult.Yes) return;

            int r = PeluquerosDao.Delete(id); 

            if (r <= 0)
            {
                MessageBox.Show(
                    "No se puede eliminar el peluquero porque tiene reservas asociadas.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            CargarDatos();
        }

        private void QuitarColumnas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                e.Cancel = true;
            }
        }
    }
}
