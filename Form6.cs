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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            string query = "SELECT EmployeeId, EName, Password, Salary, Contact, Address FROM Employee";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvEmployeeData.DataSource = dt; // Assuming dgvEmployeeData is the name of your DataGridView
            }
        }
        private void Form6_Load(object sender, EventArgs e)
        {
           
        }

        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eName = txtEName.Text;
            string password = txtPassword.Text;
            decimal salary = Convert.ToDecimal(txtSalary.Text);
            string contact = txtContact.Text;
            string address = txtAddress.Text;

            // Validate the fields
            if (string.IsNullOrEmpty(eName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please fill in all fields.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insert the new employee into the Employee table
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                string query = "INSERT INTO Employee (EName, Password, Salary, Contact, Address) VALUES (@EName, @Password, @Salary, @Contact, @Address)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EName", eName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Salary", salary);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@Address", address);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // Ensure a row is selected
            if (dgvEmployeeData.SelectedRows.Count > 0)
            {
                int employeeId = Convert.ToInt32(dgvEmployeeData.SelectedRows[0].Cells["EmployeeId"].Value); // Get EmployeeId from selected row
                string eName = txtEName.Text;
                string password = txtPassword.Text;
                decimal salary = Convert.ToDecimal(txtSalary.Text);
                string contact = txtContact.Text;
                string address = txtAddress.Text;

                // Validate the fields
                if (string.IsNullOrEmpty(eName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Please fill in all fields.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update the employee data in the database
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                {
                    string query = "UPDATE Employee SET EName = @EName, Password = @Password, Salary = @Salary, Contact = @Contact, Address = @Address WHERE EmployeeId = @EmployeeId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EName", eName);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Salary", salary);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Employee information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button3_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dgvEmployeeData.SelectedRows.Count > 0)
            {
                int employeeId = Convert.ToInt32(dgvEmployeeData.SelectedRows[0].Cells["EmployeeId"].Value); // Get EmployeeId from selected row

                // Confirm before deletion
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this employee?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    // Delete the employee data from the database
                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                    {
                        string query = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Employee deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtEName.Clear();
            txtPassword.Clear();
            txtSalary.Clear();
            txtContact.Clear();
            txtAddress.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();  // Hide the current form
            Form4 form4 = new Form4();  // Open the Admin Portal form
            form4.Show();
        }

        private void dgvEmployeeData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a row is selected
            if (dgvEmployeeData.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgvEmployeeData.SelectedRows[0];

                // Populate the textboxes with data from the selected row
                txtEName.Text = selectedRow.Cells["EName"].Value.ToString();
                txtPassword.Text = selectedRow.Cells["Password"].Value.ToString();
                txtSalary.Text = selectedRow.Cells["Salary"].Value.ToString();
                txtContact.Text = selectedRow.Cells["Contact"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value.ToString();
            }
            else
            {
                // Clear textboxes if no row is selected
                txtEName.Clear();
                txtPassword.Clear();
                txtSalary.Clear();
                txtContact.Clear();
                txtAddress.Clear();
            }
        }
    }
}
