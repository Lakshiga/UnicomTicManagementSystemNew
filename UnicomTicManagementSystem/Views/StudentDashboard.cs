using System;
using System.Windows.Forms;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Repositories;

namespace UnicomTicManagementSystem.Views
{
    public partial class StudentDashboard : Form
    {
        private string username;
        private Student student;

        public StudentDashboard(string username)
        {
            InitializeComponent();
            this.username = username;

            // ✅ Correctly assign to class-level student variable
            student = StudentRepository.GetStudentByUsername(username);

            if (student != null)
            {
                lblName.Text = student.Name;
                lblUsername.Text = username;
                lblPassword.Text = student.Password;
                lblAddress.Text = student.Address;
                lblStream.Text = student.SectionName;

                // ✅ Get subject list and display
                var subjectList = StudentRepository.GetSubjectsBySectionName(student.SectionName);
                lblSubject.Text = string.Join(", ", subjectList);
            }
            else
            {
                MessageBox.Show("Student not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimetable_Click(object sender, EventArgs e)
        {
            if (student != null)
            {
                dataGridView1.DataSource = StudentRepository.GetTimetableBySection(student.SectionName);
            }
        }

        private void btnExamMarks_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(username))
            {
                dataGridView1.DataSource = StudentRepository.GetExamMarksByUsername(username);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void lblName_Click(object sender, EventArgs e) { }

        private void lblStream_Click(object sender, EventArgs e) { }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            ResetPasswordForm resetForm = new ResetPasswordForm(username);
            resetForm.ShowDialog();
        }

    }
}
