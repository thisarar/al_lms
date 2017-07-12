using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;
using AlLibraryManagementSystem.DataAccess;
using AlLibraryManagementSystem.Utils;
using AlLibraryManagementSystem.Forms;
using AlLibraryManagementSystem.Models;

namespace AlLibraryManagementSystem
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

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.username = this.TxtUsername.Text;
            user.password = this.TxtPassword.Password;
            if (UserDataAccess.Login(user))
            {
                Home home = new Home();
                home.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(AppConstants.LoginErrorMessage, AppConstants.LoginErrorTitle, MessageBoxButton.OK
                    , MessageBoxImage.Error);
            }
        }
    }
}
