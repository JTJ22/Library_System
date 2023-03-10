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
using System.Net.Mail;
namespace Library_System
{
    public class Changing_Details
    { 
        public static void Change_Details(string address, string email, string phoneNumber)
        {

            XmlDocument logins = new XmlDocument();
            logins.Load("LibraryLogins.xml");
            foreach (XmlNode member in logins.SelectNodes("/Logins/member"))
            {
                if (member.SelectSingleNode("UserID") != null)
                {
                    if (member.SelectSingleNode("UserID").InnerText == User_Data.currentUser.User_id)
                    {
                        if (!string.IsNullOrWhiteSpace(address) || !string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(phoneNumber))
                        {
                            if (User_Data.currentUser.HomeAddress != address && !string.IsNullOrWhiteSpace(address))
                            {
                                XmlNode homeAddress = logins.SelectSingleNode("/Logins/member/Address");
                                homeAddress.InnerText = address;
                                User_Data.currentUser.HomeAddress = address;
                            }
                            if (User_Data.currentUser.Email_address != email && !string.IsNullOrWhiteSpace(email))
                            {
                                XmlNode emailAddress = logins.SelectSingleNode("/Logins/member/EmailAddress");
                                emailAddress.InnerText = email;
                                User_Data.currentUser.Email_address = email;
                            }
                            if (User_Data.currentUser.Phone_number != phoneNumber && !string.IsNullOrWhiteSpace(phoneNumber))
                            {
                                XmlNode phoneNo = logins.SelectSingleNode("/Logins/member/PhoneNumber");
                                phoneNo.InnerText = phoneNumber;
                                User_Data.currentUser.Phone_number = phoneNumber;
                            }
                            MessageBox.Show("Details Changed");
                        }
                        else
                        {
                            MessageBox.Show("No Details were input");
                        }

                    }
                }
            }
            logins.Save("LibraryLogins.xml");
        }
        
        public static void Change_Password(string currentPass, string newPass)
        {
            XmlDocument password = new XmlDocument();
            password.Load("LibraryLogins.xml");
            foreach (XmlNode member in password.SelectNodes("/Logins/member"))
            {
                if(member.SelectSingleNode("UserID").InnerText == User_Data.currentUser.User_id && currentPass == member.SelectSingleNode("Password").InnerText)
                {
                    XmlNode passwordChanged = password.SelectSingleNode("/Logins/member/Password");
                    currentPass = newPass;
                    passwordChanged.InnerText = newPass;
                    User_Data.currentUser.password = newPass;
                    password.Save("LibraryLogins.xml");
                    MessageBox.Show("Password Changed");
                }
            }
        }
        public void Member_History()
        {
            XmlDocument history = new XmlDocument();
            history.Load("LibraryHistory.xml");
        }
    }





}