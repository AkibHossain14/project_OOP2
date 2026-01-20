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

namespace sellerForm
{
    public partial class Form3 : Form
    {
        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        
        int buyerID;
        string username;
        public Form3(int buyerID, string username)
        {
            InitializeComponent();
            this.buyerID = buyerID;

            string query = "SELECT COUNT(*) FROM Orders WHERE buyerID = @buyerID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@buyerID", buyerID);
                    con.Open();
                    int soldItemCount = (int)command.ExecuteScalar();
                    label2.Text = soldItemCount.ToString();
                }
            }
            showList();
            this.username = username;
        }

        public void showList()
        {
            int buyerID = this.buyerID;
            string query = "SELECT itemID as 'Product ID', itemName as 'Product Name', price as 'Price', sellerID as 'Seller ID' FROM Orders Where buyerID = @buyerID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@buyerID", buyerID);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
         this.Close();
         Form2Dashboard form2 = new Form2Dashboard(buyerID, username);
         form2.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Form2Dashboard form2 = new Form2Dashboard(buyerID, username);
            form2.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
           "Are you sure you want to exit the application?",
           "Confirm Exit",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
