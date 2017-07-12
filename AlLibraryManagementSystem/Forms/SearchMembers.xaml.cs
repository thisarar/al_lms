using AlLibraryManagementSystem.DataAccess;
using AlLibraryManagementSystem.Models;
using AlLibraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for SearchMembers.xaml
    /// </summary>
    /// 
    public partial class SearchMembers : Window
    {
        List<Member> members;
        public static Member selectedMember;
        ObservableCollection<BorrowedBy> borrowedBooks;
        public SearchMembers()
        {
            InitializeComponent();
            members = MemberDataAccess.ListMembers();
            txtMemberName.TextChanged += new TextChangedEventHandler(memberList_TextChanged);
            this.dataGridMembers.ItemsSource = members;
        }

        private void memberList_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedText = txtMemberName.Text;
            List<string> completionList = new List<string>();
            completionList.Clear();

            foreach (Member member in members)
            {
                if (!string.IsNullOrEmpty(txtMemberName.Text))
                {
                    if (member.FullName.StartsWith(typedText, StringComparison.InvariantCultureIgnoreCase))
                    {
                        completionList.Add(member.FullName);
                    }
                }
            }
            if (completionList.Count > 0)
            {
                memberList.ItemsSource = completionList;
                memberList.Visibility = Visibility.Visible;
            }
            else if (txtMemberName.Text.Equals(""))
            {
                memberList.Visibility = Visibility.Collapsed;
                memberList.ItemsSource = null;
            }
            else
            {
                memberList.Visibility = Visibility.Collapsed;
                memberList.ItemsSource = null;
            }
        }

        private void SetMemberDataOnLabels(Member row)
        {
            lblAddress.Content = row.address;
            lblBooksPerBorrow.Content = row.BooksPerBorrow.ToString();
            lblBooksRegisteredDate.Content = row.RegisteredDate;
            lblEmail.Content = row.email;
            lblExpireDate.Content = row.ExpirationDate;
            lblName.Content = row.FullName;
            lblPhone.Content = row.phone;
        }

        private void memberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            int index = members.FindIndex(a => a.FullName == (string)listBox.SelectedItem);
            selectedMember = members[index];
            SetMemberDataOnLabels(members[index]);
            memberList.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            CreateMember createMember = new CreateMember();
            createMember.ShowDialog();
        }

        private void dataGridMembers_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            Member row = (Member)dg.Items[dg.Items.IndexOf(dg.SelectedItem)];
            selectedMember = row;
            SetMemberDataOnLabels(row);
            borrowedBooks = new ObservableCollection<BorrowedBy>(BorrowedBooksDataAccess.SearchBorrowedBooks(row.MemberId));
            lblTotalBooks.Content = borrowedBooks.Count() + " books were issued to this member";
            this.dataGridBorrowedBooks.ItemsSource = borrowedBooks;
        }

        private void dataGridBorrowedBooks_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            BorrowedBy row = (BorrowedBy)dg.Items[dg.Items.IndexOf(dg.SelectedItem)];
            Book book = BookDataAccess.FindBook(row.BookId);

            // calculate fine
            DateTime returnDate = DateTime.Parse(row.ReturnDate);
            var TotalDays = (DateTime.Parse(DateTime.Now.ToString("d")) - DateTime.Parse(returnDate.ToString("d"))).TotalDays;
            if (TotalDays > 0)
            {
                fineLbl.Content = "$ " + (double)TotalDays * (double)book.FinePerDay;
                dateLbl.Content = TotalDays + " days";
                lblNoDelays.Visibility = Visibility.Hidden;
                fineLbl.Visibility = Visibility.Visible;
                dateLbl.Visibility = Visibility.Visible;
            }
            else
            {
                fineLbl.Visibility = Visibility.Hidden;
                dateLbl.Visibility = Visibility.Hidden;
                lblNoDelays.Visibility = Visibility.Visible;

            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataGrid dg = dataGridBorrowedBooks as DataGrid;
            if (dg.SelectedItem != null)
            {
                BorrowedBy row = (BorrowedBy)dg.Items[dg.Items.IndexOf(dg.SelectedItem)];
                row.returned = 1;
                if (BorrowedBooksDataAccess.Update(row))
                {
                    MessageBox.Show(AppConstants.BookCollectedMessage, AppConstants.BookCollectedTitle, MessageBoxButton.OK
                        , MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show(AppConstants.ErrorMessage, AppConstants.ErrorTitle, MessageBoxButton.OK
                        , MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show(AppConstants.SelectMessage, AppConstants.SelectTitle, MessageBoxButton.OK
                        , MessageBoxImage.Information);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnIssue_Click_1(object sender, RoutedEventArgs e)
        {
            if (selectedMember != null)
            {
                Close();
                SearchBooks searchBook = new SearchBooks();
                //searchBook.selectedMember = selectedMember;
                searchBook.ShowDialog();
            }
            else
            {
                MessageBox.Show(AppConstants.SelectMessage, AppConstants.SelectTitle, MessageBoxButton.OK
                        , MessageBoxImage.Information);
            }
            
        }
    }
}
