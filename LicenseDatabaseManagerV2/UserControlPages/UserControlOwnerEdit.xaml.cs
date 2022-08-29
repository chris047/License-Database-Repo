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
    /// Interaction logic for UserControlOwnerEdit.xaml
    /// </summary>
    public partial class UserControlOwnerEdit : UserControl,IUserControllerScanner, IReset
    {
        public UserControlFullPreviewItem previewSubsection;
        public UserControlOwnerEdit()
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

            //Initial Owner



            var searchOptions1 = new SearchEntryObject("First Name:", "", false);
            var searchOptions2 = new SearchEntryObject("Last Name:", "", false);
            var searchOptions3 = new SearchEntryObject("Street Number:", "", false);
            var searchOptions4 = new SearchEntryObject("Street Name:", "", false);
            var searchOptions5 = new SearchEntryObject("City:", "", false);
            var searchOptions6 = new SearchEntryObject("County:", "", false);
            var searchOptions7 = new SearchEntryObject("State:", "", false);
            var searchOptions8 = new SearchEntryObject("Zip Code:", "", false);

            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions1));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions2));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions3));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions4));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions5));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions6));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions7));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions8));

            //Starting testing on how Advance scanners looks.
            var aSearchOptions1 = new SearchEntryObject("Area Code:", "", true);
            var aSearchOptions2 = new SearchEntryObject("Phone Number:", "", true);
            var aSearchOptions3 = new SearchEntryObject("Position:", "", true);
            var aSearchOptions4 = new SearchEntryObject("Blind:", "", true);
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions1));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions2));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions3));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions4));


            previewSubsection = new UserControlFullPreviewItem(0, 1, this);
            SearchResultsPreview.Children.Add(previewSubsection);
        }

        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private readonly OwnerFile _ownerFile = new();
        private List<OwnerIdLookupModel> _ownerIds = new();

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }

        //private static void SearchOwnerFiles(OwnerFile ownerFile)
        //{
        //    Sql.SearchOwnerFile(ownerFile);
        //}


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
            foreach (UserControlScannerItem searchParameters in UniformGridSearch.Children)
            {
                // This if statement will pass in null for items that are empty
                if (searchParameters.SearchInput.Text != "")
                {
                    switch (searchParameters.Holder.SearchCategoryName)
                    {
                        case "First Name:":
                            _ownerFile.first_name = searchParameters.SearchInput.Text;
                            break;
                        case "Last Name:":
                            _ownerFile.last_name = searchParameters.SearchInput.Text;
                            break;
                        case "Street Number:":
                            _ownerFile.address_line1_number = searchParameters.SearchInput.Text;
                            break;
                        case "Street Name:":
                            _ownerFile.address_line1_street_name = searchParameters.SearchInput.Text;
                            break;
                        case "City:":
                            _ownerFile.city_name = searchParameters.SearchInput.Text;
                            break;
                        case "City Code:":
                            _ownerFile.city_code = searchParameters.SearchInput.Text;
                            break;
                        case "County:":
                            _ownerFile.county_name = searchParameters.SearchInput.Text;
                            break;
                        case "State:":
                            _ownerFile.state_code = searchParameters.SearchInput.Text;
                            break;
                        case "Zipcode:":
                            _ownerFile.zip = searchParameters.SearchInput.Text;
                            break;
                        default:
                            Trace.WriteLine("Invalid category passed in: " + searchParameters.Holder.SearchCategoryName);
                            break;
                    }
                }
                else
                {
                    // eliminating debug line for now
                    //Trace.WriteLine(searchParameters.Holder.SearchCategoryName + " was blank.");
                }
            }

            _ownerIds = Sql.SearchOwnerFile(_ownerFile);

            foreach (var id in _ownerIds)
            {
                //string ownerId = id.idowner.ToString();
                //Trace.WriteLine(id.idowner);
                SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem(id.idowner, this, 1));
            }
        }

        public void PopulateFullPreview(object sender)
        {
            var uniqueId = ((int)sender);

            if (uniqueId != null)
            {
                //SearchResultsPreview.Children.Clear();
                previewSubsection.ChangeToNewId(uniqueId,1);
            }
            else
            {
                Trace.WriteLine("This shouldn't fire.");
            }
        }


        public int[] initialPopupReturnInt()
        {
            int[] result = new int[1];
            return result;
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

            SearchPreviewResults.Children.Clear(); //TODO: Somehow find a way to make this 'clear' command remove all memory instances to prevent memory leak.
        }
    }
}
