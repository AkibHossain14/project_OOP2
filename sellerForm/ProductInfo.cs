
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
        private byte[] newImageData;          // holds current image (from DB or newly selected)
        private bool imageChanged = false;    // true only if user selected a new image via OpenFileDialog
        private readonly int id;
        string username;

        string connectionString = "data source=DESKTOP-CTAQMQQ\\SQLEXPRESS; database=sellerinfo; integrated security=SSPI";
        //private readonly string connectionString ="data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        public ProductInfo(int i, string username)
        {
            id = i;
            InitializeComponent();

            // Use plain '&' for display (remove HTML entities)
            string[] categories =
            {
                "Others",
                "Electronics",
                "Fashion & Clothing",
                "Home & Living",
                "Books & Stationery",
                "Computer Accessories",
                "Gaming",
                "Beauty & Personal Care",
                "Food & Grocery",
                "Sports & Outdoor",
                "Vehicles & Accessories",
                "Toys & Baby Products"
            };
            comboBox1.DataSource = categories;

            showlist();
            this.username = username;
        }

        public void showlist()
        {
            string query =
                "SELECT itemID AS ID, itemName AS Name, category AS Category, brand AS Brand, price AS Price " +
                "FROM NormalProductList WHERE sellerID = @id";

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            dashboard d1 = new dashboard(id, username);
            d1.Show();
        }

        private void ProductInfo_Load(object sender, EventArgs e)
        {
            showID.Text = id.ToString();
            showID.ReadOnly = true;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // not used
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
                return;

            int itemID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            string query =
                @"SELECT itemName, price, image, category, brand, description 
                  FROM NormalProductList 
                  WHERE itemID = @itemID;";

            string itemName = string.Empty;
            decimal price = 0m;
            string category = string.Empty;
            string brand = string.Empty;
            string description = string.Empty;
            byte[] imageBytes = null;

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@itemID", itemID);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        itemName = r["itemName"] as string ?? string.Empty;

                        // price could be float/double/decimal in DB; convert robustly
                        object priceObj = r["price"];
                        if (priceObj != DBNull.Value)
                        {
                            // Convert to decimal safely
                            price = Convert.ToDecimal(priceObj, CultureInfo.InvariantCulture);
                        }

                        category = r["category"] as string ?? string.Empty;
                        brand = r["brand"] as string ?? string.Empty;
                        description = r["description"] as string ?? string.Empty;

                        if (r["image"] != DBNull.Value)
                            imageBytes = (byte[])r["image"];
                    }
                }
            }

            // Fill UI
            textBox1.Text = itemName;
            textBox2.Text = price.ToString("0.##", CultureInfo.InvariantCulture);
            richTextBox1.Text = description;
            textBox3.Text = brand;
            comboBox1.Text = category;

            // Load picture if available
            newImageData = imageBytes; // keep what's currently in DB
            imageChanged = false;      // user hasn't picked a new image yet

            if (newImageData != null)
            {
                using (var ms = new MemoryStream(newImageData))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
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
                        imageChanged = true; // mark that user selected a new image
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
            {
                MessageBox.Show("Please select a product from the list first.", "Update", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int itemID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            string newItemName = textBox1.Text.Trim();
            string newPriceText = textBox2.Text.Trim();
            string newDescription = richTextBox1.Text.Trim();
            string newCategory = comboBox1.Text.Trim();
            string newBrand = textBox3.Text.Trim();

            // Validate required fields
            var fields = new Dictionary<string, string>
            {
                { "itemName", newItemName },
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

            // ---- Option A: only update image if the user selected a new one ----
            bool hasNewImage = imageChanged && newImageData != null && newImageData.Length > 0;

            var setParts = new List<string>
            {
                "itemName = @itemName",
                "category = @category",
                "brand = @brand",
                "description = @description",
                "price = @price"
            };
            if (hasNewImage)
                setParts.Add("image = @image");

            string query = $@"
                UPDATE NormalProductList
                SET {string.Join(", ", setParts)}
                WHERE itemID = @id;";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@itemName", newItemName);
                command.Parameters.AddWithValue("@category", newCategory);
                command.Parameters.AddWithValue("@brand", newBrand);
                command.Parameters.AddWithValue("@description", newDescription);

                // Be explicit for money: DECIMAL param (DB will convert to float if your column is float)
                var priceParam = command.Parameters.Add("@price", SqlDbType.Decimal);
                priceParam.Precision = 18;
                priceParam.Scale = 2;
                priceParam.Value = price;

                command.Parameters.AddWithValue("@id", itemID);

                if (hasNewImage)
                {
                    command.Parameters.Add("@image", SqlDbType.VarBinary, -1).Value = newImageData;
                }

                connection.Open();
                command.ExecuteNonQuery();
            }

            MessageBox.Show("Your Product Data UPDATED successfully.", "Update",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Refresh the list to reflect changes
            showlist();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            showlist();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
            {
                MessageBox.Show("Please select a product from the list first.", "Delete", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int itemID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this product?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("DELETE FROM NormalProductList WHERE itemID = @itemID", connection))
                {
                    command.Parameters.AddWithValue("@itemID", itemID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Product deleted successfully. Please refresh the list!");
                showlist();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
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
