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
using System.Text.RegularExpressions;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for MemberDetailPage.xaml
    /// </summary>
     public delegate void OnRefreshDetails(object sender, EventArgs e);
    public partial class MemberDetailPage : Page
    {
        public static OnRefreshDetails OnRefreshDetails;

        public MemberDetailPage()
        {
            InitializeComponent();
            Show_User_Details();

            //After the window has been opened the users details are shown within the stack panel
        }

        public void Show_User_Details()
        {
            if (!User_Data.currentUser.Librarian_Permisions)
            {
                txtblkUserID.Text = "User ID: " + User_Data.currentUser.User_id;
                txtblkName.Text = "Name: " + User_Data.currentUser.First_name + " " + User_Data.currentUser.Last_name;
                txtblkPhone.Text = "Phone Number: " + User_Data.currentUser.Phone_number;
                txtblkEmail.Text = "Email Address: " + User_Data.currentUser.Email_address;
                txtblkAddress.Text = "Address: " + User_Data.currentUser.HomeAddress;
            }
            else if(User_Data.currentUser.Librarian_Permisions)
            {
                txtblkUserID.Text = "User ID: " + User_Data.impersonateUser.User_id;
                txtblkName.Text = "Name: " + User_Data.impersonateUser.First_name + " " + User_Data.impersonateUser.Last_name;
                txtblkPhone.Text = "Phone Number: " + User_Data.impersonateUser.Phone_number;
                txtblkEmail.Text = "Email Address: " + User_Data.impersonateUser.Email_address;
                txtblkAddress.Text = "Address: " + User_Data.impersonateUser.HomeAddress;
            }
            //Assigning the text blocks to contains the users details.
        }

        private void btnEditDetails_Click(object sender, RoutedEventArgs e)
        {
            Allow_Edit();
            //In my program I played with the Visibility property. This was to avoid creating lots of pages.
        }

        private void Allow_Edit()
        {
            if (grdUserDetails.Visibility == Visibility.Visible)
            {
                grdUserDetails.Visibility = Visibility.Hidden;
                grdChangeDetails.Visibility = Visibility.Visible;

            }
            else
            {
                grdUserDetails.Visibility = Visibility.Visible;
                grdChangeDetails.Visibility = Visibility.Hidden;
            }

            //A method that checks the property of visibilty on the stack panels. This was to avoid using lots of pages and windows.
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
            if(!User_Data.currentUser.Librarian_Permisions)
            { 
                Changing_Details.Change_Details(txtBoxChangeAddress.Text, txtBoxChangeEmail.Text, txtBoxChangePhone.Text);
                Allow_Edit();
                NavigationService.Refresh();
            }
            else
            {
                Changing_Details.Change_Details_Librarian(txtBoxChangeAddress.Text, txtBoxChangeEmail.Text, txtBoxChangePhone.Text);
                Allow_Edit();
                OnRefreshDetails?.Invoke(this, new EventArgs());
                Show_User_Details();
            }
            //If details are changed the first method passes the data to another class, this is to change the information on the xml file and currentUser
            //Takes the user back to the first stack panel after changing the details
            //Lastly updates the page so the information displayed reflects what the user input
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Allow_Edit();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            //Another case of regex, this was so the mobile number could only recieve numerical values. 
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            grdPanChangePassword.Visibility = Visibility.Visible;
            grdChangeDetails.Visibility= Visibility.Hidden;
            //Similar to AllowEdit, hides the previous panel
        }

        private void btnPasswordChanged_Click(object sender, RoutedEventArgs e)
        {
            if (psdBoxNew.Password == psdBoxConfirm.Password)
            {
                Changing_Details.Change_Password(psdBoxCurrent.Password, psdBoxNew.Password);
                btnCancelPassword_Click(sender, e);
                //If user cancels returns them to the previous page
            }//Confirms that the 2 passwords match. 
            else
            {
                MessageBox.Show("Passwords Do Not Match");
            }
        }

        private void btnCancelPassword_Click(object sender, RoutedEventArgs e)
        {
            grdPanChangePassword.Visibility= Visibility.Hidden;
            grdChangeDetails.Visibility = Visibility.Visible;
        }

        private void txtBoxChangeEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
        }
    }
}

