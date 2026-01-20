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
    public partial class adminDashboard : Form
    {
        int id;
        string name;
        public adminDashboard(int id, string name)
        {
            InitializeComponent();
            this.id = id;
            this.name = name;
            label3.Text = name;
            label5.Text = id.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
             sellerManagement sm = new sellerManagement(id,name);
            sm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            buyerManagement bm = new buyerManagement(id,name);
            bm.Show();
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            adminLogin f2 = new adminLogin(); 
            f2.Show();
        }
    }
}
