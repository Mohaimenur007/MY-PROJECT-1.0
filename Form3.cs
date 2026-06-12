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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true; // Hide password
            txtConfirmPassword.UseSystemPasswordChar = true; // Hide password
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (chkShowPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false; // Show password
                txtConfirmPassword.UseSystemPasswordChar = false; // Show password
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true; // Hide password
                txtConfirmPassword.UseSystemPasswordChar = true; // Hide password
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string role = cmbRole.SelectedItem.ToString(); // Get selected role (Admin or Cashier)

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Call the method to register the user
            bool isRegistered = RegisterUser(name, username, password, role);

            if (isRegistered)
            {
                MessageBox.Show("Registration Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // After successful registration, redirect to login page
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Registration Failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to register the user in the database
        private bool RegisterUser(string name, string username, string password, string role)
        {
            bool isRegistered = false;

            // Connection string
            string connectionString = "Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Name, Username, Password, Role) VALUES (@Name, @Username, @Password, @Role)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", role);

                try
                {
                    con.Open();
                    int result = cmd.ExecuteNonQuery(); // Execute the insert query

                    if (result > 0) // Check if the user is successfully added
                    {
                        isRegistered = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return isRegistered;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
