using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class buyerManagement : Form
    {
        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";

        int id;
        string username;
        public buyerManagement(int id, string username)
        {
            InitializeComponent();
            this.id = id;
            this.username = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT buyerID, username, phone, email FROM dbo.Buyer";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;  
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading buyers:\n" + ex.Message);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(userTXT.Text))
            {
                MessageBox.Show("Please enter a Buyer ID.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                userTXT.Focus();
                return;
            }

            if (!int.TryParse(userTXT.Text.Trim(), out int buyerId))
            {
                MessageBox.Show("Buyer ID must be a number.", "Validation",

MessageBoxButtons.OK, MessageBoxIcon.Warning);
                userTXT.Focus();
                userTXT.SelectAll();
                return;
            }

            
            var confirm = MessageBox.Show(
                $"Are you sure you want to delete Buyer ID {buyerId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);


            if (confirm != DialogResult.Yes) return;

            
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM dbo.Buyer WHERE buyerID = @id", conn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = buyerId;

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();


                    if (rows > 0)
                    {
                        MessageBox.Show("Buyer deleted successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                        userTXT.Clear();

                    }
                    else
                    {
                        MessageBox.Show("No buyer found with that ID.", "Not Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete buyer.\n\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var da = new SqlDataAdapter("SELECT buyerID, username, phone, email FROM dbo.Buyer", conn))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed to refresh buyers.\n\n" + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            adminDashboard ad = new adminDashboard(id,username);
            ad.Show();
        }

        private void button6_Click(object sender, EventArgs e)
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

