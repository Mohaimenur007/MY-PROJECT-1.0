using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supershop_Management
{
    public partial class FormShowOrder : Form
    {
        public FormShowOrder()
        {
            InitializeComponent();
        }
        private void LoadOrderData()
        {
            string query = "SELECT OrderId, ProductName, Quantity, Price, TotalAmount, OrderDate FROM [Users].[dbo].[OrderDetails]";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvOrder.DataSource = dt;  // Bind the Order data to the dgvOrder DataGridView
            }
        }
        private void FormShowOrder_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadOrderData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
