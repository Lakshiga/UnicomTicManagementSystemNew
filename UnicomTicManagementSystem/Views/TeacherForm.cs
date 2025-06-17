// TeacherForm.cs
using UnicomTicManagementSystem.Controllers;
using UnicomTicManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace UnicomTicManagementSystem.Views
{
    public partial class TeacherForm : Form
    {
        private TeacherController _controller = new TeacherController();
        private int selectedTeacherId = -1;

        public TeacherForm()
        {
            InitializeComponent();
            LoadTeachers();
            LoadSections();
        }

        private void LoadTeachers()
        {
            dgvTeachers.DataSource = _controller.GetAllTeachers();
            dgvTeachers.ClearSelection();
        }

        private void LoadSections()
        {
            comboSection.DataSource = _controller.GetAllSections();
            comboSection.DisplayMember = "Name";
            comboSection.ValueMember = "Id";
        }

        private void ClearInputs()
        {
            txtName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            comboSection.SelectedIndex = -1;
            selectedTeacherId = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Teacher teacher = new Teacher
            {
                Name = txtName.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text
            };

            int teacherId = _controller.AddTeacherWithReturnId(teacher);

            int sectionId = Convert.ToInt32(comboSection.SelectedValue);
            _controller.AssignSectionToTeacher(teacherId, sectionId);

            if (!string.IsNullOrWhiteSpace(txtUsername.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                _controller.AddUser(txtUsername.Text, txtPassword.Text, "Lecture");
            }

            LoadTeachers();
            ClearInputs();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedTeacherId != -1)
            {
                Teacher teacher = new Teacher
                {
                    Id = selectedTeacherId,
                    Name = txtName.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text
                };

                _controller.UpdateTeacher(teacher);

                int sectionId = Convert.ToInt32(comboSection.SelectedValue);
                _controller.AssignSectionToTeacher(selectedTeacherId, sectionId);

                LoadTeachers();
                ClearInputs();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedTeacherId != -1)
            {
                _controller.DeleteTeacher(selectedTeacherId);
                LoadTeachers();
                ClearInputs();
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            dgvTeachers.DataSource = _controller.SearchTeachers(keyword);
        }

        private void dgvTeachers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTeachers.SelectedRows.Count > 0)
            {
                var row = dgvTeachers.SelectedRows[0];
                var teacher = row.DataBoundItem as Teacher;
                if (teacher != null)
                {
                    selectedTeacherId = teacher.Id;
                    txtName.Text = teacher.Name;
                    txtPhone.Text = teacher.Phone;
                    txtAddress.Text = teacher.Address;
                }
            }
        }
    }
}
