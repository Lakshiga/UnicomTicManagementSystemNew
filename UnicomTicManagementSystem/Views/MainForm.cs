using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTicManagementSystem.Data;

namespace UnicomTicManagementSystem.Views
{
    public partial class MainForm : Form
    {
        private string userRole;

        public MainForm()
        {
            InitializeComponent();
            LoadDashboardCounts();
        }

        public MainForm(string role) : this()
        {
            userRole = role;
            lblWelcome.Text = $"Welcome to {userRole} Dashboard";
            ApplyRoleAccess();
        }

        private void ApplyRoleAccess()
        {
            // Clear flowSidebar first (FlowLayoutPanel inside panel1)
            flowSidebar.Controls.Clear();


            flowSidebar.Controls.Add(lblWelcome);

            // Add buttons according to role
            if (userRole == "Admin")
            {
                flowSidebar.Controls.Add(button1); // Student
                flowSidebar.Controls.Add(button2); // Lectures
                flowSidebar.Controls.Add(button3); // Section
                flowSidebar.Controls.Add(button4); // Subject
                flowSidebar.Controls.Add(button5); // Staff
                flowSidebar.Controls.Add(button6); // Timetable
                flowSidebar.Controls.Add(button8); // Exam
                flowSidebar.Controls.Add(button9); // Room
            }
            else if (userRole.ToLower() == "staff")
            {
                flowSidebar.Controls.Add(button6); // Timetable
                flowSidebar.Controls.Add(button7); // Marks
                flowSidebar.Controls.Add(button8); // Exam
            }
            else if (userRole.ToLower() == "lecture")
            {
                flowSidebar.Controls.Add(button6); // Timetable
                flowSidebar.Controls.Add(button7); // Marks
            }

            // Always add Logout at the bottom
            flowSidebar.Controls.Add(btnlogout);
        }

        private void LoadFormInPanel(Form form)
        {
            panel2.Controls.Clear();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panel2.Controls.Add(form);
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new StudentForm());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new TeacherForm());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new SectionForm());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new SubjectForm());
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            LoadFormInPanel(new StaffForm());
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            LoadFormInPanel(new TimeTableForm());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new MarkForm());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new ExamForm());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new RoomForm());
        }

        private void btnlogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void LoadDashboardCounts()
        {
            using (var conn = DbCon.GetConnection())
            {
                var cmd1 = new SQLiteCommand("SELECT COUNT(*) FROM Students", conn);
                lblTotalStudents.Text = "TOTAL \nSTUDENTS:\n⚫" + cmd1.ExecuteScalar().ToString();

                var cmd2 = new SQLiteCommand("SELECT COUNT(*) FROM Teachers", conn);
                lblTotalLectures.Text = "TOTAL \nLECTURES:\n⚫" + cmd2.ExecuteScalar().ToString();

                var cmd3 = new SQLiteCommand("SELECT COUNT(*) FROM Sections", conn);
                lblTotalCourses.Text = "TOTAL \nCOURSES:\n⚫" + cmd3.ExecuteScalar().ToString();

                var cmd4 = new SQLiteCommand("SELECT COUNT(*) FROM Subjects", conn);
                lblTotalSubjects.Text = "TOTAL \nSUBJECTS:\n⚫" + cmd4.ExecuteScalar().ToString();
            }
        }

        private void label2_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }
    }
}