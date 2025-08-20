using Pharmacy_Inventory_Management;
using System;
using System.Windows.Forms;

namespace Pharmacy_Inventory_System
{
    public partial class MainForm : Form
    {
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = "Pharmacy Inventory System";
        }
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var f = new AddMedicineForm())
                f.ShowDialog();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            using (var f = new ViewMedicinesForm())
                f.ShowDialog();
        }
    }
}
