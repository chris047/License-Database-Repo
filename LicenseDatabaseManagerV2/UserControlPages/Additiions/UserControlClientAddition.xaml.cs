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

namespace LicenseDatabaseManagerV2.UserControlPages.Additiions
{
    /// <summary>
    /// Interaction logic for UserControlClientAddition.xaml
    /// </summary>
    public partial class UserControlClientAddition : UserControl, IUserControllerScanner, IReset
    {
        private IUserControlAddition _context;
        public UserControlFullPreviewItemAddition previewSubsection;
        public int[]? CurrentClients; //current clients.
        public UserControlClientAddition(IUserControlAddition context, int[]? currentClients)
        {
            InitializeComponent();
            var searchOptions1 = new SearchEntryObject("Corporation:", "", false);
            var searchOptions2 = new SearchEntryObject("First Name:", "", false);
            var searchOptions3 = new SearchEntryObject("Last Name:", "", false);
            var searchOptions4 = new SearchEntryObject("Street Number:", "", false);
            var searchOptions5 = new SearchEntryObject("Street Name:", "", false);
            var searchOptions6 = new SearchEntryObject("City:", "", false);
            var searchOptions7 = new SearchEntryObject("County:", "", false);
            var searchOptions8 = new SearchEntryObject("State:", "", false);
            var searchOptions9 = new SearchEntryObject("Zip Code:", "", false);

            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions1));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions2));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions3));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions4));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions5));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions6));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions7));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions8));
            UniformGridSearch.Children.Add(new UserControlScannerItem(searchOptions9));

            //Starting testing on how Advance scanners looks.
            var aSearchOptions1 = new SearchEntryObject("Area Code:", "", true);
            var aSearchOptions2 = new SearchEntryObject("Phone Number:", "", true);

            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions1));
            UniformGridAdvanceSearch.Children.Add(new UserControlScannerItem(aSearchOptions2));

            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00001", this, 3));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00002", this, 3));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00003", this, 3));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00004", this, 3));
            //SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem("00005", this, 3));

            _context = context;
            if (currentClients != null)
            {
                CurrentClients = currentClients; //Lets try not cloning it
            }
            previewSubsection = new UserControlFullPreviewItemAddition(0, 3, CurrentClients);
            SearchResultsPreviewFull.Children.Add(previewSubsection);
        }

        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private ClientFile _clientFile = new(); //Changed and removed "Readonly" as it has to be reset every search.
        private List<ClientIdLookupModel> _clientIds = new();

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
            //Have to reset client file right before search. Hopefully setting it to new() wont cause issues.
            _clientFile = new();
            foreach (UserControlScannerItem searchParameters in UniformGridSearch.Children)
            {
                // This if statement will pass in null for items that are empty
                if (searchParameters.SearchInput.Text != "")
                {
                    switch (searchParameters.Holder.SearchCategoryName)
                    {
                        case "Corporation:":
                            _clientFile.business_name = searchParameters.SearchInput.Text;
                            break;
                        case "First Name:":
                            _clientFile.first_name = searchParameters.SearchInput.Text;
                            break;
                        case "Last Name:":
                            _clientFile.last_name = searchParameters.SearchInput.Text;
                            break;
                        case "Street Number:":
                            _clientFile.address_line1_number = searchParameters.SearchInput.Text;
                            break;
                        case "Street Name:":
                            _clientFile.address_line1_street_name = searchParameters.SearchInput.Text;
                            break;
                        case "City:":
                            _clientFile.city_name = searchParameters.SearchInput.Text;
                            break;
                        case "City Code:":
                            _clientFile.city_code = searchParameters.SearchInput.Text;
                            break;
                        case "County:":
                            _clientFile.county_name = searchParameters.SearchInput.Text;
                            break;
                        case "State:":
                            _clientFile.state_code = searchParameters.SearchInput.Text;
                            break;
                        case "Zipcode:":
                            _clientFile.zip = searchParameters.SearchInput.Text;
                            break;
                        default:
                            Trace.WriteLine("Invalid category passed in: " +
                                            searchParameters.Holder.SearchCategoryName);
                            break;
                    }
                }

                _clientIds = Sql.SearchClientFile(_clientFile);

            }

            //Just added this to see if it removes the searches without memory leak 5/28/22 - CP
            SearchPreviewResults.Children.Clear(); //TODO: Somehow find a way to make this 'clear' command remove all memory instances to prevent memory leak.


            foreach (var id in _clientIds)
            {
                SearchPreviewResults.Children.Add(new UserControlScannerResultsSubItem(id.idclient, this, 3));
            }
        }
        public void PopulateFullPreview(object sender)
        {
            var uniqueId = ((int)sender);

            if (uniqueId != null)
            {
                //SearchResultsPreview.Children.Clear();
                previewSubsection.ChangeToNewId(uniqueId);
            }
            else
            {
                Trace.WriteLine("This shouldn't fire.");//rr
            }
        }



        public int[] initialPopupReturnInt()
        {
            int[] result = new int[1];
            return result;
        }

        public void UIResetState() //UI reset state does not reset any logic variables E.G. Current Owners. It only resets visual UI systems.
        {
            foreach (object button in RadioSearchOptions.Children) //Overall radio button reset, even if it doesn't exist.
            {
                Trace.WriteLine(button.GetType().ToString() + " is the type of Type");
                if (button.GetType().ToString() != "System.Windows.Controls.Label") //Telling code NOT to run IReset on Loose Label
                {
                    IReset radioObject = (IReset)button;
                    radioObject.ResetState();
                }
            }

            foreach (IReset searchParameter in UniformGridSearch.Children) //Resets search parameters.
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in UniformGridAdvanceSearch.Children) // Resets advance search parameters.
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in SearchResultsPreviewFull.Children) // Resets search mini previews. TODO: Find out if I should just clear children instead of looping.
            {
                searchParameter.ResetState();

            }

            UniformGridAdvanceSearch.Visibility = Visibility.Collapsed; // If advance search was open, close it.
            MoreOptionsLabel.Content = "More Options";
            MoreOptionsIcon.Kind = PackIconKind.ChevronDown;

            SearchPreviewResults.Children.Clear(); //TODO: Somehow find a way to make this 'clear' command remove all memory instances to prevent memory leak.
        }

        public void ResetState()
        {
            Trace.WriteLine("RESETING ADDITION");
            if (CurrentClients != null)
            {
                for (int i = 0; i < CurrentClients.Length; i++)
                {
                    Trace.WriteLine("Current clients numbers of usercontroladdition on reset: " + CurrentClients[i]);
                }
            }



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
            foreach (IReset fullPreviewUI in SearchResultsPreviewFull.Children)
            {
                fullPreviewUI.ResetState();

            }




            UniformGridAdvanceSearch.Visibility = Visibility.Collapsed;
            MoreOptionsLabel.Content = "More Options";
            MoreOptionsIcon.Kind = PackIconKind.ChevronDown;
            SearchPreviewResults.Children.Clear(); //TODO: Somehow find a way to make this 'clear' command remove all memory instances to prevent memory leak.
            CurrentClients = null;
        }

        private void BackButton_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (UserControlFullPreviewItemAddition additionUI in SearchResultsPreviewFull.Children) //Gets the updated list from the full preview current owners.
            {
                CurrentClients = (int[]?)additionUI.CurrentClients?.Clone();
                //Trace.WriteLine(CurrentClients.Length + "IS THE LENGTH");
                additionUI.ResetState(); //Clears the CurrentList from full preview too.
            }
            UIResetState(); //resetting current visual UI.

            if (CurrentClients != null)
            {
                for (int i = 0; i < CurrentClients.Length; i++)
                {
                    Trace.WriteLine("Current clients numbers of usercontroladdition on reset: " + CurrentClients[i]);
                }
            }
            _context.ChangeAdditionSearchVisibility(3);

        }
    }
}
