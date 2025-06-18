using System;
using System.Data;
using System.Windows.Forms;
using UnicomTicManagementSystem.Controllers;

namespace UnicomTicManagementSystem.Views
{
    public partial class TimeTableForm : Form
    {
        private TimetableController controller = new TimetableController();
        private int selectedTimetableId = -1;
        private string userRole;

        public TimeTableForm(string role = "Admin")
        {
            InitializeComponent();
            userRole = role;
            LoadSubjects();
        }

        private void TimeTableForm_Load(object sender, EventArgs e)
        {
            LoadTimetableData();
            LoadSubjects();
            LoadRooms();
            ApplyRolePermissions();
        }

        private void LoadSubjects()
        {
            comboSubject.Items.Clear();
            DataTable dt = controller.GetSubjects();
            foreach (DataRow row in dt.Rows)
            {
                comboSubject.Items.Add(row["SubjectName"].ToString());
            }
        }

        private void LoadRooms()
        {
            comboRoom.Items.Clear();
            DataTable dt = controller.GetRooms();
            foreach (DataRow row in dt.Rows)
            {
                comboRoom.Items.Add(row["RoomName"].ToString());
            }
        }

        private void LoadTimetableData()
        {
            dataGridView1.DataSource = controller.GetAllTimetables();
        }

        private void ClearForm()
        {
            comboSubject.SelectedIndex = -1;
            txtTimeSlot.Clear();
            textBox1.Clear();
            selectedTimetableId = -1;
            datePicker.Value = DateTime.Today;
            comboRoom.Items.Clear();
            LoadRooms();
        }

        private void ApplyRolePermissions()
        {
            if (userRole.ToLower() == "lecture" || userRole.ToLower() == "staff")
            {
                // Hide all controls except the DataGridView
                comboSubject.Visible = false;
                comboRoom.Visible = false;
                txtTimeSlot.Visible = false;
                textBox1.Visible = false;
                datePicker.Visible = false;
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnPickDate.Visible = false;

                // Also hide associated labels (adjust label names based on your form)
                label5.Visible = false;
                label3.Visible = false;
                label2.Visible = false;
                label4.Visible = false;
                label6.Visible = false;

                // Expand the DataGridView to fill the form
                dataGridView1.Dock = DockStyle.Fill;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (comboSubject.SelectedItem == null || string.IsNullOrWhiteSpace(txtTimeSlot.Text) || comboRoom.SelectedItem == null)
            {
                MessageBox.Show("Please enter all fields: Subject, TimeSlot, Room, and Date.");
                return;
            }

            DateTime selectedDate = datePicker.Value;

            controller.AddTimetable(
                comboSubject.SelectedItem.ToString(),
                txtTimeSlot.Text.Trim(),
                comboRoom.SelectedItem.ToString(),
                selectedDate
            );

            MessageBox.Show("Timetable entry added.");
            LoadTimetableData();
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedTimetableId == -1)
            {
                MessageBox.Show("Select a record to update.");
                return;
            }

            DateTime selectedDate = datePicker.Value;

            controller.UpdateTimetable(
                selectedTimetableId,
                comboSubject.SelectedItem.ToString(),
                txtTimeSlot.Text.Trim(),
                comboRoom.SelectedItem.ToString(),
                selectedDate
            );

            MessageBox.Show("Timetable updated.");
            LoadTimetableData();
            ClearForm();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedTimetableId == -1)
            {
                MessageBox.Show("Select a record to delete.");
                return;
            }

            controller.DeleteTimetable(selectedTimetableId);

            MessageBox.Show("Timetable deleted.");
            LoadTimetableData();
            ClearForm();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells["Id"].Value != null)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedTimetableId = Convert.ToInt32(row.Cells["Id"].Value);
                comboSubject.SelectedItem = row.Cells["Subject"].Value.ToString();
                txtTimeSlot.Text = row.Cells["TimeSlot"].Value.ToString();
                comboRoom.Text = row.Cells["Room"].Value.ToString();
                textBox1.Text = Convert.ToDateTime(row.Cells["Date"].Value).ToString("yyyy-MM-dd");
                datePicker.Value = Convert.ToDateTime(row.Cells["Date"].Value);
            }
        }

        private void btnPickDate_Click(object sender, EventArgs e)
        {
            datePicker.Visible = true;
            datePicker.BringToFront();
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = datePicker.Value.ToString("yyyy-MM-dd");
            datePicker.Visible = false;
        }
    }
}
