using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class ResetPass : Form
    {
        public ResetPass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string email = textBox2.Text;
            string pass = textBox3.Text;
            string conPass = textBox4.Text;

            string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
            //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";

            if (pass != conPass)
            {
                MessageBox.Show("Password and Confirm Password do not match.");
                return;
            }

            if (pass.Length < 4 || pass.Length > 8)
            {
                MessageBox.Show("Passwrod should contain 4-8 charecters!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) ||
             string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(conPass))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query1 = "UPDATE Seller SET password = @Password WHERE username = @Username AND email = @Email";

            string query2 = "SELECT COUNT(*) FROM Seller WHERE username = @Username AND email = @Email";

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                using (var command2 = new System.Data.SqlClient.SqlCommand(query2, connection))
                {
                    command2.Parameters.AddWithValue("@Username", userName);
                    command2.Parameters.AddWithValue("@Email", email);
                    int userCount = (int)command2.ExecuteScalar();
                    if (userCount == 0)
                    {
                        MessageBox.Show("No user found with the provided username and email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                using (var command1 = new System.Data.SqlClient.SqlCommand(query1, connection))
                {
                    command1.Parameters.AddWithValue("@Password", pass);
                    command1.Parameters.AddWithValue("@Username", userName);
                    command1.Parameters.AddWithValue("@Email", email);
                    int rowsAffected = command1.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password reset successfully.");
                        this.Hide();
                        Form1 f1 = new Form1();
                        f1.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Password reset failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


            }
        }
    }
}
