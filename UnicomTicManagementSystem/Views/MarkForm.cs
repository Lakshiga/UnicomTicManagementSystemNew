using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTicManagementSystem.Controllers;

namespace UnicomTicManagementSystem.Views
{
    public partial class MarkForm : Form
    {
        private MarkController controller = new MarkController();
        private int selectedMarkId = -1;
        public MarkForm()
        {
            InitializeComponent();
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

            int studentId = Convert.ToInt32(txtStudentID.Text);
            int score = Convert.ToInt32(txtScore.Text);
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
