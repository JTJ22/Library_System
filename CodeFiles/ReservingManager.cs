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
using System.Xml.Linq;
namespace Library_System
{
    public class Reserving
    {
        public string User_ID { get; set; }
        public string Book_Name { get; set; }
        public Guid Unique_Id { get; set; }
        public string Time_Reserved { get; set; }
        public string Expiration { get; set; }
        public bool Reserve_Complete { get; set; }

        public static Reserving reservingInstance = new Reserving();
        public void Reserved(SingleBook reservedBook)
        {
            if (ResChecker())
            {

                if (reservedBook.Availability == false)
                {
                    if (reservedBook.IsReserved == false && Display_Reservations().Count < 3 || User_Data.currentUser.Librarian_Permisions)
                    {
                        DateTime expiration = DateTime.Now.AddDays(7).Date;
                        XDocument reserves;
                        reserves = XDocument.Load("LibraryReservations.xml");
                        Update_Book_On_Reserve(reservedBook);
                        XElement newRecord = new XElement("Record",
                        new XElement("UserID", User_Data.currentUser.User_id),
                        new XElement("Book", reservedBook.Title),
                        new XAttribute("UniqueID", reservedBook.Unique_ID),
                        new XElement("TimeReserved", Convert.ToString(DateTime.Now)),
                        new XElement("Expires", expiration.ToString()),
                        new XElement("ReserveComplete", "false"));
                        reserves.Root.Add(newRecord);
                        reserves.Save("LibraryReservations.xml");
                        MessageBox.Show("Book Reserved");
                    }
                    else
                    {
                        MessageBox.Show("Book is reserved elsewhere");
                    }
                }
                else
                {
                    MessageBox.Show("Book is available, please withdraw");
                }
            }
            else
            {
                MessageBox.Show("You already have this book withdrawn");
            }
        }//Creates a reservation if conditions are met, won't allow a user to make one if the reservations are over 3

