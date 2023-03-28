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
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Page
    {
        public Reservations()
        {
            InitializeComponent();
            dgReserveDisplay.ItemsSource = Reserving.Display_Reservations();
        }

        private void btnWithdrawBook_Click(object sender, RoutedEventArgs e)
        {
            Updating_Res();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Reserving cancelled = (Reserving)dgReserveDisplay.SelectedItem;
            Reserving.Cancel_Reservation(cancelled);
            dgReserveDisplay.Items.Refresh();
        }

        private void dgReserveDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void Updating_Res()
        {
            XmlDocument books = new XmlDocument();
            books.Load("LibraryBooks.xml");
            Reserving reservedBook = (Reserving)dgReserveDisplay.SelectedItem;
            SingleBook bookBeingWithdrawn = GettingBook(reservedBook);
            if (bookBeingWithdrawn.IsReserved)
            {
                foreach (var record in Reserving.Display_Reservations())
                {
                    if (User_Data.currentUser.User_id == record.User_ID)
                    {
                        User_Record user_Record = new User_Record();
                        XmlNode bookTaken = books.SelectSingleNode($"/Books/SingleBook[UniqueID='{bookBeingWithdrawn.Unique_ID}']/Availabilty");
                        XmlNode bookRes = books.SelectSingleNode($"/Books/SingleBook[UniqueID='{bookBeingWithdrawn.Unique_ID}']/IsReserved");
                        record.Complete_Reservation(record);
                        bookTaken.InnerText = "false";
                        bookRes.InnerText = "false";
                        bookBeingWithdrawn.Availability = false;
                        bookBeingWithdrawn.IsReserved = false;
                        books.Save("LibraryBooks.xml");
                        user_Record.Update_History(bookBeingWithdrawn);
                        MessageBox.Show("Reservation Withdrawn");
                    }
                }
            }
        }

        private SingleBook GettingBook(Reserving reservedBook)
        {
            foreach (var book in BookHandling.DisplayBooks())
            {
                if (reservedBook.Unique_Id == book.Unique_ID)
                {
                    SingleBook bookBeingWithdrawn = book;
                    return bookBeingWithdrawn;
                }
            }
            return null;
        }

    }
}
