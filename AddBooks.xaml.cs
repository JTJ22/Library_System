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
        public SingleBook currentBook;
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
            Add_Book_Event();
            Grid_Creator();
            dgBooksEditRemove.Visibility = Visibility.Visible;
            stkPanAdd.Visibility = Visibility.Collapsed; 
            btnAddBookAccess.Visibility = Visibility.Visible;
        }

        private void Add_Book_Event()
        {
            MessageBoxResult check = MessageBox.Show("Are you sure you want to add this book?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (check == MessageBoxResult.Yes)
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
        }
        private void dgBooksEditRemove_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddBookAccess_Click(object sender, RoutedEventArgs e)
        {
            btnAddBookAccess.Visibility = Visibility.Hidden;
            dgBooksEditRemove.Visibility = Visibility.Collapsed;
            stkPanAdd.Visibility = Visibility.Visible;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Edit_Display();
            btnAddBookAccess.Visibility = Visibility.Hidden;
        }

        private void Edit_Display()
        {
            currentBook = (SingleBook)dgBooksEditRemove.SelectedItem;
            txtBoxEditTitle.Text = currentBook.Title;
            txtBoxEditAuthor.Text = currentBook.Author;
            txtBoxEditGenre.Text = currentBook.Genre;
            chkBoxAvailable.IsChecked = currentBook.Availability;
            chkBoxAvailable.Content = currentBook.Availability ? "Available" : "Unavailable";
            txtBoxEditDesciption.Text = currentBook.Description;
            dgBooksEditRemove.Visibility = Visibility.Collapsed;
            stkPanEdit.Visibility = Visibility.Visible;
            btnAddBook.Visibility = Visibility.Collapsed;
            btnAddBook.Visibility = Visibility.Hidden;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SingleBook book = (SingleBook)dgBooksEditRemove.SelectedItem;
            BookHandling.handler.Remove_Book(book);
            Grid_Creator();
        }
        private void txtDescription_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            MessageBox.Show(textBlock.Text, "Description", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }

        private void btnUpdateBook_Click(object sender, RoutedEventArgs e)
        {
            Update_Book();
        }


        private void Update_Book()
        {
            bool checkedBox = chkBoxAvailable.IsChecked ?? false;
            BookDetailsUpdate newInfo = new BookDetailsUpdate
            {
                Title = txtBoxEditTitle.Text,
                Author = txtBoxEditAuthor.Text,
                Genre = txtBoxEditGenre.Text,
                Description = txtBoxEditDesciption.Text,
                Available = checkedBox
            };
            currentBook.Change_Book_Details(currentBook, newInfo);
            Grid_Creator();
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            MessageBox.Show(textBlock.Text, "Unique ID", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }

        private void btnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            dgBooksEditRemove.Visibility = Visibility.Visible;
            stkPanAdd.Visibility = Visibility.Collapsed;
            btnAddBookAccess.Visibility = Visibility.Visible;
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            dgBooksEditRemove.Visibility = Visibility.Visible;
            stkPanEdit.Visibility = Visibility.Collapsed;
            btnAddBook.Visibility = Visibility.Visible;
            
        }

        private void TextBlock_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            MessageBox.Show(textBlock.Text, "Title", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }

        private void txtBoxBookSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<SingleBook> books = BookHandling.handler.Display_Books();
            List<SingleBook> filteredBooks = BookHandling.handler.Filter_Books(books, txtBoxBookSearch.Text.ToLower());
            this.dgBooksEditRemove.ItemsSource = filteredBooks;
        }


        //https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.datagridtemplatecolumn?view=windowsdesktop-7.0
        //https://stackoverflow.com/questions/52745984/wpf-get-textbox-value-from-datagridtemplatecolumn
    }
}
