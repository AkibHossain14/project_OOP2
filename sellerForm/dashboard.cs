using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class dashboard : Form
    {

        //string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        int  id;
        string username;

        public dashboard( int  i,string username )
        {
            InitializeComponent();
            id=i;
            this.username = username;
            label5.Text = id.ToString();
            label3.Text = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProductInfo p1 = new ProductInfo(id,username);
            p1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewProduct f1 = new NewProduct(id,username);
            f1.Show();
        }

        private void dashboard_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f2 = new Form1();
            f2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddProductAuction a = new AddProductAuction(id,username);
            a.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            AuctionProductList a1 = new AuctionProductList(id, username);
            a1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            AuctionResultSeller a2 = new AuctionResultSeller(id, username);
            a2.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            SoldItemList s1 = new SoldItemList(id,username);
            s1.Show();
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
