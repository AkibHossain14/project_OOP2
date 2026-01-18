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
using System.IO;

namespace sellerForm
{
    public partial class payment : Form
    {
        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        float price;
        string selectedItemId;
        string name;
        int buyerID;
        int sellerID;
        bool forAuction = false;
        public payment(float price, string selectedItemId, string name, int buyerID, int sellerID, bool forAuction)
        {
            InitializeComponent();
            string[] payment = new string[4];
            payment[0] = "Bank";
            payment[1] = "Bkash";
            payment[2] = "Nagad";
            payment[3] = "COD";
            this.price = price;
            this.selectedItemId = selectedItemId;
            this.name = name;
            this.buyerID = buyerID;
            this.sellerID = sellerID;
            comboBox1.DataSource = payment;
            label4.Text = price.ToString();
            this.forAuction = forAuction;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float paidAmount;
            paidAmount = float.Parse(textBox1.Text);
            
            if(paidAmount != price)
            {
                MessageBox.Show("Please enter the correct amount!");
            }
            else if (paidAmount == price)
            {
                string insertQuery = "INSERT INTO Orders (itemID, itemName, price, buyerID, sellerID) " +
                                 "VALUES (@itemID, @itemName, @price, @buyerID, @sellerID)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, con))
                    {
                        command.Parameters.AddWithValue("@itemID", selectedItemId);
                        command.Parameters.AddWithValue("@itemName", name);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@buyerID", buyerID);
                        command.Parameters.AddWithValue("@sellerID", sellerID);
                        con.Open();
                        command.ExecuteNonQuery();
                    }
                }

                if (forAuction == false) {
                    string deleteQuery = "DELETE FROM NormalProductList WHERE itemID = @itemID";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(deleteQuery, con))
                        {
                            command.Parameters.AddWithValue("@itemID", selectedItemId);
                            con.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    string deleteQuery = "DELETE FROM AuctionWinner WHERE itemID = @itemID";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(deleteQuery, con))
                        {
                            command.Parameters.AddWithValue("@itemID", selectedItemId);
                            con.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Payment Successful. Order has been placed. Product will reach to you soon!");
                this.Close();
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
