using System;
using System.Data;
using System.Windows.Forms;
using UnicomTicManagementSystem.Controllers;

namespace UnicomTicManagementSystem.Views
{
    public partial class MarkForm : Form
    {
        private MarkController controller = new MarkController();
        private int selectedMarkId = -1;
        private string userRole;

        public MarkForm(string role = "Lecture")
        {
            InitializeComponent();
            userRole = role.ToLower(); // normalize role for comparison
        }

        private void MarkForm_Load(object sender, EventArgs e)
        {
            ApplyRolePermissions();
            LoadExams();
            LoadMarks();
        }

        private void LoadExams()
        {
            comboExam.Items.Clear();
            var dt = controller.GetExams();
            foreach (DataRow row in dt.Rows)
            {
                comboExam.Items.Add(row["ExamName"].ToString());
            }
        }

        private void ApplyRolePermissions()
        {
            if (userRole == "admin")
            {
                // Hide input controls and buttons
                txtScore.Visible = false;
                comboExam.Visible = false;
                comboSubject.Visible = false;
                txtStudentID.Visible = false;
                txtStudentName.Visible = false;
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;

                // Hide labels
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;

                // Expand DataGridView
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.ReadOnly = true;
                dataGridView1.ClearSelection();
                dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
                dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
            }
        }

        private void LoadSubjects(int studentId)
        {
            comboSubject.Items.Clear();
            var dt = controller.GetSubjectsByStudent(studentId);
            foreach (DataRow row in dt.Rows)
            {
                comboSubject.Items.Add(row["SubjectName"].ToString());
            }
        }

        private void LoadMarks()
        {
            dataGridView1.DataSource = controller.GetAllMarks();
        }

        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtStudentID.Text, out int studentId))
            {
                txtStudentName.Text = controller.GetStudentName(studentId);
                LoadSubjects(studentId);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (userRole == "admin") return;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedMarkId = Convert.ToInt32(row.Cells["MarkID"].Value);
                txtStudentID.Text = row.Cells["StudentID"].Value.ToString();
                comboSubject.SelectedItem = row.Cells["Subject"].Value.ToString();
                comboExam.SelectedItem = row.Cells["Exam"].Value.ToString();
                txtScore.Text = row.Cells["Score"].Value.ToString();
            }
        }

        private void ClearForm()
        {
            txtStudentID.Clear();
            txtStudentName.Clear();
            comboSubject.SelectedIndex = -1;
            comboExam.SelectedIndex = -1;
            txtScore.Clear();
            selectedMarkId = -1;
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedMarkId == -1)
            {
                MessageBox.Show("Select a row to delete.");
                return;
            }

            controller.DeleteMark(selectedMarkId);
            MessageBox.Show("Deleted.");
            LoadMarks();
            ClearForm();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (selectedMarkId == -1)
            {
                MessageBox.Show("Select a row to update.");
                return;
            }

            if (!int.TryParse(txtStudentID.Text, out int studentId) || !int.TryParse(txtScore.Text, out int score))
            {
                MessageBox.Show("Invalid Student ID or Score.");
                return;
            }

            controller.UpdateMark(selectedMarkId, studentId, comboSubject.SelectedItem.ToString(), comboExam.SelectedItem.ToString(), score);
            MessageBox.Show("Updated.");
            LoadMarks();
            ClearForm();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(txtStudentID.Text, out int studentId) || comboSubject.SelectedItem == null || comboExam.SelectedItem == null || !int.TryParse(txtScore.Text, out int score))
            {
                MessageBox.Show("Please complete all fields.");
                return;
            }

            controller.AddMark(studentId, comboSubject.SelectedItem.ToString(), comboExam.SelectedItem.ToString(), score);
            MessageBox.Show("Mark added.");
            LoadMarks();
            ClearForm();
        }
    }
}
