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
    public class Changing_Details
    { 
        public static void Change_Details(string address)
        {

            XmlDocument logins = new XmlDocument();
            logins.Load("LibraryLogins.xml");
            foreach (XmlNode member in logins.SelectNodes("/Logins/member"))
            {
                if (member.SelectSingleNode("UserID") != null)
                {
                    if (member.SelectSingleNode("UserID").InnerText == User_Data.currentUser.User_id)
                    {
                        XmlNode homeAddress = logins.SelectSingleNode("/Logins/member/Address");
                        homeAddress.InnerText = address;
                        User_Data.currentUser.HomeAddress = address;
                        logins.Save("LibraryLogins.xml");
                    }
                }
            }
        }
    }
}