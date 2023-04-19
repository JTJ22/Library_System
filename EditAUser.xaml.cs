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
    /// Interaction logic for EditAUser.xaml
    /// </summary>
    /// 
    public delegate void ChangeSource(object sender, EventArgs e);
    public partial class EditAUser : Page
    {
        public static ChangeSource ChangePage;
        public EditAUser()
        {
            InitializeComponent();
            Grid_Creator();

            MemberDetailPage.OnRefreshDetails += (o, _) =>
            {
                Grid_Creator();
            };
        }


        private void dgShowUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Grid_Creator()
        { 
            dgShowUsers.ItemsSource = User_Data.currentUser.Display_Users();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Go_To_Page();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            User_Data deletedUser = dgShowUsers.SelectedItem as User_Data;
            deletedUser.Delete_User(deletedUser);
            Grid_Creator();
           
        }
        private void frmMember_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Go_To_Page()
        {
            User_Data.impersonateUser = (dgShowUsers.SelectedItem as User_Data);
            ChangeMemberDeets page = new ChangeMemberDeets();
            page.Show();
        }

        private void btnAddMember_Click(object sender, RoutedEventArgs e)
        {
            ChangePage?.Invoke(this, new EventArgs());
        }

        private void txtBoxSearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<User_Data> users = User_Data.currentUser.Display_Users();
            List<User_Data> filteredUsers = User_Data.currentUser.Filter_Users(users, txtBoxSearchUser.Text.ToLower());
            this.dgShowUsers.ItemsSource = filteredUsers;
        }
    }
}
