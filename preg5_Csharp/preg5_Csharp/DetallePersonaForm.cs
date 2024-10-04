using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace preg5_Csharp
{
    public partial class DetallePersonaForm : Form
    {
        private string connectionString = "Server=localhost;Database=bdjeyson;Uid=root;Pwd=;";
        private int codPersona;
        public DetallePersonaForm(int codPersona)
        {
            InitializeComponent();
            this.codPersona = codPersona;
            comboBox1.Items.Add("funcionario");
            comboBox1.Items.Add("usuario");
            CargarDatosPersona();
            CargarCatastros();
        }

        private void DetallePersonaForm_Load(object sender, EventArgs e)
        {
        
        }

        private void CargarDatosPersona()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM persona WHERE cod_persona = @codPersona";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codPersona", codPersona);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                       
                        textBox1.Text = reader["nombre"].ToString();
                        textBox2.Text =  reader["apellidos"].ToString();
                        textBox3.Text = reader["ci"].ToString();
                        comboBox1.Text = reader["rol"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void CargarCatastros()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
            
                    string query = @"SELECT c.codigo_catastral, c.xini, c.yini, c.xfin, c.yfin, c.superficie, d.nombre AS nombre_distrito
                                     FROM catastro c
                                     INNER JOIN distrito d ON c.cod_distrito = d.cod_distrito
                                     WHERE c.cod_persona = @codPersona";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codPersona", codPersona);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                  
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los catastros: " + ex.Message);
                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void editaPersona_Click(object sender, EventArgs e)
        {

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE persona 
                             SET nombre = @nombre, apellidos = @apellidos, ci = @ci, rol = @rol
                             WHERE cod_persona = @codPersona";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", textBox1.Text);
                    cmd.Parameters.AddWithValue("@apellidos", textBox2.Text);
                    cmd.Parameters.AddWithValue("@ci", textBox3.Text);
                    cmd.Parameters.AddWithValue("@rol", comboBox1.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@codPersona", codPersona);

                    int resultado = cmd.ExecuteNonQuery();

                    if (resultado > 0)
                    {
                        MessageBox.Show("Los datos de la persona han sido actualizados.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar la persona.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la persona: " + ex.Message);
                }
            }
        }

        private void agregarCatastro_Click(object sender, EventArgs e)
        {
            AgregarCatastroForm agregarForm = new AgregarCatastroForm(codPersona);
            agregarForm.ShowDialog(); 
            CargarCatastros();
        }

      
    }
}
