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
    public class User_Record
    {
        public string User_Id { get; set; }
        public string Book_Name { get; set; }
        public Guid Unique_Id { get; set; }
        public string Withdraw_Date { get; set; }
        public string Return_Expected { get; set; }
        public string Return_Actual { get; set; }
        public bool Is_Returned { get; set; }
        public bool Is_Renewed { get; set; }

        public static User_Record UserRecordInstance = new User_Record();

        public List<User_Record> DisplayRecord()
        {
            //Creating a list of records, the XML file is read and each "User_Record" node is added to the list, this made it much easier to create data grids
            if (User_Data.currentUser.Librarian_Permisions)
            {


                List<User_Record> records = new List<User_Record>();
                XDocument recordFile;
                recordFile = XDocument.Load("LibraryHistory.xml");
                IEnumerable<User_Record> create = from record in recordFile.Descendants("Record")
                                                  select new User_Record
                                                  {
                                                      User_Id = (string)record.Element("UserID"),
                                                      Book_Name = (string)record.Element("Book"),
                                                      Unique_Id = (Guid)record.Attribute("UniqueID"),
                                                      Withdraw_Date = (string)record.Element("WithdrawDate"),
                                                      Return_Expected = (string)record.Element("ReturnExpected"),
                                                      Return_Actual = (string)record.Element("ReturnActual"),
                                                      Is_Returned = bool.Parse((string)record.Element("IsBookReturned")),
                                                      Is_Renewed = bool.Parse((string)record.Element("IsRenewed"))
                                                  };
                records.AddRange(create);
                return records;
            }
            else if (!User_Data.currentUser.Librarian_Permisions)
            {

                List<User_Record> records = new List<User_Record>();
                XDocument recordFile;
                recordFile = XDocument.Load("LibraryHistory.xml");
                IEnumerable<User_Record> create = from record in recordFile.Descendants("Record")
                                                  where (string)record.Element("UserID") == User_Data.currentUser.User_id
                                                  select new User_Record
                                                  {
                                                      User_Id = (string)record.Element("UserID"),
                                                      Book_Name = (string)record.Element("Book"),
                                                      Unique_Id = (Guid)record.Attribute("UniqueID"),
                                                      Withdraw_Date = (string)record.Element("WithdrawDate"),
                                                      Return_Expected = (string)record.Element("ReturnExpected"),
                                                      Return_Actual = (string)record.Element("ReturnActual"),
                                                      Is_Returned = bool.Parse((string)record.Element("IsBookReturned")),
                                                      Is_Renewed = bool.Parse((string)record.Element("IsRenewed"))
                                                  };
                records.AddRange(create);
                return records;
            }
            return null;
        }

        public void RecordAdjust(User_Record bookReturned)
        {
            XDocument records;
            records = XDocument.Load("LibraryHistory.xml");
            XElement recordToChange = records.Descendants("Record")
                                     .SingleOrDefault(record => (Guid)record.Attribute("UniqueID") == bookReturned.Unique_Id &&
                                     (string)record.Element("UserID") == bookReturned.User_Id &&
                                     (string)record.Element("WithdrawDate") == bookReturned.Withdraw_Date);
            if (recordToChange != null)
            {
                if ((bool)recordToChange.Element("IsBookReturned") == false)
                {
                    recordToChange.SetElementValue("ReturnActual", Convert.ToString(DateTime.Now.Date));
                    bookReturned.Is_Returned = true;
                    recordToChange.SetElementValue("IsBookReturned", "true");
                    records.Save("LibraryHistory.xml");
                }
            }
        }
        //When the book is returned the relevant record is updated



        public void Renew_Book(User_Record recordToChange)
        {
            XDocument records;
            records = XDocument.Load("LibraryHistory.xml");
            XElement recordToRenew = records.Descendants("Record")
                                     .SingleOrDefault(record => (Guid)record.Attribute("UniqueID") == recordToChange.Unique_Id &&
                                     (string)record.Element("UserID") == recordToChange.User_Id &&
                                     (string)record.Element("WithdrawDate") == recordToChange.Withdraw_Date);
            if (recordToRenew != null)
            {
                if ((bool?)recordToRenew.Element("IsRenewed") == false && (bool)recordToRenew.Element("IsBookReturned") == false)
                {
                    DateTime returnExpected = Convert.ToDateTime(recordToChange.Return_Expected);
                    DateTime currentDate = DateTime.Now;
                    TimeSpan timeCheck = returnExpected.Subtract(currentDate);
                    if (timeCheck.Days >= 1 && timeCheck.Days <= 7)
                    {
                        recordToRenew.SetElementValue("IsRenewed", "true");
                        recordToChange.Is_Renewed = true;
                        recordToRenew.SetElementValue("ReturnExpected", Convert.ToString(Convert.ToDateTime(recordToChange.Return_Expected).AddDays(7)));
                        records.Save("LibraryHistory.xml");
                        MessageBox.Show($"Renewed. New returned date is {Convert.ToString(Convert.ToDateTime(recordToChange.Return_Expected).AddDays(7))}");
                    }
                    else
                    {
                        MessageBox.Show("Must be within a week of the return date to renew");
                    }
                }
                else
                {
                    MessageBox.Show("Cannot be renewed, book is returned/renewed already");
                }
            }
        }
        //Renewing a book will update the return expected 


        public void Late_Check()
        {
            double fineCharge = 0.35;
            DateTime timeNow = DateTime.Now.Date; //Creating a varible for the time now. 
            foreach (User_Record record in DisplayRecord())//Using the list returned by this method for my foreach
            {
                if (record != null)
                {
                    DateTime returnExpectedDate = Convert.ToDateTime(record.Return_Expected); //Creating a variable that holds the expected return date.
                    if (returnExpectedDate < timeNow && record.Is_Returned == false)
                    {

                        int daysLate = (timeNow - returnExpectedDate).Days; //If the book is returned AFTER the expected date, this variable holds the difference in days.
                        if (daysLate >= 7)
                        {
                            int wholeWeeksLate = daysLate / 7;
                            double fineAmount = (wholeWeeksLate + 1) * (double)fineCharge;
                            Fining.finingInstance.Create_Fine(record.Unique_Id, record.User_Id, record.Book_Name, returnExpectedDate, fineAmount);
                            //I only charge if it has been more than a week, if that is true the cust is informed of the fine. 
                        }
                        else
                        {
                            Fining.finingInstance.Create_Fine(record.Unique_Id, record.User_Id, record.Book_Name, returnExpectedDate, 0);
                        }
                    }
                }
            }
        }

        public void Update_History(SingleBook bookBeingWithdrawn)
        {
            DateTime expected = DateTime.Now.AddMonths(1);
            XDocument records;
            records = XDocument.Load("LibraryHistory.xml");
            XElement newRecord = new XElement("Record",
            new XElement("UserID", User_Data.currentUser.User_id),
            new XElement("Book", bookBeingWithdrawn.Title),
            new XAttribute("UniqueID", bookBeingWithdrawn.Unique_ID),
            new XElement("WithdrawDate", Convert.ToString(DateTime.Now)),
            new XElement("ReturnExpected", expected.ToString()),
            new XElement("ReturnActual", "0"),
            new XElement("IsBookReturned", "false"),
            new XElement("IsRenewed", "false"));
            records.Root.Add(newRecord);
            records.Save("LibraryHistory.xml");

        } //Creating a history record in XML
        public User_Record Record_Finder(Guid UniqueID)
        {
            XDocument recordFind;

            recordFind = XDocument.Load("LibraryHistory.xml");
            XElement recordEl = recordFind.Descendants("Record")
                               .SingleOrDefault(record => (Guid)record.Attribute("UniqueID") == UniqueID);
            if (recordEl != null)
            {
                User_Record recordFound = new User_Record
                {
                    User_Id = (string)recordEl.Element("UserID"),
                    Book_Name = (string)recordEl.Element("Book"),
                    Unique_Id = UniqueID,
                    Withdraw_Date = (string)recordEl.Element("WithdrawDate"),
                    Return_Expected = (string)recordEl.Element("ReturnExpected"),
                    Return_Actual = (string)recordEl.Element("ReturnActual"),
                    Is_Returned = bool.Parse((string)recordEl.Element("IsReturned")),
                    Is_Renewed = bool.Parse((string)recordEl.Element("IsRenewed"))
                };

                return recordFound;

            }
            return null;
        }//Find a record based on the GUID, this is used to change a book availability

        public void ChangeBookFile(Guid UniqueID)
        {
            XDocument books;
            books = XDocument.Load("LibraryBooks.xml");
            XElement bookToChange = books.Descendants("SingleBook")
                                     .SingleOrDefault(record => (Guid)record.Attribute("UniqueID") == UniqueID);
            if (bookToChange != null)
            {
                bookToChange.SetElementValue("Availability", "true");
                books.Save("LibraryBooks.xml");
            }
        }
        //Changes the book file when a book is returned

        public void BookReturned(User_Record selectItem)
        {
            User_Record bookReturned = selectItem;
            if (bookReturned != null)
            {
                if (!bookReturned.Is_Returned)
                {
                    if (Convert.ToDateTime(bookReturned.Return_Expected) > DateTime.Now.Date)
                    {
                        ChangeBookFile(bookReturned.Unique_Id);
                        RecordAdjust(bookReturned);
                        MessageBox.Show("Book Returned Within Due Date");
                    }
                    else
                    {
                        ChangeBookFile(bookReturned.Unique_Id);
                        RecordAdjust(bookReturned);
                        MessageBox.Show("Book Returned After Due Date");
                    }
                }
                else
                {
                    MessageBox.Show("Already Returned");
                }
            }

        }//Method the calls the other methods for returning a book based on conditions

        public void Book_Deleted(SingleBook book)
        {
            XDocument recordFind;
            recordFind = XDocument.Load("LibraryHistory.xml");
            XElement recordEl = recordFind.Descendants("Record")
                               .SingleOrDefault(record => (Guid)record.Attribute("UniqueID") == book.Unique_ID
                               && (bool)record.Element("IsBookReturned") == false);
                               
            if(recordEl != null)
            {
                if (!book.IsReserved)
                {
                    recordEl.SetElementValue("IsBookReturned", "true");
                    recordEl.SetElementValue("Book", $"[BOOK DELETED]{book.Title}");
                    recordFind.Save("LibraryHistory.xml");
                }
                else if(book.IsReserved)
                {
                    recordEl.SetElementValue("IsBookReturned", "true");
                    recordEl.SetElementValue("Book", $"[BOOK DELETED]{book.Title}");
                    Reserving.reservingInstance.Book_Deleted(book);
                    recordFind.Save("LibraryHistory.xml");
                }
            }
        }
        //Deleted books will adjust any records to reflect the deleted book
        public void User_Deleted(string UserID)
        {
            XDocument records = XDocument.Load("LibraryHistory.xml");
            records.Descendants("Record")
               .Where(user => (string)user.Element("UserID") == UserID && (bool)user.Element("IsBookReturned") == false)
               .Remove();
            records.Save("LibraryHistory.xml");
        }
        //When a user is deleted their records are also deleted
        public List<User_Record> Filter_History()
        {
            List<User_Record> searchRecords = new List<User_Record>();
            foreach (User_Record record in DisplayRecord())
            {
                if (!record.Is_Returned)
                {
                    searchRecords.Add(record);
                }
            }
            if(searchRecords.Count >= 1)
            {
                return searchRecords;
            }
            else
            {
                return DisplayRecord();
            }
        }
        //Filter records based on if they are returned or not
    }
}
