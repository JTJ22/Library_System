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

namespace Library_System
{
    public class User_Data
    {
        public string User_id { get; set; }
        public string password { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set;  }
        public string Phone_number { get; set; }
        public string Email_address { get; set; }
        public bool Librarian_Permisions { get; set;  }
        public string HomeAddress { get; set; }

        public static User_Data currentUser = new User_Data();
        public void Log_Out(User_Data user)
        {
            user.First_name = null;
            user.password = null;
            user.Last_name = null;
            user.User_id = null;
            user.Phone_number = null;
            user.Email_address = null;
            user.Librarian_Permisions = false;
            (Application.Current.MainWindow as MainWindow).Visibility = Visibility.Visible;
        }

        public User_Data Logging_In(string inputUserid, string inputPassword)
        {
            XmlDocument logins = new XmlDocument();
            logins.Load("LibraryLogins.xml");
            foreach (XmlNode member in logins.SelectNodes("/Logins/member"))
            {
                currentUser.User_id = member.SelectSingleNode("UserID").InnerText;
                currentUser.password = member.SelectSingleNode("Password").InnerText;
                currentUser.First_name = member.SelectSingleNode("FirstName").InnerText;
                currentUser.Last_name = member.SelectSingleNode("Surname").InnerText;
                currentUser.Phone_number = member.SelectSingleNode("PhoneNumber").InnerText;
                currentUser.Email_address = member.SelectSingleNode("EmailAddress").InnerText;
                currentUser.Librarian_Permisions = bool.Parse(member.SelectSingleNode("LibrarianPerms").InnerText);
                currentUser.HomeAddress = member.SelectSingleNode("Address").InnerText;

                if (currentUser.User_id != null)
                {
                    if (currentUser.User_id == inputUserid && currentUser.password == inputPassword)
                    {

                        if (currentUser.Librarian_Permisions == true)
                        {
                            /* MessageBox.Show("You have logged in!");
                             Window1 window2 = new Window1();
                             Visibility = Visibility.Hidden;
                             window2.Show();*/
                        }
                        Login_Action();
                        return currentUser;
                    }
                }
            }
            MessageBox.Show("Wrong Details");
            return null;
        }

        public void Login_Action()
        {
            MessageBox.Show("You have logged in!");
            Logging.Logger("User ID: " + currentUser.User_id + " Name: " + currentUser.First_name + " " + currentUser.Last_name);
            MemberLoginWindow memberLoggedIn = new MemberLoginWindow();
            (Application.Current.MainWindow as MainWindow).Visibility = Visibility.Hidden;
            memberLoggedIn.Show();
        }
    }
}

