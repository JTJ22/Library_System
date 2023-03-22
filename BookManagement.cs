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
    public class SingleBook
    {
        //creating a class for "SingleBook" This will allow books to be objects within my code, making it much easier to handle data grids and editing data
        public string Title { get; set; }
        public string Unique_ID { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Date { get; set; }
        public bool Availability { get; set; }
        public string Description { get; set; }
        public bool IsReserved { get; set; }
    }

    public class BookHandling
    {

        public static List<SingleBook> DisplayBooks()
        {
            //Creating a list of book, the XML file is read and each "SingleBook" node is added to the list, this made it much easier to create data grids
            List<SingleBook> books = new List<SingleBook>();
            XmlDocument bookFile = new XmlDocument();
            bookFile.Load("LibraryBooks.xml");

            foreach (XmlNode node in bookFile.SelectNodes("/Books/SingleBook"))
            {
                SingleBook book = new SingleBook
                {
                    Title = node.SelectSingleNode("Title").InnerText,
                    Unique_ID = node.SelectSingleNode("UniqueID").InnerText,
                    Genre = node.SelectSingleNode("Genre").InnerText,
                    Author = node.SelectSingleNode("Author").InnerText,
                    ISBN = node.SelectSingleNode("ISBN").InnerText,
                    Date = node.SelectSingleNode("Date").InnerText,
                    Availability = bool.Parse(node.SelectSingleNode("Availabilty").InnerText),
                    Description = node.SelectSingleNode("Description").InnerText,
                    IsReserved =  bool.Parse(node.SelectSingleNode("IsReserved").InnerText)
                };
                books.Add(book);
            }
            return books;
            //Lastly the list "books" is returned, this made is so the list is easily accessible elsewhere in the code (Same reason it is public static)
        }

    }

}