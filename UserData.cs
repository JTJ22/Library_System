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
        public string First_name { get; set; }
        public string Last_name { get; set;  }
        public string Phone_number { get; set; }
        public string Email_address { get; set; }
        public string Librarian_Permisions { get; set;  }

        
        public User_Data(string userID, string firstName, string lastName, string phoneNumber, string emailAddress, string librarianPermissions)
        {  
            this.User_id = userID;
            this.First_name = firstName;
            this.Last_name = lastName;
            this.Phone_number = phoneNumber;
            this.Email_address = emailAddress;
            this.Librarian_Permisions = librarianPermissions;
        }

        public void Log_Out(User_Data user)
        {
            user.First_name = null;
            user.Last_name = null;
            user.User_id = null;
            user.Phone_number = null;
            user.Email_address = null;
            user.Librarian_Permisions = null;
            (Application.Current.MainWindow as MainWindow).Visibility = Visibility.Visible;
        }
    }
}
