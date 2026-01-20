using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class adminSignup : Form
    {
        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        public adminSignup()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            adminLogin f2 = new adminLogin();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            adminLogin f2 = new adminLogin();
            f2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            


            string userName = txtUserName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPass.Text.Trim();
            string confirmPassword = txtConfirmPass.Text.Trim();


            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) ||
             string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Password Must be same!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (phone.Length != 11)
            {
                MessageBox.Show("Phone number should cotain 11 numbers!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 4 || password.Length > 8)
            {
                MessageBox.Show("Passwrod should contain 4-8 charecters!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!long.TryParse(phone, out long parsedPhone))
            {
                MessageBox.Show("Phone number must contain only digits.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"INSERT INTO Admin ( userName, password, phone, email) 
                 VALUES ( @userName, @password, @phone, @email)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", userName);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@email", email);


                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Profile created successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    Form1 f1 = new Form1();
                    f1.Show();
                }
                else
                {
                    MessageBox.Show("Failed to create the profile. Please try again.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
