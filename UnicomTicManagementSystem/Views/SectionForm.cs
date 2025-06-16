using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolManageSystem.Controllers;
using UnicomTicManagementSystem.Controllers;
using UnicomTicManagementSystem.Models;

namespace UnicomTicManagementSystem.Views
{

    public partial class SectionForm : Form
    {
        private SectionController sectionController = new SectionController();
        public SectionForm()
        {
            InitializeComponent();

            // Add these lines when embedding a Form into a Panel
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;

            LoadSections();
        }

        private void LoadSections()
        {
            /* dgvSections.DataSource = null;
             dgvSections.DataSource = sectionController.GetAllSections();
             dgvSections.ClearSelection(); */

            var sections = sectionController.GetAllSections();

            if (sections != null && sections.Any())
            {
                dgvSections.AutoGenerateColumns = true;
                dgvSections.DataSource = null;
                dgvSections.DataSource = sections;
                dgvSections.ClearSelection();

            }
        }  

        private void dgvSections_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSections.SelectedRows.Count > 0)
            {
                textBox1.Text = dgvSections.SelectedRows[0].Cells["Name"].Value.ToString();
            }
        }
              
        private void button3_Click_1(object sender, EventArgs e)
        {
             var section = new Section
            {
                Name = textBox1.Text.Trim()
            };
            sectionController.AddSection(section);
            LoadSections();
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvSections.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvSections.SelectedRows[0].Cells["Id"].Value);
                var section = new Section
                {
                    Id = id,
                    Name = textBox1.Text.Trim()
                };
                sectionController.UpdateSection(section);
                LoadSections();
                textBox1.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvSections.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvSections.SelectedRows[0].Cells["Id"].Value);
                sectionController.DeleteSection(id);
                LoadSections();
                textBox1.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string keyword = secSearch.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(keyword))
            {
                var filteredSections = sectionController.GetAllSections()
                                         .Where(s => s.Name.ToLower().Contains(keyword))
                                         .ToList();

                dgvSections.DataSource = null;
                dgvSections.DataSource = filteredSections;
                dgvSections.ClearSelection();
            }
            else
            {
                LoadSections(); // If search box is empty, reload all sections
            }
        }

        private void dgvSections_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
