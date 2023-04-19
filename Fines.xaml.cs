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

namespace Library_System
{
    /// <summary>
    /// Interaction logic for Fines.xaml
    /// </summary>
    /// 
    
    public partial class Fines : Page
    {
       
        public Fines()
        {
            InitializeComponent();
            Grid_Creator();
        }

        private void dgFineGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Grid_Creator() 
        {
            dgFineGrid.ItemsSource = Fining.finingInstance.Display_Fines();
        }
        private void btnReturnFine_Click(object sender, RoutedEventArgs e)
        {
            Return_Fine((Fining)dgFineGrid.SelectedItem);
            Grid_Creator();
        }

        private void Return_Fine(Fining fineReturned)
        {
            if (fineReturned != null)
            {
                if (fineReturned.Fine_Paid != true)
                {
                    fineReturned.Fine_Being_Paid(fineReturned);
                    ;
                }

            }

        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            MessageBox.Show(textBlock.Text, "Unique ID", MessageBoxButton.OK, MessageBoxImage.Information);
            e.Handled = true;
        }

        private void txtBoxFineSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Fining> fines = Fining.finingInstance.Display_Fines();
            List<Fining> filteredBooks = Fining.finingInstance.Filter_Fines(fines, txtBoxFineSearch.Text.ToLower());
            this.dgFineGrid.ItemsSource = filteredBooks;
        }
    }
}
