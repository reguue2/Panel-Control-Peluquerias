using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using PeluGestor.Data;
using PeluGestor.Dialogs;

namespace PeluGestor.Views
{
    public partial class ServiciosView : UserControl
    {
        private DataTable dt = new DataTable();

        public ServiciosView()
        {
            InitializeComponent();
            try 
            {
                CargarPeluqueria();
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

        private void CargarPeluqueria()
        {
            DataTable dtPelus = PeluqueriasDao.ObtenerTodo();
            CmbPeluqueria.ItemsSource = dtPelus.DefaultView;

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
                dt = ServiciosDao.ObtenerTodos();
            }
            else
            {
                dt = ServiciosDao.ObtenerPorPeluqueria(pid);
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
                dt = ServiciosDao.BuscarTodos(q);
            }
            else
            {
                dt = ServiciosDao.Buscar(pid, q);
            }

            Grid.ItemsSource = dt.DefaultView;
        }

        private void btnNuevo(object sender, RoutedEventArgs e)
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

            ServicioDialog dlg = new ServicioDialog();
            dlg.Owner = Window.GetWindow(this);

            if (dlg.ShowDialog() == true)
            {
                int res = ServiciosDao.Insertar( 
                    pid,
                    dlg.Nombre,
                    dlg.Descripcion,
                    dlg.Precio,
                    dlg.DuracionMin
                );

                if (res <= 0)
                {
                    MessageBox.Show(
                        "No se pudo insertar el servicio.",
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
                    "Seleccione un servicio para editar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            string nombre = row["Nombre"].ToString();
            string desc = row["Descripcion"].ToString();
            decimal precio = Convert.ToDecimal(row["Precio"]);
            int dur = Convert.ToInt32(row["DuracionMin"]);

            ServicioDialog dlg = new ServicioDialog();
            dlg.Owner = Window.GetWindow(this);
            dlg.CargarParaEditar(nombre, desc, precio, dur);

            if (dlg.ShowDialog() == true)
            {
                int res = ServiciosDao.Update(
                    id,
                    dlg.Nombre,
                    dlg.Descripcion,
                    dlg.Precio,
                    dlg.DuracionMin
                );

                if (res <= 0)
                {
                    MessageBox.Show(
                        "No se pudo actualizar el servicio.",
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
                    "Seleccione un servicio para eliminar.",
                    "Aviso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int id = Convert.ToInt32(row["Id"]);
            string nombre = row["Nombre"].ToString();

            MessageBoxResult res = MessageBox.Show("Eliminar servicio '" + nombre + "'?\nSi tiene reservas, no se permitira.", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (res != MessageBoxResult.Yes) return;

            int r = ServiciosDao.Delete(id);

            if (r <= 0)
            {
                MessageBox.Show(
                    "No se puede eliminar el servicio porque tiene reservas asociadas.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            CargarDatos();
        }

        private void QuitarColumnas(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id" || e.PropertyName == "PeluqueriaId")
            {
                e.Cancel = true;
            }

            if (e.PropertyName == "DuracionMin")
            {
                e.Column.Header = "Duracion";
            }
        }
    }
}
