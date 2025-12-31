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

        string connectionString = "data source=LAPTOP-F7UNN87C\\SQLEXPRESS; database=sellerInfo; integrated security=SSPI";
        int  id;

        public dashboard( int  i )
        {
            id = i;
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProductInfo p1 = new ProductInfo(id);
            p1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewProduct f1 = new NewProduct(id);
            f1.Show();
        }

        private void dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
