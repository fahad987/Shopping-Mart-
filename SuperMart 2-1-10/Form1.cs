using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace SuperMart_2_1_10
{
    public partial class Form1 : Form
    {
        int FinalCost = 0;
        int SrNo = 0;
        int tax = 0;
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
            getInvoiceID();
            USERtextBox.Text = Login.username;
            GetItems();
            dataGridView1.ColumnCount = 8;
            dataGridView1.Columns[0].Name = "SR NO";
            dataGridView1.Columns[1].Name = "ITEM NAME";
            dataGridView1.Columns[2].Name = "UNIT PRICE";
            dataGridView1.Columns[3].Name = "DISCOUNT PER ITEM";
            dataGridView1.Columns[4].Name = "QUANTITY";
            dataGridView1.Columns[5].Name = "SUB TOTAL";
            dataGridView1.Columns[6].Name = "TAX";
            dataGridView1.Columns[7].Name = "TOTAL COST";

        }
        void GetItems()
        {
            SELECTcomboBox.Items.Clear();
            SqlConnection con = new SqlConnection(cs);
            string query = "select * from item_tbl";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item_names = dr.GetString(1);
                SELECTcomboBox.Items.Add(item_names);
            }
             SELECTcomboBox.Sorted = true;
            con.Close();
        }

        void getPrice()
        {
            if (SELECTcomboBox.SelectedItem == null)
            {

            }
            else
            {

                int price = 0;
                SqlConnection con = new SqlConnection(cs);
                string query = "select item_price from item_tbl where item_name = @name";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.Parameters.AddWithValue("@name", SELECTcomboBox.SelectedItem.ToString());
                DataTable data = new DataTable();
                sda.Fill(data);

                if (data.Rows.Count > 0)
                {
                    price = Convert.ToInt32(data.Rows[0]["item_price"]);

                }
                UNITPRICEtextBox.Text = price.ToString();
            }

        }

        void getDiscount()
        {
            if (SELECTcomboBox.SelectedItem == null)
            {

            }
            else
            {
                int discount = 0;
                SqlConnection con = new SqlConnection(cs);
                string query = "select item_discount from item_tbl where item_name = @name";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.Parameters.AddWithValue("@name", SELECTcomboBox.SelectedItem.ToString());
                DataTable data = new DataTable();
                sda.Fill(data);

                if (data.Rows.Count > 0)
                {
                    discount = Convert.ToInt32(data.Rows[0]["item_discount"]);

                }
                DISCOUNTtextBox.Text = discount.ToString();
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SELECTcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPrice();
            getDiscount();
            QUANTITYtextBox.Enabled = true;
        }



        private void QUANTITYtextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(QUANTITYtextBox.Text) == true)
            {

            }
            else
            {
                int price = Convert.ToInt32(UNITPRICEtextBox.Text);
                int discount = Convert.ToInt32(DISCOUNTtextBox.Text);
                int quantity = Convert.ToInt32(QUANTITYtextBox.Text);
                int subTotal = price * quantity;
                subTotal = subTotal - discount * quantity;
                SUBTOTALtextBox.Text = subTotal.ToString();
            }
        }

        private void SUBTOTALtextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SUBTOTALtextBox.Text) == true)
            {

            }
            else
            {
                int subTotal = Convert.ToInt32(SUBTOTALtextBox.Text);

                if (subTotal >= 10000)
                {
                    tax = (int)(subTotal * 0.15);
                    TAXtextBox.Text = tax.ToString();
                }
                else if (subTotal >= 6000)
                {
                    tax = (int)(subTotal * 0.10);
                    TAXtextBox.Text = tax.ToString();
                }
                else if (subTotal >= 3000)
                {
                    tax = (int)(subTotal * 0.07);
                    TAXtextBox.Text = tax.ToString();
                }
                else if (subTotal >= 1000)
                {
                    tax = (int)(subTotal * 0.05);
                    TAXtextBox.Text = tax.ToString();
                }
                else
                {
                    tax = (int)(subTotal * 0.03);
                    TAXtextBox.Text = tax.ToString();
                }
            }


        }

        private void TAXtextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TAXtextBox.Text) == true)
            {

            }
            else
            {
                int subTotal = Convert.ToInt32(SUBTOTALtextBox.Text);
                int tax = Convert.ToInt32(TAXtextBox.Text);
                int TotalCost = subTotal + tax;
                TOTALCOSTtextBox.Text = TotalCost.ToString();
            }
        }
        void AddDataToGridView(string Sr_No, string item_name, string unit_price, string discount, string quantity, string sub_total, string tax, string total_cost)
        {
            string[] row = { Sr_No, item_name, unit_price, discount, quantity, sub_total, tax, total_cost };
            dataGridView1.Rows.Add(row);
        }

        private void ADDbutton_Click(object sender, EventArgs e)
        {
            if (SELECTcomboBox.SelectedItem != null)
            {
            AddDataToGridView((++SrNo).ToString(), SELECTcomboBox.SelectedItem.ToString(), UNITPRICEtextBox.Text, DISCOUNTtextBox.Text, QUANTITYtextBox.Text, SUBTOTALtextBox.Text, TAXtextBox.Text, TOTALCOSTtextBox.Text);
            ResetControls();
            CalculateFinalCost();
            }
            else
            {
                MessageBox.Show("Please Select An Item!!");
            }
        }

        void ResetControls()
        {
            SELECTcomboBox.SelectedItem = null;
            UNITPRICEtextBox.Clear();
            DISCOUNTtextBox.Clear();
            QUANTITYtextBox.Clear();
            SUBTOTALtextBox.Clear();
            TAXtextBox.Clear();
            TOTALCOSTtextBox.Clear();
            FINALCOSTtextBox.Clear();
            AMOUNTPAIDtextBox.Clear();
            CHANGEtextBox.Clear();
            QUANTITYtextBox.Enabled = false;

        }

        void CalculateFinalCost()
        {
            FinalCost = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                FinalCost = FinalCost + Convert.ToInt32(dataGridView1.Rows[i].Cells[7].Value);
            }
            FINALCOSTtextBox.Text = FinalCost.ToString();
        }

        private void AMOUNTPAIDtextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AMOUNTPAIDtextBox.Text) == true)
            {

            }
            else
            {
                int AmountPaid = Convert.ToInt32(AMOUNTPAIDtextBox.Text);
                int FCost = Convert.ToInt32(FINALCOSTtextBox.Text);
                int change = AmountPaid - FCost;
                CHANGEtextBox.Text = change.ToString();
            }
        }

        private void RESETbutton_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
              SrNo = 0;
        }

        void getInvoiceID()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select invoice_id from order_master";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data = new DataTable();
            sda.Fill(data);
            if (data.Rows.Count < 1)
            {
                INVOICEtextBox.Text = "1";
            }
            else
            {
                string query2 = "select max(invoice_id) from  order_master";
                SqlCommand cmd = new SqlCommand(query2, con);
                con.Open();
                int a = Convert.ToInt32(cmd.ExecuteScalar());
                a = a + 1;
                INVOICEtextBox.Text = a.ToString();
                con.Close();

            }
        }

        private void INSERTbutton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "insert into order_master values(@id,@user,@datetime,@finalcost)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", INVOICEtextBox.Text);
            cmd.Parameters.AddWithValue("@user", USERtextBox.Text);
            cmd.Parameters.AddWithValue("@datetime", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@finalcost", FINALCOSTtextBox.Text);
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Inserted successfully !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getInvoiceID();
                ResetControls();
            }
            else
            {
                MessageBox.Show("Insertion Failure !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            con.Close();
            InsertINtoOrderDetails();
        }

        int getLastInsertedInvoiceID()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select max(invoice_id) from order_master";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int MaxInvoiceId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return MaxInvoiceId;
        }

        void InsertINtoOrderDetails()
        {
            int a = 0;
            SqlConnection con = new SqlConnection(cs);
            try
            {

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    string query = "insert into order_details values(@invoice_id,@name,@price,@discount, @quantity,@subtotal,@tax,@finalcost)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@invoice_id", getLastInsertedInvoiceID());
                    cmd.Parameters.AddWithValue("@name", dataGridView1.Rows[i].Cells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("@price", dataGridView1.Rows[i].Cells[2].Value);
                    cmd.Parameters.AddWithValue("@discount", dataGridView1.Rows[i].Cells[3].Value);
                    cmd.Parameters.AddWithValue("@quantity", dataGridView1.Rows[i].Cells[4].Value);
                    cmd.Parameters.AddWithValue("@subtotal", dataGridView1.Rows[i].Cells[5].Value);
                    cmd.Parameters.AddWithValue("@tax", dataGridView1.Rows[i].Cells[6].Value);
                    cmd.Parameters.AddWithValue("@finalcost", dataGridView1.Rows[i].Cells[7].Value);
                    con.Open();
                    a = a + cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch
            {

            }

            if (a > 0)
            {
                MessageBox.Show("Data Added in order Details Table ");
            }
            else
            {
                MessageBox.Show("Data not Added in order Details Table ");

            }
        }



        private void QUANTITYtextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsDigit(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void AMOUNTPAIDtextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (char.IsDigit(ch) == true)
            {
                e.Handled = false;
            }
            else if (ch == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void PRINTPREVIEWbutton_Click(object sender, EventArgs e)
        {
            
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp = Properties.Resources.download;
            Image img = bmp;
            e.Graphics.DrawImage(img, 30, 5, 800, 250);

            e.Graphics.DrawString("Invoice Id: " + INVOICEtextBox.Text, new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 300));
            e.Graphics.DrawString("User Name: " + USERtextBox.Text, new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 330));
            e.Graphics.DrawString("Date: " + DateTime.Now.ToShortDateString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 360));
            e.Graphics.DrawString("Time: " + DateTime.Now.ToLongTimeString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 390));
            e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 420));
            e.Graphics.DrawString("ITEM", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 450));
            e.Graphics.DrawString("PRICE", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(240, 450));
            e.Graphics.DrawString("QUANTITY", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(390, 450));
            e.Graphics.DrawString("DISCOUNT", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(590, 450));
            e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 480));
          
             int gap = 520;
             if (dataGridView1.Rows.Count > 0)
             {
                 for (int i = 0; i < dataGridView1.Rows.Count; i++)
                 {
                     try
                     {

                         e.Graphics.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, gap));
                         gap = gap + 30;
                     }
                     catch
                     {

                     }
                 }
             }

             // Item Name

             int gap1 = 520;
             if (dataGridView1.Rows.Count > 0)
             {
                 for (int i = 0; i < dataGridView1.Rows.Count; i++)
                 {
                     try
                     {
                         e.Graphics.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(260, gap1));
                         gap1 = gap1 + 30;
                     }
                     catch
                     {

                     }

                 }
             }

             // Quantity Print
             int gap2 = 520;
             if (dataGridView1.Rows.Count > 0)
             {
                 for (int i = 0; i < dataGridView1.Rows.Count; i++)
                 {
                     try
                     {
                         e.Graphics.DrawString(dataGridView1.Rows[i].Cells[4].Value.ToString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(410, gap2));
                         gap2 = gap2 + 30;
                     }
                     catch
                     {

                     }

                 }

             }

             // Discount Per Item Print
             int gap3 = 520;
             if (dataGridView1.Rows.Count > 0)
             {
                 for (int i = 0; i < dataGridView1.Rows.Count; i++)
                 {
                     try
                     {
                         e.Graphics.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(590, gap3));
                         gap3 = gap3 + 30;
                     }
                     catch
                     {

                     }

                 }

             }

             // Sub-Total Print

             int SubtotalPrint = 0;
             for (int i = 0; i < dataGridView1.Rows.Count; i++)
             {
                 SubtotalPrint = SubtotalPrint + Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
             }
             e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 850));
             e.Graphics.DrawString("Sub-Total:" + SubtotalPrint.ToString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 880));

             // Tax Print

             int TaxPrint = 0;
             for (int i = 0; i < dataGridView1.Rows.Count; i++)
             {
                 TaxPrint = TaxPrint + Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
             }
             e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 850));
             e.Graphics.DrawString("Tax:" + TaxPrint.ToString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 910));
             e.Graphics.DrawString("Final AMount:" + FINALCOSTtextBox.Text, new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 940));
             e.Graphics.DrawString("--------------------------------------------------------------------------------------------------------------------", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 970));
             e.Graphics.DrawString("Amount Paid:" + AMOUNTPAIDtextBox.Text, new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 1000));
             e.Graphics.DrawString("Change:" + CHANGEtextBox.Text, new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(30, 1030));
           

        }

        private void PRINTbutton_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddItemForm ad = new AddItemForm ();
            ad.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void editItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditItemForm edf = new EditItemForm();
            edf.ShowDialog();
        }

        private void viewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewDataForm vdf = new ViewDataForm();
            vdf.ShowDialog();
        }

        private void detailsAndSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetailsAndSearch das = new DetailsAndSearch();
            das.ShowDialog();
        }


    }
}