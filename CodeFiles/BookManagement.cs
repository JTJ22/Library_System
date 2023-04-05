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
    public class SingleBook
    {
        public string Title { get; set; }
        public Guid Unique_ID { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Date { get; set; }
        public bool Availability { get; set; }
        public string Description { get; set; }
        public bool IsReserved { get; set; }

        public void Change_Book_Details((SingleBook, string, string, string, string, string) bookTuple)
        {
            XDocument books = XDocument.Load("LibraryBooks.xml");
            foreach (XElement book in books.Descendants("SingleBook"))
            {
                if (book.Element("Title") != null)
                {
                    if (book.Element("UniqueID").Value == bookTuple.Item1.Unique_ID.ToString())
                    {
                        if (!string.IsNullOrWhiteSpace(bookTuple.Item2) || !string.IsNullOrWhiteSpace(bookTuple.Item3) || !string.IsNullOrWhiteSpace(bookTuple.Item4) || !string.IsNullOrWhiteSpace(bookTuple.Item5))
                        {
                            if (bookTuple.Item1.Title != bookTuple.Item2 && !string.IsNullOrWhiteSpace(bookTuple.Item2))
                            {
                                book.Element("Title").Value = bookTuple.Item2;
                            }
                            if (bookTuple.Item1.Genre != bookTuple.Item3 && !string.IsNullOrWhiteSpace(bookTuple.Item3))
                            {
                                book.Element("Genre").Value = bookTuple.Item3;
                            }
                            if (bookTuple.Item1.Author != bookTuple.Item4 && !string.IsNullOrWhiteSpace(bookTuple.Item4))
                            {
                                book.Element("Author").Value = bookTuple.Item4;
                            }
                            if (bookTuple.Item1.Description != bookTuple.Item5 && !string.IsNullOrWhiteSpace(bookTuple.Item5))
                            {
                                book.Element("Description").Value = bookTuple.Item5;
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
            books.Save("LibraryBooks.xml");
        }
    }

    public class BookHandling
    {

        public static BookHandling handler = new BookHandling();
        public void Add_Book(SingleBook book)
        {
            if (Null_Check(book))//Checks if any of the input details are blank, to prevent a blank book being made
            {
                XDocument books;
                books = XDocument.Load("LibraryBooks.xml");
                XElement newBook = new XElement("SingleBook",
                new XElement("Title", book.Title),
                new XAttribute("UniqueID", book.Unique_ID),
                new XElement("Genre", book.Genre),
                new XElement("Author", book.Author),
                new XElement("ISBN", book.ISBN),
                new XElement("Date", book.Date),
                new XElement("Availability", book.Availability),
                new XElement("Description", book.Description),
                new XElement("IsReserved", book.IsReserved));
                books.Root.Add(newBook);
                books.Save("LibraryBooks.xml");
                MessageBox.Show("Book Added");
            }
        }//A method that create a new element of singlebook with sub-elements defining it's details

        private bool Null_Check(SingleBook book)
        {
                if (string.IsNullOrEmpty(book.Title))
                {
                MessageBox.Show("Book title cannot be blank");
                return false;
                }
                if (string.IsNullOrEmpty(book.Genre))
                {
                    MessageBox.Show("Book genre cannot be blank");
                    return false;
                }
                if (string.IsNullOrEmpty(book.Author))
                {
                    MessageBox.Show("Book author cannot be blank");
                    return false;
                }
                if (string.IsNullOrEmpty(book.ISBN))
                {
                    MessageBox.Show("Book ISBN cannot be blank");
                    return false;
                }
                if (string.IsNullOrEmpty(book.Date))
                {
                    MessageBox.Show("Book date cannot be blank");
                    return false;
                }
                if (string.IsNullOrEmpty(book.Description))
                {
                    MessageBox.Show("Book description cannot be blank");
                    return false;
                }
            return true;
        }//Checks the input values, if it's empty will not allow book to be made
        
        public bool Check_Duplicate(Guid UniqueID)
        {
            var bookIDs = Display_Books().Select(book => book.Unique_ID);
            return !bookIDs.Contains(UniqueID);
        }//A method that checks through every book, this is to avoid having the same unique ID on two seperate books
        public List<SingleBook> Display_Books()
        {
            List<SingleBook> books = new List<SingleBook>();
            XDocument booksFile;
            booksFile = XDocument.Load("LibraryBooks.xml");
            IEnumerable<SingleBook> getBooks = from book in booksFile.Descendants("SingleBook")
                        select new SingleBook
                        {
                            Unique_ID = (Guid)book.Attribute("UniqueID"),
                            Title = (string)book.Element("Title"),
                            Genre = (string)book.Element("Genre"),
                            Author = (string)book.Element("Author"),
                            ISBN = (string)book.Element("ISBN"),
                            Date = (string)book.Element("Date"),
                            Availability = bool.Parse((string)book.Element("Availability")),
                            Description = (string)book.Element("Description"),
                            IsReserved = bool.Parse((string)book.Element("IsReserved"))
                        };
            books.AddRange(getBooks);
            return books;
        }//Adds each books to a list, iterates over every singlebook node

        public void Withdrawn(SingleBook bookBeingWithdrawn)
        {
            XDocument books;
            books = XDocument.Load("LibraryBooks.xml");
            XElement bookToWithdraw = books.Descendants("SingleBook")
                                     .SingleOrDefault(book => (Guid)book.Attribute("UniqueID") == bookBeingWithdrawn.Unique_ID);

            if(bookToWithdraw != null)
            {
                if ((bool?)bookToWithdraw.Element("Availability") == true && (bool)bookToWithdraw.Element("IsReserved") == false)
                {
                    bookToWithdraw.SetElementValue("Availability", "false");
                    bookBeingWithdrawn.Availability = false;
                    User_Record.UserRecordInstance.Update_History(bookBeingWithdrawn);
                    books.Save("LibraryBooks.xml");
                    MessageBox.Show("Book Withdrawn");
                }
                else if ((bool?)bookToWithdraw.Element("Availability") == false)
                {
                    MessageBox.Show("Not Available");
                }
                else if ((bool)bookToWithdraw.Element("IsReserved") == true)
                {
                    MessageBox.Show("Reserved, Not Available");
                }
            }
            }//Updates the xml files when a book is withdrawn

        }

    }

