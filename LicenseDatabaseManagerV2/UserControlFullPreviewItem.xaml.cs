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
using DataAccessLibrary.Models.BusinessData;
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.UserControlPages;
using LicenseDatabaseManagerV2.UserControlPages.Additiions;
using LicenseDatabaseManagerV2.ViewModel;
using MaterialDesignThemes.Wpf;

namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlFullPreviewItem.xaml
    /// </summary>
    public partial class UserControlFullPreviewItem : UserControl, IReset ,IUserControllerScanner, IUserControlAddition, IUserControlCreate
    {
        /*
        0: General File
        1: Owner File
        2: Business File
        3: Client File

        */
        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private BusinessFile _businessFile = new();
        private ClientFile _clientFile = new();
        private OwnerFile _ownerFile = new();
        private GeneralFile _generalFile = new();
        private IUserControllerScanner _context;

        public int[]? CurrentOwners; //Need to have something to feed into system.
        public int[]? CurrentClients;
        public int[]? CurrentBusinesses;

        


        private int? currentUniqueId = null;

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }
        public UserControlFullPreviewItem(int uniqueId, int typeOfMain, IUserControllerScanner context)
        {
            InitializeComponent();
            UniqueIdGrid.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("UniqueId:", ""+uniqueId)));


            if (typeOfMain == 0)
            {




                SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Business Info")));

                QuickSearch1.Children.Add(new UserControlDataEntryFilesList(2, this, this));
                BusinessAdditionGrid.Children.Add(new UserControlBusinessAddition(this, CurrentBusinesses));
                BusinessAdditionPopup.PlacementTarget = (UIElement)context;
                //QuickSearch1.Margin = new Thickness(15);

                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "Joe's Crab Shack")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License:", "L2634", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County Region:", "South/North", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zipcode:", "91304", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Establishment:", "Bar, Store, etc", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Entity:", "0000", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Active:", "true/false", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Date:", "2022-12-25", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Worked Date:", "2022-12-25", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Completed Date:", "2022-12-25", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Memo:", "Sample memo text here", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Codes:", "A,T,O", false, true)));


                SectionTitle2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Owner Info")));

                //This here is the "Quick add search function for owners
                QuickSearch2.Children.Add(new UserControlDataEntryFilesList(1, this, this));
                OwnerAdditionGrid.Children.Add(new UserControlOwnerAddition(this, CurrentOwners));
                OwnerAdditionPopup.PlacementTarget = (UIElement)context;
                //QuickSearch2.Margin = new Thickness(15);

                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "George", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Washington", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Position:", "Executive")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Stock:", "10%")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("From Date:", "03-20-2022")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("To Date:", "03-20-2024")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Blind:", "This can be anything")));





                SectionTitle3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));

                QuickSearch3.Children.Add(new UserControlDataEntryFilesList(3, this, this));
                ClientAdditionGrid.Children.Add(new UserControlClientAddition(this, CurrentClients));
                ClientAdditionPopup.PlacementTarget = (UIElement)context;
                //QuickSearch3.Margin = new Thickness(15);

                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Corporation:", "Corporation Name", false, false)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "Thomas", false, false)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Jefferson", false, false)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 11", false, false)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));
            }
            else if (typeOfMain == 1)
            {
                SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Owner Info")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "George", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Washington", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Position:", "Executive", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Stock:", "10%", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("From Date:", "2022-03-20", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("To Date:", "2024-03-20", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Blind:", "This can be anything", false, true)));

            }
            else if (typeOfMain == 2)
            {
                SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Business Info")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "Joe's Crab Shack")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License:", "L2634", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County Region:", "South/North", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zipcode:", "91304", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Establishment:", "Bar, Store, etc", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Entity:", "0000", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Active:", "true/false", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Date:", "2022-12-25", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Worked Date:", "2022-12-25", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Completed Date:", "2022-12-25", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Memo:", "Sample memo text here", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Codes:", "A,T,O", false, true)));

            }
            else if (typeOfMain == 3)
            {
                SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Corporation:", "Corporation Name", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "Thomas", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Jefferson", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 11", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));
            }
            else
            {
                throw new Exception("No valid type of main preview given");
            }

            _context = context;
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (SaveItem.IsEnabled == false)
            {
                if (currentUniqueId != null)
                {
                    Row1.IsEnabled = true;
                    Row2.IsEnabled = true;
                    Row3.IsEnabled = true;
                    Row4.IsEnabled = true;
                    SaveItem.IsEnabled = true;
                    EditItemLabel.Content = "Cancel Edit";
                    EditItemIcon.Kind = PackIconKind.Cancel;
                }
                
            }
            else
            {
                Row1.IsEnabled = false;
                Row2.IsEnabled = false;
                Row3.IsEnabled = false;
                Row4.IsEnabled = false;
                SaveItem.IsEnabled = false;
                EditItemLabel.Content = "Edit";
                // TODO: This has been temp. disabled. Review w/ Pretti
                //if (currentUniqueId != null)
                //{
                //    Trace.WriteLine("Cancel Request on unique ID: "+currentUniqueId);
                //    ChangeToNewId((int)currentUniqueId);
                //}
                EditItemIcon.Kind = PackIconKind.LeadPencil;
            }
            //TODO:Make Entries reset to original load data.
        }


        private void SaveItem_OnClickItem_Click(object sender, RoutedEventArgs e)
        {
            //var updatedOwnerFile = new OwnerFile();
            // TODO: Searched owner file retains values that get passed into new (happening because they're not getting set by some code somewhere)
            // Trace.WriteLine("Printing out variables that would be saved!");

            // NOTE: The unique id of the owner file we're working with is set by Change to new ID; It is not modified in the current method
            if (_ownerFile.idowner != 0)
            {
                foreach (UserControlDataEntrySubItem target in Info1.Children)
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
                            var filteredPhoneString =
                                string.Concat(target.DataInput.Text.Where(c => !char.IsWhiteSpace(c)));
                            filteredPhoneString = filteredPhoneString.Replace("(", "");
                            filteredPhoneString = filteredPhoneString.Replace(")", "");
                            filteredPhoneString = filteredPhoneString.Replace("-", "");
                            if (!(filteredPhoneString.Length > 10 || filteredPhoneString.Length < 10))
                            {
                                _ownerFile.area_code = filteredPhoneString.Substring(0, 3);
                                _ownerFile.phone_number = filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
                                break;
                            }
                            else // TODO: test removing redundant else
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
                    }
                }

                //foreach (UserControlDataEntrySubItem target in Info2.Children)
                //{
                //    Trace.WriteLine(target.DataInformation.InputCategoryName + " is the name of the variable");
                //    Trace.WriteLine("Text present in search box: " + target.DataInput.Text);
                //}
                //foreach (UserControlDataEntrySubItem target in Info3.Children)
                //{
                //    Trace.WriteLine(target.DataInformation.InputCategoryName + " is the name of the variable");
                //    Trace.WriteLine("Text present in search box: " + target.DataInput.Text);
                //}
                //foreach (UserControlDataEntrySubItem target in Info4.Children)
                //{
                //    Trace.WriteLine(target.DataInformation.InputCategoryName + " is the name of the variable");
                //    Trace.WriteLine("Text present in search box: " + target.DataInput.Text);
                //}
                Sql.UpdateOwnerFile(_ownerFile);
            }
            else if (_businessFile.idbusiness != 0)
            {
                foreach (UserControlDataEntrySubItem target in Info1.Children)
                {
                    switch (target.DataInformation.InputCategoryName)
                    {
                        case "DBA:":
                            _businessFile.dba = target.DataInput.Text;
                            break;
                        case "License:":
                            _businessFile.license_number = target.DataInput.Text;
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
                        case "County Region:":
                            _businessFile.county_region = target.DataInput.Text;
                            break;
                        case "State:":
                            _businessFile.state_code = target.DataInput.Text;
                            break;
                        case "Zip Code:":
                            _businessFile.zip = target.DataInput.Text;
                            break;
                        case "Phone:":
                            var filteredPhoneString =
                                string.Concat(target.DataInput.Text.Where(c => !char.IsWhiteSpace(c)));
                            filteredPhoneString = filteredPhoneString.Replace("(", "");
                            filteredPhoneString = filteredPhoneString.Replace(")", "");
                            filteredPhoneString = filteredPhoneString.Replace("-", "");
                            if (!(filteredPhoneString.Length > 10 || filteredPhoneString.Length < 10))
                            {
                                _businessFile.area_code = filteredPhoneString.Substring(0, 3);
                                _businessFile.phone_number = filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
                                break;
                            }
                            else // TODO: test removing redundant else
                            {
                                Trace.WriteLine("incorrect pattern detected");
                                break;
                            }
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
                                _businessFile.business_activity_codes.Add(new BusinessActivityCode() { activity_code_name = code });
                            }
                            break;
                        default:
                            Trace.WriteLine("Error: 100. Invalid category name detected.");
                            break;
                    }
                }
                Sql.UpdateBusinessFile(_businessFile);
            }
            else if (_clientFile.idclient != 0)
            {
                foreach (UserControlDataEntrySubItem target in Info1.Children)
                {
                    // if input text is default or blank return an error and stop execution
                    if (target.DataInformation.Mandatory && ((target.DataInput.Text == target.DataInformation.DataEntryText && target.DataInformation.HasBeenClicked == false) || target.DataInput.Text == ""))
                    {
                        MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                        // return statement stops execution
                        return;
                    }
                    // If not blank run switch
                    else if (target.DataInput.Text != "")
                    {
                        switch (target.DataInformation.InputCategoryName)
                        {
                            case "Corporation:":
                                _clientFile.business_name = target.DataInput.Text;
                                break;
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
                                    _clientFile.phone_number = filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
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
                Sql.UpdateClientFile(_clientFile);
            }
            

        }

        public void ChangeToNewId(int uniqueId, int typeOfMain)
        {
            this.ResetState();
            Trace.WriteLine("Changing to new ID: " + uniqueId);
            currentUniqueId = uniqueId; //setting integer value for current uniqueId
            foreach (UserControlPreviewLabelsSubItem target in UniqueIdGrid.Children)
            {
                if (target.Var1Label.Content.ToString() == "UniqueId:")
                {
                    target.Var1Value.Content = uniqueId;
                }

            }

            switch(typeOfMain)
            {
                case 0:
                    _generalFile = Sql.GetGeneralFileById(idBusiness: uniqueId);
                    Trace.WriteLine(_generalFile.idbusiness + " is the general file business");
                    
                    int[]? temp = new int[1];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = _generalFile.idbusiness;
                    }
                    CurrentBusinesses = temp;
                    CurrentOwners = _generalFile.owner_idowner.ToArray();
                    CurrentClients = _generalFile.client_idclient.ToArray();


                    Trace.WriteLine(_generalFile.owner_idowner.Count + " is its length of idowner");
                    Trace.WriteLine(_generalFile.client_idclient.Count + " is its length of idclient");

                    
                    foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch1.Children)
                    {
                        Trace.WriteLine("Trying to repopulate business list");


                        dataEntryFiles.RepopulateState(CurrentBusinesses);

                    }
                    UpdateAdditionCurrentBusinessArrays();
                    
                    foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch2.Children)
                    {
                        Trace.WriteLine("Trying to repopulate owner list");
                        Trace.WriteLine(CurrentOwners.Length + " is its length");
                        foreach (int x in CurrentOwners)
                        {
                            Trace.WriteLine(x + " is a valid owner id");
                        }

                        dataEntryFiles.RepopulateState(CurrentOwners);

                    }
                    UpdateAdditionCurrentOwnerArrays();

                    foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch3.Children)
                    {
                        Trace.WriteLine("Trying to repopulate client list");
                        Trace.WriteLine(CurrentClients.Length+" is its length");
                        foreach(int x in CurrentClients)
                        {
                            Trace.WriteLine(x+ " is a valid client id");
                        }

                        dataEntryFiles.RepopulateState(CurrentClients);

                    }
                    UpdateAdditionCurrentClientArrays();
                    

                    break;
                case 1:
                    _ownerFile = Sql.GetOwnerFileById(uniqueId);
                    _ownerFile.idowner = uniqueId;
                    foreach (UserControlDataEntrySubItem target in Info1.Children)
                    {
                        target.DataInput.Foreground = Brushes.Black;
                        switch (target.DataName.Content)
                        {
                            case "First Name:":
                                target.DataInput.Text = _ownerFile.first_name;
                                break;
                            case "Last Name:":
                                target.DataInput.Text = _ownerFile.last_name;
                                break;
                            case "Address:":
                                target.DataInput.Text =
                                    $"{_ownerFile.address_line1_number} {_ownerFile.address_line1_street_name}";
                                break;
                            case "Address 2:":
                                target.DataInput.Text = _ownerFile.address_line2;
                                break;
                            case "City:":
                                target.DataInput.Text = _ownerFile.city_name;
                                break;
                            case "City Code:":
                                target.DataInput.Text = _ownerFile.city_code;
                                break;
                            case "County:":
                                target.DataInput.Text = _ownerFile.county_name;
                                break;
                            case "State:":
                                target.DataInput.Text = _ownerFile.state_code;
                                break;
                            case "Zip Code:":
                                target.DataInput.Text = _ownerFile.zip;
                                break;
                            case "Phone:":
                                target.DataInput.Text = $"{_ownerFile.area_code} {_ownerFile.phone_number}";
                                break;
                            case "Position:":
                                target.DataInput.Text = _ownerFile.title;
                                break;
                            case "Stock:":
                                target.DataInput.Text = _ownerFile.stock;
                                break;
                            case "From Date:":
                                target.DataInput.Text = _ownerFile.from_date;
                                break;
                            case "To Date:":
                                target.DataInput.Text = _ownerFile.to_date;
                                break;
                            case "Blind:":
                                target.DataInput.Text = _ownerFile.blind_text;
                                break;
                        }
                    }
                    break;

                case 2:
                    _businessFile = Sql.GetBusinessFileById(uniqueId);
                    _businessFile.idbusiness = uniqueId;
                    foreach (UserControlDataEntrySubItem target in Info1.Children)
                    {
                        target.DataInput.Foreground = Brushes.Black;
                        switch (target.DataName.Content)
                        {
                            case "DBA:":
                                target.DataInput.Text = _businessFile.dba;
                                break;
                            case "License:":
                                target.DataInput.Text = _businessFile.license_number;
                                break;
                            case "Address:":
                                target.DataInput.Text = $"{_businessFile.address_line1_number} {_businessFile.address_line1_street_name}";
                                break;
                            case "Address 2:":
                                target.DataInput.Text = _businessFile.address_line2;
                                break;
                            case "City:":
                                target.DataInput.Text = _businessFile.city_name;
                                break;
                            case "City Code:":
                                target.DataInput.Text = _businessFile.city_code;
                                break;
                            case "County:":
                                target.DataInput.Text = _businessFile.county_name;
                                break;
                            case "County Region:":
                                target.DataInput.Text = _businessFile.county_region;
                                break;
                            case "State:":
                                target.DataInput.Text = _businessFile.state_code;
                                break;
                            case "Zipcode:":
                                target.DataInput.Text = _businessFile.zip;
                                break;
                            case "Phone:":
                                target.DataInput.Text = $"{_businessFile.area_code} {_businessFile.phone_number}";
                                break;
                            case "Establishment:":
                                target.DataInput.Text = _businessFile.establishment;
                                break;
                            case "Entity:":
                                target.DataInput.Text = _businessFile.entity;
                                break;
                            case "Active:":
                                target.DataInput.Text = _businessFile.active.ToString();
                                break;
                            case "Activity Date:":
                                target.DataInput.Text = _businessFile.activity_date;
                                break;
                            case "Completed Date:":
                                target.DataInput.Text = _businessFile.completed_date;
                                break;
                            case "Memo:":
                                target.DataInput.Text = _businessFile.memo_text;
                                break;
                            case "Activity Codes:":
                                // If activity codes is empty or has one entry then print that an exit
                                if (_businessFile.business_activity_codes.Count == 1)
                                {
                                    target.DataInput.Text = _businessFile.business_activity_codes[0].activity_code_name;
                                }
                                // In all other cases loop through list
                                else
                                {
                                    // Add every index except last to text
                                    // We do the above to add commas after every activity code
                                    for (int i = 0; i < _businessFile.business_activity_codes.Count - 1; i++)
                                    {
                                        target.DataInput.Text +=
                                            $"{_businessFile.business_activity_codes[i].activity_code_name},";
                                    }
                                    // Add last activity code to text (no comma at end)
                                    target.DataInput.Text +=
                                        _businessFile.business_activity_codes.Last().activity_code_name;
                                }

                                break;
                                
                        }
                    }
                    break;

                case 3:
                    _clientFile = Sql.GetClientFileById(uniqueId);
                    _clientFile.idclient = uniqueId;
                    foreach (UserControlDataEntrySubItem target in Info1.Children)
                    {
                        target.DataInput.Foreground = Brushes.Black;
                        switch (target.DataName.Content)
                        {
                            case "Corporation:":
                                target.DataInput.Text = _clientFile.business_name;
                                break;
                            case "First Name:":
                                target.DataInput.Text = _clientFile.first_name;
                                break;
                            case "Last Name:":
                                target.DataInput.Text = _clientFile.last_name;
                                break;
                            case "Address:":
                                target.DataInput.Text =
                                    $"{_clientFile.address_line1_number} {_clientFile.address_line1_street_name}";
                                break;
                            case "Address 2:":
                                target.DataInput.Text = _clientFile.address_line2;
                                break;
                            case "City:":
                                target.DataInput.Text = _clientFile.city_name;
                                break;
                            case "City Code:":
                                target.DataInput.Text = _clientFile.city_code;
                                break;
                            case "County:":
                                target.DataInput.Text = _clientFile.county_name;
                                break;
                            case "State:":
                                target.DataInput.Text = _clientFile.state_code;
                                break;
                            case "Zip Code:":
                                target.DataInput.Text = _clientFile.zip;
                                break;
                            case "Phone:":
                                target.DataInput.Text = _clientFile.phone_number;
                                break;
                        }
                    }
                    break;
        }
    }

        public void ResetState()
        {
            foreach (IReset searchParameter in Info1.Children)  //resetting the data entry input starting values
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Info2.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Info3.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Info4.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in QuickSearch1.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in QuickSearch2.Children)
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in QuickSearch3.Children)
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

            //QuickSearch1.Margin = new Thickness(0);
            //QuickSearch2.Margin = new Thickness(0);
            //QuickSearch3.Margin = new Thickness(0);

            UpdateAdditionCurrentOwnerArrays();
            UpdateAdditionCurrentClientArrays();
            UpdateAdditionCurrentBusinessArrays();


            Row1.IsEnabled = false;
            Row2.IsEnabled = false;
            Row3.IsEnabled = false;
            Row4.IsEnabled = false;
            SaveItem.IsEnabled = false;
            EditItemLabel.Content = "Edit";
            currentUniqueId = null;
            EditItemIcon.Kind = PackIconKind.LeadPencil;
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

        public void ChangeAdditionSearchVisibility(int type)
        {
            //Trace.WriteLine("EXECUTING CHANGE ADDITION. Type is: "+type);
            if (type == 0) // General change addition activation
            {

            }
            else if (type == 1) //owner change addition activation
            {
                foreach (IReset ownerPreviews in QuickSearch2.Children)
                {
                    ownerPreviews.ResetState();

                }

                if (OwnerAdditionPopup.IsOpen) //If the addition popup is open this starts the process to remove the former owners shown and update the list.
                {
                    Trace.WriteLine("Resetting new general file with new variables added.");

                    UpdateGeneralCurrentOwnerArray();

                    foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch2.Children)
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
                foreach (IReset businessPreviews in QuickSearch1.Children)
                {
                    businessPreviews.ResetState();

                }

                if (BusinessAdditionPopup.IsOpen) //If the addition popup is open this starts the process to remove the former owners shown and update the list.
                {
                    Trace.WriteLine("Resetting new general file with new variables added.");

                    UpdateGeneralCurrentBusinessArray();

                    foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch1.Children)
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
                foreach (IReset clientPreviews in QuickSearch3.Children)
                {
                    clientPreviews.ResetState();

                }

                if (ClientAdditionPopup.IsOpen) //If the addition popup is open this starts the process to remove the former owners shown and update the list.
                {
                    Trace.WriteLine("Resetting new general file with new variables added.");

                    UpdateGeneralCurrentClientArray();

                    foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch3.Children)
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



















        public void PopulateFullPreview(object sender)
        {
            throw new NotImplementedException("May add this in future date for mouse over preview");
        }

        public void RemovePopulateEntryArray(int type, int uniqueId)
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

                foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch2.Children)
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

                foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch1.Children)
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

                foreach (UserControlDataEntryFilesList dataEntryFiles in QuickSearch3.Children)
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
