using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Pharmacy_Inventory_System
{
    public partial class AddMedicineForm : Form
    {
        public AddMedicineForm()
        {
            InitializeComponent();
        }

        private void AddMedicineForm_Load(object sender, EventArgs e)
        {
            this.Text = "Add Medicine";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                string category = txtCategory.Text.Trim();
                decimal price = decimal.Parse(txtPrice.Text.Trim());
                int quantity = int.Parse(txtQuantity.Text.Trim());
                
                string connStr = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Medicines (Name, Category, Price, Quantity) VALUES (@n, @c, @p, @q)", conn);

                    cmd.Parameters.AddWithValue("@n", name);
                    cmd.Parameters.AddWithValue("@c", category);
                    cmd.Parameters.AddWithValue("@p", price);
                    cmd.Parameters.AddWithValue("@q", quantity);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtName.Clear();
                    txtCategory.Clear();
                    txtPrice.Clear();
                    txtQuantity.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCategory_TextChanged(object sender, EventArgs e) { }
        private void txtName_TextChanged(object sender, EventArgs e) { }
        private void txtPrice_TextChanged(object sender, EventArgs e) { }
        private void txtQuantity_TextChanged(object sender, EventArgs e) { }
        private void lblName_Click(object sender, EventArgs e) { }
        private void lblCategory_Click(object sender, EventArgs e) { }
        private void lblPrice_Click(object sender, EventArgs e) { }
        private void lblQuantity_Click(object sender, EventArgs e) { }
    }
}
