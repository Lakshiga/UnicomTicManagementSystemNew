using System;
using System.Windows.Forms;
using UnicomTicManagementSystem.Controllers;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Repositories;

namespace UnicomTicManagementSystem.Views
{
    public partial class StaffForm : Form
    {
        private readonly StaffController _staffController;
        private int selectedStaffId = -1;
        private int selectedUserId = -1;

        public StaffForm()
        {
            InitializeComponent();
            _staffController = new StaffController();
            LoadStaff();
        }

        private void LoadStaff()
        {
            dgvStaff.DataSource = null;
            dgvStaff.DataSource = _staffController.GetAllStaff();

            dgvStaff.ClearSelection();
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            textBox5.Text = "";
            selectedStaffId = -1;
            selectedUserId = -1;
        }
        

        private void dgvStaff_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStaff.SelectedRows.Count > 0)
            {
                var row = dgvStaff.SelectedRows[0].DataBoundItem as Staff;

                if (row != null)
                {
                    selectedStaffId = row.Id;
                    selectedUserId = row.UserId;

                    txtName.Text = row.Name;
                    txtAddress.Text = row.Address;
                    txtEmail.Text = row.Email;

                    var user = UserRepository.GetUserById(row.UserId);
                    txtUsername.Text = user?.Username ?? "";
                    textBox5.Text = user?.Password ?? "";
                }
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedStaffId == -1)
            {
                MessageBox.Show("Select a staff to delete.");
                return;
            }

            _staffController.DeleteStaff(selectedStaffId);
            LoadStaff();
            ClearForm();
            MessageBox.Show("Staff deleted successfully.");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedStaffId == -1)
            {
                MessageBox.Show("Select a staff to update.");
                return;
            }

            var staff = new Staff
            {
                Id = selectedStaffId,
                Name = txtName.Text,
                Address = txtAddress.Text,
                Email = txtEmail.Text,
                UserId = selectedUserId
            };

            _staffController.UpdateStaff(staff, txtUsername.Text.Trim(), textBox5.Text.Trim());
            LoadStaff();
            ClearForm();
            MessageBox.Show("Staff updated successfully.");
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtEmail.Text == "" ||
                txtUsername.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (UserRepository.UserExists(txtUsername.Text))
            {
                MessageBox.Show("Username already exists.");
                return;
            }

            var staff = new Staff
            {
                Name = txtName.Text,
                Address = txtAddress.Text,
                Email = txtEmail.Text
            };

            _staffController.AddStaff(staff, txtUsername.Text.Trim(), textBox5.Text.Trim());
            LoadStaff();
            ClearForm();
            MessageBox.Show("Staff added successfully.");
        }

        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
