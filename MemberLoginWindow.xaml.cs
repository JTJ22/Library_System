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
using System.Windows.Threading;

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
        }
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Log_Out_Event(sender, e);
        }

        public void Log_Out_Event(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have logged out");
            Close();
            User_Data.currentUser.Log_Out(User_Data.currentUser);
        }


        private void btnMemberDetails_Click(object sender, RoutedEventArgs e)
        {
            frmMemberFrame.Source = new Uri("MemberDetailPage.xaml", UriKind.Relative);
            
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
    }
}
