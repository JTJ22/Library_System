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
using System.Windows.Threading;
using System.Text.RegularExpressions;
namespace Library_System
{
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
            Time_Updater();
        }
        
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
         
            User_Data.currentUser.Logging_In(TxtUsername.Text, PsdBoxPassword.Password);
            TxtUsername.Text = null;
            PsdBoxPassword.Password = null;
        }

        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Time_Updater()
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.txtblkDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }, this.Dispatcher);
            timer.Start();

        }

        private void TxtUsername_TextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
