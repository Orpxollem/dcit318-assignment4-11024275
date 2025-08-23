using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pharmacy_Inventory_Management
{
    public partial class MainForm : Form
    {
        string connectionString;

        public MainForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;
        }

        
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadMedicines();
        }

        
        private void LoadMedicines(string search = "")
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT MedicineID, Name, Category, Price, Quantity FROM Medicines";

                if (!string.IsNullOrEmpty(search))
                {
                    query += " WHERE Name LIKE @search OR Category LIKE @search";
                }

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                if (!string.IsNullOrEmpty(search))
                    da.SelectCommand.Parameters.AddWithValue("@search", "%" + search + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMedicines.DataSource = dt;
            }
        }

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Medicines (Name, Category, Price, Quantity) VALUES (@Name, @Category, @Price, @Quantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Category", txtCategory.Text);
                cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Medicine Added Successfully!");
                LoadMedicines();
            }
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMedicines(txtSearch.Text);
        }

        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineID"].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Medicines SET Name=@Name, Category=@Category, Price=@Price, Quantity=@Quantity WHERE MedicineID=@MedicineID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MedicineID", id);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Category", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Medicine Updated Successfully!");
                    LoadMedicines();
                }
            }
            else
            {
                MessageBox.Show("Please select a medicine to update.");
            }
        }

        
        private void btnRecordSale_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineID"].Value);
                int qtyToReduce = int.Parse(txtQuantity.Text);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Medicines SET Quantity = Quantity - @qty WHERE MedicineID=@MedicineID AND Quantity >= @qty";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MedicineID", id);
                    cmd.Parameters.AddWithValue("@qty", qtyToReduce);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        MessageBox.Show("Sale Recorded Successfully!");
                    else
                        MessageBox.Show("Not enough stock!");

                    LoadMedicines();
                }
            }
            else
            {
                MessageBox.Show("Please select a medicine to record sale.");
            }
        }

        
        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadMedicines();
        }

        
        private void dgvMedicines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvMedicines.Rows[e.RowIndex].Cells["MedicineID"].Value != null)
            {
                DataGridViewRow row = dgvMedicines.Rows[e.RowIndex];
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtCategory.Text = row.Cells["Category"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value.ToString();
            }
        }

        
        private void txtCategory_TextChanged(object sender, EventArgs e) { }
        private void txtPrice_TextChanged(object sender, EventArgs e) { }
        private void txtQuantity_TextChanged(object sender, EventArgs e) { }
        private void txtSearch_TextChanged(object sender, EventArgs e) { }
        private void txtName_TextChanged(object sender, EventArgs e) { }
        
    }
}
