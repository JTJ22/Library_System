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
using System.Xml.Linq;
namespace Library_System
{
    public class Changing_Details
    {
        public static void Change_Details(string address, string email, string phoneNumber)
        {
            XDocument logins = XDocument.Load("LibraryLogins.xml");

            XElement currentUser = logins.Descendants("member")
                                        .FirstOrDefault(member => (string)member.Element("UserID") == User_Data.currentUser.User_id);

            if (currentUser != null)
            {
                if (!string.IsNullOrWhiteSpace(address) && User_Data.currentUser.HomeAddress != address)
                {
                    XElement homeAddress = currentUser.Element("Address");
                    homeAddress.Value = address;
                    User_Data.currentUser.HomeAddress = address;
                }

                if (!string.IsNullOrWhiteSpace(email) && User_Data.currentUser.Email_address != email)
                {
                    XElement emailAddress = currentUser.Element("EmailAddress");
                    emailAddress.Value = email;
                    User_Data.currentUser.Email_address = email;
                }

                if (!string.IsNullOrWhiteSpace(phoneNumber) && User_Data.currentUser.Phone_number != phoneNumber)
                {
                    XElement phoneNo = currentUser.Element("PhoneNumber");
                    phoneNo.Value = phoneNumber;
                    User_Data.currentUser.Phone_number = phoneNumber;
                }

                if (!string.IsNullOrWhiteSpace(address) || !string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("Details Changed");
                }
                else
                {
                    MessageBox.Show("No Details were input");
                }
            }

            logins.Save("LibraryLogins.xml");
        }//Changes the users details based on what they input, changes the xml file

        public static void Change_Password(string currentPass, string newPass)
        {
            XDocument password = XDocument.Load("LibraryLogins.xml");
            XElement member = password.Descendants("member")
                                 .FirstOrDefault(user => (string)user.Element("UserID") == User_Data.currentUser.User_id
                                                    && (string)user.Element("Password") == currentPass);
            if (member != null)
            {
                member.Element("Password").Value = newPass;
                User_Data.currentUser.password = newPass;
                password.Save("LibraryLogins.xml");
                MessageBox.Show("Password Changed");
            }
        }//A seperate method for passwords. 
    }






}


