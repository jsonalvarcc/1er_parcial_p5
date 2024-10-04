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
    public partial class FuncionarioForm : Form
    {
        private string connectionString = "Server=localhost;Database=bdjeyson;Uid=root;Pwd=;";
        public FuncionarioForm()
        {
            InitializeComponent();
        }
        private void CargarPersonas()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT cod_persona, nombre, apellidos, ci, rol FROM persona";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                  
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
               
                if (dataGridView1.Columns.Contains("Eliminar") && e.ColumnIndex == dataGridView1.Columns["Eliminar"].Index)
                {
                    int codPersona = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["cod_persona"].Value);
                    EliminarPersona(codPersona);
                }
                else 
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    int codPersona = Convert.ToInt32(row.Cells["cod_persona"].Value);
                    DetallePersonaForm detalleForm = new DetallePersonaForm(codPersona);
                    detalleForm.ShowDialog();
                    CargarPersonas();
                }
            }

        }
        private void EliminarPersona(int codPersona)
        {
         
            if (PersonaTieneCatastros(codPersona))
            {
                MessageBox.Show("No se puede eliminar la persona porque tiene catastros asociados.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM persona WHERE cod_persona = @codPersona";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codPersona", codPersona);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Persona eliminada exitosamente.");
                        CargarPersonas(); 
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar la persona.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar la persona: " + ex.Message);
                }
            }
        }

        private bool PersonaTieneCatastros(int codPersona)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM catastro WHERE cod_persona = @codPersona";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@codPersona", codPersona);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al verificar catastros: " + ex.Message);
                    return false;
                }
            }
        }






        private void FuncionarioForm_Load(object sender, EventArgs e)
        {
            CargarPersonas();
            AgregarColumnaEliminar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AgregarPersonaForm agregarForm = new AgregarPersonaForm();
            agregarForm.ShowDialog(); 
            CargarPersonas();
            AgregarColumnaEliminar();
        }
        private void AgregarColumnaEliminar()
        {
            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.HeaderText = "Eliminar";
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseColumnTextForButtonValue = true; 
            dataGridView1.Columns.Add(btnEliminar);
        }
    }
}
