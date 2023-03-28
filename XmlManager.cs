﻿using System;
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
                if (member.SelectSingleNode("UserID").InnerText == User_Data.currentUser.User_id && currentPass == member.SelectSingleNode("Password").InnerText)
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
            //Creating a list of records, the XML file is read and each "User_Record" node is added to the list, this made it much easier to create data grids
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
            //Lastly the list "records" is returned, this made is so the list is easily accessible elsewhere in the code (Same reason it is public static)

        }

        public static void RecordAdjust(User_Record bookReturned)
        {
            XmlDocument recordFileToChange = new XmlDocument();
            recordFileToChange.Load("LibraryHistory.xml");
            foreach (XmlNode node in recordFileToChange.SelectNodes("/History/Record"))
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

        public static void Late_Check()
        {
            double fineCharge = 4.00;
            DateTime timeNow = DateTime.Now.Date; //Creating a varible for the time now. 
            foreach (var record in DisplayRecord())//Using the list returned by this method for my foreach
            {
                DateTime returnExpectedDate = Convert.ToDateTime(record.Return_Expected).Date; //Creating a variable that holds the expected return date.
                if (returnExpectedDate < timeNow && record.Is_Returned == false && record.Is_Returned == false)
                {
                    
                    int daysLate = (timeNow - returnExpectedDate).Days; //If the book is returned AFTER the expected date, this variable holds the difference in days.
                    if (daysLate >= 7)
                    {
                        int wholeWeeksLate = daysLate / 7;
                        int fineAmount = (wholeWeeksLate + 1) * (int)fineCharge;
                        Fining.Create_Fine(record.Unique_Id, record.User_Id, record.Book_Name, returnExpectedDate, fineAmount);
                        /*
                        MessageBox.Show($"You have been fined: £{fineAmount} for the overdue book {record.Book_Name}");*/
                        //I only charge if it has been more than a week, if that is true the cust is informed of the fine. 
                    }
                }
            }
        }

        public void Update_History(SingleBook bookBeingWithdrawn)
        {
            DateTime expected = DateTime.Now.AddMonths(1);
            XmlDocument history = new XmlDocument();
            history.Load("LibraryHistory.xml");
            XmlNode ROOT = history.SelectSingleNode("/History");
            XmlNode Record = history.CreateElement("Record");
            XmlNode UserID = history.CreateElement("UserID");
            XmlNode Book = history.CreateElement("Book");
            XmlNode UniqueID = history.CreateElement("UniqueID");
            XmlNode WithdrawDate = history.CreateElement("WithdrawDate");
            XmlNode ReturnExpected = history.CreateElement("ReturnExpected");
            XmlNode ReturnActual = history.CreateElement("ReturnActual");
            XmlNode IsBookReturned = history.CreateElement("IsBookReturned");

            UserID.InnerText = User_Data.currentUser.User_id;
            Book.InnerText = bookBeingWithdrawn.Title;
            UniqueID.InnerText = bookBeingWithdrawn.Unique_ID;
            WithdrawDate.InnerText = Convert.ToString(DateTime.Now);
            ReturnExpected.InnerText = expected.ToString();
            ReturnActual.InnerText = "0";
            IsBookReturned.InnerText = "false";

            Record.AppendChild(UserID);
            Record.AppendChild(Book);
            Record.AppendChild(UniqueID);
            Record.AppendChild(WithdrawDate);
            Record.AppendChild(ReturnExpected);
            Record.AppendChild(ReturnActual);
            Record.AppendChild(IsBookReturned);
            ROOT.AppendChild(Record);
            history.Save("LibraryHistory.xml");
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
        public void Reserved(SingleBook reservedBook)
        {
            if (ResChecker())
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
                        Expiration.InnerText = Convert.ToString(DateTime.Now.AddDays(4).Date);
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
            else
            {
                MessageBox.Show("You already have this book withdrawn");
            }
        }

        private bool ResChecker()
        {
            foreach (var record in User_Record.DisplayRecord())
            {
                if (record.User_Id == User_Data.currentUser.User_id)
                {
                    return false;
                }
            }
            return true;
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

        public void Complete_Reservation(Reserving record)
        {
            XmlDocument reserveFile = new XmlDocument();
            reserveFile.Load("LibraryReservations.xml");

            foreach (XmlNode records in reserveFile.SelectNodes("/Reservations/Record"))
            {
                if(record.User_ID == records.SelectSingleNode("/Reservations/Record/UserID").InnerText && record.Unique_Id == records.SelectSingleNode("/Reservations/Record/UniqueID").InnerText)
                {
                    XmlNode completed = reserveFile.SelectSingleNode($"/Reservations/Record[UniqueID='{record.Unique_Id}'][UserID='{record.User_ID}']/ReserveComplete");
                    completed.InnerText = "true";
                    record.Reserve_Complete = true;
                    reserveFile.Save("LibraryReservations.xml");
                }
            }
        }




    }

    public class Fining
    {
        public string User_ID { get; set; }
        public string Book_Name { get; set; }
        public string Unique_Id { get; set; }
        public string Expiration { get; set; }
        public double Fine_Value { get; set; }
        public bool Fine_Paid { get; set; }
        public static void Create_Fine(string UniqueID, string UserID, string bookName, DateTime returnExpected, int fineCost)
        {
            XmlDocument fines = new XmlDocument();
            fines.Load("LibraryFines.xml");
            if (!Does_Fine_Exist(UniqueID, UserID, returnExpected))
            {
                XmlNode Root = fines.SelectSingleNode("/Fines");
                XmlNode Record = fines.CreateElement("Record");
                XmlNode UserId = fines.CreateElement("UserID");
                XmlNode Book = fines.CreateElement("Book");
                XmlNode Unique_ID = fines.CreateElement("UniqueID");
                XmlNode Expiration = fines.CreateElement("Expires");
                XmlNode Fine_Cost = fines.CreateElement("FineValue");
                XmlNode FinePaid = fines.CreateElement("FinePaid");

                UserId.InnerText = User_Data.currentUser.User_id;
                Book.InnerText = bookName;
                Unique_ID.InnerText = UniqueID;
                Expiration.InnerText = returnExpected.ToString();
                Fine_Cost.InnerText = fineCost.ToString();
                FinePaid.InnerText = "false";

                Record.AppendChild(UserId);
                Record.AppendChild(Book);
                Record.AppendChild(Unique_ID);
                Record.AppendChild(Expiration);
                Record.AppendChild(FinePaid);
                Record.AppendChild(Fine_Cost);
                Root.AppendChild(Record);
                fines.Save("LibraryFines.xml");
            }
            else
            {
                XmlNode fineUpdate = fines.SelectSingleNode($"/Fines/Record[UniqueID='{UniqueID}'][UserID='{UserID}'][Book='{bookName}'][Expires='{returnExpected}']/FineValue");
                fineUpdate.InnerText = string.Format("{0:C}", fineCost);
                fines.Save("LibraryFines.xml");
            }
        }

        public static bool Does_Fine_Exist(string UniqueID, string UserID, DateTime returnExpected)
        {
            XmlDocument fines = new XmlDocument();
            fines.Load("LibraryFines.xml");
            
            foreach (XmlNode fine in fines.SelectNodes("/Fines/Record"))
            {

                if (fine.SelectSingleNode("UniqueID").InnerText == UniqueID && fine.SelectSingleNode("UserID").InnerText == UserID && fine.SelectSingleNode("Expires").InnerText == returnExpected.ToString() && fine.SelectSingleNode("FinePaid").InnerText == "false")
                {
                    return true;
                }
                
            }
            return false;



        }

        }
    }


