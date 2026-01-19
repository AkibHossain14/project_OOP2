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
        public adminDashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
             sellerManagement sm = new sellerManagement();
            sm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            buyerManagement bm = new buyerManagement();
            bm.Show();
        }
    }
}
