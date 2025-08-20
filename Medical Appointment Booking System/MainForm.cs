using System;
using System.Windows.Forms;

namespace Medical_Appointment_Booking_System
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
  
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            using (var f = new AppointmentForm())
                f.ShowDialog();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            using (var f = new ManageAppointmentsForm())
                f.ShowDialog();
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            using (var f = new DoctorListForm())
                f.ShowDialog();
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
