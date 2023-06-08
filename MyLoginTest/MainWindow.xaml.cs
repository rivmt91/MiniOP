using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyLoginTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            PerformLogin();


        }

        private void PerformLogin()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Perform login authentication logic here
            if (Authenticate(username, password))
            {
                MessageBox.Show("Login successful!");
                // Proceed to the next view or perform additional actions

                string appDirectory = AppDomain.CurrentDomain.BaseDirectory; // Get the current application directory
                string exePath = appDirectory + "MiniOPd.exe";

                Process.Start(exePath);
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private bool Authenticate(string username, string password)
        {
            // Implement your authentication logic here
            // This is a simple example for demonstration purposes
            return username == "admin" && password == "admin";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLogin();
                e.Handled = true;
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLogin();
                e.Handled = true;
            }
        }
    }
}
