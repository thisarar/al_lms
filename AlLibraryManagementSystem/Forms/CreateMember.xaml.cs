using AlLibraryManagementSystem.DataAccess;
using AlLibraryManagementSystem.Models;
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
using System.Windows.Shapes;

namespace AlLibraryManagementSystem.Forms
{
    /// <summary>
    /// Interaction logic for CreateMember.xaml
    /// </summary>
    public partial class CreateMember : Window
    {
        public CreateMember()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Member member = new Member();
            member.FullName = txtFullName.Text;
            member.address = txtAddress.Text;
            member.RegisteredDate = DateTime.Today;
            member.BooksPerBorrow = txtBooksAtATime.Text;
            member.phone = txtPhone.Text;
            member.email = txtEmail.Text;
            member.ExpirationDate = datePicker.SelectedDate;
            MemberDataAccess.Insert(member);

        }
    }
}
