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
using LicenseDatabaseManagerV2.ViewModel;

namespace LicenseDatabaseManagerV2.UserControlPages
{
    /// <summary>
    /// Interaction logic for UserControlBusinessCreate.xaml
    /// </summary>
    public partial class UserControlBusinessCreate : UserControl, IReset
    {
        public string ActiveFileType;
        public UserControlBusinessCreate()
        {
            InitializeComponent();
            ActiveFileType = "business";
            string fileType = "business";
            UniqueIdGrid.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("UniqueId:", fileType)));
            Section1.Visibility = Visibility.Collapsed; //initial reset of sections
            Section2.Visibility = Visibility.Collapsed;
            Section3.Visibility = Visibility.Collapsed;
            Section4.Visibility = Visibility.Collapsed;


            Section1.Visibility = Visibility.Visible;
            SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Business Info")));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "Joe's Crab Shack", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County Region:", "South/North", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License Number:", "L2634", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Establishment:", "Bar, Store, etc", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Entity:", "0000", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Active:", "true/false", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Date:", "2022-12-25", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Worked Date:", "2022-12-25", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Completed Date:", "2022-12-25", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Memo:", "Sample memo text here", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Codes:", "A,T,O", false, true)));

        }
        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private readonly BusinessFile _businessFile = new();

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }

        private static void CreateBusinessFile(BusinessFile businessFile)
        {
            Sql.CreateBusinessFile(businessFile);
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            switch (ActiveFileType.ToLower())
            {
                case "business":
                    foreach (UserControlDataEntrySubItem target in Part1.Children)
                    {
                        if (target.DataInformation.Mandatory && (target.DataInput.Text == ""))
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
                                        _businessFile.phone_number = filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
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
                                        _businessFile.business_activity_codes.Add(new BusinessActivityCode() { activity_code_name = code });
                                    }
                                    break;
                                default:
                                    Trace.WriteLine("Error: 100. Invalid category name detected.");
                                    break;
                            }
                        }
                    }
                    CreateBusinessFile(_businessFile);
                    MessageBox.Show($"{_businessFile.dba} successfully added to the database.");
                    break;

                    // Check file type being created and do below accordingly
                    // if file type is owner create owner file etc.


                    //ResetState();
            }
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
