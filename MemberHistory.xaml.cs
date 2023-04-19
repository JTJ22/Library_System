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
            gridCreator();
        }

        private void txtBoxSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<User_Record> records = User_Record.UserRecordInstance.DisplayRecord();
            List<User_Record> filteredRecords = Filter_Records(records, txtBoxSearchBar.Text.ToLower());
            this.dgMemberHistory.ItemsSource = filteredRecords;
        }


        private List<User_Record> Filter_Records(List<User_Record> records, string searchText)
        {

            List<User_Record> searchRecords = new List<User_Record>();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                foreach (User_Record record in records)
                {
                    if (record.User_Id.ToLower().Contains(searchText) ||
                        record.Book_Name.ToLower().Contains(searchText) ||
                        record.Unique_Id.ToString().ToLower().Contains(searchText) ||
                        record.Withdraw_Date.ToLower().Contains(searchText) ||
                        record.Return_Expected.ToLower().Contains(searchText) ||
                        record.Return_Actual.ToLower().Contains(searchText))
                    {
                        searchRecords.Add(record);
                    }
                }
                return searchRecords;
            }
            else 
            {
                return records; 
            }
        }

    }
}
