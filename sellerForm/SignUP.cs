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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sellerForm
{
    public partial class SignUP : Form
    {
        public SignUP()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form f2 = new Form1();
            f2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
            //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
            
            string userName = txtUserName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password= txtPass.Text.Trim();
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

            string query = @"INSERT INTO Seller ( userName, password, phone, email) 
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void SignUP_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
