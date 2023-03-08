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
    public class SingleBook
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Date { get; set; }
        public bool Availability { get; set; }
        public string Description { get; set; }
    }

    public class BookHandling
    {
        public List<SingleBook> DisplayBooks()
        {
            List<SingleBook> books = new List<SingleBook>();
            XmlDocument bookFile = new XmlDocument();
            bookFile.LoadXml("LibraryBooks.xml");

            foreach (XmlNode node in bookFile.DocumentElement.ChildNodes)
            {
                SingleBook book = new SingleBook
                {
                    Title = node.SelectSingleNode("Title").InnerText,
                    Genre = node.SelectSingleNode("Genre").InnerText,
                    Author = node.SelectSingleNode("Author").InnerText,
                    ISBN = node.SelectSingleNode("ISBN").InnerText,
                    Date = node.SelectSingleNode("Date").InnerText,
                    Availability = bool.Parse(node.SelectSingleNode("Availabilty").InnerText),
                    Description = node.SelectSingleNode("Description").InnerText
                };
                books.Add(book);
            }
            return books;
        }
    }

}