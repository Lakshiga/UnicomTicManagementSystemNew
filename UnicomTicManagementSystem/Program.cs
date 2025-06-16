using UnicomTicManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnicomTicManagementSystem.Views;
using UnicomTicManagementSystem.Models;
using UnicomTicManagementSystem.Repositories;
using UnicomTicManagementSystem.Services;

namespace UnicomTicManagementSystem
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Initialize database table
            DatabaseInitializer.CreateTables();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // Show Login Form first
            LoginForm loginForm = new LoginForm();
            DialogResult result = loginForm.ShowDialog();

            // If login is successful, go to MainForm
            if (result == DialogResult.OK)
            {
                Application.Run(new MainForm());
            }
        }
    }
}
