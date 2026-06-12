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
    public partial class FormCustomerShow : Form
    {
        public FormCustomerShow()
        {
            InitializeComponent();
        }
        private void LoadCustomerData()
        {
            string query = "SELECT UserId, Username, Name, Contact, Address, Password FROM [Users].[dbo].[Users] WHERE Role = 'Customer'";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind the DataTable to the DataGridView
                dgvCdata.DataSource = dt;

                // Hide the Password column for security reasons
                if (dgvCdata.Columns.Contains("Password"))
                {
                    dgvCdata.Columns["Password"].Visible = false;
                }
            }
        }

        private void FormCustomerShow_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }
    }
}
