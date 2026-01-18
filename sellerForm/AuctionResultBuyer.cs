using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class AuctionResultBuyer : Form
    {
        int buyerID;

        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        public AuctionResultBuyer(int buyerID)
        {
            InitializeComponent();
            this.buyerID = buyerID;
            showList();
            panel1.Hide();
        }

        public void showList()
        {
            string query = "SELECT itemID as 'Product ID', itemName as 'Product Name', bidAmount as 'Bid Amount',sellerID as 'Seller ID' FROM AuctionWinner WHERE buyerID = @buyerID";
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

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            panel1.Show();
            int itemID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Product ID"].Value);
            string itemName = dataGridView1.Rows[e.RowIndex].Cells["Product Name"].Value.ToString();
            float bidAmount = Convert.ToSingle(dataGridView1.Rows[e.RowIndex].Cells["Bid Amount"].Value);
            int sellerID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Seller ID"].Value);
            richTextBox1.Text = itemName;
            textBox1.Text = bidAmount.ToString();
            textBox2.Text = itemID.ToString();
            textBox3.Text = sellerID.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            float bidAmount = Convert.ToSingle(textBox1.Text);
            string itemID = textBox2.Text;
            int sellerID = Convert.ToInt32(textBox3.Text);
            string itemName = richTextBox1.Text;

            payment p1 = new payment(bidAmount, itemID, itemName, buyerID, sellerID,true);
            p1.Show();
            showList();
        }
    }
}
