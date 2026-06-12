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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false; // Show password
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true; // Hide password
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit if input is empty
            }

            // Call the method to validate the login credentials
            string role = GetUserRole(username, password);

            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Admin")
                {
                    // Open the Admin Portal form if role is Admin
                    Form4 adminPortal = new Form4();
                    adminPortal.Show();
                    this.Hide();
                }
                else if (role == "Cashier")
                {
                    // Open the Cashier Portal form if role is Cashier
                    Form2 cashierPortal = new Form2();
                    cashierPortal.Show();
                    this.Hide();
                }
                else if (role == "Customer")
                {
                    // Redirect to Customer Portal
                    FormCustomerPortal customerPortal = new FormCustomerPortal();
                    this.Hide();  // Hide the login form
                    customerPortal.Show();  // Show the Customer Portal

                }
                else
                {
                    // Display an error message if login fails
                    MessageBox.Show("Invalid Username or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method to retrieve the user's role from the database
        private string GetUserRole(string username, string password)
        {
            string role = null;

            // Use the connection string from Server Explorer
            string connectionString = "Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        role = result.ToString(); // Get the user's role
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return role;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}
