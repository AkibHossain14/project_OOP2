
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;


namespace sellerForm
{
    public partial class NewProduct : Form
    {
        private readonly int id;
        byte[] imageData;

        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        public NewProduct(int i)
        {
            InitializeComponent();
            id = i;
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
        }

        private void NewProduct_Load(object sender, EventArgs e)
        {
            
            //selleridTXT.Text = id.ToString();
            selleridTXT.ReadOnly = true;
        }
        
        private void label1_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }

        private void button4_Click(object sender, EventArgs e)
        {
            
            selleridTXT.Text = id.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            dashboard ds = new dashboard(id);
            ds.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            txtItemName.Clear();
            txtPrice.Clear();
            txtDiscription.Clear();
            txtBrand.Clear();
            selleridTXT.Text = id.ToString();
        }

        private void button3_Click_1(object sender, EventArgs e)
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
                        label9.Text = filePath;
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

        private void button1_Click(object sender, EventArgs e)
        {
            
            int sellerId = id;

            string itemName = txtItemName.Text.Trim();
            string priceText = txtPrice.Text.Trim();
            string description = txtDiscription.Text.Trim();
            string category = comboBox1.Text.Trim();
            string brand = txtBrand.Text.Trim();

            
            var fields = new Dictionary<string, string>
            {
                { "itemname", itemName },
                { "price", priceText },
                { "description", description },
                { "category", category },
                { "brand", brand }
            };

            var missingField = fields.FirstOrDefault(f => string.IsNullOrWhiteSpace(f.Value));
            if (!string.IsNullOrEmpty(missingField.Key))
            {
                MessageBox.Show($"{missingField.Key} must be filled out.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (!decimal.TryParse(priceText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
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


            string query = "INSERT INTO NormalProductList (itemName, category, brand, description, price, image, sellerID) "+
                           "VALUES (@itemName, @category, @brand, @description, @price, @image, @iD)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@brand", brand);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@image", imageData);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Item added successfully.");
            this.Close();
            dashboard sellerDashBoard = new dashboard(id);
            sellerDashBoard.Show();
        }



            /* try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {



                    command.Parameters.AddWithValue("@sellerID", sellerId);
                    command.Parameters.AddWithValue("@itername", itemName);   // ← use itername here
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@brand", brand);





                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product saved successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                        txtItemName.Clear();
                        txtPrice.Clear();
                        txtDiscription.Clear();
                        txtcategory.Clear();
                        txtBrand.Clear();
                        selleridTXT.Text = sellerId.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save the product. Please try again.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}",
                    "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Operation error: {ex.Message}",
                    "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
    }

        
}

