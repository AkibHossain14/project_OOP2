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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace sellerForm
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private bool ValidateLoginFields()
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Password is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Focus();
                return false;
            }

            return true;
        }





        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateLoginFields())
                return;

            string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
            //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";

            string username = txtUser.Text.Trim();
            string password = txtPass.Text.Trim();

            const string loginSql = @"SELECT COUNT(*) 
                              FROM Seller 
                              WHERE userName = @userName AND [password] = @password";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(loginSql, con))
            {
                // Prefer explicit parameter types (less surprises than AddWithValue)
                cmd.Parameters.Add("@userName", SqlDbType.NVarChar, 100).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;

                try
                {
                    con.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Login successful!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Fetch the actual sellerID now
                        const string idSql = @"SELECT sellerID 
                                       FROM Seller 
                                       WHERE userName = @userName AND [password] = @password";

                        using (SqlCommand idCmd = new SqlCommand(idSql, con))
                        {
                            idCmd.Parameters.Add("@userName", SqlDbType.NVarChar, 100).Value = username;
                            idCmd.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;

                            object result = idCmd.ExecuteScalar();

                            if (result != null && result != DBNull.Value)
                            {
                                int sellerId = Convert.ToInt32(result);

                                this.Hide();
                                var dash = new dashboard(sellerId); // pass INT
                                dash.Show();
                            }
                            else
                            {
                                MessageBox.Show("Could not retrieve Seller ID.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password.", "Login Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex) // SqlException is fine too; Exception shows all
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form f1 = new SignUP();
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        
            if (show.Checked)
            {
                
                txtPass.UseSystemPasswordChar = false;
            }
            else
            {
                
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            ResetPass r1 = new ResetPass();
            r1.Show();
        }
    }
}

