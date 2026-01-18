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
    public partial class AuctionResultSeller : Form
    {
        int sellerID;

        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        public AuctionResultSeller(int sellerID)
        {
            InitializeComponent();
            this.sellerID = sellerID;
            showList();
            panel1.Hide();
        }

        public void showList()
        {
            string query = "SELECT auctionID as ID, itemName as 'Product Name',buyerID as 'Buyer ID', bidAmount as 'Bid Amount',bidTime as 'Time' FROM Bids WHERE sellerID = @sellerID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@sellerID", sellerID);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.ReadOnly = true;
            richTextBox1.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox2.ReadOnly = true;
            int auctionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            string itemName = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Product Name"].Value);
            int buyerID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Buyer ID"].Value);
            float bidAmount = Convert.ToSingle(dataGridView1.Rows[e.RowIndex].Cells["Bid Amount"].Value);
            DateTime bidTime = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["Time"].Value);

            panel1.Show();
            textBox1.Text = buyerID.ToString();
            richTextBox1.Text = itemName;
            textBox3.Text = bidAmount.ToString();

            string query = "SELECT userName from Buyer where buyerID = @buyerID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@buyerID", buyerID);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string buyerName = reader.GetString(0);
                        textBox2.Text = buyerName;
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int sellerID = this.sellerID;
            int buyerID = Convert.ToInt32(textBox1.Text);
            int auctionID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            string itemName = richTextBox1.Text;
            float bidAmount = Convert.ToSingle(textBox3.Text);

            string query = "INSERT INTO AuctionWinner (itemID, itemName, bidAmount, buyerID, sellerID) " +
                           "VALUES (@itemID, @itemName,@bidAmount, @buyerID, @sellerID)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemID", auctionID);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@buyerID", buyerID);
                    command.Parameters.AddWithValue("@sellerID", sellerID);
                    command.Parameters.AddWithValue("@bidAmount", bidAmount);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            

            string deleteQuery = "DELETE FROM Bids WHERE auctionID = @auctionID AND buyerID = @buyerID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@auctionID", auctionID);
                    command.Parameters.AddWithValue("@buyerID", buyerID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Auction Winner Has Been Chossen Successfully!");
            panel1.Hide();
            showList();
        }
    }
}
