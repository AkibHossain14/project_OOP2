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
using System.IO;

namespace sellerForm
{
    public partial class aProductList : Form
    {
        int id;
        string username;

        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        public aProductList(int id, string username)
        {
            InitializeComponent();

            panel1.Hide();
            showlist();
            this.id = id;
            this.username = username;
        }

        public aProductList()
        {
            InitializeComponent();
        }

        public void showlist()
        {
            string query = "SELECT auctionID as 'AuctionID', itemName as 'Item Name', category as 'Category', brand as 'Brand'," +
             "startingBid as 'Starting Price' FROM AuctionProductList";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            panel1.Show();
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            int auctionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["AuctionID"].Value);
            string name = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Item Name"].Value);
            string category = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Category"].Value);
            string brand = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Brand"].Value);
            string price = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Starting Price"].Value);
            string description;
            byte[] imagedata;
            DateTime start; DateTime end; DateTime now = DateTime.Now;

            string query = "SELECT description FROM AuctionProductList WHERE auctionID = @auctionID";
            string query2 = "SELECT image FROM AuctionProductList WHERE auctionID = @auctionID";
            string query3 = "SELECT startDate FROM AuctionProductList WHERE auctionID = @auctionID";
            string query4 = "SELECT endDate FROM AuctionProductList WHERE auctionID = @auctionID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query2, con))
                {
                    command.Parameters.AddWithValue("@auctionID", auctionID);
                    con.Open();
                    imagedata = (byte[])command.ExecuteScalar();
                    using (MemoryStream ms = new MemoryStream(imagedata))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@auctionID", auctionID);
                    con.Open();
                    description = (string)command.ExecuteScalar();
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query3, con))
                {
                    command.Parameters.AddWithValue("@auctionID", auctionID);
                    con.Open();
                    start = (DateTime)command.ExecuteScalar();
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query4, con))
                {
                    command.Parameters.AddWithValue("@auctionID", auctionID);
                    con.Open();
                    end = (DateTime)command.ExecuteScalar();
                }
            }
            richTextBox1.Text = name;
            richTextBox2.Text = description;
            label6.Text = category;
            label7.Text = brand;
            label8.Text = price;
            
            numericUpDown1.Minimum = Convert.ToDecimal(price)+500;
            numericUpDown1.Value = Convert.ToDecimal(price)+500;
            numericUpDown1.Maximum = Convert.ToDecimal(price) + 5000;
            numericUpDown1.Increment = 500;
            if (now < start) { label11.Text = "Upcoming"; }
            else if (now > end) { label11.Text = "Ended"; }
            else { label11.Text = "Live"; }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (label11.Text == "Ended")
            {
                MessageBox.Show("Auction has ended. You cannot place a bid.");
                return;
            }
            else if (label11.Text == "Upcoming")
            {
                MessageBox.Show("Auction has not started yet. You cannot place a bid.");
                return;
            }
            else if (label11.Text == "Live")
            {
                string auctionID = dataGridView1.CurrentRow.Cells["AuctionID"].Value.ToString();
                string name = richTextBox1.Text;
                decimal price = numericUpDown1.Value;
                int buyerID = id;
                int sellerID;

                string getSellerQuery = "SELECT sellerID FROM AuctionProductList WHERE auctionID = @auctionID";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(getSellerQuery, con))
                    {
                        command.Parameters.AddWithValue("@auctionID", auctionID);
                        con.Open();
                        sellerID = (int)command.ExecuteScalar();
                    }
                }

                string insertQuery = "INSERT INTO Bids (auctionID, itemName, buyerID, sellerID, bidAmount, bidTime) " +
                                     "VALUES (@auctionID,@itemName, @buyerID, @sellerID, @bidAmount, @bidTime)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, con))
                    {
                        command.Parameters.AddWithValue("@auctionID", auctionID);
                        command.Parameters.AddWithValue("@itemName", name);
                        command.Parameters.AddWithValue("@buyerID", buyerID);
                        command.Parameters.AddWithValue("@sellerID", sellerID);
                        command.Parameters.AddWithValue("@bidAmount", price);
                        command.Parameters.AddWithValue("@bidTime", DateTime.Now);
                        con.Open();
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Your bid has been placed successfully!");
                panel1.Hide();
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2Dashboard form2 = new Form2Dashboard(id, username);
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
