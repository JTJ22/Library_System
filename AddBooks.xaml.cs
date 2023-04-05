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

namespace Library_System
{
    /// <summary>
    /// Interaction logic for AddBooks.xaml
    /// </summary>
    public partial class AddBooks : Window
    {
        public AddBooks()
        {
            InitializeComponent();
            Grid_Creator();
        }
        public void Grid_Creator()
        {
            dgBooksEditRemove.ItemsSource = BookHandling.handler.Display_Books();
        }
        private void txtBoxTitle_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxAuthor_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxGenre_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxISBN_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxDate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxDesciption_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            BookHandling handler = new BookHandling();
            SingleBook newBook = new SingleBook
            {
                Unique_ID = Guid.NewGuid(),
                Title = txtBoxTitle.Text,
                Author = txtBoxAuthor.Text,
                Genre = txtBoxGenre.Text,
                ISBN = txtBoxISBN.Text,
                Date = txtBoxDate.Text,
                Availability = true,
                Description = txtBoxDesciption.Text,
                IsReserved = false
            };

            while (!handler.Check_Duplicate(newBook.Unique_ID))
            {
                newBook.Unique_ID = Guid.NewGuid();
            }

            handler.Add_Book(newBook);
        }

        private void dgBooksEditRemove_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddBookAccess_Click(object sender, RoutedEventArgs e)
        {
            dgBooksEditRemove.Visibility = Visibility.Collapsed;
            stkPanAdd.Visibility = Visibility.Visible;
        }
    }
}
