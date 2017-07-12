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
using System.Data;
using System.Windows.Media.Animation;

namespace AlLibraryManagementSystem.Forms
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateCategory createCat = new CreateCategory();
            createCat.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SearchBooks searchBook = new SearchBooks();
            searchBook.ShowDialog();
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SearchMembers searchMemebers = new SearchMembers();
            searchMemebers.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SearchMembers searchMemebers = new SearchMembers();
            searchMemebers.ShowDialog();
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SearchMembers searchMemebers = new SearchMembers();
            searchMemebers.ShowDialog();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SearchMembers searchMemebers = new SearchMembers();
            searchMemebers.ShowDialog();
        }
    }
}
