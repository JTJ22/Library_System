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
                    if (Valid_Email(email))
                    {
                        XElement emailAddress = currentUser.Element("EmailAddress");
                        emailAddress.Value = email;
                        User_Data.currentUser.Email_address = email;
                    }
                    else
                    {
                        MessageBox.Show("Email is invalid");
                    }

                }

                if (!string.IsNullOrWhiteSpace(phoneNumber) && User_Data.currentUser.Phone_number != phoneNumber)
                {
                    XElement phoneNo = currentUser.Element("PhoneNumber");
                    phoneNo.Value = phoneNumber;
                    User_Data.currentUser.Phone_number = phoneNumber;
                }

                if ((!string.IsNullOrWhiteSpace(address) && User_Data.currentUser.HomeAddress != address) || (!string.IsNullOrWhiteSpace(email) && Valid_Email(email) && User_Data.currentUser.Email_address != email) || (!string.IsNullOrWhiteSpace(phoneNumber) && User_Data.currentUser.Phone_number != phoneNumber))
                {
                    MessageBox.Show("Details Changed");
                }
                else if (string.IsNullOrWhiteSpace(address) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("No Details were input");
                }
            }

            logins.Save("LibraryLogins.xml");
        }//Changes the users details based on what they input, changes the xml file

        public static void Change_Details_Librarian(string address, string email, string phoneNumber)
        {
            XDocument logins = XDocument.Load("LibraryLogins.xml");

            XElement currentUser = logins.Descendants("member")
                                        .FirstOrDefault(member => (string)member.Element("UserID") == User_Data.impersonateUser.User_id);

            if (currentUser != null)
            {
                if (!string.IsNullOrWhiteSpace(address) && User_Data.impersonateUser.User_id != address)
                {
                    XElement homeAddress = currentUser.Element("Address");
                    homeAddress.Value = address;
                    User_Data.impersonateUser.HomeAddress = address;
                }

                if (!string.IsNullOrWhiteSpace(email) && User_Data.impersonateUser.User_id != email)
                {
                    if (Valid_Email(email))
                    {
                        XElement emailAddress = currentUser.Element("EmailAddress");
                        emailAddress.Value = email;
                        User_Data.impersonateUser.Email_address = email;
                    }
                    else
                    {
                        MessageBox.Show("Email is invalid");
                    }

                }

                if (!string.IsNullOrWhiteSpace(phoneNumber) && User_Data.impersonateUser.User_id != phoneNumber)
                {
                    XElement phoneNo = currentUser.Element("PhoneNumber");
                    phoneNo.Value = phoneNumber;
                    User_Data.impersonateUser.Phone_number = phoneNumber;
                }

                if ((!string.IsNullOrWhiteSpace(address) && User_Data.impersonateUser.User_id != address) || (!string.IsNullOrWhiteSpace(email) && Valid_Email(email) && User_Data.impersonateUser.User_id != email) || (!string.IsNullOrWhiteSpace(phoneNumber) && User_Data.impersonateUser.User_id != phoneNumber))
                {
                    MessageBox.Show("Details Changed");
                }
                else if (string.IsNullOrWhiteSpace(address) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
                {
                    MessageBox.Show("No Details were input");
                }
            }

            logins.Save("LibraryLogins.xml");
        }//Changes the users details based on what they input, changes the xml file

        public static bool Valid_Email(string email)
        {
            try
            {
                MailAddress isValid = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static void Change_Password(string currentPass, string newPass)
        {
            if (!User_Data.currentUser.Librarian_Permisions)
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
            }
            else if (User_Data.currentUser.Librarian_Permisions)
            {
                XDocument password = XDocument.Load("LibraryLogins.xml");
                XElement member = password.Descendants("member")
                                     .FirstOrDefault(user => (string)user.Element("UserID") == User_Data.impersonateUser.User_id
                                                        && (string)user.Element("Password") == currentPass);
                if (member != null)
                {
                    member.Element("Password").Value = newPass;
                    User_Data.impersonateUser.password = newPass;
                    password.Save("LibraryLogins.xml");
                    MessageBox.Show("Password Changed");
                }
            }
        }//A seperate method for passwords. 

     




    }

}


