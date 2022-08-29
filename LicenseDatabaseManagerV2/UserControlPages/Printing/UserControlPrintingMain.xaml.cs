using LicenseDatabaseManagerV2.Interfaces;
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

namespace LicenseDatabaseManagerV2.UserControlPages.Printing
{
    /// <summary>
    /// Interaction logic for UserControlPrintingMain.xaml
    /// </summary>
    public partial class UserControlPrintingMain : UserControl, IReset
    {
        public UserControlPrintingMain()
        {
            InitializeComponent();
        }

        public void ResetState()
        {
            //throw new NotImplementedException();
        }
    }
}
