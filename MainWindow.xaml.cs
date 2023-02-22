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
    public partial class MainWindow : Window
    {
        public User_Data user = new User_Data("", "", "", "", "", "");
       
        public MainWindow()
        {
            InitializeComponent();
            Time_Updater();
        }
        
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login_Checker();
        }

        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Login_Checker()
        {
            XmlDocument logins = new XmlDocument();
            logins.Load("LibraryLogins.xml");
            foreach (XmlNode member in logins.SelectNodes("/Logins/member"))
            {
                user.User_id = member.SelectSingleNode("UserID").InnerText;
                string password = member.SelectSingleNode("Password").InnerText;
                user.First_name = member.SelectSingleNode("FirstName").InnerText;
                user.Last_name = member.SelectSingleNode("Surname").InnerText;
                user.Phone_number = member.SelectSingleNode("PhoneNumber").InnerText;
                user.Email_address = member.SelectSingleNode("EmailAddress").InnerText;
                user.Librarian_Permisions = member.SelectSingleNode("LibrarianPerms")?.InnerText;
                if (user.User_id == TxtUsername.Text && password == PsdBoxPassword.Password)
                {
                       
                    if (user.Librarian_Permisions == "Yes")
                    {
                        /* MessageBox.Show("You have logged in!");
                         Window1 window2 = new Window1();
                         Visibility = Visibility.Hidden;
                         window2.Show();*/
                    }
                    Login_Action();
                    Logging.Logger(user.User_id + ", " + user.First_name + " " + user.Last_name);
                }
                else
                {
                    lblFailLogin.Content = "Incorrect details";
                }

            }
        }
        public void Login_Action()
        {
            MessageBox.Show("You have logged in!");
            MemberLoginWindow memberLoggedIn = new MemberLoginWindow();
            Visibility = Visibility.Hidden;
            TxtUsername.Clear();
            PsdBoxPassword.Clear();
            memberLoggedIn.Show();
        }

        private void Time_Updater()
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.txtblkDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            }, this.Dispatcher);
            timer.Start();

        }
    }
}
