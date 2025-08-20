using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pharmacy_Inventory_Management
{
    public partial class ViewMedicinesForm : Form
    {
        private string connectionString;

        public ViewMedicinesForm()
        {
            InitializeComponent();
            connectionString = System.Configuration.ConfigurationManager
                               .ConnectionStrings["PharmacyDb"]
                               .ConnectionString;
            LoadMedicines();
        }

        private void ViewMedicinesForm_Load(object sender, EventArgs e)
        {
            LoadMedicines();
        }

        private void LoadMedicines(string filter = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MedicineId, Name, Category, Price, Quantity FROM Medicines";

                if (!string.IsNullOrWhiteSpace(filter))
                    query += " WHERE Name LIKE @filter OR Category LIKE @filter";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        cmd.Parameters.AddWithValue("@filter", "%" + filter + "%");

                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dgvMedicines.DataSource = dt;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMedicines(txtSearch.Text.Trim());
        }

        // original TextChanged handler (if wired)
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadMedicines(txtSearch.Text.Trim());
        }

        // Designer referenced duplicate handler: add this so Designer can load
        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            LoadMedicines(txtSearch.Text.Trim());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to update.");
                return;
            }

            int medicineId = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineId"].Value);
            string name = dgvMedicines.SelectedRows[0].Cells["Name"].Value.ToString();
            string category = dgvMedicines.SelectedRows[0].Cells["Category"].Value.ToString();
            decimal price = Convert.ToDecimal(dgvMedicines.SelectedRows[0].Cells["Price"].Value);
            int quantity = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["Quantity"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Medicines 
                                 SET Name=@Name, Category=@Category, Price=@Price, Quantity=@Quantity 
                                 WHERE MedicineId=@MedicineId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MedicineId", medicineId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Medicine updated successfully!");
            LoadMedicines();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMedicines.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            int medicineId = Convert.ToInt32(dgvMedicines.SelectedRows[0].Cells["MedicineId"].Value);

            var confirm = MessageBox.Show("Are you sure you want to delete this medicine?",
                                          "Confirm Delete",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Medicines WHERE MedicineId=@MedicineId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MedicineId", medicineId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Medicine deleted successfully!");
            LoadMedicines();
        }

        private void dgvMedicines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // reserved for optional click handling; Designer expects this method if wired
        }
    }
}
