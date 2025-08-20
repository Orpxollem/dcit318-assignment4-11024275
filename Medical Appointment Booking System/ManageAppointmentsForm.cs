using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Medical_Appointment_Booking_System
{
    public partial class ManageAppointmentsForm : Form
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["MedicalDBConn"].ConnectionString;
        private int selectedAppointmentId = -1;

        public ManageAppointmentsForm()
        {
            InitializeComponent();
        }

        // Form Load
        private void ManageAppointmentsForm_Load(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT A.AppointmentID, D.FullName AS Doctor, 
                             P.FullName AS Patient, A.AppointmentDate, A.Notes
                      FROM Appointments A
                      JOIN Doctors D ON A.DoctorID = D.DoctorID
                      JOIN Patients P ON A.PatientID = P.PatientID", con))
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    dgvAppointments.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message);
            }
        }

        // Capture selected row
        private void dgvAppointments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                selectedAppointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["AppointmentID"].Value);
            }
        }

        // Update appointment
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentId == -1)
            {
                MessageBox.Show("Please select an appointment to update.");
                return;
            }

            DateTime newDate = DateTime.Now.AddDays(1); // Example: move to tomorrow
            string newNotes = "Updated notes";          // In real app: open dialog to edit

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Appointments SET AppointmentDate=@dt, Notes=@n WHERE AppointmentID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@dt", newDate);
                    cmd.Parameters.AddWithValue("@n", newNotes);
                    cmd.Parameters.AddWithValue("@id", selectedAppointmentId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Appointment updated!");
                    LoadAppointments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating appointment: " + ex.Message);
            }
        }

        // Delete appointment
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentId == -1)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this appointment?",
                "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Appointments WHERE AppointmentID=@id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedAppointmentId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Appointment deleted!");
                        LoadAppointments();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting appointment: " + ex.Message);
                }
            }
        }
    }
}
