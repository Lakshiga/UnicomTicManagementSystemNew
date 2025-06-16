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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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

        private void button6_Click_1(object sender, EventArgs e)
        {
            LoadFormInPanel(new TimeTableForm());
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            LoadFormInPanel(new StaffForm());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new ExamForm());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new MarkForm());
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

    }
}
