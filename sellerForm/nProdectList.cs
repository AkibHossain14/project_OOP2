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
    public partial class nProdectList : Form
    {
        int id;
     
        //string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        public nProdectList(int id)
        {
            InitializeComponent();
            panel1.Hide();  
            showlist();
            this.id = id;
        }
        public void showlist()
        {
            string query =
                "SELECT itemID AS ID, itemName AS Name, category AS Category, brand AS Brand, price AS Price, sellerID AS 'Seller ID' " +
                "FROM NormalProductList";

            using (var con = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            panel1.Show();
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            string selectedItemId = dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            string name = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Name"].Value);
            string category = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Category"].Value);
            string brand = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Brand"].Value);
            string price = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Price"].Value);
            byte[] imagedata;
            string query = "SELECT description FROM NormalProductList WHERE itemID = @itemID";
            string query2 = "SELECT image FROM NormalProductList WHERE itemID = @itemID";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query2, con))
                {
                    command.Parameters.AddWithValue("@itemID", selectedItemId);
                    con.Open();
                    imagedata = (byte[])command.ExecuteScalar();
                    using (MemoryStream ms = new MemoryStream(imagedata))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@itemID", selectedItemId);
                    con.Open();
                    string description = Convert.ToString(command.ExecuteScalar());
                    richTextBox2.Text = description;
                }
            }
            richTextBox1.Text = name;
            label6.Text = category;
            label7.Text = brand;
            label8.Text = price;
        }
        
      private void button1_Click(object sender, EventArgs e)
        {
            
            string selectedItemId = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
            string name = richTextBox1.Text;
            float price = float.Parse(label8.Text);
            int buyerID = id;
            int sellerID;
            
            string query = "SELECT sellerID FROM NormalProductList WHERE itemID = @itemID";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@itemID", selectedItemId);
                    con.Open();
                    sellerID = (int)command.ExecuteScalar();
                }
            }
            payment p1 = new payment(price, selectedItemId, name, buyerID, sellerID,false);
            p1.Show();
            
        }
        private void richTextBox2_TextChanged(object sender, EventArgs e) { }

        private void button3_Click(object sender, EventArgs e)
        {
            showlist();
        }

        private void nProdectList_Load(object sender, EventArgs e)
        {

        }
    }
}
