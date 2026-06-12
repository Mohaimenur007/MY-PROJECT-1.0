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
    public partial class FormSupplier : Form
    {
        public FormSupplier()
        {
            InitializeComponent();
        }

        private void LoadSupplierData()
        {
            string query = "SELECT SupplierId, SName, Address, Contact FROM [Users].[dbo].[Suppliers]";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSupplier.DataSource = dt; // Assuming dgvSupplierData is the name of your DataGridView for suppliers
            }
        }
        private void FormSupplier_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string supplierName = txtSupplierName.Text;
            string address = txtAddress.Text;
            string contact = txtContact.Text;

            // Validate the fields
            if (string.IsNullOrEmpty(supplierName) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(contact))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insert the new supplier into the Suppliers table
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                string query = "INSERT INTO [Users].[dbo].[Suppliers] (SName, Address, Contact) VALUES (@SName, @Address, @Contact)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SName", supplierName);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Contact", contact);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSupplierData();  // Reload data after adding
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Add Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvSupplier.SelectedRows.Count > 0)
            {
                int supplierId = Convert.ToInt32(dgvSupplier.SelectedRows[0].Cells["SupplierId"].Value);

                string supplierName = txtSupplierName.Text;
                string address = txtAddress.Text;
                string contact = txtContact.Text;

                // Validate the fields
                if (string.IsNullOrEmpty(supplierName) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(contact))
                {
                    MessageBox.Show("Please fill in all fields correctly.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update the supplier data in the Suppliers table
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                {
                    string query = "UPDATE [Users].[dbo].[Suppliers] SET SName = @SName, Address = @Address, Contact = @Contact WHERE SupplierId = @SupplierId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SName", supplierName);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@SupplierId", supplierId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSupplierData();  // Reload data after updating
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
            if (dgvSupplier.SelectedRows.Count > 0)
            {
                int supplierId = Convert.ToInt32(dgvSupplier.SelectedRows[0].Cells["SupplierId"].Value);

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                    {
                        string query = "DELETE FROM [Users].[dbo].[Suppliers] WHERE SupplierId = @SupplierId";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@SupplierId", supplierId);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Supplier deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadSupplierData();  // Reload data after deleting
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
            txtSupplierName.Clear();
            txtAddress.Clear();
            txtContact.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void dgvSupplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSupplier.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvSupplier.SelectedRows[0];

                txtSupplierName.Text = selectedRow.Cells["SName"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value.ToString();
                txtContact.Text = selectedRow.Cells["Contact"].Value.ToString();
            }
            else
            {
                txtSupplierName.Clear();
                txtAddress.Clear();
                txtContact.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadSupplierData();
        }
    }
}
