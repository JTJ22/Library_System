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
using System.Windows.Threading;
using System.ComponentModel;
namespace Library_System
{
    /// <summary>
    /// Interaction logic for MemberLoginWindow.xaml
    /// </summary>
    
    public partial class MemberLoginWindow : Window
    {
        
        public MemberLoginWindow()
        {

            InitializeComponent();
            Time_Updater();
            if(Fining.finingInstance.Fine_Locker(User_Data.currentUser) && !User_Data.currentUser.Librarian_Permisions)
            {
                btnSearchBooks.IsEnabled = false;
                btnReservations.IsEnabled = false;
                btnHistoryMember.IsEnabled = false;
                frmMemberFrame.Visibility = Visibility.Hidden;
                lblWarning.Visibility = Visibility.Visible;
            }
            EditAUser.ChangePage += (o, _) =>
            {
                AddMember member = new AddMember();
                frmMemberFrame.Navigate(member);
            };
            Fining.RefreshFine += (o, _) =>
            {
                btnSearchBooks.IsEnabled = true;
                btnReservations.IsEnabled = true;
                btnHistoryMember.IsEnabled = true;
                frmMemberFrame.Visibility = Visibility.Visible;
            };

        }

        private void MemberLoginWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Log_Out_Event(sender, e);
        }

        public void Log_Out_Event(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have logged out");
            User_Data.currentUser.Log_Out();
        }


        private void btnMemberDetails_Click(object sender, RoutedEventArgs e)
        {
            if (!User_Data.currentUser.Librarian_Permisions)
            {
                frmMemberFrame.Source = new Uri("MemberDetailPage.xaml", UriKind.Relative);
            }
            else 
            {
                EditAUser page = new EditAUser();
                frmMemberFrame.NavigationService.Navigate(page);
            }
        }

        private void btnHistoryMember_Click(object sender, RoutedEventArgs e)
        {
            frmMemberFrame.Source = new Uri("MemberHistory.xaml", UriKind.Relative);
        }
        private void Time_Updater()
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.txtblkDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }, this.Dispatcher);
            this.txtblkUserInfo.Text = "User ID: " + User_Data.currentUser.User_id + " Name: " + User_Data.currentUser.First_name + " " + User_Data.currentUser.Last_name;
            timer.Start();

        }

        private void frmMemberFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        {
            if (!User_Data.currentUser.Librarian_Permisions)
            {
                frmMemberFrame.Source = new Uri("BookSearch.xaml", UriKind.Relative);
            }
            else
            {
                AddBooks addBook = new AddBooks();
                addBook.Show();
               
            }
        }

        private void btnReservations_Click(object sender, RoutedEventArgs e)
        {
            frmMemberFrame.Source = new Uri("Reservations.xaml", UriKind.Relative);
        }

        private void btnFines_Click(object sender, RoutedEventArgs e)
        {
            frmMemberFrame.Visibility = Visibility.Visible;
            lblWarning.Visibility = Visibility.Hidden;
            frmMemberFrame.Source = new Uri("Fines.xaml", UriKind.Relative);
        }

        public void EnableAccess()
        {

            btnSearchBooks.IsEnabled = true;
            btnReservations.IsEnabled = true;
            btnHistoryMember.IsEnabled = true;
            frmMemberFrame.Visibility = Visibility.Visible;
            lblWarning.Visibility = Visibility.Hidden;

        }
    }
}
