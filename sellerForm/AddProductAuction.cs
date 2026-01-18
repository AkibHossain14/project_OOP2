using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace sellerForm
{
    public partial class AddProductAuction : Form
    {
        int id;
        byte[] imageData;

        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";


        public AddProductAuction(int id)
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
        }
        public AddProductAuction()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int sellerId = id;
            // Implementation for adding product to auction goes here
            string itemName = textBox1.Text.Trim();
            string category = comboBox1.SelectedItem.ToString();
            string brand = textBox2.Text.Trim();
            string description = richTextBox1.Text.Trim();
            string startingBidText = textBox3.Text.Trim();
            DateTime start = dateTimePicker1.Value;
            DateTime end = dateTimePicker2.Value;

            if(string.IsNullOrWhiteSpace(itemName) ||
               string.IsNullOrWhiteSpace(category) ||
               string.IsNullOrWhiteSpace(brand) ||
               string.IsNullOrWhiteSpace(description) ||
               string.IsNullOrWhiteSpace(startingBidText) ||
               imageData == null)
            {
                MessageBox.Show("Please fill in all fields and upload an image.");
                return;
            }
            
            if(!decimal.TryParse(startingBidText, out decimal startingBid) || startingBid < 0)
            {
                MessageBox.Show("Starting bid must be a valid non-negative number.");
                return;
            }
            
            if(start >= end)
            {
                MessageBox.Show("End date must be after start date.");
                return;
            }

            if (imageData == null)
            {
                MessageBox.Show("Please upload an image for the product.");
                return;
            }

            string query = "INSERT INTO AuctionProductList (itemName, category, brand, description, startingBid, startDate, endDate, image, sellerID) " +
                           "VALUES (@itemName, @category, @brand, @description, @startingBid, @startDate, @endDate, @image, @id)";

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@brand", brand);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@startingBid", startingBid);
                    command.Parameters.AddWithValue("@startDate", start);
                    command.Parameters.AddWithValue("@endDate", end);
                    command.Parameters.AddWithValue("@image", imageData);
                    command.Parameters.AddWithValue("@id", sellerId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Product added to auction successfully.");
            this.Close();
            dashboard sellerDashBoard = new dashboard(id);
            sellerDashBoard.Show();
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
                        imageData = File.ReadAllBytes(filePath);
                        label11.Text = filePath;
                        pictureBox1.Image = Image.FromFile(filePath);
                        MessageBox.Show("Image uploaded successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void AddProductAuction_Load(object sender, EventArgs e)
        {

        }
    }
}
