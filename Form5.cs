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
using System.Xml.Linq;

namespace Supershop_Management
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            string query = "SELECT UserId, Username, Password, Name, Role, Address, Contact FROM Users WHERE Role = 'Cashier'";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCashierInfo.DataSource = dt;
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string name = txtName.Text;
            string role = "Cashier";  // Fixed as Cashier
            string address = txtAddress.Text;
            string contact = txtContact.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(contact))
            {
                MessageBox.Show("Please fill in all fields.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                string query = "INSERT INTO Users (Username, Password, Role, Name, Address, Contact) VALUES (@Username, @Password, @Role, @Name, @Address, @Contact)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Contact", contact);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cashier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();  // Reload data after adding
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Add Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvCashierInfo.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dgvCashierInfo.SelectedRows[0].Cells["UserId"].Value); // Get UserId from selected row

                // Confirm before deletion
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this cashier?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                    {
                        string query = "DELETE FROM Users WHERE UserId = @UserId";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Cashier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();  // Reload data after deletion
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvCashierInfo.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dgvCashierInfo.SelectedRows[0].Cells["UserId"].Value); // Get UserId from selected row
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string name = txtName.Text;
                string address = txtAddress.Text;
                string contact = txtContact.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(contact))
                {
                    MessageBox.Show("Please fill in all fields.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                {
                    string query = "UPDATE Users SET Username = @Username, Password = @Password, Name = @Name, Address = @Address, Contact = @Contact WHERE UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cashier information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();  // Reload data after updating
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
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();  // Open the Admin Portal form
            form4.Show();
            this.Hide();
        }

        private void dgvCashierInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a row is selected
            if (dgvCashierInfo.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgvCashierInfo.SelectedRows[0];

                // Populate the textboxes with data from the selected row
                txtUsername.Text = selectedRow.Cells["Username"].Value.ToString();
                txtPassword.Text = selectedRow.Cells["Password"].Value.ToString();
                txtName.Text = selectedRow.Cells["Name"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value.ToString();
                txtContact.Text = selectedRow.Cells["Contact"].Value.ToString();

            }
            else
            {
                // Clear textboxes if no row is selected
                txtUsername.Clear();
                txtPassword.Clear();
                txtName.Clear();
                txtAddress.Clear();
                txtContact.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Clear all textboxes
            txtUsername.Clear();
            txtPassword.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtContact.Clear();
        }
    }
}
