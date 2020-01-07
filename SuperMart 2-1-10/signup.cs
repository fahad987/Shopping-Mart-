using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace SuperMart_2_1_10
{
    public partial class signup : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

        public signup()
        {
            InitializeComponent();
        }

        private void signup_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "insert into signup values(@name,@surname,@gender,@age,@address,@email,@password)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", NametextBox.Text);
            cmd.Parameters.AddWithValue("@surname", SurNametextBox.Text);
            cmd.Parameters.AddWithValue("@Gender", GendercomboBox.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@age", AgenumericUpDown.Value);
            cmd.Parameters.AddWithValue("@address", ADDRESStextBox.Text);
            cmd.Parameters.AddWithValue("@email", EmailtextBox.Text);
            cmd.Parameters.AddWithValue("@password", PASStextBox.Text);


            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Registered Success !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Username is: " + NametextBox.Text + "\n \n" + "Password is: " + PASStextBox.Text, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Login LoginForm = new Login();
                LoginForm.ShowDialog();

            }
            else
            {
                MessageBox.Show("Registration Failed !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            con.Close();
        }
    }
}
