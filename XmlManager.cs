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
                        if (User_Data.currentUser.HomeAddress != address && !string.IsNullOrWhiteSpace(address))
                        {
                            XmlNode homeAddress = logins.SelectSingleNode("/Logins/member/Address");
                            homeAddress.InnerText = address;
                            User_Data.currentUser.HomeAddress = address;
                        }
                        if(User_Data.currentUser.Email_address != email && !string.IsNullOrWhiteSpace(email))
                        {
                            XmlNode emailAddress = logins.SelectSingleNode("/Logins/member/EmailAddress");
                            emailAddress.InnerText = email;
                            User_Data.currentUser.Email_address = email;
                        }
                        if(User_Data.currentUser.Phone_number != phoneNumber && !string.IsNullOrWhiteSpace(phoneNumber))
                        {
                            XmlNode phoneNo = logins.SelectSingleNode("/Logins/member/PhoneNumber");
                            phoneNo.InnerText = phoneNumber;
                            User_Data.currentUser.Phone_number = phoneNumber;
                        }
                       
                    }
                }
            }
            logins.Save("LibraryLogins.xml");
            MessageBox.Show("Details Changed");
        }

        public void Member_History()
        {
            XmlDocument history = new XmlDocument();
            history.Load("LibraryHistory.xml");
        }
    }



}