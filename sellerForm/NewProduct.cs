
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class NewProduct : Form
    {
        private readonly int id; 

        public NewProduct(int i)
        {
            id = i;
            InitializeComponent();
        }

        private void NewProduct_Load(object sender, EventArgs e)
        {
            
            selleridTXT.Text = id.ToString();
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
            txtcategory.Clear();
            txtBrand.Clear();
            selleridTXT.Text = id.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            int sellerId = id;

            string itemName = txtItemName.Text.Trim();
            string priceText = txtPrice.Text.Trim();
            string description = txtDiscription.Text.Trim();
            string category = txtcategory.Text.Trim();
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

            
            string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";




            string query = @"
    INSERT INTO [sellerinfo].[dbo].[productinfo2]
        ([sellerID], [itername], [price], [description], [category], [brand])
    VALUES
        (@sellerID, @itername, @price, @description, @category, @brand)";



            try
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
            }
        }
    }
}
