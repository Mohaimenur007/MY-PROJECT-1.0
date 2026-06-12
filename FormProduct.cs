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
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            string query = "SELECT ProductId, ProductName, Price, SupplierId, Quantity FROM Products";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProductData.DataSource = dt; // Assuming dgvProductData is the name of your DataGridView
            }
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            decimal price = Convert.ToDecimal(txtPrice.Text);
            int supplierId = Convert.ToInt32(txtSupplierId.Text);
            int quantity = Convert.ToInt32(txtQuantity.Text);

            // Validate the fields
            if (string.IsNullOrEmpty(productName) || price <= 0 || supplierId <= 0 || quantity <= 0)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insert the new product into the Products table
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                string query = "INSERT INTO Products (ProductName, Price, SupplierId, Quantity) VALUES (@ProductName, @Price, @SupplierId, @Quantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (dgvProductData.SelectedRows.Count > 0)
            {
                // Get the ProductId from the selected row
                int productId = Convert.ToInt32(dgvProductData.SelectedRows[0].Cells["ProductId"].Value);

                // Get the updated data from the textboxes
                string productName = txtProductName.Text;
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int supplierId = Convert.ToInt32(txtSupplierId.Text);
                int quantity = Convert.ToInt32(txtQuantity.Text);

                // Validate the fields
                if (string.IsNullOrEmpty(productName) || price <= 0 || supplierId <= 0 || quantity <= 0)
                {
                    MessageBox.Show("Please fill in all fields correctly.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update the product data in the Products table
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                {
                    string query = "UPDATE Products SET ProductName = @ProductName, Price = @Price, SupplierId = @SupplierId, Quantity = @Quantity WHERE ProductId = @ProductId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@ProductId", productId); // Specify the ProductId to identify which record to update

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery(); // Execute the query to update the product data
                        MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // Ensure that a row is selected
            if (dgvProductData.SelectedRows.Count > 0)
            {
                // Get the ProductId from the selected row
                int productId = Convert.ToInt32(dgvProductData.SelectedRows[0].Cells["ProductId"].Value);

                // Confirm before deletion
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    // Delete the product from the Products table
                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                    {
                        string query = "DELETE FROM Products WHERE ProductId = @ProductId";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@ProductId", productId);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery(); // Execute the query to delete the product
                            MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();  // Reload data after deleting
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

        private void button5_Click(object sender, EventArgs e)
        {
            txtProductName.Clear();
            txtPrice.Clear();
            txtSupplierId.Clear();
            txtQuantity.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();  
            Form4 form4 = new Form4();  
            form4.Show();  
        }

        private void dgvProductData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a row is selected
            if (dgvProductData.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgvProductData.SelectedRows[0];

                // Populate the textboxes with data from the selected row
                txtProductName.Text = selectedRow.Cells["ProductName"].Value.ToString();
                txtPrice.Text = selectedRow.Cells["Price"].Value.ToString();
                txtSupplierId.Text = selectedRow.Cells["SupplierId"].Value.ToString();
                txtQuantity.Text = selectedRow.Cells["Quantity"].Value.ToString();
            }
            else
            {
                // Clear textboxes if no row is selected
                txtProductName.Clear();
                txtPrice.Clear();
                txtSupplierId.Clear();
                txtQuantity.Clear();
            }
        }
    }
}
