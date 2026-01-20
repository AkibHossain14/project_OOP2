using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace sellerForm
{
    public partial class SoldItemList : Form
    {
        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        int sellerID;
        string username;
        public SoldItemList(int sellerID, string username)
        {
            InitializeComponent();
            this.sellerID = sellerID;
            showList();

            string query = "SELECT COUNT(*) FROM Orders WHERE sellerID = @sellerID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@sellerID", sellerID);
                    con.Open();
                    int soldItemCount = (int)command.ExecuteScalar();
                    label2.Text = soldItemCount.ToString();
                }
            }

            this.username = username;
        }

        public void showList()
        {
            string query = "SELECT itemID as 'Product ID', itemName as 'Product Name', price as 'Price', buyerID as 'Buyer ID' FROM Orders Where sellerID = @sellerID";
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            dashboard d1 = new dashboard(sellerID, username);
            d1.Show();
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
