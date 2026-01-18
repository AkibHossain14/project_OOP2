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
    public partial class Form2SignUp : Form
    {
        public Form2SignUp()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 ds = new Form2();
            ds.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
