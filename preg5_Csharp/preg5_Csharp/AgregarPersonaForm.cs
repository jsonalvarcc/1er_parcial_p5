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
    public partial class AgregarPersonaForm : Form
    {
        private string connectionString = "Server=localhost;Database=bdjeyson;Uid=root;Pwd=;";
        public AgregarPersonaForm()
        {
            InitializeComponent();
            comboBox1.Items.Add("funcionario");
            comboBox1.Items.Add("usuario");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombre = textBox1.Text;
            string apellidos = textBox1.Text;
            string ci = textBox1.Text;
            string rol = comboBox1.SelectedItem.ToString();

      
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellidos) || string.IsNullOrWhiteSpace(ci) || string.IsNullOrWhiteSpace(rol))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

               
                    string query = @"INSERT INTO persona (nombre, apellidos, ci, rol) 
                                     VALUES (@nombre, @apellidos, @ci, @rol)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@apellidos", apellidos);
                    cmd.Parameters.AddWithValue("@ci", ci);
                    cmd.Parameters.AddWithValue("@rol", rol);

                    int resultado = cmd.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        MessageBox.Show("Persona agregada exitosamente.");
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show("No se pudo agregar la persona.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar la persona: " + ex.Message);
                }
            }
        
        }
    }
}
