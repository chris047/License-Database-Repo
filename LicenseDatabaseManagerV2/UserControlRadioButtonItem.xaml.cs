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
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.UserControlPages;
using LicenseDatabaseManagerV2.ViewModel;

namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlRadioButtonItem.xaml
    /// </summary>
    public partial class UserControlRadioButtonItem : UserControl, IReset
    {
        private RadioButtonSubItem holder;
        UserControlGeneralSearch Context;
        public UserControlRadioButtonItem(RadioButtonSubItem startingInformation)
        {
            InitializeComponent();
            holder = startingInformation;
            RadioObject.GroupName = startingInformation.RadioGroup;
            RadioObject.IsChecked = startingInformation.StartingChecked;
            RadioObject.Content = startingInformation.NameOfRadioButton;

        }
        public UserControlRadioButtonItem(RadioButtonSubItem startingInformation, UserControlGeneralSearch _context) //this is for if we have to call functions on click outside of scope.
        {
            InitializeComponent();
            holder = startingInformation;
            RadioObject.GroupName = startingInformation.RadioGroup;
            RadioObject.IsChecked = startingInformation.StartingChecked;
            RadioObject.Content = startingInformation.NameOfRadioButton;
            Context = _context;
        }

        public void ResetState()
        {
            RadioObject.GroupName = holder.RadioGroup;
            RadioObject.IsChecked = holder.StartingChecked;
            RadioObject.Content = holder.NameOfRadioButton;
        }

        private void RadioObject_Clicked(object sender, RoutedEventArgs e)
        {
            /* 
           0: General addition File
           1: Owner addition File
           2: Business addition File
           3: Client addition File
           */
            string tempContent = holder.NameOfRadioButton.ToLower(); //Will never be null

            if (tempContent == "general")
            {
                Context.changeSearchOptions(0);
            }
            if (tempContent == "owner")
            {
                Context.changeSearchOptions(1);
            }
            if (tempContent == "business")
            {
                Context.changeSearchOptions(2);
            }
            if (tempContent == "client")
            {
                Context.changeSearchOptions(3);
            }

        }
    }
}
