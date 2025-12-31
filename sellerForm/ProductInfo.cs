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
    public partial class ProductInfo : Form
    {

        int  id;
        public ProductInfo(int  i)
        {
            id = i;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
            dashboard d1= new dashboard(id);
            d1.Show();

        }

        private void ProductInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
