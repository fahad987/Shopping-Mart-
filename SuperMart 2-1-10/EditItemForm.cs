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

    public partial class EditItemForm : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

        public EditItemForm()
        {
            InitializeComponent();
            BindGridView();
        }
        void BindGridView()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select * from item_tbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data = new DataTable();
            sda.Fill(data);
            dataGridView1.DataSource = data;
        }

        private void AddItemForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            IDtextBox.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            NAMEtextBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            PRICEtextBox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            DISCOUNTtextBox.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void UPDATEbutton_Click(object sender, EventArgs e)
        {
            
            SqlConnection con = new SqlConnection(cs);
            string query = "update item_tbl set item_name = @name, item_price = @price, item_discount = @discount where item_id = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", IDtextBox.Text);
            cmd.Parameters.AddWithValue("@name", NAMEtextBox.Text);
            cmd.Parameters.AddWithValue("@price", PRICEtextBox.Text);
            cmd.Parameters.AddWithValue("@discount", DISCOUNTtextBox.Text);


            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Item Updated Successfully  !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGridView();
                IDtextBox.Clear();
                NAMEtextBox.Clear();
                PRICEtextBox.Clear();
                DISCOUNTtextBox.Clear();
                NAMEtextBox.Focus();


            }
            else
            {
                MessageBox.Show("Item is not Updated !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            con.Close();
        }

        private void DELETEbutton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "delete from item_tbl where item_id = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", IDtextBox.Text);



            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Item Deleted Successfully  !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGridView();
                IDtextBox.Clear();
                NAMEtextBox.Clear();
                PRICEtextBox.Clear();
                DISCOUNTtextBox.Clear();
                NAMEtextBox.Focus();


            }
            else
            {
                MessageBox.Show("Item is not Deleted !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            con.Close();
        }

        }
    }

