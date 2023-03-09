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
        }

        private void dgSearchDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void GridCreator()
        {
            dgSearchDisplay.ItemsSource = BookHandling.DisplayBooks();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Searching();
        }

        private void txtBoxSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void Searching()
        {
            if (!string.IsNullOrWhiteSpace(txtBoxSearchBox.Text))
                {
                DataSet dataSearch = new DataSet();
                dataSearch.ReadXml(@"LibraryBooks.xml");
                DataView dataView = new DataView();
                dataView = dataSearch.Tables[0].DefaultView;
                dataView.RowFilter = $"Title='{txtBoxSearchBox.Text.ToLower()}'";
                this.dgSearchDisplay.ItemsSource = dataView;
                }
            else
            {
                GridCreator();
            }

        }

        private void btnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            var row_list = (SingleBook)dgSearchDisplay.SelectedItem;
            
        }
    }
}
