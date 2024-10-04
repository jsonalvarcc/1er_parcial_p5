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
    public partial class Login : Form
    {
        private string connectionString = "Server=localhost;Database=bdjeyson;Uid=root;Pwd=;";
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string usuario = textBox1.Text;
            string contrasenia = textBox2.Text;

            string rol = ObtenerRol(usuario);

            if (rol != null)
            {
                MessageBox.Show("Inicio de sesión exitoso. Rol: " + rol);

                if (rol == "funcionario")
                {          
                    FuncionarioForm funcionarioForm = new FuncionarioForm();
                    funcionarioForm.Show();
                }
                else
                {
                              UsuarioForm usuarioForm = new UsuarioForm();
                    usuarioForm.Show();
                }
          
              
            }
            else
            {
                if (usuario != contrasenia) MessageBox.Show("credenciales incorrectos");
                else
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }

        }
        private bool ValidarCredenciales(string usuario, string contrasenia)
        {
            
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "SELECT * FROM persona WHERE  ci = @ci";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ci", contrasenia);

                    try
                    {
                        conn.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        return reader.HasRows;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                        return false;
                    }
                }
          
        }
        private string ObtenerRol(string usuario )
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT rol FROM persona WHERE   ci = @ci";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ci", usuario);
        

                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())                    {
                       
                        return reader["rol"].ToString();
                    }
                    else
                    {
           
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return null;
                }
            }
        }
    }
}
