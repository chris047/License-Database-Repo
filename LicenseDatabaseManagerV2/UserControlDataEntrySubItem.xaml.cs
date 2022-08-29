using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using LicenseDatabaseManagerV2.ViewModel;

namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlDataEntrySubItem.xaml
    /// </summary>
    public partial class UserControlDataEntrySubItem : UserControl,IReset
    {
        public DataEntrySubItem DataInformation;
        public UserControlDataEntrySubItem(DataEntrySubItem dataEntrySubItem)
        {
            InitializeComponent();

            this.DataContext = dataEntrySubItem;
            DataInformation = dataEntrySubItem;
            //DataInput.Text = DataInformation.DataEntryText;
            DataInput.ToolTip = DataInformation.DataEntryText;

            if (DataInformation.Mandatory == true) //Mandatory items will be outlined in red
            {
                DataInput.BorderThickness = new Thickness(1);
                DataInput.BorderBrush = Brushes.Red;
            }
            if (DataInformation.Searchable == true) //searchable items will be filled in until a button can be made
            {
                DataInput.Background = Brushes.Blue;
            }
        }


        public void RemoveStartingText(object sender, RoutedEventArgs e) //on first click remove the text from the box.
        {
            DataInput.Text = new string(String.Empty); //Makes the text input cleared to allow for data entry
            DataInformation.HasBeenClicked = true;
            DataInput.Foreground = Brushes.Black;
            //Trace.WriteLine(this.SearchInput.Text);
            DataInput.GotFocus -= RemoveStartingText; //after text is removed, removed focus event to prevent unintentional removal.
        }


        public void ResetState()
        {
            //Trace.WriteLine(DataInformation.StartingText + " is the reset");
            this.DataInformation.HasBeenClicked = false;
            //this.DataInput.Text = DataInformation.StartingText;
            this.DataInput.Text = "";
            this.DataInput.Foreground = Brushes.Gray;
            DataInput.GotFocus += RemoveStartingText;
        }
    }
}
