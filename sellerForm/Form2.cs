using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class Form2 : Form
    {
       // string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        public Form2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            roleSelection r3 = new roleSelection();
            r3.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private bool ValidateLoginFields()
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Username is required.");
                txtUser.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Password is required.");
                txtPass.Focus();
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        
            if (!ValidateLoginFields())
                return;

            

            string username = txtUser.Text.Trim();
            string password = txtPass.Text.Trim();

            const string loginSql = @"SELECT COUNT(*)
                              FROM Buyer
                              WHERE userName = @userName AND [password] = @password";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(loginSql, con))
            {
                cmd.Parameters.Add("@userName", SqlDbType.NVarChar, 100).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;

                try
                {
                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        const string idSql = @"SELECT buyerID
                                       FROM Buyer
                                       WHERE userName = @userName AND [password] = @password";

                        using (SqlCommand idCmd = new SqlCommand(idSql, con))
                        {
                            idCmd.Parameters.Add("@userName", SqlDbType.NVarChar, 100).Value = username;
                            idCmd.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;

                            int buyerId = Convert.ToInt32(idCmd.ExecuteScalar());

                            this.Hide();
                            Form2Dashboard dash = new Form2Dashboard(buyerId);
                            dash.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2SignUp signup = new Form2SignUp();
            signup.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2ResetPass reset = new Form2ResetPass();
            reset.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2SignUp signup = new Form2SignUp();
            signup.Show();
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2ResetPass reset = new Form2ResetPass();
            reset.Show();
        }

        private void show_CheckedChanged(object sender, EventArgs e)
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
    }
}


