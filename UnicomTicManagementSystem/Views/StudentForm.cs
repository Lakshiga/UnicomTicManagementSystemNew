using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Controllers;
using UnicomTicManagementSystem.Repositories;
using UnicomTicManagementSystem.Services;

namespace UnicomTicManagementSystem.Views
{
    public partial class StudentForm : Form
    {
        private readonly StudentController _studentController;
        private readonly SectionController _sectionController;
        private int selectedStudentId = -1;

        private string typedUsername = "";
        private string typedPassword = "";

        public StudentForm()
        {
            InitializeComponent();
            _studentController = new StudentController();
            _sectionController = new SectionController();

            LoadStudents();
            LoadSections();
        }

        private void LoadStudents()
        {
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = _studentController.GetAllStudents();

            if (dgvStudents.Columns.Contains("SectionId"))
            {
                dgvStudents.Columns["SectionId"].Visible = false;
                dgvStudents.Columns["Username"].Visible = false;
                dgvStudents.Columns["Password"].Visible = false;
                dgvStudents.Columns["Stream"].Visible = false;
                dgvStudents.Columns["UserId"].Visible = false;
            }

            dgvStudents.ClearSelection();
        }

        private void LoadSections()
        {
            var sections = _sectionController.GetAllSections();
            cmbSection.DataSource = sections;
            cmbSection.DisplayMember = "Name";
            cmbSection.ValueMember = "Id";
        }

        private void ClearForm()
        {
            name.Clear();
            address.Clear();
            username.Clear(); // Clear username textbox
            password.Clear(); // Clear password textbox
            cmbSection.SelectedIndex = -1;
            selectedStudentId = -1;
        }

        private void ClearInputs()
        {
            name.Text = "";
            address.Text = "";
            username.Text = "";
            password.Text = "";
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var row = dgvStudents.SelectedRows[0];
                var studentView = row.DataBoundItem as Student;

                if (studentView != null)
                {
                    selectedStudentId = studentView.Id;

                    var student = _studentController.GetStudentById(selectedStudentId);
                    if (student != null)
                    {
                        name.Text = student.Name;
                        address.Text = student.Address;
                        cmbSection.SelectedValue = student.SectionId;
                    }
                }
            }
            else
            {
                ClearInputs();
                selectedStudentId = -1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedStudentId == -1)
            {
                MessageBox.Show("Please select a student to delete.");
                return;
            }

            var confirmResult = MessageBox.Show("Are you sure to delete this student?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                _studentController.DeleteStudent(selectedStudentId);
                LoadStudents();
                ClearForm();
                MessageBox.Show("Student Deleted Successfully");
            }
        }

        private void update_Click(object sender, EventArgs e)
        {
            if (selectedStudentId == -1)
            {
                MessageBox.Show("Please select a student to update.");
                return;
            }

            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                MessageBox.Show("Please enter both Name and Address.");
                return;
            }

            var student = new Student
            {
                Id = selectedStudentId,
                Name = name.Text,
                Address = address.Text,
                SectionId = (int)cmbSection.SelectedValue
            };

            _studentController.UpdateStudent(student, username.Text.Trim(), password.Text.Trim());
            LoadStudents();
            ClearForm();
            MessageBox.Show("Student Updated Successfully");
        }

        private void add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrWhiteSpace(address.Text))
            {
                MessageBox.Show("Please enter both Name and Address.");
                return;
            }

            if (cmbSection.SelectedValue == null)
            {
                MessageBox.Show("Please select a section.");
                return;
            }

            // Check for username and password
            if (string.IsNullOrWhiteSpace(typedUsername) || string.IsNullOrWhiteSpace(typedPassword))
            {
                MessageBox.Show("Please enter a valid Username and Password.");
                return;
            }

            // Check if user already exists
            if (UserRepository.UserExists(typedUsername))
            {
                MessageBox.Show("Username already exists. Please choose another.");
                return;
            }

            // Add student data
            var student = new Student
            {
                Name = name.Text,
                Address = address.Text,
                SectionId = (int)cmbSection.SelectedValue
            };

            _studentController.AddStudent(student, typedUsername, typedPassword);


            // Add user credentials to repository
            UserRepository.AddUser(new User
            {
                Username = typedUsername,
                Password = typedPassword,
                Role = "student"
            });

            LoadStudents();
            ClearForm();
            MessageBox.Show("Student Added Successfully");
        }

        private void username_TextChanged(object sender, EventArgs e)
        {
            typedUsername = username.Text.Trim();
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            typedPassword = password.Text.Trim();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void StudentForm_Load(object sender, EventArgs e)
        {

        }
    }
}
