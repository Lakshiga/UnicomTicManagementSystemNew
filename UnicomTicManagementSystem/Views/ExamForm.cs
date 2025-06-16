using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnicomTicManagementSystem.Views
{
    public partial class ExamForm : Form
    {
        private ExamController examController = new ExamController();
        private int selectedExamId = -1;
        public ExamForm()
        {
            InitializeComponent();
            LoadSubjects();
            LoadExams();
        }

        private void LoadSubjects()
        {
            DataTable subjects = examController.GetAllSubjects();
            cmbSubject.DataSource = subjects;
            cmbSubject.DisplayMember = "SubjectName";
            cmbSubject.ValueMember = "SubjectID";
            cmbSubject.SelectedIndex = -1;
        }

        private void LoadExams()
        {
            dataGridView.DataSource = examController.GetExams();
            dataGridView.ClearSelection();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                string examName = txtname.Text.Trim();
                int subjectId = Convert.ToInt32(cmbSubject.SelectedValue);

                examController.AddExam(examName, subjectId);
                MessageBox.Show("Exam added successfully.");

                ClearInputs();
                LoadExams();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedExamId == -1)
            {
                MessageBox.Show("Please select an exam to update.");
                return;
            }

            if (ValidateInputs())
            {
                string examName = txtname.Text.Trim();
                int subjectId = Convert.ToInt32(cmbSubject.SelectedValue);

                examController.UpdateExam(selectedExamId, examName, subjectId);
                MessageBox.Show("Exam updated successfully.");

                ClearInputs();
                LoadExams();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedExamId == -1)
            {
                MessageBox.Show("Please select an exam to delete.");
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this exam?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                examController.DeleteExam(selectedExamId);
                MessageBox.Show("Exam deleted successfully.");

                ClearInputs();
                LoadExams();
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];

                selectedExamId = Convert.ToInt32(row.Cells["ExamID"].Value);
                txtname.Text = row.Cells["ExamName"].Value.ToString();
                cmbSubject.Text = row.Cells["Subject"].Value.ToString();
            }
        }

        private bool ValidateInputs()
        {
            if (cmbSubject.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtname.Text))
            {
                MessageBox.Show("Please enter an exam name.");
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtname.Clear();
            cmbSubject.SelectedIndex = -1;
            selectedExamId = -1;
            dataGridView.ClearSelection();
        }

    }
}
