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
    public partial class AddItemForm : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

        public AddItemForm()
        {
            InitializeComponent();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "insert into item_tbl values(@name,@price,@discount)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", NAMEtextBox.Text);
            cmd.Parameters.AddWithValue("@price", PRICEtextBox.Text);
            cmd.Parameters.AddWithValue("@discount", DISCOUNTtextBox.Text);


            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Item Added Successfully  !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NAMEtextBox.Clear();
                PRICEtextBox.Clear();
                DISCOUNTtextBox.Clear();
                NAMEtextBox.Focus();


            }
            else
            {
                MessageBox.Show("Item is not Added !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            con.Close();
        }
    }
}
