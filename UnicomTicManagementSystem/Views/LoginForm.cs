using System;
using System.Windows.Forms;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Repositories;

namespace UnicomTicManagementSystem.Views
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (!UserRepository.HasAnyUsers())
            {
                lblMessage.Text = "First-time setup: Create Admin Account";
                btnLogin.Text = "Register Admin";
            }
            else
            {
                lblMessage.Text = "Login to your account";
                btnLogin.Text = "Login";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            if (!UserRepository.HasAnyUsers())
            {
                // First-time admin registration
                var newAdmin = new User
                {
                    Username = username,
                    Password = password,
                    Role = "Admin"
                };

                UserRepository.AddUser(newAdmin);
                MessageBox.Show("Admin account created successfully. You can now log in.");
                LoginForm_Load(sender, e); // Refresh the form to switch to login mode
                return;
            }

            // Regular login process
            var user = UserRepository.Authenticate(username, password);

            if (user != null)
            {
                UserLogin.Username = user.Username;
                UserLogin.Role = user.Role;

                this.Hide(); // Hide login form

                switch (user.Role.ToLower())
                {
                    case "admin":
                    case "staff":
                    case "lecture":
                        var mainForm = new MainForm(user.Role,username); // Pass role to MainForm
                        this.Hide();
                        mainForm.ShowDialog();
                        this.Show();
                        break;

                    /*case "staff":
                        var staffDashboard = new StaffDashboard();
                        this.Hide();
                        staffDashboard.ShowDialog();
                        this.Show();
                        break;*/

                    case "student":
                        var student = StudentRepository.GetStudentByUsername(user.Username);
                        if (student != null)
                        {
                            UserLogin.StudentId = student.Id;
                            UserLogin.Id = student.Id;
                            UserLogin.Name = student.Name;
                            UserLogin.Username = student.Username;
                            UserLogin.Password = student.Password;
                            UserLogin.Address = student.Address;
                            UserLogin.Stream = student.Stream;

                            StudentDashboard dashboard = new StudentDashboard(txtUsername.Text.Trim());                            
                            this.Hide();
                            dashboard.ShowDialog();
                            this.Show();

                        }
                        else
                        {
                            MessageBox.Show("Student profile not found.");
                            this.Show(); 
                        }
                        break;

                    default:
                        MessageBox.Show("Unknown role. Access denied.");
                        this.Show(); 
                        break;
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void LoginForm_Load_1(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
