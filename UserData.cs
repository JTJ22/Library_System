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
        public string Last_name { get; set; }
        public string Phone_number { get; set; }
        public string Email_address { get; set; }
        public bool Librarian_Permisions { get; set; }
        public string HomeAddress { get; set; }

        public static User_Data currentUser = new User_Data();
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
            (Application.Current.MainWindow as MainWindow).Visibility = Visibility.Visible;
        } //Sets the current user back to empty. Then returns the application back to the mainwindow

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
                            Login_Action();
                        }
                        User_Record.UserRecordInstance.Late_Check();
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
            Reservation_Overdue();
            MessageBox.Show("You have logged in!");
            Logging.Logger($"User ID: '{currentUser.User_id}'  Name: '{currentUser.First_name}''{currentUser.Last_name}'");
            MemberLoginWindow memberLoggedIn = new MemberLoginWindow();
            (Application.Current.MainWindow as MainWindow).Visibility = Visibility.Hidden;
            (Application.Current.MainWindow as MainWindow).Close();
            Application.Current.MainWindow = memberLoggedIn;
            Application.Current.MainWindow.Show();
        }//Upon logging in a new window is opened. The mainwindow is hidden. 

        public void Reservation_Overdue()
        {
            XmlDocument reservations = new XmlDocument();
            reservations.Load("LibraryReservations.xml");
            foreach (XmlNode record in reservations.SelectNodes("/Reservations/Record"))
            {
                DateTime expiration = Convert.ToDateTime(record.SelectSingleNode("Expires").InnerText).Date;
                if (expiration < DateTime.Now.Date && record.SelectSingleNode("ReserveComplete").InnerText == "false")
                {
                    XmlNode removeRes = reservations.SelectSingleNode($"/Reservations/Record[UniqueID='{record.SelectSingleNode("UniqueID").InnerText}'][TimeReserved='{record.SelectSingleNode("TimeReserved").InnerText}']");
                    MessageBox.Show($"Reservation for '{record.SelectSingleNode("Book").InnerText}' has expired.");
                    removeRes.ParentNode.RemoveChild(removeRes);
                    reservations.Save("LibraryReservations.xml");


                    XmlDocument bookEdit = new XmlDocument();
                    bookEdit.Load("LibraryBooks.xml");
                    foreach (XmlNode book in bookEdit.SelectNodes("/Books/SingleBook"))
                    {
                        if (record.SelectSingleNode("UniqueID").InnerText == book.SelectSingleNode("UniqueID").InnerText)
                        {
                            XmlNode bookEdited = bookEdit.SelectSingleNode($"/Books/SingleBook[UniqueID='{record.SelectSingleNode("UniqueID").InnerText}']/IsReserved");
                            bookEdited.InnerText = "false";
                            bookEdit.Save("LibraryBooks.xml");
                        }
                    }
                }
            }


        }//Upon any user logging in the system checks every reservation. If a book is over it's expiry date the reservation is automatically cancelled.
    }
}

