﻿using System;
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
            BookReturned((User_Record)dgMemberHistory.SelectedItem);
        }
        private void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            User_Record record = (User_Record)dgMemberHistory.SelectedItem;
            record.Renew_Book(record);
        }
        private void BookReturned(User_Record selectItem)
        {
            User_Record bookReturned = selectItem;
            if (bookReturned != null)
            {
                if (bookReturned.Is_Returned != true)
                {
                    if (Convert.ToDateTime(bookReturned.Return_Expected) > DateTime.Now.Date)
                    {
                        User_Record.UserRecordInstance.ChangeBookFile(bookReturned.Unique_Id);
                        User_Record.UserRecordInstance.RecordAdjust(bookReturned);
                        MessageBox.Show("Book Returned Within Due Date");
                        gridCreator();
                    }
                }
                else
                {
                    MessageBox.Show("Already Returned");
                }
            }

        }

    }
}
