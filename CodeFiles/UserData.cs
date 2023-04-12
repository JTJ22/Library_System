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
using System.ComponentModel;
namespace Library_System
{
    public class User_Data
    {
        public User_Data()
        {

        }
        public string User_id 
        { 
            get; 
            set; 
        }
        public string password { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Phone_number { get; set; }
        public string Email_address { get; set; }
        public bool Librarian_Permisions { get; set; }
        public string HomeAddress { get; set; }

        public static User_Data currentUser = new User_Data();
        public static User_Data impersonateUser = new User_Data();

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
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Dispatcher.Invoke(() =>
            {
                (Application.Current.MainWindow as Window)?.Close();
            });
            Application.Current.MainWindow = mainWindow;
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
            memberLoggedIn.Show();
            Application.Current.Dispatcher.Invoke(() =>
            {
                (Application.Current.MainWindow as MainWindow)?.Close();
            });
            Application.Current.MainWindow = memberLoggedIn;
        }//Upon logging in a new window is opened. The mainwindow is hidden. 

        public List<User_Data> Display_Users()
        {
            List<User_Data> users = new List<User_Data>();
            XDocument reservingFile;
            reservingFile = XDocument.Load("LibraryLogins.xml");
            IEnumerable<User_Data> myUsers = from record in reservingFile.Descendants("member")
                                           select new User_Data
                                           {
                                               User_id = (string)record.Element("UserID"),
                                               password = (string)record.Element("Password"),
                                               First_name = (string)record.Element("FirstName"),
                                               Last_name = (string)record.Element("Surname"),
                                               Phone_number = (string)record.Element("PhoneNumber"),
                                               Email_address = (string)record.Element("EmailAddress"),
                                               Librarian_Permisions = (bool)record.Element("LibrarianPerms"),
                                               HomeAddress = (string)record.Element("Address")
                                           };
            users.AddRange(myUsers);
            return users;
        }

        public void Create_Member(string password, string firstName, string lastName, string number, string email, bool perms, string address)
        {
            MessageBoxResult confirm = MessageBox.Show("Add this member?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                XDocument logins = XDocument.Load("LibraryLogins.xml");
                HashSet<string> checkID = new HashSet<string>(logins.Descendants("member")
                    .Select(ID => (string)ID.Attribute("UserID")));

                string newId;
                do
                {
                    newId = new Random().Next(100000, 999999).ToString();
                } while (checkID.Contains(newId));

                if (Changing_Details.Valid_Email(email))
                {
                    User_Data newUser = new User_Data
                    {
                        User_id = newId,
                        password = password,
                        First_name = firstName,
                        Last_name = lastName,
                        Phone_number = number,
                        Email_address = email,
                        Librarian_Permisions = perms,
                        HomeAddress = address
                    };
                    Add_User(newUser);
                }
                else
                {
                    MessageBox.Show("Invalid Email");
                }
            }
            
        }
        public void Add_User(User_Data newUser)
        {
            if (newUser != null)//Checks if any of the input details are blank, to prevent a blank book being made
            {
                XDocument logins;
                logins = XDocument.Load("LibraryLogins.xml");
                XElement newMember = new XElement("member",
                new XElement("UserID", newUser.User_id),
                new XElement("Password", newUser.password),
                new XElement("FirstName", newUser.First_name),
                new XElement("Surname", newUser.Last_name),
                new XElement("PhoneNumber", newUser.Phone_number),
                new XElement("EmailAddress", newUser.Email_address),
                new XElement("LibrarianPerms", newUser.Librarian_Permisions),
                new XElement("Address", newUser.HomeAddress));
                logins.Root.Add(newMember);
                logins.Save("LibraryLogins.xml");
                MessageBox.Show("User Added");
            }
        }

        public void Delete_User(User_Data deletedUser)
        {
            XDocument users;
            users = XDocument.Load("LibraryLogins.xml");
            if (deletedUser != null)
            {
                foreach (User_Record record in User_Record.UserRecordInstance.DisplayRecord())
                {
                    if (record.User_Id == deletedUser.User_id && record.Is_Returned == false)
                    {
                        MessageBoxResult userWarn = MessageBox.Show("This user has outstanding books/reservations. Please confirm book location. Continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (userWarn == MessageBoxResult.Yes)
                        {
                            User_Record.UserRecordInstance.User_Deleted(deletedUser.User_id);
                            Reserving.reservingInstance.User_Deleted(deletedUser.User_id);
            
                            XElement userToRemove = users.Descendants("member")
                                                     .SingleOrDefault(records => (string)records.Element("UserID") == deletedUser.User_id);
                            userToRemove.Remove();
                            users.Save("LibraryLogins.xml");
                            MessageBox.Show($"Deleted. Please adjust the book record {record.Unique_Id}");
                            return;
                        }
                    }
                }
                XElement userToRemove2 = users.Descendants("member")
                                         .SingleOrDefault(records => (string)records.Element("UserID") == deletedUser.User_id);
                userToRemove2.Remove();
                users.Save("LibraryLogins.xml");
                MessageBox.Show("User Deleted");



            }
        }
    }
}






