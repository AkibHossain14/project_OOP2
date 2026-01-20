using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sellerForm
{
    public partial class Form2Dashboard : Form
    {
        public Form2Dashboard()
        {
            InitializeComponent();
        }
        int buyerId;
        string username;
        public Form2Dashboard(int id, string username)
        {
            InitializeComponent();
            buyerId = id;
            this.username = username;
            label3.Text = username;
            label5.Text = buyerId.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            nProdectList npl = new nProdectList(buyerId,username);
            npl.Show();
        }

        private void Form2Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            aProductList a = new aProductList(buyerId,username);
            a.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            AuctionResultBuyer arb = new AuctionResultBuyer(buyerId,username);
            arb.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3(buyerId, username);
            form3.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 form2 = new Form2();
            form2.Show();
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
