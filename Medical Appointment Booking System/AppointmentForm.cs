using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Medical_Appointment_Booking_System
{
    public partial class AppointmentForm : Form
    {

        private readonly string connStr = ConfigurationManager.ConnectionStrings["MedicalDBConn"].ConnectionString;

        public AppointmentForm()
        {
            InitializeComponent();
        }

        
        private void AppointmentForm_Load(object sender, EventArgs e)
        {
            LoadDoctors();
            LoadPatients();
            dtpDate.MinDate = DateTime.Now;
        }

        private void LoadDoctors()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("SELECT DoctorID, FullName FROM Doctors WHERE Availability = 1", con))
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    cmbDoctors.DisplayMember = "FullName";
                    cmbDoctors.ValueMember = "DoctorID";
                    cmbDoctors.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading doctors: " + ex.Message);
            }
        }

        private void LoadPatients()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("SELECT PatientID, FullName FROM Patients", con))
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    cmbPatients.DisplayMember = "FullName";
                    cmbPatients.ValueMember = "PatientID";
                    cmbPatients.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading patients: " + ex.Message);
            }
        }

        
        private void btnBook_Click(object sender, EventArgs e)
        {
            if (cmbDoctors.SelectedValue == null || cmbPatients.SelectedValue == null)
            {
                MessageBox.Show("Please select both a doctor and a patient.");
                return;
            }

            if (dtpDate.Value < DateTime.Now)
            {
                MessageBox.Show("Choose a future date/time.");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes) VALUES (@d, @p, @dt, @n)", con))
                {
                    cmd.Parameters.AddWithValue("@d", cmbDoctors.SelectedValue);
                    cmd.Parameters.AddWithValue("@p", cmbPatients.SelectedValue);
                    cmd.Parameters.AddWithValue("@dt", dtpDate.Value);
                    cmd.Parameters.AddWithValue("@n", txtNotes.Text.Trim());

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Appointment booked successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error booking appointment: " + ex.Message);
            }
        }
    }
}
