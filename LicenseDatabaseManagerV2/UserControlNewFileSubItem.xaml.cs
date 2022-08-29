using System;
using System.Collections.Generic;
using System.Configuration; // imported
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
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
using DataAccessLibrary.Models.OwnerData;
using DataAccessLibrary.Models.SharedData;
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.ViewModel;
using LicenseDatabaseManagerV2.UserControlPages;
using OwnerFile = DataAccessLibrary.Models.OwnerFile;

namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlNewFileSubItem.xaml
    /// </summary>
    public partial class UserControlNewFileSubItem : UserControl, IReset
    {
        public string ActiveFileType;
        public UserControlNewFileSubItem(string fileType)
        {
            InitializeComponent();
            ActiveFileType = fileType;

            UniqueIdGrid.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("UniqueId:", fileType)));
            Section1.Visibility = Visibility.Collapsed; //initial reset of sections
            Section2.Visibility = Visibility.Collapsed;
            Section3.Visibility = Visibility.Collapsed;
            Section4.Visibility = Visibility.Collapsed;
            switch (fileType.ToLower())
            {
                case "general":
                    Section1.Visibility = Visibility.Visible;
                    SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Owner Address Info")));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "First Name", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Last Name", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "#### Street Name", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "", false, false)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "City Name", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "City Code", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "County Name", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "#####", false, true)));

                    Section2.Visibility = Visibility.Visible;
                    SectionTitle2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Owner Contact/Position Info")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Position:", "Executive")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Stock:", "10%")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("From Date:", "2004-03-15")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("To Date:", "2024-03-20")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Blind:", "This can be anything")));

                    Section3.Visibility = Visibility.Visible;
                    SectionTitle3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Business Info")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "Example Business Name",true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Establishment:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Entity:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Active:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Codes:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Date:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Worked Date:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Completed Date:", "LICENSE NUMBER")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Memo:", "LICENSE NUMBER")));

                    Section4.Visibility = Visibility.Visible;
                    SectionTitle4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));
                    Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Corporation:", "0000")));
                    Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "0000")));
                    Part4.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "0000")));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                    Part3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                    break;
                case "owner":
                    Section1.Visibility = Visibility.Visible;
                    SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Owner Info")));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "George", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Washington", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));

                    Section2.Visibility = Visibility.Visible;
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Position:", "Executive")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Stock:", "10%")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("From Date:", "2022-03-20")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("To Date:", "2024-03-20")));
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Blind:", "This can be anything")));
                    break;
                case "license":
                    Section1.Visibility = Visibility.Visible;
                    SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "License Info")));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Name:", "0000", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "0000", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Street Name:", "0000", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "0000", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "0000")));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License:", "0000", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Owner1:", "0000", true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Owner2:", "0000", true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Owner3:", "0000", true)));
                    break;
                case "client":
                    Section1.Visibility = Visibility.Visible;
                    SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "Thomas", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Jefferson", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 11", false, false)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                    Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));

                    Section2.Visibility = Visibility.Visible;
                    Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                    break;
                default:
                    throw new Exception("invalid new file type given.");
            }



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

        private static void CreateNewBusiness(BusinessFile businessFile)
        {
            Sql.CreateBusinessFile(businessFile);
        }

        private static void CreateNewClient(ClientFile clientFile)
        {
            Sql.CreateClientFile(clientFile);
        }

        private static void CreateNewOwner(OwnerFile ownerFile)
        {
            Sql.CreateOwnerFile(ownerFile);
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
           
            Trace.WriteLine(ActiveFileType + "ACTIVE TEST");
            switch (ActiveFileType.ToLower())
            {
                case "client": // TODO: Make sure all variables that can be saved are saved. 4/8/22
                    foreach (UserControlDataEntrySubItem target in Part1.Children)
                    {
                        // if input text is default or blank return an error and stop execution
                        if (target.DataInformation.Mandatory && ((target.DataInput.Text == target.DataInformation.DataEntryText && target.DataInformation.HasBeenClicked == false) || target.DataInput.Text == ""))
                        {
                            MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                            // return statement stops execution
                            return;
                        }
                        // else run this switch
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
                            default:
                                Trace.WriteLine("Error: 100. Invalid category name detected.");
                                break;
                        }
                    }


                    foreach (UserControlDataEntrySubItem target in Part2.Children)
                    {

                        if (target.DataInformation.Mandatory && (target.DataInput.Text == target.DataInformation.DataEntryText || target.DataInput.Text == ""))
                        {
                            MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                            // return statement stops execution
                            return;
                        }

                        switch (target.DataInformation.InputCategoryName)
                        {
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
                        }
                    }
                    CreateNewClient(_clientFile);
                    break;
                case "owner":
                    foreach (UserControlDataEntrySubItem target in Part1.Children)
                    {
                        // if input text is default or blank return an error and stop execution
                        if (target.DataInformation.Mandatory && (target.DataInput.Text == target.DataInformation.DataEntryText || target.DataInput.Text == ""))
                        {
                            MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                            // return statement stops execution
                            return;
                        }
                        // else run this switch
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
                                default:
                                    Trace.WriteLine("Error: 100. Invalid category name detected.");
                                    break;
                            }
                    }


                    foreach (UserControlDataEntrySubItem target in Part2.Children)
                    {

                        if (target.DataInformation.Mandatory && (target.DataInput.Text == target.DataInformation.DataEntryText || target.DataInput.Text == ""))
                        {
                            MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                            // return statement stops execution
                            return;
                        }

                        switch (target.DataInformation.InputCategoryName)
                            {
                                case "Phone:":
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
                    CreateNewOwner(_ownerFile);
                    break;

                case "general":
                    foreach (UserControlDataEntrySubItem target in Part1.Children)
                    {
                        switch (target.DataInformation.InputCategoryName)
                        {
                            case "First Name:":
                                _businessFile.dba = target.DataInput.Text;
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
                            default:
                                Trace.WriteLine("Error: 100. Invalid category name detected.");
                                break;
                        }
                        //Trace.WriteLine(target.DataInformation.InputCategoryName + " is the name of the variable");
                        //Trace.WriteLine("Text present in search box: " + target.DataInput.Text);
                    }


                    foreach (UserControlDataEntrySubItem target in Part2.Children)
                    {
                        if (target.DataInformation.HasBeenClicked)
                        {
                            switch (target.DataInformation.InputCategoryName)
                            {
                                case "Phone:":
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
                        //Trace.WriteLine(target.DataInformation.InputCategoryName + " is the name of the variable");
                        //Trace.WriteLine("Text present in search box: " + target.DataInput.Text);
                    }

                    foreach (UserControlDataEntrySubItem target in Part3.Children)
                    {
                        Trace.WriteLine(target.DataInformation.InputCategoryName + " is the name of the variable");
                        Trace.WriteLine("Text present in search box: " + target.DataInput.Text);
                    }

                    foreach (UserControlDataEntrySubItem target in Part4.Children)
                    {
                        Trace.WriteLine(target.DataInformation.InputCategoryName + " is the name of the variable");
                        Trace.WriteLine("Text present in search box: " + target.DataInput.Text);
                    }

                    break;

                    // Check file type being created and do below accordingly
                    // if file type is owner create owner file etc.
                    

                //ResetState();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ResetState()
        {
            foreach (IReset searchParameter in Part1.Children)  //resetting the data entry input starting values
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Part2.Children)  
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Part3.Children)  
            {
                searchParameter.ResetState();

            }
            foreach (IReset searchParameter in Part4.Children)  
            {
                searchParameter.ResetState();

            }
        }
    }
}
