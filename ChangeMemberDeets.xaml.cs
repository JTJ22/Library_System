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
    /// Interaction logic for ChangeMemberDeets.xaml
    /// </summary>
    public partial class ChangeMemberDeets : Window
    {
        public ChangeMemberDeets()
        {
            InitializeComponent();
            Set_Frame();
        }

        private void frmMember_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void Set_Frame()
        {
            MemberDetailPage page = new MemberDetailPage();
            frmMember.NavigationService.Navigate(page);
        }
    }
}
