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
using System.Xml.Linq;

namespace Library_System
{
    public class User_Data
    {
        private User_Data()
        {

        }
        public string User_id { get; set; }
        public string password { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Phone_number { get; set; }
        public string Email_address { get; set; }
        public bool Librarian_Permisions { get; set; }
        public string HomeAddress { get; set; }

        private static User_Data CurrentUser = new User_Data();
        private static User_Data ImpersonateUser = new User_Data();
        public static User_Data currentUser
            {
                get
            {
                return CurrentUser;
            }       
            }
        public static User_Data impersonateUser
        {
            get 
            {
                return ImpersonateUser;
            }
        }
        //I decided to use a public static instance of a user. This was so i could access the data much easier. 
        public void Log_Out()
        {
            currentUser.First_name = null;
            currentUser.password = null;
            currentUser.Last_name = null;
            currentUser.User_id = null;
            currentUser.Phone_number = null;
            currentUser.Email_address = null;
            currentUser.Librarian_Permisions = false;
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        } //Sets the current user back to empty. Then returns the application back to the mainwindow

        public User_Data Logging_In(string inputUserid, string inputPassword)
        {
            XDocument logins;
            logins = XDocument.Load("LibraryLogins.xml");
            foreach (XElement member in logins.Descendants("member"))
            {
                currentUser.User_id = member.Element("UserID").Value;
                currentUser.password = member.Element("Password").Value;
                currentUser.First_name = member.Element("FirstName").Value;
                currentUser.Last_name = member.Element("Surname").Value;
                currentUser.Phone_number = member.Element("PhoneNumber").Value;
                currentUser.Email_address = member.Element("EmailAddress").Value;
                currentUser.Librarian_Permisions = bool.Parse(member.Element("LibrarianPerms").Value);
                currentUser.HomeAddress = member.Element("Address").Value;

                if (!string.IsNullOrWhiteSpace(currentUser.User_id))
                {
                    if (currentUser.User_id == inputUserid && currentUser.password == inputPassword)
                    {
                        if (currentUser.Librarian_Permisions)
                        {
                            Login_Action();
                            return currentUser;
                        }
                        if (!currentUser.Librarian_Permisions)
                        {
                            User_Record.UserRecordInstance.Late_Check();
                            Login_Action();
                            if (Reserving.reservingInstance.Notify_Res())
                            {
                                MessageBox.Show("Reservation ready to pick up");
                            }
                            return currentUser;
                        }
                    }
                }
            }
            MessageBox.Show("Wrong Details");
            return null;
        }//Checks the input details against records in the system. If there's a match the user is logged in. Logins differ between librarians and users. 

        private void Login_Action()
        {
            Reserving.reservingInstance.Expired_Reservation();
            MessageBox.Show("You have logged in!");
            Logging.Logger($"User ID: '{currentUser.User_id}'  Name: '{currentUser.First_name}''{currentUser.Last_name}'");
            MemberLoginWindow memberLoggedIn = new MemberLoginWindow();
            (Application.Current.MainWindow as MainWindow).Visibility = Visibility.Hidden;
            (Application.Current.MainWindow as MainWindow).Close();
            Application.Current.MainWindow = memberLoggedIn;
            Application.Current.MainWindow.Show();
        }//Upon logging in a new window is opened. The mainwindow is hidden. 
    }
}

