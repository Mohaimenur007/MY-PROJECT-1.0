using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supershop_Management
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormShowProducts formshowproducts = new FormShowProducts();
            formshowproducts.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCustomerShow formcustomershow = new FormCustomerShow();
            formcustomershow.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormShowOrder formshoworder = new FormShowOrder();
            formshoworder.Show();
            this.Hide();
        }
    }
}
