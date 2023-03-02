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
    /// Interaction logic for MemberDetailPage.xaml
    /// </summary>
    public partial class MemberDetailPage : Page
    {


        public MemberDetailPage()
        {
            InitializeComponent();
            Show_User_Details();
        }

        public void Show_User_Details()
        {
            txtblkUserID.Text = "User ID: " + User_Data.currentUser.User_id;
            txtblkName.Text = "Name: " + User_Data.currentUser.First_name + " " + User_Data.currentUser.Last_name;
            txtblkPhone.Text = "Phone Number: " + User_Data.currentUser.Phone_number;
            txtblkEmail.Text = "Email Address: " + User_Data.currentUser.Email_address;
            txtblkAddress.Text = "Address: " + User_Data.currentUser.HomeAddress;
        }

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            Allow_Edit();
        }

        private void Allow_Edit()
        {
            if (stkPanUserDetails.Visibility == Visibility.Visible)
            {
                stkPanUserDetails.Visibility = Visibility.Hidden;
                stkPanChangeDetails.Visibility = Visibility.Visible;
            } else
            {
                stkPanUserDetails.Visibility = Visibility.Visible;
                stkPanChangeDetails.Visibility = Visibility.Hidden;
            }


        }

        private void txtBoxChangePhone_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxChangeEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxChangeAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnChangeDetails_Click(object sender, RoutedEventArgs e)
        {
            Changing_Details.Change_Details(txtBoxChangeAddress.Text);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Allow_Edit();
        }
    }
}

