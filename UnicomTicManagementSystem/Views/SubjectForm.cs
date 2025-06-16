using System;
using System.Data;
using System.Windows.Forms;
using UnicomTicManagementSystem.Controllers; // Make sure this is correct based on your file structure

namespace UnicomTicManagementSystem.Views
{
    public partial class SubjectForm : Form
    {
        SubjectController controller = new SubjectController();
        int selectedSubjectId = -1;

        public SubjectForm()
        {
            InitializeComponent();
            LoadSubjects();
            LoadSections();
        }

        private void LoadSections()
        {
            DataTable sections = controller.GetAllSections();

            // For Add/Update
            comboBoxSelectSection.DataSource = sections.Copy();
            comboBoxSelectSection.DisplayMember = "Name";  // Adjust based on DB schema
            comboBoxSelectSection.ValueMember = "Id";
            comboBoxSelectSection.SelectedIndex = -1;

            // For Search
            comboBoxSearchSection.DataSource = sections;
            comboBoxSearchSection.DisplayMember = "Name";
            comboBoxSearchSection.ValueMember = "Id";
            comboBoxSearchSection.SelectedIndex = -1;
        }

        private void LoadSubjects()
        {
            DataTable dt = controller.GetSubjects();

            // Bind to DataGridView
            dataGridView1.DataSource = dt;

            // Hide SectionID column (if present)
            if (dataGridView1.Columns.Contains("SectionID"))
            {
                dataGridView1.Columns["SectionID"].Visible = false;
            }

            // Populate ComboBox with Section names
            DataTable comboTable = dt.DefaultView.ToTable(true, "SectionID", "SectionName");
            comboBoxSearchSection.DataSource = comboTable;
            comboBoxSearchSection.DisplayMember = "SectionName";
            comboBoxSearchSection.ValueMember = "SectionID";
            comboBoxSearchSection.SelectedIndex = -1;
        }

        private void SubjectForm_Load(object sender, EventArgs e)
        {
            // Optional: logic to run on form load
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtSubjectName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || comboBoxSelectSection.SelectedValue == null)
            {
                MessageBox.Show("Please enter subject name and select a section.");
                return;
            }

            int sectionId = Convert.ToInt32(comboBoxSelectSection.SelectedValue);

            controller.AddSubject(name, sectionId);
            MessageBox.Show("Subject added successfully.");

            LoadSubjects();
            ClearForm();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (selectedSubjectId == -1)
            {
                MessageBox.Show("Please select a subject to update.");
                return;
            }

            string name = txtSubjectName.Text.Trim();

            if (comboBoxSelectSection.SelectedValue == null || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a subject name and select a section.");
                return;
            }
            int sectionId = Convert.ToInt32(comboBoxSelectSection.SelectedValue);

            controller.UpdateSubject(selectedSubjectId, name, sectionId);
            MessageBox.Show("Subject updated successfully.");

            LoadSubjects();
            ClearForm();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (selectedSubjectId == -1)
            {
                MessageBox.Show("Please select a subject to delete.");
                return;
            }

            controller.DeleteSubject(selectedSubjectId);
            MessageBox.Show("Subject deleted successfully.");

            LoadSubjects();
            ClearForm();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (comboBoxSearchSection.SelectedValue == null)
            {
                MessageBox.Show("Please select a section to search.");
                return;
            }

            int sectionId = Convert.ToInt32(comboBoxSearchSection.SelectedValue);
            DataTable dt = controller.GetSubjectsBySection(sectionId);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedSubjectId = Convert.ToInt32(row.Cells["SubjectID"].Value);
                txtSubjectName.Text = row.Cells["SubjectName"].Value.ToString();

                // Optional: fill both combo boxes
                if (row.Cells["SectionID"].Value != DBNull.Value)
                {
                    int sectionId = Convert.ToInt32(row.Cells["SectionID"].Value);
                    comboBoxSelectSection.SelectedValue = sectionId;
                    comboBoxSearchSection.SelectedValue = sectionId;
                }
            }
        }


        private void ClearForm()
        {
            selectedSubjectId = -1;
            txtSubjectName.Clear();
            comboBoxSearchSection.SelectedIndex = -1;
            comboBoxSelectSection.SelectedIndex = -1;
        }

        private void comboBoxSearchSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: handle selection changes
            // Example: Debug.WriteLine(comboBoxSearchSection.SelectedValue?.ToString());
        }

        private void comboBoxSelectSection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
