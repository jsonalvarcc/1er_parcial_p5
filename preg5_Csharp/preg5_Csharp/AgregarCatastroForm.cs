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
    
    public partial class AgregarCatastroForm : Form
{
    private string connectionString = "Server=localhost;Database=bdjeyson;Uid=root;Pwd=;";
    private int codPersona;

    public AgregarCatastroForm(int codPersona)
    {
        InitializeComponent();
        this.codPersona = codPersona;
        CargarDistritos();
    }

    private void AgregarCatastroForm_Load(object sender, EventArgs e)
    {
        CargarDistritos();

    }
    private void CargarDistritos()
    {
        

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT cod_distrito, nombre FROM distrito";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

          
                comboBox2.Items.Clear();

                while (reader.Read())
                {
               
                    string nombreDistrito = reader["nombre"].ToString();
                    int codigoDistrito = Convert.ToInt32(reader["cod_distrito"]);


                    comboBox2.Items.Add(new ComboBoxItem
                    {
                        Text = nombreDistrito,
                        Value = codigoDistrito
                    });
                }

      
                if (comboBox2.Items.Count == 0)
                {
                    MessageBox.Show("No se encontraron distritos en la base de datos.");
                }
            }
        }
        catch (MySqlException ex)
        {
            MessageBox.Show("Error de MySQL: " + ex.Message);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al cargar los distritos: " + ex.Message);
        }
        }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Text; 
        }
    }
    private void button1_Click(object sender, EventArgs e)
    {
        if (comboBox2.SelectedItem != null) 
        {
            ComboBoxItem selectedDistrict = (ComboBoxItem)comboBox2.SelectedItem; 
            int codDistrito = selectedDistrict.Value;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = @"INSERT INTO catastro (codigo_catastral, xini, yini, xfin, yfin, superficie, cod_distrito, cod_persona) 
                                         VALUES (@codigoCatastral, @xini, @yini, @xfin, @yfin, @superficie, @codDistrito, @codPersona)";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@codigoCatastral", Convert.ToInt32(textBox1.Text)); 
                        cmd.Parameters.AddWithValue("@xini", Convert.ToDecimal(textBox2.Text));
                        cmd.Parameters.AddWithValue("@yini", Convert.ToDecimal(textBox3.Text));
                        cmd.Parameters.AddWithValue("@xfin", Convert.ToDecimal(textBox4.Text));
                        cmd.Parameters.AddWithValue("@yfin", Convert.ToDecimal(textBox5.Text));
                        cmd.Parameters.AddWithValue("@superficie", Convert.ToDecimal(textBox6.Text));
                        cmd.Parameters.AddWithValue("@codDistrito", codDistrito);
                        cmd.Parameters.AddWithValue("@codPersona", codPersona);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Catastro agregado exitosamente.");
                            this.Close(); 
                        }
                        else
                        {
                            MessageBox.Show("No se pudo agregar el catastro.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al agregar el catastro: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un distrito.");
            }
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void AgregarCatastroForm_Load_1(object sender, EventArgs e)
    {
        CargarDistritos();
    }
}

    
}
