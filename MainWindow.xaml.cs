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
                this.txtblkDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            }, this.Dispatcher);
            timer.Start();

        }
    }
}