        public void Update_Book_On_Reserve(SingleBook bookReserved)
        {
            XDocument book;
            book = XDocument.Load("LibraryBooks.xml");
            XElement bookToChange = book.Descendants("SingleBook")
                .SingleOrDefault(books => (Guid)books.Attribute("UniqueID") == bookReserved.Unique_ID);
            bookToChange.SetElementValue("IsReserved", "true");
            book.Save("LibraryBooks.xml");
        }// Updates the book when a book is reserved, to show the boolean
        public bool Notify_Res()
        {
            foreach (Reserving reservation in Display_Reservations())
            {
                if (reservation.User_ID == User_Data.currentUser.User_id && !reservation.Reserve_Complete)
                {
                    foreach (SingleBook book in BookHandling.handler.Display_Books())
                    {
                        if (reservation.Unique_Id == book.Unique_ID)
                        {
                            if (book.Availability)
                            {
                                return true;
                            }
                            else if (!book.Availability)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }//A boolean to tell a user when a reserved book is available
        public bool ResChecker()
        {
            foreach (User_Record record in User_Record.UserRecordInstance.DisplayRecord())
            {
                if (record.User_Id == User_Data.currentUser.User_id && !record.Is_Returned)
                {
                    return false;
                }
            }
            return true;
        } //Checks if a record has been returned

        public List<Reserving> Display_Reservations()
        {
            List<Reserving> reservations = new List<Reserving>();
            if (!User_Data.currentUser.Librarian_Permisions)
            {
                XDocument reservingFile;
                reservingFile = XDocument.Load("LibraryReservations.xml");
                IEnumerable<Reserving> reserves = from record in reservingFile.Descendants("Record")
                                                  where (string)record.Element("UserID") == User_Data.currentUser.User_id
                                                  select new Reserving
                                                  {
                                                      User_ID = (string)record.Element("UserID"),
                                                      Book_Name = (string)record.Element("Book"),
                                                      Unique_Id = (Guid)record.Attribute("UniqueID"),
                                                      Time_Reserved = (string)record.Element("TimeReserved"),
                                                      Expiration = (string)record.Element("Expires"),
                                                      Reserve_Complete = (bool)record.Element("ReserveComplete"),
                                                  };
                reservations.AddRange(reserves);
                return reservations;
            }
            else if(User_Data.currentUser.Librarian_Permisions)
            {   
                XDocument reservingFile;
                reservingFile = XDocument.Load("LibraryReservations.xml");
                IEnumerable<Reserving> reserves = from record in reservingFile.Descendants("Record")
                                                  select new Reserving
                                                  {
                                                      User_ID = (string)record.Element("UserID"),
                                                      Book_Name = (string)record.Element("Book"),
                                                      Unique_Id = (Guid)record.Attribute("UniqueID"),
                                                      Time_Reserved = (string)record.Element("TimeReserved"),
                                                      Expiration = (string)record.Element("Expires"),
                                                      Reserve_Complete = (bool)record.Element("ReserveComplete"),
                                                  };
                reservations.AddRange(reserves);  
            }
            return reservations;
        }
        //Shows the records based on user permissions
        public void Cancel_Reservation(Reserving cancelled)
        {

            XDocument book;
            book = XDocument.Load("LibraryBooks.xml");
            XElement bookToChange = book.Descendants("SingleBook")
                .SingleOrDefault(books => (Guid)books.Attribute("UniqueID") == cancelled.Unique_Id);
            bookToChange.SetElementValue("IsReserved", "false");
            book.Save("LibraryBooks.xml");

            if (cancelled != null && !cancelled.Reserve_Complete)
            {
                XDocument reservations;
                reservations = XDocument.Load("LibraryReservations.xml");
                XElement resToChange = reservations.Descendants("Record")
                                         .SingleOrDefault(record => (string)record.Element("UserID") == cancelled.User_ID
                                         && (bool)record.Element("ReserveComplete") == false
                                         && (Guid)record.Attribute("UniqueID") == cancelled.Unique_Id);
                resToChange.Remove();
                reservations.Save("LibraryReservations.xml");
            }



        }//Deletes a reservation from the system

        public void Expired_Reservation()
        {
            XDocument reservations;
            reservations = XDocument.Load("LibraryReservations.xml");
            XElement resToChange = reservations.Descendants("Record")
                                     .SingleOrDefault(record => (string)record.Element("UserID") == User_Data.currentUser.User_id
                                     && (bool)record.Element("ReserveComplete") == false);
            if (resToChange != null)
            {
                if (Convert.ToDateTime((string)resToChange.Element("Expires")).Date < DateTime.Now.Date)
                {
                    XDocument book;
                    book = XDocument.Load("LibraryBooks.xml");
                    XElement bookToChange = book.Descendants("SingleBook")
                        .SingleOrDefault(books => (string)books.Attribute("UniqueID") == (string)resToChange.Attribute("UniqueID"));
                    bookToChange.SetElementValue("IsReserved", "false");
                    resToChange.Remove();
                    book.Save("LibraryBooks.xml");
                    reservations.Save("LibraryReservations.xml");
                }
            }
        }

        public void Complete_Reservation(Reserving record)
        {
            XDocument reservations;
            reservations = XDocument.Load("LibraryReservations.xml");
            XElement resToChange = reservations.Descendants("Record")
                                     .SingleOrDefault(reservation => (string)reservation.Element("UserID") == record.User_ID
                                     && (bool)reservation.Element("ReserveComplete") == false
                                     && (Guid)reservation.Attribute("UniqueID") == record.Unique_Id);
            if (resToChange != null)
            {
                resToChange.SetElementValue("ReserveComplete", "true");
                reservations.Save("LibraryReservations.xml");
            }

            XDocument book;
            book = XDocument.Load("LibraryBooks.xml");
            XElement bookToChange = book.Descendants("SingleBook")
                .SingleOrDefault(books => (Guid)books.Attribute("UniqueID") == (Guid)resToChange.Attribute("UniqueID"));
            bookToChange.SetElementValue("IsReserved", "false");
            bookToChange.SetElementValue("Availability", "false");
            book.Save("LibraryBooks.xml");
        }

        public void Updating_Res(Reserving reservedBook)
        {
            if(!reservedBook.Reserve_Complete)
            { 
            SingleBook bookBeingWithdrawn = GettingBook(reservedBook);
                if (bookBeingWithdrawn.IsReserved)
                {
                    if ((reservedBook.User_ID == User_Data.currentUser.User_id || User_Data.currentUser.Librarian_Permisions) && bookBeingWithdrawn.Availability)
                    {
                        User_Record user_Record = new User_Record();
                        Complete_Reservation(reservedBook);
                        user_Record.Update_History(bookBeingWithdrawn);
                        MessageBox.Show("Reservation Withdrawn");
                    }
                    else
                    {
                        MessageBox.Show("Book not available yet");
                    }
                }
            }
            else if (reservedBook.Reserve_Complete)
            {
                MessageBox.Show("Already completed");
            }
        }


        private SingleBook GettingBook(Reserving reservedBook)
        {
            foreach (SingleBook book in BookHandling.handler.Display_Books())
            {
                if (reservedBook.Unique_Id == book.Unique_ID)
                {
                    SingleBook bookBeingWithdrawn = book;
                    return bookBeingWithdrawn;
                }
            }
            return null;
        }

        public void Book_Deleted(SingleBook book)
        {
            XDocument reservations;
            reservations = XDocument.Load("LibraryReservations.xml");
            XElement resToChange = reservations.Descendants("Record")
                                     .SingleOrDefault(reservation => (bool)reservation.Element("ReserveComplete") == false
                                     && (Guid)reservation.Attribute("UniqueID") == book.Unique_ID);

            if (resToChange != null)
            {
                resToChange.SetElementValue("Book", "[BOOK DELETED, CANCELLED]");
                resToChange.SetElementValue("ReserveComplete", "true");
                reservations.Save("LibraryReservations.xml");
            }
        }

        public void User_Deleted(string UserID)
        {
            XDocument records = XDocument.Load("LibraryReservations.xml");
            records.Descendants("Record")
               .Where(user => (string)user.Element("UserID") == UserID && (bool)user.Element("ReserveComplete") == false)
               .Remove();
            records.Save("LibraryReservations.xml");
        }

    }
}