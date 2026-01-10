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

namespace sellerForm
{
    public partial class ProductInfo : Form
    {

        private readonly int id;
        string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";

        public ProductInfo(int  i)
        {
            id = i;
            InitializeComponent();

            string query = @"SELECT *
                             FROM productInfo2
                             WHERE sellerID = @id";

            FillDataGridView(query, id);

        }

        private void FillDataGridView(string query, int currentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Bind the parameter value instead of DECLARE @id in SQL
                    command.Parameters.Add("@id", SqlDbType.Int).Value = currentId;

                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();

                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load data:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
