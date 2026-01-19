using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class sellerManagement : Form
    {

        private readonly string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        public sellerManagement()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM dbo.Seller";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM dbo.NormalProductList";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM dbo.AuctionProductList";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }


        }

        private void button2_Click_1(object sender, EventArgs e)
        {


            
            if (string.IsNullOrWhiteSpace(sellerTXT.Text))
            {
                MessageBox.Show("Please enter a Seller ID.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sellerTXT.Focus();
                return;
            }

            if (!int.TryParse(sellerTXT.Text.Trim(), out int sellerId))
            {
                MessageBox.Show("Seller ID must be a number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sellerTXT.Focus();
                sellerTXT.SelectAll();
                return;
            }

            
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete Seller ID {sellerId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            
            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(connectionString))
                using (var cmd = new System.Data.SqlClient.SqlCommand(
                    "DELETE FROM dbo.Seller WHERE sellerID = @id", conn))
                {
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = sellerId;

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Seller deleted successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        sellerTXT.Clear();

                        
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(
                            "SELECT sellerID, username, phone, email FROM dbo.Seller", conn))
                        {
                            var dt = new System.Data.DataTable();
                            da.Fill(dt);
                            dataGridView1.DataSource = dt; 
                        }
                    }
                    else
                    {
                        MessageBox.Show("No seller found with that ID.", "Not Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                
                MessageBox.Show("Failed to delete seller.\n\n" + ex.Message, "SQL Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error.\n\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {


            try
            {
                using (var conn = new System.Data.SqlClient.SqlConnection(connectionString))
                using (var da = new System.Data.SqlClient.SqlDataAdapter(
                    "SELECT sellerID, username, phone, email FROM dbo.Seller", conn))
                {
                    var dt = new System.Data.DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to refresh sellers.\n\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void sellerManagement_Load(object sender, EventArgs e)
        {

        }
    }
}
