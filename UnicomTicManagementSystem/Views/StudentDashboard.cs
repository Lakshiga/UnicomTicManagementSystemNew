using System;
using System.Windows.Forms;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Repositories;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UnicomTicManagementSystem.Views
{
    public partial class StudentDashboard : Form
    {

        private string username;
        private Student student;
                
        public StudentDashboard()
        {
            InitializeComponent();
        }
        public StudentDashboard(string username)
        {
            InitializeComponent();
            this.username = username;

            Student student = StudentRepository.GetStudentByUsername(username);

            if (student != null)
            {
                lblName.Text = student.Name;
                lblUsername.Text = username;
                lblPassword.Text = student.Password;
                lblAddress.Text = student.Address;
                lblStream.Text = student.SectionName; 
            }

        }          

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void lblStream_Click(object sender, EventArgs e)
        {

        }
    }
}
