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
using AlLibraryManagementSystem.Utils;
using System.IO;
using AlLibraryManagementSystem.Models;
using AlLibraryManagementSystem.DataAccess;
using System.Collections.ObjectModel;
using System.Globalization;

namespace AlLibraryManagementSystem.Forms
{
    /// <summary>
    /// Interaction logic for SearchBooks.xaml
    /// </summary>
    public partial class SearchBooks : Window
    {
        ObservableCollection<BookData> bookData = new ObservableCollection<BookData>();
        List<Book> books;
        public Member selectedMember;
        Book selectedBook;
        public SearchBooks()
        {
            InitializeComponent();
            selectedMember = SearchMembers.selectedMember;
            if (selectedMember == null)
            {
                memberDetailPane.IsEnabled = false;
                btnIssue.IsEnabled = false;
            }
            else
            {
                btnSelectMember.Visibility = Visibility.Hidden;
                lblName.Content = selectedMember.FullName;
                lblAddress.Content = selectedMember.address;
                lblEmail.Content = selectedMember.email;
                lblPhone.Content = selectedMember.phone;
            }
            searchTxtBox.Foreground = Brushes.Gray;
            searchTxtBox.Text = "Search Books, Journals, Magazines";
            searchTxtBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(OnTextBoxFocused);
            searchTxtBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(OnTextBoxFocusLost);
            this.booksBox.ItemsSource = bookData;

            // set counts
            lblBooks.Content = "Books Available - " + BookDataAccess.CountPublications(1);
            lblMagazines.Content = "Magazines Available - " + BookDataAccess.CountPublications(2);
            lblJournals.Content = "Journals Available - " + BookDataAccess.CountPublications(3);

            LoadAllBooks(SelectedRadioValue<int>(0, rdBooks, rdJournals, rdMagazines));
        }

        private void LoadAllBooks(int category)
        {
            books = BookDataAccess.ListBooksByCategory(category);
            foreach (Book b in books)
            {
                bookData.Add(new BookData { Title = b.BookName, ImageData = LoadImage(b.BookImage) });
                Console.WriteLine(b.BookName);
            }
        }
        // for this code image needs to be a project resource
        private BitmapImage LoadImage(string filename)
        {
            var bitmap = new BitmapImage();

            using (var stream = new FileStream("../Resources/BookImages/"+filename, FileMode.Open))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze(); // optional
            }

            return bitmap;
        }

        private void OnTextChanged(object sender, RoutedEventArgs e)
        {
            if(bookData.Count > 0)
            {
                for (int i = bookData.Count - 1; i >= 0; i--)
                {
                    bookData.RemoveAt(i);
                }
            }
            if(searchTxtBox.Text.Length > 0)
            {
                List<Book> books = BookDataAccess.SearchBooks(searchTxtBox.Text, SelectedRadioValue<int>(0, rdBooks, rdJournals, rdMagazines));
                foreach (Book b in books)
                {
                    bookData.Add(new BookData{Title=b.BookName, ImageData=LoadImage(b.BookImage)});
                    Console.WriteLine(b.BookName);
                }
            }
        }
        
        private void OnTextBoxFocused(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                //If nothing has been entered yet.
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        private void OnTextBoxFocusLost(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                //If nothing was entered, reset default text.
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    ((TextBox)sender).Text = "Search Books, Journals, Magazines";
                }
            }
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private T SelectedRadioValue<T>(T defaultValue, params RadioButton[] buttons)
        {
            foreach (RadioButton button in buttons)
            {
                if (button.IsChecked == true)
                {
                    if (button.Tag is string && typeof(T) != typeof(string))
                    {
                        string value = (string)button.Tag;
                        return (T)Convert.ChangeType(value, typeof(T));
                    }

                    return (T)button.Tag;
                }
            }

            return defaultValue;
        }

        private void RemoveAllObjects()
        {
            if (bookData.Count > 0)
            {
                for (int i = bookData.Count - 1; i >= 0; i--)
                {
                    bookData.RemoveAt(i);
                }
            }
        }

        private void rdBooks_Checked(object sender, RoutedEventArgs e)
        {
            RemoveAllObjects();
            LoadAllBooks(1);
        }

        private void rdJournals_Checked(object sender, RoutedEventArgs e)
        {
            RemoveAllObjects();
            LoadAllBooks(2);
        }

        private void rdMagazines_Checked(object sender, RoutedEventArgs e)
        {
            RemoveAllObjects();
            LoadAllBooks(3);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateBook searchBook = new CreateBook();
            searchBook.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(selectedBook != null)
            {
                BorrowedBy bb = new BorrowedBy();
                bb.MemberId = selectedMember.MemberId;
                bb.BookId = selectedBook.BookId;
                bb.BorrowedDate = DateTime.Today.ToShortDateString();
                bb.ReturnDate = DateTime.Today.AddDays(10).ToShortDateString();
                bb.CurrentFine = 0;
                bb.returned = 0;
                bool flag = false;
                if(selectedBook.NoOfCopies > 0)
                {
                    flag = BorrowedBooksDataAccess.Insert(bb);
                }
                else
                {
                    MessageBox.Show(AppConstants.BookNotAvailableMessage, AppConstants.ErrorTitle, MessageBoxButton.OK
                       , MessageBoxImage.Error);
                }
                if (flag)
                {
                    MessageBox.Show(AppConstants.IssueSuccessMessage, AppConstants.IssueSuccessTitle, MessageBoxButton.OK
                        , MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(AppConstants.ErrorMessage, AppConstants.ErrorTitle, MessageBoxButton.OK
                        , MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(AppConstants.SelectBookMessage, AppConstants.SelectTitle, MessageBoxButton.OK
                        , MessageBoxImage.Error);
            }
        }

        private void StackPanel_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            StackPanel listBox = (StackPanel)sender;
            int index = books.FindIndex(a => a.BookName == ((TextBlock)listBox.Children[1]).Text);
            Book row = (Book)books[index];
            selectedBook = row;
            lblBookName.Content = "Book Name: " + row.BookName;
            lblAuthor.Content = "Author: " + row.AuthorName;
            lblAvailable.Content = "Number of Copies: " + row.NoOfCopies;
            lblFinePerDay.Content = "Fine/day: $" +row.FinePerDay;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSelectMember_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
            SearchMembers searchMemebers = new SearchMembers();
            searchMemebers.ShowDialog();
        }

        private void btnSelectDifferent_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
            SearchMembers searchMemebers = new SearchMembers();
            searchMemebers.ShowDialog();
        }

    }
}
