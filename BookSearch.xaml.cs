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
using System.Xml;
using System.Data;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for BookSearch.xaml
    /// </summary>
    public partial class BookSearch : Page
    {
        public BookSearch()
        {
            InitializeComponent();
            GridCreator();
            //Adding and displaying the data grid for book results, in its base state it will show all books
            WithdrawPage.OnRefresh += (o, _) =>
            {
                GridCreator();
            };
        }

        private void dgSearchDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void GridCreator()
        {

            dgSearchDisplay.ItemsSource = BookHandling.handler.Display_Books();
            //Using my method which returns the list, this sets the datagrids source to the list
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Searching();
            
            //Upon clicking search the program will execute this method, allowing a user to search
        }

        private void txtBoxSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void Searching()
        {
            if (!string.IsNullOrWhiteSpace(txtBoxSearchBox.Text))//Checking if user input nothing, won't execute if empty
                {
                DataSet dataSearch = new DataSet();
                dataSearch.ReadXml(@"LibraryBooks.xml");
                DataView dataView = new DataView();
                dataView = dataSearch.Tables[0].DefaultView;
                StringBuilder search = new StringBuilder();
                foreach(DataColumn column in dataView.Table.Columns)
                    {
                    search.AppendFormat("[{0}] Like '%{1}%' OR", column.ColumnName, txtBoxSearchBox.Text.ToLower());
                    }
                search.Remove(search.Length - 3, 3);
                dataView.RowFilter = search.ToString();
                this.dgSearchDisplay.ItemsSource = dataView;
                //Changes the data grid to be filtered, this is dependant on the users input into the search box
                }
            else
            {
                GridCreator();
                //Execute the grid creator (returns to base state)
            }

        }

        private void btnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            BookToWithdraw();
            //Returns the book which is in the row of the button clicked
        }

        public SingleBook BookToWithdraw()
        {
            SingleBook bookWithdrawn = (SingleBook)dgSearchDisplay.SelectedItem;
            if(bookWithdrawn != null)
            {
                if (!User_Data.currentUser.Librarian_Permisions)
                {
                    WithdrawPage withdraw = new WithdrawPage(bookWithdrawn);

                    frmWithdrawDisplay.NavigationService.Navigate(withdraw, bookWithdrawn);

                    return bookWithdrawn;
                }
                else
                {
                    return null;
                }
                //Creates a new page where the user can withdraw the chosen book. Using frame navigation I can pass the book into the new page
            }
            else
            {
                return null;
            }
        }

        private void frmWithdrawDisplay_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            Reserve();
            GridCreator();
        }

        private void Reserve()
        {
            Reserving reserve = new Reserving();
            SingleBook reservedBook = (SingleBook)dgSearchDisplay.SelectedItem;
            if (reservedBook != null)
            {
                reserve.Reserved(reservedBook);
            }
            
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            MessageBox.Show(textBlock.Text, "Description", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }

        private void TextBlock_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            MessageBox.Show(textBlock.Text, "Unique ID", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }
    }
}
