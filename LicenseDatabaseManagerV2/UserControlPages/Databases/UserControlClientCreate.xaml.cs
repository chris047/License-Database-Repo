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
    /// Interaction logic for UserControlClientCreate.xaml
    /// </summary>
    public partial class UserControlClientCreate : UserControl, IReset
    {
        public string ActiveFileType;
        public UserControlClientCreate()
        {
            InitializeComponent();
            ActiveFileType = "client";
            string fileType = "client";
            UniqueIdGrid.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("UniqueId:", fileType)));
            Section1.Visibility = Visibility.Collapsed; //initial reset of sections
            Section2.Visibility = Visibility.Collapsed;
            Section3.Visibility = Visibility.Collapsed;
            Section4.Visibility = Visibility.Collapsed;
            Section1.Visibility = Visibility.Visible;
            SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));
            // TODO: Create way to allow for either corp or first+last name
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Corporation:", "Corporation Name", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "Thomas", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Jefferson", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 11", false, false)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
            Part1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));

            Section2.Visibility = Visibility.Visible;
            Part2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007", false, true)));

        }

        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private readonly ClientFile _clientFile = new();

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }
        private static void CreateNewClient(ClientFile clientFile)
        {
            Sql.CreateClientFile(clientFile);
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {

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
                        // If not blank run switch
                        else if (target.DataInput.Text != "")
                        {
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
                    }
                    CreateNewClient(_clientFile);
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
