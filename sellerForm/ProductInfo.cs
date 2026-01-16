using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class ProductInfo : Form
    {
        byte[] newImageData;
        private readonly int id;
        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        public ProductInfo(int  i)
        {
            id = i;
            InitializeComponent();
            string[] categories = new string[12];
            categories[0] = "Others";
            categories[1] = "Electronics";
            categories[2] = "Fashion & Clothing";
            categories[3] = "Home & Living";
            categories[4] = "Books & Stationery";
            categories[5] = "Computer Accessories";
            categories[6] = "Gaming";
            categories[7] = "Beauty & Personal Care";
            categories[8] = "Food & Grocery";
            categories[9] = "Sports & Outdoor";
            categories[10] = "Vehicles & Accessories";
            categories[11] = "Toys & Baby Products";
            comboBox1.DataSource = categories;
            showlist();
        }

        public void showlist()
        {
            string query = "SELECT itemID as ID, itemName as Name, category as Category,brand as Brand,price as Price " +
                           "FROM NormalProductList WHERE sellerID = @id";
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

        


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
            dashboard d1= new dashboard(id);
            d1.Show();

        }

        private void ProductInfo_Load(object sender, EventArgs e)
        {
            showID.Text = id.ToString();
            showID.ReadOnly = true;

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string itemName;
            double price;
            int itemID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            int sellerID = id;
            string category = "";
            string brand = "";
            string description = "";

            string query1 = "SELECT itemName FROM NormalProductList WHERE itemID = @itemID";
            string query2 = "SELECT price FROM NormalProductList WHERE itemID = @itemID";
            string query3 = "SELECT image FROM NormalProductList WHERE itemID = @itemID";
            string query4 = "SELECT category FROM NormalProductList WHERE itemID = @itemID";
            string query5 = "SELECT brand FROM NormalProductList WHERE itemID = @itemID";
            string query6 = "SELECT description FROM NormalProductList WHERE itemID = @itemID";


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand command1 = new SqlCommand(query1, con))
                {
                    command1.Parameters.AddWithValue("@itemID", itemID);
                    itemName = (string)command1.ExecuteScalar();
                }
                using (SqlCommand command2 = new SqlCommand(query2, con))
                {
                    command2.Parameters.AddWithValue("@itemID", itemID);
                    price = (double)command2.ExecuteScalar();
                }
                using (SqlCommand command3 = new SqlCommand(query3, con))
                {
                    command3.Parameters.AddWithValue("@itemID", itemID);
                    newImageData = (byte[])command3.ExecuteScalar();
                }
                using (SqlCommand command4 = new SqlCommand(query4, con))
                {
                    command4.Parameters.AddWithValue("@itemID", itemID);
                    category = (string)command4.ExecuteScalar();
                }
                using (SqlCommand command5 = new SqlCommand(query5, con))
                {
                    command5.Parameters.AddWithValue("@itemID", itemID);
                    brand = (string)command5.ExecuteScalar();
                }
                using (SqlCommand command6 = new SqlCommand(query6, con))
                {
                    command6.Parameters.AddWithValue("@itemID", itemID);
                    description = (string)command6.ExecuteScalar();
                }
            }

            textBox1.Text = itemName;
            textBox2.Text = price.ToString();
            richTextBox1.Text = description;
            textBox3.Text = brand;
            comboBox1.Text = category;
            if (newImageData != null)
            {
                using (MemoryStream ms = new MemoryStream(newImageData))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
                        label9.Text = filePath;
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

        private void button2_Click(object sender, EventArgs e)
        {
            int sellerId = id;
            int itemID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            string newItemName = textBox1.Text.Trim();
            string newPriceText = textBox2.Text.Trim();
            string newDescription = richTextBox1.Text.Trim();
            string newCategory = comboBox1.Text.Trim();
            string newBrand = textBox3.Text.Trim();


            var fields = new Dictionary<string, string>
            {
                { "itemname", newItemName },
                { "price", newPriceText },
                { "description", newDescription },
                { "category", newCategory },
                { "brand", newBrand }
            };

            var missingField = fields.FirstOrDefault(f => string.IsNullOrWhiteSpace(f.Value));
            if (!string.IsNullOrEmpty(missingField.Key))
            {
                MessageBox.Show($"{missingField.Key} must be filled out.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!decimal.TryParse(newPriceText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
            {
                MessageBox.Show("Price must be a valid number (e.g., 1999.99).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (price < 0)
            {
                MessageBox.Show("Price cannot be negative.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            string query = "UPDATE NormalProductList SET itemName = @itemName, category = @category, brand = @brand, " +
                           "description = @description, price = @price, image = @image WHERE itemID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemName", newItemName);
                    command.Parameters.AddWithValue("@category", newCategory);
                    command.Parameters.AddWithValue("@brand", newBrand);
                    command.Parameters.AddWithValue("@description", newDescription);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@image", newImageData);
                    command.Parameters.AddWithValue("@id", itemID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Your Product Data UPDATED successfully.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            showlist();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int itemID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            DialogResult result = MessageBox.Show(
            "Are you sure you want to delete this profile?",
            "Confirm Deletion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM NormalProductList WHERE itemID = @itemID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@itemID", itemID);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Product deleted successfully. Please Refresh The List!");
            }
        }
    }
    
}
