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
    public partial class Form2ResetPass : Form
    {
        public Form2ResetPass()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 ds = new Form2();
            ds.Show();
        }

        private void Form2ResetPass_Load(object sender, EventArgs e)
        {

        }
    }
}
