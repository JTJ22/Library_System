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
                                XmlNode homeAddress = logins.SelectSingleNode($"/Logins/member[UserID='{User_Data.currentUser.User_id}']/Address");
                                homeAddress.InnerText = address;
                                User_Data.currentUser.HomeAddress = address;
                            }
                            if (User_Data.currentUser.Email_address != email && !string.IsNullOrWhiteSpace(email))
                            {
                                XmlNode emailAddress = logins.SelectSingleNode($"/Logins/member[UserID='{User_Data.currentUser.User_id}']/EmailAddress");
                                emailAddress.InnerText = email;
                                User_Data.currentUser.Email_address = email;
                            }
                            if (User_Data.currentUser.Phone_number != phoneNumber && !string.IsNullOrWhiteSpace(phoneNumber))
                            {
                                XmlNode phoneNo = logins.SelectSingleNode($"/Logins/member[UserID='{User_Data.currentUser.User_id}']/PhoneNumber");
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
                    XmlNode passwordChanged = password.SelectSingleNode($"/Logins/member[UserID='{User_Data.currentUser.User_id}']/Password");
                    currentPass = newPass;
                    passwordChanged.InnerText = newPass;
                    User_Data.currentUser.password = newPass;
                    password.Save("LibraryLogins.xml");
                    MessageBox.Show("Password Changed");
                }
            }
        }
    }

    public class User_Record
    {
        public string User_Id { get; set; }
        public string Book_Name { get; set; }
        public string Unique_Id { get; set; }
        public string Withdraw_Date { get; set; }
        public string Return_Expected { get; set; }
        public string Return_Actual { get; set; }
        public bool Is_Returned { get; set; }

        public static List<User_Record> DisplayRecord()
        {
            //Creating a list of book, the XML file is read and each "SingleBook" node is added to the list, this made it much easier to create data grids
            List<User_Record> records = new List<User_Record>();
            XmlDocument recordFile = new XmlDocument();
            recordFile.Load("LibraryHistory.xml");

            foreach (XmlNode node in recordFile.SelectNodes("/History/Record"))
            {
                if (User_Data.currentUser.User_id == node.SelectSingleNode("UserID").InnerText)
                {
                    User_Record record = new User_Record
                    {
                        User_Id = node.SelectSingleNode("UserID").InnerText,
                        Book_Name = node.SelectSingleNode("Book").InnerText,
                        Unique_Id = node.SelectSingleNode("UniqueID").InnerText,
                        Withdraw_Date = node.SelectSingleNode("WithdrawDate").InnerText,
                        Return_Expected = node.SelectSingleNode("ReturnExpected").InnerText,
                        Return_Actual = node.SelectSingleNode("ReturnActual").InnerText,
                        Is_Returned = bool.Parse(node.SelectSingleNode("IsBookReturned").InnerText),

                    };
                    records.Add(record);
                }
            }
            return records;
            //Lastly the list "books" is returned, this made is so the list is easily accessible elsewhere in the code (Same reason it is public static)
            
            }

       public static void RecordAdjust(User_Record bookReturned)
        {
            XmlDocument recordFileToChange = new XmlDocument();
            recordFileToChange.Load("LibraryHistory.xml");
            foreach(XmlNode node in recordFileToChange.SelectNodes("/History/Record"))
            {
                if (node.SelectSingleNode("UserID").InnerText == bookReturned.User_Id && node.SelectSingleNode("UniqueID").InnerText == bookReturned.Unique_Id)
                {
                    XmlNode returnActual = recordFileToChange.DocumentElement.SelectSingleNode($"/History/Record[UniqueID='{bookReturned.Unique_Id}'][UserID='{bookReturned.User_Id}'][WithdrawDate='{bookReturned.Withdraw_Date}']/ReturnActual");
                    returnActual.InnerText = Convert.ToString(DateTime.Now.Date);
                    XmlNode isBookReturned = recordFileToChange.SelectSingleNode($"/History/Record[UniqueID='{bookReturned.Unique_Id}'][UserID='{bookReturned.User_Id}'][WithdrawDate='{bookReturned.Withdraw_Date}']/IsBookReturned");
                    isBookReturned.InnerText = "true";
                    recordFileToChange.Save("LibraryHistory.xml");
                }
            }
        }
    }

    public class Reserving
    {
        public string User_ID { get; set; }
        public string Book_Name { get; set; }
        public string Unique_Id { get; set; }
        public string Time_Reserved { get; set; }
        public string Expiration { get; set; }
        public bool Reserve_Complete { get; set; }
        public static void Reserved(SingleBook reservedBook)
        {
            if (reservedBook.Availability == false)
            {
                if (reservedBook.IsReserved == false)
                {
                    XmlDocument reservations = new XmlDocument();
                    reservations.Load("LibraryReservations.xml");
                    XmlNode Root = reservations.SelectSingleNode("/Reservations");
                    XmlNode Record = reservations.CreateElement("Record");
                    XmlNode UserID = reservations.CreateElement("UserID");
                    XmlNode Book = reservations.CreateElement("Book");
                    XmlNode Unique_ID = reservations.CreateElement("UniqueID");
                    XmlNode Time_Reserved = reservations.CreateElement("TimeReserved");
                    XmlNode Expiration = reservations.CreateElement("Expires");
                    XmlNode Reserve_Complete = reservations.CreateElement("ReserveComplete");

                    UserID.InnerText = User_Data.currentUser.User_id;
                    Book.InnerText = reservedBook.Title;
                    Unique_ID.InnerText = reservedBook.Unique_ID;
                    Time_Reserved.InnerText = Convert.ToString(DateTime.Now);
                    Expiration.InnerText = Convert.ToString(DateTime.Now.AddMonths(1));
                    Reserve_Complete.InnerText = "false";

                    Record.AppendChild(UserID);
                    Record.AppendChild(Book);
                    Record.AppendChild(Unique_ID);
                    Record.AppendChild(Time_Reserved);
                    Record.AppendChild(Expiration);
                    Record.AppendChild(Reserve_Complete);
                    Root.AppendChild(Record);
                    Update_Book(reservedBook);
                    reservations.Save("LibraryReservations.xml");
                    MessageBox.Show("Book Reserved");
                }
                else
                {
                    MessageBox.Show("Book is reserved elsewhere");
                }
            }
            else
            {
                MessageBox.Show("Book is unavailable");
            }
        }

        public static void Update_Book(SingleBook reservedBook)
        {
            XmlDocument books = new XmlDocument();
            books.Load("LibraryBooks.xml");
            foreach (XmlNode book in books.SelectNodes("/Books/SingleBook"))
            {
                if (book.SelectSingleNode("UniqueID").InnerText == reservedBook.Unique_ID && reservedBook.IsReserved == false)
                {
                    XmlNode bookReserved = books.SelectSingleNode($"/Books/SingleBook[UniqueID='{reservedBook.Unique_ID}']/IsReserved");
                    bookReserved.InnerText = "true";
                    reservedBook.Availability = false;
                    reservedBook.IsReserved = true;
                    books.Save("LibraryBooks.xml");
                }
            }
        }

        public static List<Reserving> Display_Reservations()
        {
            List<Reserving> reserves = new List<Reserving>();
            XmlDocument reserveFile = new XmlDocument();
            reserveFile.Load("LibraryReservations.xml");

            foreach (XmlNode record in reserveFile.SelectNodes("/Reservations/Record"))
            {
                if (User_Data.currentUser.User_id == record.SelectSingleNode("UserID").InnerText)
                {
                    Reserving reserveRecord = new Reserving
                    {
                        User_ID = record.SelectSingleNode("UserID").InnerText,
                        Book_Name = record.SelectSingleNode("Book").InnerText,
                        Unique_Id = record.SelectSingleNode("UniqueID").InnerText,
                        Time_Reserved = record.SelectSingleNode("TimeReserved").InnerText,
                        Expiration = record.SelectSingleNode("Expires").InnerText,
                        Reserve_Complete = bool.Parse(record.SelectSingleNode("ReserveComplete").InnerText),
                    };
                    reserves.Add(reserveRecord);
                }
            }
            return reserves;
        }

        public static void Cancel_Reservation(Reserving cancelled)
        {
            XmlDocument bookEdit = new XmlDocument();
            bookEdit.Load("LibraryBooks.xml");
            foreach (XmlNode book in bookEdit.SelectNodes("/Books/SingleBook"))
            {
                if (cancelled.Unique_Id == book.SelectSingleNode("UniqueID").InnerText)
                {
                    XmlNode bookEdited = bookEdit.SelectSingleNode($"/Books/SingleBook[UniqueID='{cancelled.Unique_Id}']/IsReserved");
                    bookEdited.InnerText = "false";
                    bookEdit.Save("LibraryBooks.xml");
                }
            }

            if (cancelled != null)
            {
                XmlDocument cancelledDoc = new XmlDocument();
                cancelledDoc.Load("LibraryReservations.xml");
                XmlNode removed = cancelledDoc.SelectSingleNode($"/Reservations/Record[UniqueID='{cancelled.Unique_Id}']");
                removed.ParentNode.RemoveChild(removed);
                cancelledDoc.Save("LibraryReservations.xml");
                MessageBox.Show("Reservation Cancelled");
            }


        }


    }




}