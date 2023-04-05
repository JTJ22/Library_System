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
    /// Interaction logic for MemberHistory.xaml
    /// </summary>
    public partial class MemberHistory : Page
    {
        public MemberHistory()
        {
            InitializeComponent();
            gridCreator();
        }

        private void dgMemberHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
  
        }

        public void gridCreator()
        {
            dgMemberHistory.ItemsSource = User_Record.UserRecordInstance.DisplayRecord();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            User_Record.UserRecordInstance.BookReturned((User_Record)dgMemberHistory.SelectedItem);
            gridCreator();
        }
        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            User_Record record = (User_Record)dgMemberHistory.SelectedItem;
            record.Renew_Book(record);
        }

    }
}
