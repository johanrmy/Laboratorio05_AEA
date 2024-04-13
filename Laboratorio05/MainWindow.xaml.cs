using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laboratorio05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=LAB1504-18\\SQLEXPRESS;Initial Catalog=Neptuno;User Id=johanramor;Password=123456";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            int idEmpleado = int.Parse(txtboxId.Text);
            string apellidos = txtboxApellidos.Text;
            string nombre = txtboxNombre.Text;
            string cargo = txtboxCargo.Text;
            string tratamiento = txtboxTratamiento.Text;
            DateTime fechaNacimiento = DateTime.ParseExact(txtboxNacimiento.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime fechaContratacion = DateTime.ParseExact(txtboxContratacion.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string direccion = txtboxDireccion.Text;
            string ciudad = txtboxCiudad.Text;
            string region = txtboxRegion.Text;
            string codPostal = txtboxCodPostal.Text;
            string pais = txtboxPais.Text;
            string telDomicilio = txtboxTelfDomicilio.Text;
            string extension = txtboxExtension.Text;
            string notas = txtboxNotas.Text;
            int jefe = int.Parse(txtboxJefe.Text);
            decimal sueldoBasico = decimal.Parse(txtboxSueldo.Text);

            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand command = new SqlCommand("SP_crear_empleado", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                command.Parameters.AddWithValue("@Apellidos", apellidos);
                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@Cargo", cargo);
                command.Parameters.AddWithValue("@Tratamiento", tratamiento);
                command.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                command.Parameters.AddWithValue("@FechaContratacion", fechaContratacion);
                command.Parameters.AddWithValue("@Direccion", direccion);
                command.Parameters.AddWithValue("@Ciudad", ciudad);
                command.Parameters.AddWithValue("@Region", region);
                command.Parameters.AddWithValue("@CodPostal", codPostal);
                command.Parameters.AddWithValue("@Pais", pais);
                command.Parameters.AddWithValue("@TelDomicilio", telDomicilio);
                command.Parameters.AddWithValue("@Extension", extension);
                command.Parameters.AddWithValue("@Notas", notas);
                command.Parameters.AddWithValue("@Jefe", jefe);
                command.Parameters.AddWithValue("@sueldoBasico", sueldoBasico);

                try
                {
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    MessageBox.Show($"Empleado creado correctamente. Filas afectadas: {rowsAffected}");

                    txtboxId.Text = String.Empty;
                    txtboxApellidos.Text = String.Empty;
                    txtboxNombre.Text = String.Empty;
                    txtboxCargo.Text = String.Empty;
                    txtboxTratamiento.Text = String.Empty;
                    txtboxNacimiento.Text = String.Empty;
                    txtboxContratacion.Text = String.Empty;
                    txtboxDireccion.Text = String.Empty;
                    txtboxCiudad.Text = String.Empty;
                    txtboxRegion.Text = String.Empty;
                    txtboxCodPostal.Text = String.Empty;
                    txtboxPais.Text = String.Empty;
                    txtboxTelfDomicilio.Text = String.Empty;
                    txtboxExtension.Text = String.Empty;
                    txtboxNotas.Text = String.Empty;
                    txtboxJefe.Text = String.Empty;
                    txtboxSueldo.Text = String.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al crear el empleado: {ex.Message}");
                }
            }
        }

        private void btnListar_Click(object sender, RoutedEventArgs e)
        {
            List<Empleado> empleados = new List<Empleado>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SP_listar_empleados", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string apellidos = reader.IsDBNull(reader.GetOrdinal("Apellidos")) ? string.Empty : reader.GetString(reader.GetOrdinal("Apellidos"));
                        string nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"));
                        string cargo = reader.IsDBNull(reader.GetOrdinal("Cargo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Cargo"));
                        DateTime fechaContratacion = reader.IsDBNull(reader.GetOrdinal("FechaContratacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaContratacion"));
                        string ciudad = reader.IsDBNull(reader.GetOrdinal("Ciudad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ciudad"));
                        string region = reader.IsDBNull(reader.GetOrdinal("Region")) ? string.Empty : reader.GetString(reader.GetOrdinal("Region"));
                        string telDomicilio = reader.IsDBNull(reader.GetOrdinal("TelDomicilio")) ? string.Empty : reader.GetString(reader.GetOrdinal("TelDomicilio"));
                        int jefe = reader.IsDBNull(reader.GetOrdinal("Jefe")) ? 0 : reader.GetInt32(reader.GetOrdinal("Jefe"));
                        decimal sueldoBasico = reader.IsDBNull(reader.GetOrdinal("SueldoBasico")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SueldoBasico"));

                        empleados.Add(new Empleado
                        {
                            Apellidos = apellidos,
                            Nombre = nombre,
                            Cargo = cargo,
                            FechaContratacion = fechaContratacion,
                            Ciudad = ciudad,
                            Region = region,
                            TelDomicilio = telDomicilio,
                            Jefe = jefe,
                            SueldoBasico = sueldoBasico
                        });
                    }
                }

                Listado listado = new Listado();
                listado.dgGrid.ItemsSource = empleados;
                listado.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar empleados: " + ex.Message);
            }
        }


    }
}