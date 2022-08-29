using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using DataAccessLibrary;
using DataAccessLibrary.Models;
using DataAccessLibrary.Models.BusinessData;
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.ViewModel;


namespace LicenseDatabaseManagerV2.UserControlPages.Settings
{
    /// <summary>
    /// Interaction logic for UserControlSettings.xaml
    /// </summary>
    public partial class UserControlSettings : UserControl, IReset
    {
        public UserControlSettings()
        {
            InitializeComponent();
        }

        public void ResetState()
        {
            // new NotImplementedException();
        }
    }
}
