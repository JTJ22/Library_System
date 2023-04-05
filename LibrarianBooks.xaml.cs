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

namespace Library_System
{
    /// <summary>
    /// Interaction logic for LibrarianBooks.xaml
    /// </summary>
    public partial class LibrarianBooks : Page
    {

        SingleBook bookBeingWithdrawn = new SingleBook();
        public LibrarianBooks()
        {
            InitializeComponent();
        }

        public LibrarianBooks(SingleBook bookWithdrawn) : this()
        {
            bookBeingWithdrawn = bookWithdrawn;
            this.Loaded += new RoutedEventHandler(DisplayInfo);
        }

        private void DisplayInfo(object sender, RoutedEventArgs e)
        {
            txtBookInfo.Text = bookBeingWithdrawn.Title + "\n" + bookBeingWithdrawn.Author + "\n" + bookBeingWithdrawn.Description + "\n" + bookBeingWithdrawn.ISBN;
        }

    }
}
