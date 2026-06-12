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
    public partial class FormCustomerPortal : Form
    {
        private DataTable orderTable;
        private decimal totalAmount = 0;
        public FormCustomerPortal()
        {
            InitializeComponent();
            orderTable = new DataTable();
            InitializeOrderTable();
        }
        private void InitializeOrderTable()
        {
            orderTable.Columns.Add("ProductId");
            orderTable.Columns.Add("ProductName");
            orderTable.Columns.Add("Price");
            orderTable.Columns.Add("Quantity");
            orderTable.Columns.Add("Total");
            dgvOrder.DataSource = orderTable;
        }
        private void LoadProductData()
        {
            string query = "SELECT ProductId, ProductName, Price, Quantity FROM [Users].[dbo].[Products] WHERE Quantity > 0";

            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProductList.DataSource = dt;
            }
        }
        private void FormCustomerPortal_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProductId.Text))
            {
                string query = "SELECT ProductName, Price FROM [Users].[dbo].[Products] WHERE ProductId = @ProductId";
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-F98MDEP\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;"))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProductId", txtProductId.Text);
                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtProductName.Text = reader["ProductName"].ToString();
                            txtprice.Text = reader["Price"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductId.Text) || string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Please select a product and enter quantity.");
                return;
            }

            int productId = Convert.ToInt32(txtProductId.Text);
            string productName = txtProductName.Text;
            decimal price = Convert.ToDecimal(txtprice.Text);
            int quantity = Convert.ToInt32(txtQuantity.Text);

            decimal total = price * quantity;

            // Add product to the order table
            orderTable.Rows.Add(productId, productName, price, quantity, total);
            totalAmount += total;
            txtTotal.Text = totalAmount.ToString("C");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dgvOrder.SelectedRows[0];

                // Calculate the total value for this row
                decimal total = Convert.ToDecimal(selectedRow.Cells["Total"].Value);
                totalAmount -= total;

                // Remove the selected row from the DataTable
                orderTable.Rows.Remove((selectedRow.DataBoundItem as DataRowView).Row);

                // Update the TotalAmount label
                txtTotal.Text = totalAmount.ToString("C");
            }
            else
            {
                // If no row is selected, show the error message
                MessageBox.Show("Please select an item to remove.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtprice.Clear();
            txtQuantity.Clear();
            txtTotal.Clear();
            txtAmountPaid.Clear();
            
            lblChange.Text = "0.00";       // Reset change label

            // Clear order table
            orderTable.Clear();  // Clear any existing order data in DataGridView
            totalAmount = 0;     // 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            decimal amountPaid = Convert.ToDecimal(txtAmountPaid.Text);
            decimal change = amountPaid - totalAmount;

            if (change >= 0)
            {
                lblChange.Text = change.ToString("C");
                MessageBox.Show("Payment successful! Change: " + change.ToString("C"));
                PrintReceipt();
            }
            else
            {
                MessageBox.Show("Insufficient amount. Please enter the correct amount.");
            }

        }
        private void PrintReceipt()
        {
            string receipt = "Receipt\n";
            receipt += "---------------------------------\n";
            foreach (DataRow row in orderTable.Rows)
            {
                receipt += $"Product: {row["ProductName"]}, Quantity: {row["Quantity"]}, Total: {row["Total"]}\n";
            }
            receipt += "---------------------------------\n";
            receipt += $"Total Amount: {totalAmount:C}\n";
            receipt += $"Amount Paid: {txtAmountPaid.Text}\n";
            receipt += $"Change: {lblChange.Text}\n";
            MessageBox.Show(receipt, "Receipt");
        }

        private void dgvProductList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row (DataRowView)
                DataGridViewRow selectedRow = dgvProductList.Rows[e.RowIndex];

                // Convert DataRowView to DataRow
                DataRow row = (selectedRow.DataBoundItem as DataRowView).Row;

                // Get the selected product details
                txtProductId.Text = row["ProductId"].ToString();
                txtProductName.Text = row["ProductName"].ToString();
                txtprice.Text = row["Price"].ToString();
            }
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row (DataRowView)
                DataGridViewRow selectedRow = dgvOrder.Rows[e.RowIndex];

                // Convert DataRowView to DataRow
                DataRow row = (selectedRow.DataBoundItem as DataRowView).Row;

                // Get the selected order details and display them in textboxes
                txtProductName.Text = row["ProductName"].ToString();
                txtQuantity.Text = row["Quantity"].ToString();
                txtTotal.Text = row["Total"].ToString();

               
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadProductData();
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
