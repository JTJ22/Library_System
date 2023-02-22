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
using System.Windows.Shapes;

namespace Library_System
{
    /// <summary>
    /// Interaction logic for MemberLoginWindow.xaml
    /// </summary>
    public partial class MemberLoginWindow : Window
    {
        public MemberLoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            User_Data userLoggedOut = new User_Data("","","","","","");
            Log_Out_Event(sender, e, userLoggedOut);
        }

        public void Log_Out_Event(object sender, RoutedEventArgs e, User_Data userLoggedOut)
        {
            MessageBox.Show("You have logged out");
            this.Close();
            userLoggedOut.Log_Out(userLoggedOut);

        }
    }
}
