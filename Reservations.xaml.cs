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
using System.Data;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Page
    {
        public Reservations()
        {
            InitializeComponent();
            dgReserveDisplay.ItemsSource = Reserving.Display_Reservations();
        }

        private void btnWithdrawBook_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Reserving cancelled = (Reserving)dgReserveDisplay.SelectedItem;
            Reserving.Cancel_Reservation(cancelled);
            dgReserveDisplay.Items.Refresh();
        }

        private void dgReserveDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


           
    }
}
