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
    public partial class AuctionProductList : Form
    {
        byte[] newImageData;
        int id;
       // string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        public AuctionProductList(int id)
        {
            InitializeComponent();
            this.id = id;
            string[] categories = new string[12];
            categories[0] = "Others";
            categories[1] = "Electronics";
            categories[2] = "Fashion & Clothing";
            categories[3] = "Home & Living";
            categories[4] = "Books & Stationery";
            categories[5] = "Computer Accessories";
            categories[6] = "Gaming";
            categories[7] = "Beauty & Personal Care";
            categories[8] = "Sports & Outdoor";
            categories[9] = "Vehicles & Accessories";
            comboBox1.DataSource = categories;
            showList();
        }

        public void showList()
        {
            string query = "SELECT auctionID as 'AuctionID', itemName as 'Item Name', category as 'Category', brand as 'Brand'," +
            "startingBid as 'Starting Price', startDate as 'Start', endDate as 'End' FROM AuctionProductList WHERE sellerID = @id";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        public AuctionProductList()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select an Image File";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        newImageData = File.ReadAllBytes(filePath);
                        label12.Text = filePath;
                        pictureBox1.Image = Image.FromFile(filePath);
                        MessageBox.Show("New Image uploaded successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int auctionID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AuctionID"].Value);
            string newItemName = textBox1.Text;
            string newCategory = comboBox1.SelectedItem.ToString();
            string newBrand = textBox2.Text;
            string newStartingPrice = textBox3.Text;
            string newDescription = richTextBox1.Text;
            DateTime newStartDate = dateTimePicker1.Value;
            DateTime newEndDate = dateTimePicker2.Value;

            if(string.IsNullOrWhiteSpace(newItemName) ||
               string.IsNullOrWhiteSpace(newCategory) ||
               string.IsNullOrWhiteSpace(newBrand) ||
               string.IsNullOrWhiteSpace(newStartingPrice) ||
               string.IsNullOrWhiteSpace(newDescription))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if(!decimal.TryParse(newStartingPrice, out decimal startingBidDecimal) || startingBidDecimal < 0)
            {
                MessageBox.Show("Starting Price must be a valid non-negative number.");
                return;
            }

            if(newEndDate <= newStartDate)
            {
                MessageBox.Show("End Date must be later than Start Date.");
                return;
            }

            string query = "UPDATE AuctionProductList SET itemName = @itemName, category = @category, brand = @brand, " +
                "startingBid = @startingBid, description = @description, startDate = @startDate, endDate = @endDate, image = @image" +
                " WHERE auctionID = @auctionID";
            using (SqlConnection con = new SqlConnection(connectionString))
            { 
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@itemName", newItemName);
                    command.Parameters.AddWithValue("@category", newCategory);
                    command.Parameters.AddWithValue("@brand", newBrand);
                    command.Parameters.AddWithValue("@startingBid", newStartingPrice);
                    command.Parameters.AddWithValue("@description", newDescription);
                    command.Parameters.AddWithValue("@startDate", newStartDate);
                    command.Parameters.AddWithValue("@endDate", newEndDate);
                    command.Parameters.AddWithValue("@image", newImageData);
                    command.Parameters.AddWithValue("@auctionID",auctionID);
                    con.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Auction product updated successfully.Please refresh the list!");
                    }
                    else
                    {
                        MessageBox.Show("Update failed. Please try again.");
                    }
                }
            }


        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string itemName = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Item Name"].Value);
            string category = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Category"].Value);
            string brand = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Brand"].Value);
            string description;
            string startingPrice = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Starting Price"].Value);
            int auctionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["AuctionID"].Value);
            DateTime startDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["Start"].Value);
            DateTime endDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["End"].Value);

            string query = "SELECT description FROM AuctionProductList WHERE auctionID = @auctionID";
            string query2 = "SELECT image FROM AuctionProductList WHERE auctionID = @auctionID";
           

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@auctionID", auctionID);
                    con.Open();
                    description = Convert.ToString(command.ExecuteScalar());
                }
                using (SqlCommand command2 = new SqlCommand(query2, con))
                {
                    command2.Parameters.AddWithValue("@auctionID", auctionID);
                    newImageData = (byte[])command2.ExecuteScalar();
                    using (MemoryStream ms = new MemoryStream(newImageData))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
            }
            textBox1.Text = itemName;
            comboBox1.SelectedItem = category;
            textBox2.Text = brand;
            textBox3.Text = startingPrice;
            richTextBox1.Text = description;
            dateTimePicker1.Value = startDate;
            dateTimePicker2.Value = endDate;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            showList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int auctionID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["AuctionID"].Value);
            string query = "DELETE FROM AuctionProductList WHERE auctionID = @auctionID";
            DialogResult result = MessageBox.Show(
            "Are you sure you want to delete this product?",
            "Confirm Deletion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
            );
            if ( result == DialogResult.Yes ) {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@auctionID", auctionID);
                        con.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Auction product deleted successfully. Please refresh the list!");
                        }
                        else
                        {
                            MessageBox.Show("Deletion failed. Please try again.");
                        }
                    }
                }
            }
        }

        private void AuctionProductList_Load(object sender, EventArgs e)
        {

        }
    }
}
