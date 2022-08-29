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
using LicenseDatabaseManagerV2.UserControlPages.Additiions;
using LicenseDatabaseManagerV2.ViewModel;

namespace LicenseDatabaseManagerV2.UserControlPages
{
    /// <summary>
    /// Interaction logic for UserControlGeneralCreate.xaml
    /// </summary>
    public partial class UserControlGeneralCreate : UserControl, IReset, IUserControllerScanner, IUserControlAddition, IUserControlCreate
    {
        public string ActiveFileType;
        public int[]? CurrentOwners; //Need to have something to feed into system.
        public int[]? CurrentClients;
        public int[]? CurrentBusinesses;

        public UserControlGeneralCreate()
        {
            InitializeComponent();
            ActiveFileType = "general";
            string fileType = "general";
            UniqueIdGrid.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("UniqueId:", fileType)));
            Section1.Visibility = Visibility.Collapsed; //initial reset of sections
            Section2.Visibility = Visibility.Collapsed;
            Section3.Visibility = Visibility.Collapsed;
            Section4.Visibility = Visibility.Collapsed;
            Section5.Visibility = Visibility.Collapsed;
            Section6.Visibility = Visibility.Collapsed;

            Section1.Visibility = Visibility.Visible;
            SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Quick Add Business")));

            Part1.Children.Add(new UserControlDataEntryFilesList(2, this, this));

            BusinessAdditionGrid.Children.Add(new UserControlBusinessAddition(this, CurrentBusinesses));

            //instantiating popup for owner search
            Section2.Visibility = Visibility.Visible;
            SectionTitle2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Quick Add Owner")));

            Part2.Children.Add(new UserControlDataEntryFilesList(1, this, this));
            
            OwnerAdditionGrid.Children.Add(new UserControlOwnerAddition(this, CurrentOwners));

            Section3.Visibility = Visibility.Visible;
            SectionTitle3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Quick Add Client")));

            Part3.Children.Add(new UserControlDataEntryFilesList(3, this, this));

            ClientAdditionGrid.Children.Add(new UserControlClientAddition(this, CurrentClients));

