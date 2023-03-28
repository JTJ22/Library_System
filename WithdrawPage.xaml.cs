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
using System.Windows.Threading;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for WithdrawPage.xaml
    /// </summary>
    public partial class WithdrawPage : Page
    {
        SingleBook bookBeingWithdrawn = new SingleBook();
        public WithdrawPage()
        {
            InitializeComponent();
        }

        public WithdrawPage(SingleBook bookWithdrawn):this()
        {
            bookBeingWithdrawn = bookWithdrawn;
            this.Loaded += new RoutedEventHandler(DisplayInfo);
        }

        private void btnCompleteWithdraw_Click(object sender, RoutedEventArgs e)
        {
            Withdrawn();
        }

        private void DisplayInfo(object sender, RoutedEventArgs e)
        {
            DateTime expected = DateTime.Now.AddMonths(1);
            txtBlkBookName.Text = bookBeingWithdrawn.Title + "\n" + bookBeingWithdrawn.Author + "\n" + bookBeingWithdrawn.Description + "\n" + bookBeingWithdrawn.ISBN;
            txtBlkExptReturn.Text = "Maximum Return Date: " + expected.ToString("yyyy-MM-dd");
        }

        public void Withdrawn()
        {
            XmlDocument books = new XmlDocument();
            books.Load("LibraryBooks.xml");
            foreach (XmlNode book in books.SelectNodes("/Books/SingleBook"))
            {
                if (book.SelectSingleNode("UniqueID").InnerText == bookBeingWithdrawn.Unique_ID)
                {
                    if (bookBeingWithdrawn.Availability == true && bookBeingWithdrawn.IsReserved == false)
                    {
                        User_Record user_Record = new User_Record();
                        XmlNode bookTaken = books.SelectSingleNode($"/Books/SingleBook[UniqueID='{bookBeingWithdrawn.Unique_ID}']/Availabilty");
                        bookTaken.InnerText = "false";
                        bookBeingWithdrawn.Availability = false;
                        books.Save("LibraryBooks.xml");
                        user_Record.Update_History(bookBeingWithdrawn);
                        MessageBox.Show("Withdrawn");
                    }
                    else
                    {
                        MessageBox.Show("Book Cannot be withdrawn");
                    }

                }
            }
            
        }
        
    }
}
