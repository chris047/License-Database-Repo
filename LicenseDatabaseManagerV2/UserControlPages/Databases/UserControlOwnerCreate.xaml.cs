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
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.ViewModel;

namespace LicenseDatabaseManagerV2.UserControlPages
{
    /// <summary>
    /// Interaction logic for UserControlOwnerCreate.xaml
    /// </summary>
    public partial class UserControlOwnerCreate : UserControl, IReset
    {
        public string ActiveFileType;
        public UserControlOwnerCreate()
        {
            InitializeComponent();
            ActiveFileType = "owner";
            string fileType = "owner";
            UniqueIdGrid.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("UniqueId:", fileType)));
            Section1.Visibility = Visibility.Collapsed; //initial reset of sections
            Section2.Visibility = Visibility.Collapsed;
            Section3.Visibility = Visibility.Collapsed;
            Section4.Visibility = Visibility.Collapsed;

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
            
        }

        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private readonly OwnerFile _ownerFile = new();

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }

        private static void CreateNewOwner(OwnerFile ownerFile)
        {
            Sql.CreateOwnerFile(ownerFile);
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {

            Trace.WriteLine(ActiveFileType);
            switch (ActiveFileType.ToLower())
            {
                case "owner":
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
                    }


                    foreach (UserControlDataEntrySubItem target in Part2.Children)
                    {

                        if (target.DataInformation.Mandatory && (target.DataInput.Text == target.DataInformation.DataEntryText || target.DataInput.Text == ""))
                        {
                            MessageBox.Show(target.DataInformation.InputCategoryName + " is required.");
                            // return statement stops execution
                            return;
                        }
                        // if not blank run switch
                        else if (target.DataInput.Text != "")
                        {

                            switch (target.DataInformation.InputCategoryName)
                            {
                                case "Phone:":
                                    /* This part makes me angry, we can change it back if my fix is more stupid.
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
                                        _ownerFile.phone_number = filteredPhoneString.Substring(3, filteredPhoneString.Length - 3);
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
                    CreateNewOwner(_ownerFile);
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
