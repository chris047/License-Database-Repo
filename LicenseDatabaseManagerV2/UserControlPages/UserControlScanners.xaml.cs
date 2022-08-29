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
using LicenseDatabaseManagerV2.ViewModel;
using System.Diagnostics;
using LicenseDatabaseManagerV2.Interfaces;

namespace LicenseDatabaseManagerV2.UserControlPages
{
    /// <summary>
    /// Interaction logic for UserControlScannersPage.xaml
    /// </summary>
    public partial class UserControlScanners : UserControl , IUserControllerScanner, IReset
    {
        
        public UserControlScanners()
        {
            InitializeComponent();
            
            // Probably not going to use dashboard 
            //var item0 = new ItemMenu("Dashboard", new UserControl(), PackIconKind.ViewDashboard);



            //Starting testing on how scanner looks.
            //var searchOptions1 = new SearchEntryObject("Name", "Enter a person's name here", false);
            //var searchOptions2 = new SearchEntryObject("DBA", "Whatever DBA is here", false);
            //var searchOptions3 = new SearchEntryObject("Address", "Address here?", false);
            //var searchOptions4 = new SearchEntryObject("Street Name", "Street Name here", false);
            //var searchOptions5 = new SearchEntryObject("License", "License number here?", false);
            //var searchOptions6 = new SearchEntryObject("UniqueID", "Unique ID here", false);

            var searchOptions1 = new SearchEntryObject("Name", "Enter a person's name here", false);
            var searchOptions2 = new SearchEntryObject("DBA", "Whatever DBA is here", false);
            var searchOptions3 = new SearchEntryObject("Address", "Address here?", false);
            var searchOptions4 = new SearchEntryObject("Street Name", "Street Name here", false);
            var searchOptions5 = new SearchEntryObject("License", "License number here?", false);
            var searchOptions6 = new SearchEntryObject("UniqueID", "Unique ID here", false);

            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions1));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions2));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions3));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions4));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions5));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions6));

            //Starting testing on how Advance scanners looks.
            var aSearchOptions1 = new SearchEntryObject("Zip", "Zipcode here", true);
            var aSearchOptions2 = new SearchEntryObject("Starred", "Put a 1 if starred, 0 if not", true);
            var aSearchOptions3 = new SearchEntryObject("Ect1", "Ect1", true);
            var aSearchOptions4 = new SearchEntryObject("Ect2", "Ect2", true);
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions1));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions2));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions3));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions4));

            /*
            SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00001",this));
            SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00002",this));
            SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00003",this));
            SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00004",this));
            SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00005",this));
            */
        }


       
        private void ShowMoreSearch_OnClick(object sender, RoutedEventArgs e) //Shows advanced search options
        {
            UniformGridAdvanceSearch.Visibility = UniformGridAdvanceSearch.Visibility == Visibility.Visible //Opens the advanced search categories.
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void SearchDataBase_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (UserControlScannerItem searchParameters in UniformGridSearch.Children)  //A dummy search printout to show data of search inquiries.
            {
                Trace.WriteLine(searchParameters.Holder.SearchCategoryName + " is active: " + searchParameters.Holder.HasBeenClicked);
                Trace.WriteLine("Text present in search box: " + searchParameters.SearchInput.Text);

                var searchOptions69 = new SearchEntryObject("Name", "Enter a person's name here", false);
                
            }
           
        }

        public void PopulateFullPreview(object sender)
        {
            var uniqueId = ((int)sender);

            if (uniqueId != null)
            {

                SearchResultsPreview.Children.Clear();
                SearchResultsPreview.Children.Add(new UserControlFullPreviewItem(uniqueId,0, this));

            }
            else
            {
                Trace.WriteLine("This shouldn't fire.");
            }
        }

        
        public void ResetState()
        {
            foreach (IReset searchParameter in UniformGridSearch.Children)  //A dummy search printout to show data of search inquiries.
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in UniformGridAdvanceSearch.Children)  //A dummy search printout to show data of search inquiries.
            {
                searchParameter.ResetState();

            }
            UniformGridAdvanceSearch.Visibility = Visibility.Collapsed;
        }
    }
}
