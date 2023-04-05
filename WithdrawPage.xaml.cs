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
          BookHandling.handler.Withdrawn(bookBeingWithdrawn);
        }

        private void DisplayInfo(object sender, RoutedEventArgs e)
        {
            DateTime expected = DateTime.Now.AddMonths(1);
            txtBlkBookName.Text = bookBeingWithdrawn.Title + "\n" + bookBeingWithdrawn.Author + "\n" + bookBeingWithdrawn.Description + "\n" + bookBeingWithdrawn.ISBN;
            txtBlkExptReturn.Text = "Maximum Return Date: " + expected.ToString("yyyy-MM-dd");
        }
    }
}
