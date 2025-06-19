using System;
using System.Data;
using System.Windows.Forms;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Services;

namespace UnicomTicManagementSystem.Views
{
    public partial class AttendanceForm : Form
    {
        private AttendanceService _attendanceService = new AttendanceService();
        private int selectedAttendanceId = -1;
        private string userRole;

        public AttendanceForm(string role = "lecture")
        {
            InitializeComponent();
            userRole = role;
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            LoadSubjects();
            LoadStatusOptions();
            LoadAttendanceGrid();
            ApplyRolePermissions();
            datePicker.Visible = true;
            datePicker.Value = DateTime.Today;
        }

        private void LoadSubjects()
        {
            var dt = _attendanceService.GetAllSubjects();
            comboBoxSubject.DataSource = dt;
            comboBoxSubject.DisplayMember = "SubjectName";
            comboBoxSubject.ValueMember = "SubjectID";
            comboBoxSubject.SelectedIndex = -1;
        }


        private void LoadStatusOptions()
        {
            comboBoxStatus.Items.Clear();
            comboBoxStatus.Items.AddRange(new[] { "Present", "Absent", "Late", "Excused" });
            comboBoxStatus.SelectedIndex = 0;
        }

        private void LoadAttendanceGrid()
        {
            var dt = _attendanceService.GetAllAttendance();
            dataGridViewAttendance.DataSource = dt;

            if (dataGridViewAttendance.Columns["AttendanceID"] != null)
                dataGridViewAttendance.Columns["AttendanceID"].Visible = false;

            if (dataGridViewAttendance.Columns["SubjectID"] != null)
                dataGridViewAttendance.Columns["SubjectID"].Visible = false;
        }

        private void ApplyRolePermissions()
        {
            if (userRole.ToLower() == "admin" || userRole.ToLower() == "staff")
            {
                // Hide all controls except the DataGridView
                comboBoxSubject.Visible = false;
                comboBoxStatus.Visible = false;
                textBoxStudentID.Visible = false;
                textBoxStudentName.Visible = false;
                textBoxDate.Visible = false;
                textBox3.Visible = false;   
                datePicker.Visible = false;
                btnMarkAttendance.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnPickDate.Visible = false;
                btnSearch.Visible = false;

                labelStudentID.Visible = false;
                labelStudentName.Visible = false;
                label3.Visible = false;
                labelStatus.Visible = false;
                labelDate.Visible = false;
                label6.Visible = false;
                dataGridViewAttendance.Dock = DockStyle.Fill;

            }
        }

        private void textBoxStudentID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxStudentID.Text))
            {
                textBoxStudentName.Text = "";
                return;
            }

            var student = _attendanceService.GetStudentByID(textBoxStudentID.Text);
            if (student != null)
            {
                textBoxStudentName.Text = student.StudentName;
                var subjects = _attendanceService.GetSubjectsByStudentID(student.StudentID);
                if (subjects != null && subjects.Rows.Count > 0)
                {
                    comboBoxSubject.DataSource = subjects;
                    comboBoxSubject.DisplayMember = "SubjectName";
                    comboBoxSubject.ValueMember = "SubjectID";
                }
            }
            else
            {
                textBoxStudentName.Text = "Not found";
            }
        }

        private void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            if (comboBoxSubject.SelectedItem == null || comboBoxStatus.SelectedItem == null || string.IsNullOrWhiteSpace(textBoxStudentID.Text))
            {
                MessageBox.Show("Please enter Student ID, select Subject and Status.");
                return;
            }

            DateTime selectedDate = datePicker.Value;

            Attendance attendance = new Attendance
            {
                Date = selectedDate,
                StudentID = textBoxStudentID.Text.Trim(),
                SubjectID = comboBoxSubject.SelectedValue?.ToString(),
                Status = comboBoxStatus.SelectedItem.ToString()
            };

            _attendanceService.AddAttendance(attendance);
            MessageBox.Show("Attendance added successfully.");
            LoadAttendanceGrid();
            ClearForm();
        }

        private void dataGridViewAttendance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewAttendance.Rows[e.RowIndex].Cells["AttendanceID"].Value != null)
            {
                DataGridViewRow row = dataGridViewAttendance.Rows[e.RowIndex];
                selectedAttendanceId = Convert.ToInt32(row.Cells["AttendanceID"].Value);
                textBoxStudentID.Text = row.Cells["StudentID"].Value.ToString();
                textBoxStudentName.Text = row.Cells["StudentName"].Value.ToString();
                comboBoxSubject.SelectedValue = row.Cells["SubjectID"].Value.ToString();
                comboBoxStatus.SelectedItem = row.Cells["Status"].Value.ToString();
                datePicker.Value = Convert.ToDateTime(row.Cells["Date"].Value);
                textBoxDate.Text = datePicker.Value.ToString("yyyy-MM-dd");
            }
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            textBoxDate.Text = datePicker.Value.ToString("yyyy-MM-dd");
            datePicker.Visible = false;
        }

        private void ClearForm()
        {
            selectedAttendanceId = -1;
            textBoxStudentID.Clear();
            textBoxStudentName.Clear();
            comboBoxSubject.SelectedIndex = -1;
            comboBoxStatus.SelectedIndex = 0;
            textBoxDate.Clear();
            datePicker.Value = DateTime.Today;
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedAttendanceId == -1)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            _attendanceService.DeleteAttendance(selectedAttendanceId);
            MessageBox.Show("Attendance deleted successfully.");
            LoadAttendanceGrid();
            ClearForm();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (selectedAttendanceId == -1)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            DateTime selectedDate = datePicker.Value;

            Attendance attendance = new Attendance
            {
                AttendanceID = selectedAttendanceId,
                Date = selectedDate,
                StudentID = textBoxStudentID.Text.Trim(),
                SubjectID = comboBoxSubject.SelectedValue?.ToString(),
                Status = comboBoxStatus.SelectedItem.ToString()
            };

            _attendanceService.UpdateAttendance(attendance);
            MessageBox.Show("Attendance updated successfully.");
            LoadAttendanceGrid();
            ClearForm();
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            DateTime selectedDate = datePicker.Value;

            var filtered = _attendanceService.SearchAttendance(
                selectedDate,
                textBoxStudentID.Text.Trim(),
                textBoxStudentName.Text.Trim(),
                comboBoxSubject.Text.Trim(),
                comboBoxStatus.Text.Trim()
            );

            dataGridViewAttendance.DataSource = filtered;
        }

        private void btnPickDate_Click_1(object sender, EventArgs e)
        {
            datePicker.Visible = true;
            datePicker.BringToFront();
        }
    }
}
