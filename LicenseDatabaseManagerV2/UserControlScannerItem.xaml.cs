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
    /// Interaction logic for UserControlScannerItem.xaml
    /// </summary>

    public partial class UserControlScannerItem : UserControl, IReset
    {
        public SearchEntryObject
        Holder; //This references/creates a SearchEntryObject that allows one to save and store data in it that the program can reference.
        
        public String _StartingText; // saves the original starting text for reset later
        public int Font;
        public int Scale;

    public UserControlScannerItem(SearchEntryObject searchEntryObject)
    {
        InitializeComponent();

        this.DataContext = searchEntryObject;
        //Removed this to implement tool tip.
        //this.SearchInput.Text = searchEntryObject.StartingSearchText; //sets the text input to the starting search text.
        if (searchEntryObject.StartingSearchText != "") // Made a tool tip use the starting text that would have been in text box.
        { 
            TheGrid.ToolTip = searchEntryObject.StartingSearchText;
        }
       
        _StartingText = searchEntryObject.StartingSearchText.ToString();
        //Trace.WriteLine(_StartingText);
        Holder = searchEntryObject;
    }

    public UserControlScannerItem(SearchEntryObject searchEntryObject, int font, int scale)
    {
        InitializeComponent();

        this.DataContext = searchEntryObject;
        this.SearchInput.Text = searchEntryObject.StartingSearchText; //sets the text input to the starting search text.
        Holder = searchEntryObject;
    }


    public void RemoveStartingText(object sender, RoutedEventArgs e) //on first click remove the text from the box.
    {
        this.SearchInput.Text = new string(String.Empty); //Makes the text input cleared to allow for data entry
        Holder.HasBeenClicked = true; // Tells SearchEntryObject holder that data may have been added to area.
        this.SearchInput.Foreground = Brushes.Black; //TODO: Maybe just set it to black in the XAML since there is no 'starting text' as we know it anymore.
        //Trace.WriteLine(this.SearchInput.Text);
        SearchInput.GotFocus -= RemoveStartingText; //after text is removed, removed focus event to prevent unintentional removal.
    }

    public void ResetState() //resets the search parameters to original hint text, and original settings.
    {
        //Trace.WriteLine(_StartingText+" is the reset");
        //this.SearchInput.Text = _StartingText.ToString();
        this.SearchInput.Text = "";
        Holder.HasBeenClicked = false;
        this.SearchInput.Foreground = Brushes.Gray;
        SearchInput.GotFocus += RemoveStartingText; //TODO: Remove this function or at least the color changing as there is no starting text with tool tip.
                                
        }



    }
}
