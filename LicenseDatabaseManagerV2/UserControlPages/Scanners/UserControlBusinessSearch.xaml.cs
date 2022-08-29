using System;
using System.Collections.Generic;
using System.Configuration;
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
using DataAccessLibrary;
using DataAccessLibrary.Models;
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.ViewModel;
using MaterialDesignThemes.Wpf;

namespace LicenseDatabaseManagerV2.UserControlPages
{
    /// <summary>
    /// Interaction logic for UserControlBusinessSearch.xaml
    /// </summary>
    public partial class UserControlBusinessSearch : UserControl, IUserControllerScanner, IReset
    {
        public UserControlFullPreviewItem previewSubsection;
        public UserControlBusinessSearch()
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


            var searchOptions1 = new SearchEntryObject("DBA:", "Business name here", false);
            var searchOptions2 = new SearchEntryObject("Area Code:", "Area code here", false);
            var searchOptions3 = new SearchEntryObject("Phone Number:", "Phone number here", false);
            var searchOptions4 = new SearchEntryObject("Street Number:", "Street number here?", false);
            var searchOptions5 = new SearchEntryObject("Street Name:", "Street name here", false);
            var searchOptions6 = new SearchEntryObject("City:", "City name here", false);
            var searchOptions7 = new SearchEntryObject("City Code:", "City code here", false);
            var searchOptions8 = new SearchEntryObject("County:", "County name here", false);
            var searchOptions9 = new SearchEntryObject("County Region:", "County region here", false);
            var searchOptions10 = new SearchEntryObject("Zip Code:", "Zipcode here", false);


            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions1));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions2));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions3));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions4));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions5));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions6));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions7));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions8));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions9));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions10));

                //Starting testing on how Advance scanners looks.
            var aSearchOptions1 = new SearchEntryObject("Former DBA:", "Former names of business", true);
            var aSearchOptions2 = new SearchEntryObject("License:", "License number here", true);
            var aSearchOptions3 = new SearchEntryObject("UniqueID:", "Unique ID here", true);
            var aSearchOptions4 = new SearchEntryObject("Memo:", "Memo text here", true);

            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions1));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions2));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions3));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions4));


            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00001", this,2));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00002", this,2));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00003", this,2));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00004", this,2));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00005", this,2));

            previewSubsection = new UserControlFullPreviewItem(0, 2, this);
            SearchResultsPreview.Children.Add(previewSubsection);

        }

        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private BusinessFile _businessFile = new(); //Changed and removed "Readonly" as it has to be reset every search.
        private List<BusinessIdLookupModel> _businessIds = new();

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }

        private void textBlock1_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

        }

        private void ShowMoreSearch_OnClick(object sender, RoutedEventArgs e) //Shows advanced search options
        {
            UniformGridAdvanceSearch.Visibility = UniformGridAdvanceSearch.Visibility == Visibility.Visible //Opens the advanced search categories.
                ? Visibility.Collapsed
                : Visibility.Visible;
            MoreOptionsIcon.Kind = MoreOptionsIcon.Kind == PackIconKind.ChevronDown //Opens the advanced search categories.
                ? PackIconKind.ChevronUp
                : PackIconKind.ChevronDown;
            MoreOptionsLabel.Content = MoreOptionsLabel.Content.ToString() == "More Options"
                ? "Less Options"
                : "More Options";
        }

        private void SearchDataBase_OnClick(object sender, RoutedEventArgs e)
        {
            //Have to reset business file right before search. Hopefully setting it to new() wont cause issues.
            _businessFile = new();
            foreach (UserControlScannerItem searchParameters in UniformGridSearch.Children)  //A dummy search printout to show data of search inquiries.
            {
                if (searchParameters.SearchInput.Text != "")
                {
                    switch (searchParameters.Holder.SearchCategoryName)
                    {
                        case "DBA:":
                            _businessFile.dba = searchParameters.SearchInput.Text;                            
                            break;
                        case "Area Code:":
                            _businessFile.area_code = searchParameters.SearchInput.Text;
                            break;
                        case "Phone Number:":
                            _businessFile.phone_number = searchParameters.SearchInput.Text;
                            break;
                        case "Street Number:":
                            _businessFile.address_line1_number = searchParameters.SearchInput.Text;
                            break;
                        case "Street Name:":
                            _businessFile.address_line1_street_name = searchParameters.SearchInput.Text;
                            break;
                        case "City:":
                            _businessFile.city_name = searchParameters.SearchInput.Text;
                            break;
                        case "City Code:":
                            _businessFile.city_code = searchParameters.SearchInput.Text;
                            break;
                        case "County:":
                            _businessFile.county_name = searchParameters.SearchInput.Text;
                            break;
                        case "County Region:":
                            _businessFile.county_region = searchParameters.SearchInput.Text;
                            break;
                        case "Zipcode:":
                            _businessFile.zip = searchParameters.SearchInput.Text;
                            break;
                        default:
                            Trace.WriteLine("Invalid category passed in: " + searchParameters.Holder.SearchCategoryName);
                            break;
                    }
                }
            }
            Trace.WriteLine("Made it out of loop");
            _businessIds = Sql.SearchBusinessFile(_businessFile);

            //Just added this to see if it removes the searches without memory leak 5/28/22 - CP
            SearchPreviewResults.Children.Clear(); //TODO: Somehow find a way to make this 'clear' command remove all memory instances to prevent memory leak.

            foreach (var id in _businessIds)
            {
                Trace.WriteLine("BusinessId" + id.idbusiness);
                SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem(id.idbusiness, this, 2));
            }
        }

        public void PopulateFullPreview(object sender)
        {
            var uniqueId = ((int)sender);

            if (uniqueId != null)
            {
                //SearchResultsPreview.Children.Clear();
                previewSubsection.ChangeToNewId(uniqueId, 2);
            }
            else
            {
                Trace.WriteLine("This shouldn't fire.");
            }
        }

        public void ResetState()
        {
            foreach (object button in RadioSearchOptions.Children)
            {
                Trace.WriteLine(button.GetType().ToString() + " is the type of Type");
                if (button.GetType().ToString() != "System.Windows.Controls.Label") //Telling code NOT to run IReset on Loose Label
                {
                    IReset radioObject = (IReset)button;
                    radioObject.ResetState();
                }
            }
            /*
            foreach (IReset searchParameter in RadioSearchOptions.Children) //Reseting all substates
            {
                

            }
            */
            foreach (IReset searchParameter in UniformGridSearch.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in UniformGridAdvanceSearch.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in SearchResultsPreview.Children)
            {
                searchParameter.ResetState();

            }
            UniformGridAdvanceSearch.Visibility = Visibility.Collapsed;
            MoreOptionsLabel.Content = "More Options";
            MoreOptionsIcon.Kind = PackIconKind.ChevronDown;

            SearchPreviewResults.Children.Clear();
        }
    }
}