            Section4.Visibility = Visibility.Visible;
            SectionTitle4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Business Info")));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "Example Inc.", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County Region:", "South/North", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License Number:", "D1234", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Establishment:", "Bar, Store, etc", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Entity:", "0000", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Active:", "true/false", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Date:", "2022-03-20", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Worked Date:", "2022-03-20", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Completed Date:", "2022-03-20", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Memo:", "An example memo", false, true)));
            Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Codes:", "A,T,O", false, true)));

            Section5.Visibility = Visibility.Visible;
            SectionTitle5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Owner Address Info")));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "George", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Washington", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Position:", "Executive")));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Stock:", "10%")));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("From Date:", "2022-03-20")));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("To Date:", "2024-03-20")));
            Part5.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Blind:", "This can be anything")));

            Section6.Visibility = Visibility.Visible;
            SectionTitle6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Corporation:", "Corporation Name", false, false)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "Thomas", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Jefferson", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 11", false, false)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
            Part6.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));

        }

        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private readonly OwnerFile _ownerFile = new();
        private readonly BusinessFile _businessFile = new();
        private readonly ClientFile _clientFile = new();

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }
        private static int CreateNewClient(ClientFile clientFile)
        {
            return Sql.CreateClientFile(clientFile);
        }
        private static int CreateNewBusiness(BusinessFile businessFile)
        { 
            return Sql.CreateBusinessFile(businessFile);
        }

        private static int CreateNewOwner(OwnerFile ownerFile)
        {
           return Sql.CreateOwnerFile(ownerFile);
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            // case "business":
            if (CurrentBusinesses == null)
            {
                foreach (UserControlDataEntrySubItem target in Part4.Children)
                {
                    if (target.DataInformation.Mandatory && (target.DataInput.Text == ""))
                    {
                        MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                        // return statement stops execution
                        return;
                    }

                    // If not blank run switch
                    if (target.DataInput.Text != "")
                    {
                        switch (target.DataInformation.InputCategoryName)
                        {
                            case "DBA:":
                                _businessFile.dba = target.DataInput.Text;
                                //Trace.WriteLine(target.DataInput.Text + "Made it here");
                                break;
                            case "Address:":
                                var addressStrings = target.DataInput.Text.Split(' ');
                                _businessFile.address_line1_number = addressStrings[0];
                                _businessFile.address_line1_street_name = addressStrings[1] + " " + addressStrings[2];
                                break;
                            case "Address 2:":
                                _businessFile.address_line2 = target.DataInput.Text;
                                break;
                            case "City:":
                                _businessFile.city_name = target.DataInput.Text;
                                break;
                            case "City Code:":
                                _businessFile.city_code = target.DataInput.Text;
                                break;
                            case "County:":
                                _businessFile.county_name = target.DataInput.Text;
                                break;
                            case "State:":
                                _businessFile.state_code = target.DataInput.Text;
                                break;
                            case "County Region:":
                                // TODO: Make this better
                                if (_businessFile.state_code == "CA")
                                {
                                    _businessFile.county_region = target.DataInput.Text;
                                    break;
                                }

                                Trace.WriteLine("County Region only required in CA");
                                break;
                            case "Zip Code:":
                                _businessFile.zip = target.DataInput.Text;
                                break;
                            case "Phone:":
                                /* //Found a copy of this code, removed it and put a probably better filter, this should not be owner though, right?
                                var filteredPhoneString =
                                    string.Concat(target.DataInput.Text.Where(c => !char.IsWhiteSpace(c)));
                                if (Regex.IsMatch(filteredPhoneString, @"\([0-9]{3}\)[0-9]{3}-[0-9]{4}"))
                                {
                                    var filteredPhoneStrings = filteredPhoneString.Split(')');
                                    _ownerFile.area_code = filteredPhoneStrings[0].Replace("(", "");
                                    _ownerFile.phone_number = filteredPhoneStrings[1].Replace("-", "");
                                    Trace.WriteLine("Correct pattern detected");
                                    break;
                                }
                                */
                                var filteredPhoneString =
                                    string.Concat(target.DataInput.Text.Where(c => !char.IsWhiteSpace(c)));
                                filteredPhoneString = filteredPhoneString.Replace("(", "");
                                filteredPhoneString = filteredPhoneString.Replace(")", "");
                                filteredPhoneString = filteredPhoneString.Replace("-", "");
                                if (!(filteredPhoneString.Length > 10 || filteredPhoneString.Length < 10))
                                {
                                    _businessFile.area_code = filteredPhoneString.Substring(0, 3);
                                    _businessFile.phone_number =
                                        filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
                                    break;
                                }
                                else
                                {
                                    Trace.WriteLine("incorrect pattern detected");
                                    break;
                                }
                            case "License Number:":
                                _businessFile.license_number = target.DataInput.Text;
                                break;
                            case "Establishment:":
                                _businessFile.establishment = target.DataInput.Text;
                                break;
                            case "Entity:":
                                _businessFile.entity = target.DataInput.Text;
                                break;
                            case "Active:":
                                // Convert string to boolean... this is probably really inefficient will fix l8er
                                _businessFile.active = bool.Parse(target.DataInput.Text);
                                break;
                            case "Activity Date:":
                                _businessFile.activity_date = target.DataInput.Text;
                                break;
                            case "Worked Date:":
                                _businessFile.worked_date = target.DataInput.Text;
                                break;
                            case "Completed Date:":
                                _businessFile.completed_date = target.DataInput.Text;
                                break;
                            case "Memo:":
                                _businessFile.memo_text = target.DataInput.Text;
                                break;
                            case "Activity Codes:":
                                // build list of activity codes split by a comma
                                var listOfCodes = target.DataInput.Text.Split(',').ToList();
                                _businessFile.business_activity_codes = new List<BusinessActivityCode>();
                                // loop through list and save each activity code to activity code list in businessfile
                                foreach (var code in listOfCodes)
                                {
                                    _businessFile.business_activity_codes.Add(new BusinessActivityCode()
                                        { activity_code_name = code });
                                }

                                break;
                            default:
                                Trace.WriteLine("Error: 100. Invalid category name detected.");
                                break;
                        }
                    }
                }

                var tempList = new List<int>();
                tempList.Add(CreateNewBusiness(_businessFile));
                CurrentBusinesses = tempList.ToArray();
            }

            if (CurrentOwners == null)
            {
                foreach (UserControlDataEntrySubItem target in Part5.Children)
                {
                    // if input text is default or blank return an error and stop execution
                    if (target.DataInformation.Mandatory && (target.DataInput.Text == ""))
                    {
                        MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                        // return statement stops execution
                        return;
                    }

                    if (target.DataInput.Text != "")
                    {
                        switch (target.DataInformation.InputCategoryName)
                        {
                            case "First Name:":
                                _ownerFile.first_name = target.DataInput.Text;
                                break;
                            case "Last Name:":
                                _ownerFile.last_name = target.DataInput.Text;
                                break;
                            case "Address:":
                                var addressStrings = target.DataInput.Text.Split(' ');
                                _ownerFile.address_line1_number = addressStrings[0];
                                _ownerFile.address_line1_street_name = addressStrings[1] + " " + addressStrings[2];
                                break;
                            case "Address 2:":
                                _ownerFile.address_line2 = target.DataInput.Text;
                                break;
                            case "City:":
                                _ownerFile.city_name = target.DataInput.Text;
                                break;
                            case "City Code:":
                                _ownerFile.city_code = target.DataInput.Text;
                                break;
                            case "County:":
                                _ownerFile.county_name = target.DataInput.Text;
                                break;
                            case "State:":
                                _ownerFile.state_code = target.DataInput.Text;
                                break;
                            case "Zip Code:":
                                _ownerFile.zip = target.DataInput.Text;
                                break;
                            case "Phone:":
                                /* this way is possibly cringe, adding a better way 'hopefully'
                                var filteredPhoneString =
                                    string.Concat(target.DataInput.Text.Where(c => !char.IsWhiteSpace(c)));
                                if (Regex.IsMatch(filteredPhoneString, @"\([0-9]{3}\)[0-9]{3}-[0-9]{4}"))
                                {
                                    var filteredPhoneStrings = filteredPhoneString.Split(')');
                                    _ownerFile.area_code = filteredPhoneStrings[0].Replace("(", "");
                                    _ownerFile.phone_number = filteredPhoneStrings[1].Replace("-", "");
                                    Trace.WriteLine("Correct pattern detected");
                                    break;
                                }
                                */
                                var filteredPhoneString =
                                    string.Concat(target.DataInput.Text.Where(c => !char.IsWhiteSpace(c)));
                                filteredPhoneString = filteredPhoneString.Replace("(", "");
                                filteredPhoneString = filteredPhoneString.Replace(")", "");
                                filteredPhoneString = filteredPhoneString.Replace("-", "");
                                if (!(filteredPhoneString.Length > 10 || filteredPhoneString.Length < 10))
                                {
                                    _ownerFile.area_code = filteredPhoneString.Substring(0, 3);
                                    _ownerFile.phone_number =
                                        filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
                                    break;
                                }
                                else
                                {
                                    Trace.WriteLine("incorrect pattern detected");
                                    break;
                                }
                            case "Position:":
                                _ownerFile.title = target.DataInput.Text;
                                break;
                            case "Stock:":
                                _ownerFile.stock = target.DataInput.Text;
                                break;
                            case "From Date:":
                                _ownerFile.from_date = target.DataInput.Text;
                                break;
                            case "To Date:":
                                _ownerFile.to_date = target.DataInput.Text;
                                break;
                            case "Blind:":
                                _ownerFile.blind_text = target.DataInput.Text;
                                break;
                            default:
                                Trace.WriteLine("Error: 100. Invalid category name detected.");
                                break;
                        }
                    }
                }
                var tempList = new List<int>();
                tempList.Add(CreateNewOwner(_ownerFile));
                CurrentOwners = tempList.ToArray();
            }

            if (CurrentClients == null)
            {
                foreach (UserControlDataEntrySubItem target in Part6.Children)
                {
                    // if input text is default or blank return an error and stop execution
                    if (target.DataInformation.Mandatory && (target.DataInput.Text == ""))
                    {
                        MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                        // return statement stops execution
                        return;
                    }

                    if (target.DataInput.Text != "")
                    {
                        switch (target.DataInformation.InputCategoryName)
                        {
                            case "First Name:":
                                _clientFile.first_name = target.DataInput.Text;
                                break;
                            case "Last Name:":
                                _clientFile.last_name = target.DataInput.Text;
                                break;
                            case "Address:":
                                var addressStrings = target.DataInput.Text.Split(' ');
                                _clientFile.address_line1_number = addressStrings[0];
                                _clientFile.address_line1_street_name = addressStrings[1] + " " + addressStrings[2];
                                break;
                            case "Address 2:":
                                _clientFile.address_line2 = target.DataInput.Text;
                                break;
                            case "City:":
                                _clientFile.city_name = target.DataInput.Text;
                                break;
                            case "City Code:":
                                _clientFile.city_code = target.DataInput.Text;
                                break;
                            case "County:":
                                _clientFile.county_name = target.DataInput.Text;
                                break;
                            case "State:":
                                _clientFile.state_code = target.DataInput.Text;
                                break;
                            case "Zip Code:":
                                _clientFile.zip = target.DataInput.Text;
                                break;
                            case "Phone:":

                                var filteredPhoneString =
                                    string.Concat(target.DataInput.Text.Where(c => !char.IsWhiteSpace(c)));
                                filteredPhoneString = filteredPhoneString.Replace("(", "");
                                filteredPhoneString = filteredPhoneString.Replace(")", "");
                                filteredPhoneString = filteredPhoneString.Replace("-", "");
                                if (!(filteredPhoneString.Length > 10 || filteredPhoneString.Length < 10))
                                {
                                    _clientFile.area_code = filteredPhoneString.Substring(0, 3);
                                    _clientFile.phone_number =
                                        filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
                                    break;
                                }
                                else
                                {
                                    Trace.WriteLine("incorrect pattern detected");
                                    break;
                                }
                            default:
                                Trace.WriteLine("Error: 100. Invalid category name detected.");
                                break;
                        }
                    }
                }
                var tempList = new List<int>();
                tempList.Add(CreateNewClient(_clientFile));
                CurrentClients = tempList.ToArray();
            }
            Sql.CreateGeneralFile(CurrentBusinesses[0], CurrentOwners.ToList(), CurrentClients.ToList());
            Trace.WriteLine($"Business: {CurrentBusinesses[0]}\nOwners: {CurrentOwners[0]}\nClients: {CurrentClients[0]}");
            

            //ResetState();
        }

        public void ResetState()
        {
            foreach (IReset searchParameter in Part1.Children)  //resetting the data entry input starting values
            {
                searchParameter.ResetState();
                Trace.WriteLine("REEEEEEEEEEEEEEEEEESET");

            }
            foreach (IReset ownerPreview in Part2.Children)
            {
                ownerPreview.ResetState();

            }
            foreach (IReset searchParameter in Part3.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Part4.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Part5.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Part6.Children)
            {
                searchParameter.ResetState();

            }

            foreach (IReset searchParameter in OwnerAdditionGrid.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in ClientAdditionGrid.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in BusinessAdditionGrid.Children)
            {
                searchParameter.ResetState();

            }

            OwnerAdditionPopup.IsOpen = false;
            CurrentOwners = null;

            ClientAdditionPopup.IsOpen = false;
            CurrentClients = null;

            BusinessAdditionPopup.IsOpen = false;
            CurrentBusinesses = null;

            UpdateAdditionCurrentOwnerArrays();
            UpdateAdditionCurrentClientArrays();
            UpdateAdditionCurrentBusinessArrays();

        }

        public void PopulateFullPreview(object sender)
        {
            throw new NotImplementedException("May add this in future date for mouse over preview");
        }

        public void UpdateAdditionCurrentOwnerArrays() //Updates Owner Addition Current Owner Arrays and Addition Full Preview Current Owner Arrays.
        {
            foreach (UserControlOwnerAddition additionUI in OwnerAdditionGrid.Children)
            {
                //Trace.WriteLine("The length of additionUI current is: " + additionUI.CurrentOwners?.Length + " and is being assigned to general create");

                additionUI.CurrentOwners = (int[]?)CurrentOwners?.Clone();
                //Trace.WriteLine("The length of general owner current is: " + CurrentOwners?.Length);

                foreach (UserControlFullPreviewItemAddition additionPreview in additionUI.SearchResultsPreviewFull.Children)
                { //This goes into the full preview currentowners variable and updates that as well.

                    additionPreview.CurrentOwners = (int[]?)CurrentOwners?.Clone();

                }
            }
        }
        public void UpdateGeneralCurrentOwnerArray() //Gets the CurrentOwner array from OwnerAdditionUserControl and assigns it to general currentowners.
        {
            foreach (UserControlOwnerAddition additionUI in OwnerAdditionGrid.Children)
            {

                CurrentOwners = (int[]?)additionUI.CurrentOwners?.Clone(); //Should I use clone?
            }
        }

        public void UpdateAdditionCurrentBusinessArrays() //Updates Owner Addition Current Owner Arrays and Addition Full Preview Current Owner Arrays.
        {
            foreach (UserControlBusinessAddition additionUI in BusinessAdditionGrid.Children)
            {
                //Trace.WriteLine("The length of additionUI current is: " + additionUI.CurrentOwners?.Length + " and is being assigned to general create");
                
                additionUI.CurrentBusinesses = (int[]?)CurrentBusinesses?.Clone();
                //Trace.WriteLine("The length of general owner current is: " + CurrentOwners?.Length);

                foreach (UserControlFullPreviewItemAddition additionPreview in additionUI.SearchResultsPreviewFull.Children)
                { //This goes into the full preview currentowners variable and updates that as well.
                    additionPreview.CurrentBusinesses = (int[]?)CurrentBusinesses?.Clone();
                }
            }
        }
        public void UpdateGeneralCurrentBusinessArray() //Gets the CurrentBusiness array from businessAdditionUserControl and assigns it to general CurrentBusinesses.
        {
            foreach (UserControlBusinessAddition additionUI in BusinessAdditionGrid.Children)
            {
                CurrentBusinesses = (int[]?)additionUI.CurrentBusinesses?.Clone(); //Should I use clone?
            }
        }

        public void UpdateAdditionCurrentClientArrays() //Updates Owner Addition Current Owner Arrays and Addition Full Preview Current Owner Arrays.
        {
            foreach (UserControlClientAddition additionUI in ClientAdditionGrid.Children)
            {
                //Trace.WriteLine("The length of additionUI current is: " + additionUI.CurrentOwners?.Length + " and is being assigned to general create");
                additionUI.CurrentClients = (int[]?)CurrentClients?.Clone();
                //Trace.WriteLine("The length of general owner current is: " + CurrentOwners?.Length);

                foreach (UserControlFullPreviewItemAddition additionPreview in additionUI.SearchResultsPreviewFull.Children)
                { //This goes into the full preview currentowners variable and updates that as well.
                    additionPreview.CurrentClients = (int[]?)CurrentClients?.Clone();
                }
            }
        }
        public void UpdateGeneralCurrentClientArray() //Gets the CurrentOwner array from OwnerAdditionUserControl and assigns it to general currentowners.
        {
            foreach (UserControlClientAddition additionUI in ClientAdditionGrid.Children)
            {
                CurrentClients = (int[]?)additionUI.CurrentClients?.Clone(); //Should I use clone?
            }
        }



        public void ChangeAdditionSearchVisibility(int type) //TODO: Make sure that if I add another type of addition search change it knowns what to do.
        {
            //Trace.WriteLine("EXECUTING CHANGE ADDITION. Type is: "+type);
            if (type == 0) // General change addition activation
            {

            }
            else if (type == 1) //owner change addition activation
            {
                foreach (IReset ownerPreviews in Part2.Children)
                {
                    ownerPreviews.ResetState();

                }

                if (OwnerAdditionPopup.IsOpen) //If the addition popup is open this starts the process to remove the former owners shown and update the list.
                {
                    Trace.WriteLine("Resetting new general file with new variables added.");

                    UpdateGeneralCurrentOwnerArray();

                    foreach (UserControlDataEntryFilesList dataEntryFiles in Part2.Children)
                    {
                        Trace.WriteLine("Trying to repopulate general list");


                        dataEntryFiles.RepopulateState(CurrentOwners);

                    }
                }
                else if (!OwnerAdditionPopup.IsOpen)
                {
                    UpdateAdditionCurrentOwnerArrays();
                }

                OwnerAdditionPopup.IsOpen = OwnerAdditionPopup.IsOpen == false; //If the popup is open, close. If close, open. 
            }
            else if (type == 2) //license change addition activation
            {
                foreach (IReset clientPreviews in Part1.Children)
                {
                    clientPreviews.ResetState();

                }

                if (BusinessAdditionPopup.IsOpen) //If the addition popup is open this starts the process to remove the former owners shown and update the list.
                {
                    Trace.WriteLine("Resetting new general file with new variables added.");

                    UpdateGeneralCurrentBusinessArray();

                    foreach (UserControlDataEntryFilesList dataEntryFiles in Part1.Children)
                    {
                        Trace.WriteLine("Trying to repopulate general list");


                        dataEntryFiles.RepopulateState(CurrentBusinesses);

                    }
                }
                else if (!BusinessAdditionPopup.IsOpen)
                {
                    UpdateAdditionCurrentBusinessArrays();
                }

                BusinessAdditionPopup.IsOpen = BusinessAdditionPopup.IsOpen == false; //If the popup is open, close. If close, open. 
            }
            else if (type == 3) //client change addition activation
            {
                foreach (IReset clientPreviews in Part3.Children)
                {
                    clientPreviews.ResetState();

                }

                if (ClientAdditionPopup.IsOpen) //If the addition popup is open this starts the process to remove the former owners shown and update the list.
                {
                    Trace.WriteLine("Resetting new general file with new variables added.");

                    UpdateGeneralCurrentClientArray();

                    foreach (UserControlDataEntryFilesList dataEntryFiles in Part3.Children)
                    {
                        Trace.WriteLine("Trying to repopulate general list");


                        dataEntryFiles.RepopulateState(CurrentClients);

                    }
                }
                else if (!ClientAdditionPopup.IsOpen)
                {
                    UpdateAdditionCurrentClientArrays();
                }
                ClientAdditionPopup.IsOpen = ClientAdditionPopup.IsOpen == false; //If the popup is open, close. If close, open. 
            }
            else
            {
                throw new NotImplementedException("Unknown file change addition");
            }
        }

        public void RemovePopulateEntryArray(int type,int uniqueId) //This is called by the DataEntryResultsSubItem to remove itself by pressing the "X" button UI.
        {
            if (type == 0) // General change addition activation
            {

            }
            else if (type == 1) //owner change addition activation
            {
                for (int i = 0; i < CurrentOwners.Length; i++)
                {
                    Trace.WriteLine(CurrentOwners[i]);
                }
                List<int> tempList = CurrentOwners.ToList();
                tempList.Remove(uniqueId); //will probably never be null to be honest
                if (tempList.Count > 0)
                {
                    CurrentOwners = tempList.ToArray();
                }
                else
                {
                    CurrentOwners = null;
                }

                UpdateAdditionCurrentOwnerArrays();

                foreach (UserControlDataEntryFilesList dataEntryFiles in Part2.Children)
                {
                    dataEntryFiles.RepopulateState(CurrentOwners);
                }
            }
            else if (type == 2) //business change addition activation
            {
                for (int i = 0; i < CurrentBusinesses.Length; i++)
                {
                    Trace.WriteLine(CurrentBusinesses[i]);
                }
                List<int> tempList = CurrentBusinesses.ToList();
                tempList.Remove(uniqueId); //will probably never be null to be honest
                if (tempList.Count > 0)
                {
                    CurrentBusinesses = tempList.ToArray();

                }
                else
                {
                    CurrentBusinesses = null;
                }
                UpdateAdditionCurrentBusinessArrays();

                foreach (UserControlDataEntryFilesList dataEntryFiles in Part1.Children)
                {
                    dataEntryFiles.RepopulateState(CurrentBusinesses);
                }
            }
            else if (type == 3) //client change addition activation
            {
                for (int i = 0; i < CurrentClients.Length; i++)
                {
                    Trace.WriteLine(CurrentClients[i]);
                }
                List<int> tempList = CurrentClients.ToList();
                tempList.Remove(uniqueId); //will probably never be null to be honest
                if (tempList.Count > 0)
                {
                    CurrentClients = tempList.ToArray();

                }
                else
                {
                    CurrentClients = null;
                }
                UpdateAdditionCurrentClientArrays();

                foreach (UserControlDataEntryFilesList dataEntryFiles in Part3.Children)
                {
                    dataEntryFiles.RepopulateState(CurrentClients);
                }
            }
            else
            {
                throw new NotImplementedException("Unknown file change addition");
            }
        }
    }
}
