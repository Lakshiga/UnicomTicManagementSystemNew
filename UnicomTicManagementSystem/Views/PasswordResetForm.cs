using System;
using System.Windows.Forms;
using UnicomTicManagementSystem.Repositories;

namespace UnicomTicManagementSystem.Views
{
    public partial class ResetPasswordForm : Form
    {
        private string username;

        public ResetPasswordForm(string loggedInUsername)
        {
            InitializeComponent();
            username = loggedInUsername;
        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            string current = txtCurrent.Text;
            string newPass = txtNew.Text;
            string confirm = txtConfirm.Text;

            if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirm))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Check current password
            if (!UserRepository.ValidateUser(username, current))
            {
                MessageBox.Show("Current password is incorrect.");
                return;
            }

            if (newPass != confirm)
            {
                MessageBox.Show("New password and confirm password do not match.");
                return;
            }

            // Update password
            if (UserRepository.ResetUserPassword(username, newPass))
            {
                MessageBox.Show("Password updated successfully.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to update password.");
            }
        }
    }
}
