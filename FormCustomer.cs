using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Supershop_Management
{
    public partial class FormCustomer : Form
    {
        public FormCustomer()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void LoadCustomerData()
        {
            string query = "SELECT UserId, Username, Name, Contact, Address, Password FROM [Users].[dbo].[Users] WHERE Role = 'Customer'";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCustomerData.DataSource = dt;

                // Hide the Password column (as it shouldn't be displayed)
                if (dgvCustomerData.Columns.Contains("Password"))
                {
                    dgvCustomerData.Columns["Password"].Visible = false;
                }
            }

        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string name = txtName.Text;
            string contact = txtContact.Text;
            string address = txtAddress.Text;

            // Validate fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hash the password before storing it
            string hashedPassword = HashPassword(password);

            // Insert new customer into the Users table
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                string query = "INSERT INTO [Users].[dbo].[Users] (Username, Password, Role, Name, Contact, Address) " +
                               "VALUES (@Username, @Password, 'Customer', @Name, @Contact, @Address)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", hashedPassword); // Store hashed password
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@Address", address);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomerData();  // Reload data after adding
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Add Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvCustomerData.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dgvCustomerData.SelectedRows[0].Cells["UserId"].Value);

                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string name = txtName.Text;
                string contact = txtContact.Text;
                string address = txtAddress.Text;

                // Validate fields
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Please fill in all fields correctly.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hash the password before storing it
                string hashedPassword = HashPassword(password);

                // Update customer data in the Users table
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                {
                    string query = "UPDATE [Users].[dbo].[Users] SET Username = @Username, Password = @Password, Name = @Name, " +
                                   "Contact = @Contact, Address = @Address WHERE UserId = @UserId AND Role = 'Customer'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword); // Store hashed password
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomerData();  // Reload data after updating
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dgvCustomerData.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dgvCustomerData.SelectedRows[0].Cells["UserId"].Value);

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    // Delete customer from the Users table
                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                    {
                        string query = "DELETE FROM [Users].[dbo].[Users] WHERE UserId = @UserId AND Role = 'Customer'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadCustomerData();  // Reload data after deleting
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadCustomerData(); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtName.Clear();
            txtContact.Clear();
            txtAddress.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void dgvCustomerData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomerData.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvCustomerData.SelectedRows[0];

                txtUsername.Text = selectedRow.Cells["Username"].Value.ToString();
                txtPassword.Text = selectedRow.Cells["Password"].Value.ToString();
                txtName.Text = selectedRow.Cells["Name"].Value.ToString();
                txtContact.Text = selectedRow.Cells["Contact"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value.ToString();
            }
            else
            {
                txtUsername.Clear();
                txtPassword.Clear();
                txtName.Clear();
                txtContact.Clear();
                txtAddress.Clear();
            }
        }
    }
}
