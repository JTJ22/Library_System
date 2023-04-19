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
using System.Text.RegularExpressions;


namespace Library_System
{
    /// <summary>
    /// Interaction logic for AddMember.xaml
    /// </summary>
    public partial class AddMember : Page
    {
        public AddMember()
        {
            InitializeComponent();
        }

        private void txtBoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxEmailAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtBoxAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAddMember_Click(object sender, RoutedEventArgs e)
        {
            bool checkedBox = chkBoxLibrarianPerms.IsChecked ?? false;
            User_Data.currentUser.Create_Member(txtBoxPassword.Text, txtBoxFirstName.Text, txtBoxSurname.Text, txtBoxPhoneNumber.Text, txtBoxEmailAddress.Text, checkedBox, txtBoxAddress.Text);
            Refresh();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Refresh()
        {
            txtBoxPassword.Text = "";
            txtBoxFirstName.Text = "";
            txtBoxSurname.Text = "";
            txtBoxPhoneNumber.Text = "";
            txtBoxEmailAddress.Text = "";
            chkBoxLibrarianPerms.IsChecked = false;
            txtBoxAddress.Text = "";
        }
        private void txtBoxPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
