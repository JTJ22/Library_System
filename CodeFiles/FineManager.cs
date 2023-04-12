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
    public class Fining
    {
        public string User_ID { get; set; }
        public string Book_Name { get; set; }
        public Guid Unique_Id { get; set; }
        public string Expiration { get; set; }
        public double Fine_Value { get; set; }
        public bool Fine_Paid { get; set; }

        public static Fining finingInstance = new Fining();

        public List<Fining> Display_Fines()
        {
            List<Fining> fines = new List<Fining>();
            XDocument fineFile;
            fineFile = XDocument.Load("LibraryFines.xml");
            if (!User_Data.currentUser.Librarian_Permisions)
            {
                IEnumerable<Fining> query = from record in fineFile.Descendants("Record")
                                            where (string)record.Element("UserID") == User_Data.currentUser.User_id
                                            select new Fining
                                            {
                                                User_ID = (string)record.Element("UserID"),
                                                Book_Name = (string)record.Element("Book"),
                                                Unique_Id = (Guid)record.Attribute("UniqueID"),
                                                Expiration = (string)record.Element("Expires"),
                                                Fine_Value = double.Parse(((string)record.Element("FineValue")).Replace("£", "")),
                                                Fine_Paid = (bool)record.Element("FinePaid")
                                            };
                fines.AddRange(query);
                return fines;

            }//If the user is a member this method will only return fines relevant to them
            else
            {
                IEnumerable<Fining> query = from record in fineFile.Descendants("Record")
                                            select new Fining
                                            {
                                                User_ID = (string)record.Element("UserID"),
                                                Book_Name = (string)record.Element("Book"),
                                                Unique_Id = (Guid)record.Attribute("UniqueID"),
                                                Expiration = (string)record.Element("Expires"),
                                                Fine_Value = double.Parse(((string)record.Element("FineValue")).Replace("£", "")),
                                                Fine_Paid = (bool)record.Element("FinePaid")
                                            };
                fines.AddRange(query);
                return fines;

            }//Otherwise in the case a librarian is logged in they can view every fine on record


        }

        public void Create_Fine(Guid UniqueID, string UserID, string bookName, DateTime returnExpected, int fineCost)
        {
            XDocument fines;
            fines = XDocument.Load("LibraryFines.xml");
            XElement fineToEdit = fines.Descendants("Record")
                                        .SingleOrDefault(fine => (Guid)fine.Attribute("UniqueID") == UniqueID
                                        && (string)fine.Element("UserID") == UserID
                                        && (string)fine.Element("Book") == bookName
                                        && (bool)fine.Element("FinePaid") == false);

            if (fineToEdit != null)
            {
                fineToEdit.SetElementValue("Expires", returnExpected.ToString());
                fineToEdit.SetElementValue("FineValue", string.Format("{0:C}", fineCost));
                fines.Save("LibraryFines.xml");
            }
            else
            {
                XElement newRecord = new XElement("Record",
                new XElement("UserID", UserID),
                new XElement("Book", bookName),
                new XAttribute("UniqueID", UniqueID),
                new XElement("Expires", returnExpected.ToString()),
                new XElement("FineValue", string.Format("{0:C}", fineCost)),
                new XElement("FinePaid", "false"));
                fines.Root.Add(newRecord);
                fines.Save("LibraryFines.xml");
            }//Otherwise the fine value on an existing fine is increased
        }
        public bool Fine_Locker(User_Data currentUser)
        {
            foreach (Fining fine in Display_Fines())
            {

                if (fine.User_ID == currentUser.User_id && !fine.Fine_Paid)
                {
                    return true;
                }

            }
            return false;
        }//A boolean to prevent the user from accessing certain parts whilst a fine is active

        public void Fine_Being_Paid(Fining finePaid)
        {
            User_Record recordToChange = new User_Record();
            foreach (User_Record record in User_Record.UserRecordInstance.DisplayRecord())
            {
                if (record.Unique_Id == finePaid.Unique_Id)
                {
                    recordToChange = record;
                }
            }
            XDocument fines;
            fines = XDocument.Load("LibraryFines.xml");
            XElement fineToPay = fines.Descendants("Record")
                .SingleOrDefault(fine => (Guid)fine.Attribute("UniqueID") == finePaid.Unique_Id
                && (string)fine.Element("UserID") == finePaid.User_ID
                && (string)fine.Element("Expires") == finePaid.Expiration);
            if ((bool)fineToPay.Element("FinePaid") == false)
            {
                MessageBoxResult confirm = MessageBox.Show($"You will be charged {finePaid.Fine_Value}", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    fineToPay.SetElementValue("FinePaid", "true");
                    MessageBox.Show("Fine Paid");
                    User_Record.UserRecordInstance.BookReturned(recordToChange);
                    fines.Save("LibraryFines.xml");
                }
                else
                {
                    return;
                }// Save the entire XML file, not just the fineToPay element
            }
            else
            {
                MessageBox.Show("You have already paid this fine");
            }
        }//A method for when the fine is paid, returns the book and sets the finepaid boolean as true
    }
}