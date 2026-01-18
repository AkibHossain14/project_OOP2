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
    public partial class SoldItemList : Form
    {
        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        int sellerID;
        public SoldItemList(int sellerID)
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
            dashboard ds = new dashboard(sellerID);
            ds.Show();
        }
    }
}
