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
    public delegate void OnRefresh(object sender, EventArgs e);

    public partial class WithdrawPage : Page
    {
        public static OnRefresh OnRefresh;

        SingleBook bookBeingWithdrawn = new SingleBook();

        public WithdrawPage()
        {
            InitializeComponent();
        }


        public WithdrawPage(SingleBook bookWithdrawn) : this()
        {
            bookBeingWithdrawn = bookWithdrawn;
            this.Loaded += new RoutedEventHandler(DisplayInfo);
        }

        private void btnCompleteWithdraw_Click(object sender, RoutedEventArgs e)
        {
            BookHandling.handler.Withdrawn(bookBeingWithdrawn);
            OnRefresh?.Invoke(this, new EventArgs());
        }

        private void DisplayInfo(object sender, RoutedEventArgs e)
        {
            if (bookBeingWithdrawn.Availability)
            {
                btnCompleteWithdraw.IsEnabled = true;
                string bookInfo = $"{bookBeingWithdrawn.Title}\n{bookBeingWithdrawn.Author}\n\n{bookBeingWithdrawn.Description}\n{bookBeingWithdrawn.ISBN}";
                txtBlkBookName.Text = bookInfo;
                DateTime expected = DateTime.Now.AddMonths(1);
                txtBlkExptReturn.Text = $"Maximum Return Date: {expected:yyyy-MM-dd}";
            }
            else 
            {
                string bookInfo = $"{bookBeingWithdrawn.Title}\n{bookBeingWithdrawn.Author}\n\n{bookBeingWithdrawn.Description}\n{bookBeingWithdrawn.ISBN}";
                txtBlkBookName.Text = bookInfo;
                txtBlkExptReturn.Text = "This book is currently unavailable";
                btnCompleteWithdraw.IsEnabled = false;
            }
        }
    }
}
