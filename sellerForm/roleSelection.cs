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
    public partial class roleSelection : Form
    {
        public roleSelection()
        {
            InitializeComponent();
        }

        private void roleSelection_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            adminLogin a1= new adminLogin();
            a1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
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
