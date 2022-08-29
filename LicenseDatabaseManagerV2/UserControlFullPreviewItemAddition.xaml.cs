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


namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlFullPreviewAddition.xaml
    /// </summary>
    public partial class UserControlFullPreviewItemAddition : UserControl, IReset
    {

        /*
        0: General addition File
        1: Owner addition File
        2: License addition File
        3: Client addition File


        */
        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private OwnerFile _ownerFile = new();
        private ClientFile _clientFile = new();
        private BusinessFile _businessFile = new();


        public int[]? CurrentOwners;
        public int[]? CurrentClients;
        public int[]? CurrentBusinesses;
        public int TypeOfMain;
        private int? _currentUniqueId = null;

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }
        public UserControlFullPreviewItemAddition(int uniqueId, int typeOfMain, int[]? currentArray)
        {
            InitializeComponent();
            UniqueIdGrid.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("UniqueId:", "" + uniqueId)));
            TypeOfMain = typeOfMain;

            //Trace.WriteLine("IN THE FULL PREVIEW ADDITION CURRENT OWNER LENGTH Is: " + listOfCurrentOwners.Length);

            if(TypeOfMain == 0)
            {

            }
            else if(TypeOfMain == 1) //owner
            {
                if (currentArray != null)
                {
                    //_listOfCurrentOwners = (int[]) listOfCurrentOwners.Clone(); 
                    CurrentOwners = currentArray; //can I just reference it and change it like that?
                                                  //Trace.WriteLine("CLOOOOOOOOOOOOOOOOOOOOOOOOONING");
                                                  //Trace.WriteLine("IN THE FULL PREVIEW ADDITION CURRENT OWNER LENGTH Is XXXXXX: " + _listOfCurrentOwners.Length);
                    RemoveItem.IsEnabled = false;
                    AddItem.IsEnabled = true;
                    for (int i = 0; i < CurrentOwners.Length; i++)
                    {
                        Trace.WriteLine("The unique id is:" + uniqueId + " and the current owner" + i + " is: " + CurrentOwners[i]);
                        if (CurrentOwners[i] == uniqueId)
                        {
                            RemoveItem.IsEnabled = true;
                            AddItem.IsEnabled = false;
                        }
                    }
                } //TODO: Make reset state reset the variables changed here
                else
                {
                    AddItem.IsEnabled = true;
                }
            }
            else if (TypeOfMain == 2) //business
            {
                if (currentArray != null)
                {
                    //_listOfCurrentOwners = (int[]) listOfCurrentOwners.Clone(); 
                    CurrentBusinesses = currentArray; //can I just reference it and change it like that?
                                                   //Trace.WriteLine("CLOOOOOOOOOOOOOOOOOOOOOOOOONING");
                                                   //Trace.WriteLine("IN THE FULL PREVIEW ADDITION CURRENT OWNER LENGTH Is XXXXXX: " + _listOfCurrentOwners.Length);
                    RemoveItem.IsEnabled = false;
                    AddItem.IsEnabled = true;
                    for (int i = 0; i < CurrentBusinesses.Length; i++)
                    {
                        Trace.WriteLine("The unique id is:" + uniqueId + " and the current businesses" + i + " is: " + CurrentBusinesses[i]);
                        if (CurrentBusinesses[i] == uniqueId)
                        {
                            RemoveItem.IsEnabled = true;
                            AddItem.IsEnabled = false;
                        }
                    }
                } //TODO: Make reset state reset the variables changed here
                else
                {
                    AddItem.IsEnabled = true;
                }
            }
            else if (TypeOfMain == 3) // client
            {
                if (currentArray != null)
                {
                    //_listOfCurrentOwners = (int[]) listOfCurrentOwners.Clone(); 
                    CurrentClients = currentArray; //can I just reference it and change it like that?
                                                  //Trace.WriteLine("CLOOOOOOOOOOOOOOOOOOOOOOOOONING");
                                                  //Trace.WriteLine("IN THE FULL PREVIEW ADDITION CURRENT OWNER LENGTH Is XXXXXX: " + _listOfCurrentOwners.Length);
                    RemoveItem.IsEnabled = false;
                    AddItem.IsEnabled = true;
                    for (int i = 0; i < CurrentClients.Length; i++)
                    {
                        Trace.WriteLine("The unique id is:" + uniqueId + " and the current clients" + i + " is: " + CurrentClients[i]);
                        if (CurrentClients[i] == uniqueId)
                        {
                            RemoveItem.IsEnabled = true;
                            AddItem.IsEnabled = false;
                        }
                    }
                } //TODO: Make reset state reset the variables changed here
                else
                {
                    AddItem.IsEnabled = true;
                }
            }
            else
            {
                throw new Exception("Out of bounds type of main");
            }
            


            if (typeOfMain == 0)
            {
                SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Owner Info")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "First Name", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Last Name", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "#### Street Name", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "City Name", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "City Code", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "County Name", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "#####", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Position:", "Executive")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Stock:", "10%")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("From Date:", "2004-03-15")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("To Date:", "2024-03-20")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Blind:", "This can be anything")));


                SectionTitle2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Business Info")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "Example Business Name", true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Establishment:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Entity:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Active:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Codes:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Date:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Worked Date:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Completed Date:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Memo:", "LICENSE NUMBER")));


                SectionTitle3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Corporation:", "0000")));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "0000")));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "0000")));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info3.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
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
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Position:", "Executive")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Stock:", "10%")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("From Date:", "2022-03-20")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("To Date:", "2024-03-20")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Blind:", "This can be anything")));

            }
            else if (typeOfMain == 2)
            {
                SectionTitle2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Business Info")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("DBA:", "Example Business Name", true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 10", false, false)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Phone:", "(818) 713-1007")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("License:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Establishment:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Entity:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Active:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Codes:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Activity Date:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Worked Date:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Completed Date:", "LICENSE NUMBER")));
                Info2.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Memo:", "LICENSE NUMBER")));
            }
            else if (typeOfMain == 3)
            {
                SectionTitle1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Section:", "Client Info")));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Corporation:", "Bestbuy", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("First Name:", "Thomas", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Last Name:", "Jefferson", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address:", "21601 Victory Blvd.", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Address 2:", "Unit 11", false, false)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City:", "Canoga Park", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("City Code:", "CP", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("County:", "Los Angeles", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("State:", "CA", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
                Info1.Children.Add(new UserControlDataEntrySubItem(new DataEntrySubItem("Zip Code:", "91304", false, true)));
            }
            else
            {
                throw new Exception("No valid type of main preview given");
            }
        }

        

        public void ChangeToNewId(int uniqueId)
        {

            //SUDO RESET STATE
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
            //END

            Trace.WriteLine("Changing to new ID: " + uniqueId);
            _currentUniqueId = uniqueId; //setting integer value for current uniqueId
            foreach (UserControlPreviewLabelsSubItem target in UniqueIdGrid.Children)
            {
                if (target.Var1Label.Content.ToString() == "UniqueId:")
                {
                    target.Var1Value.Content = uniqueId;
                }

            }

            if (TypeOfMain == 0)
            {

            }
            else if (TypeOfMain == 1) //owner
            {
                _ownerFile = Sql.GetOwnerFileById(uniqueId);
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
                            target.DataInput.Text = $"{_ownerFile.address_line1_number} {_ownerFile.address_line1_street_name}";
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
                    }
                }
                foreach (UserControlDataEntrySubItem target in Info2.Children)
                {
                    target.DataInput.Foreground = Brushes.Black;
                    switch (target.DataName.Content)
                    {
                        //case "Phone:":
                        //    // split area code and number then update
                        //    break;
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

                Trace.WriteLine("Is current owner exist on change to new id " + (CurrentOwners != null));

                if (CurrentOwners != null)
                {
                    //Trace.WriteLine("XXXCurrent owner for full preview is not null in change to new id.");
                    RemoveItem.IsEnabled = false;
                    AddItem.IsEnabled = true;
                    for (int i = 0; i < CurrentOwners.Length; i++)
                    {

                        if (CurrentOwners[i] == uniqueId)
                        {
                            RemoveItem.IsEnabled = true;
                            AddItem.IsEnabled = false;
                            //Trace.WriteLine("FOUND IT ALREADY!");
                        }
                    }
                } //TODO: Make reset state reset the variables changed here
                else
                {
                    //Trace.WriteLine("THIS SHOULDNT FIRE");
                    AddItem.IsEnabled = true;
                }
            }
            else if (TypeOfMain == 2) //business
            {
                _businessFile = Sql.GetBusinessFileById(uniqueId);
                foreach (UserControlDataEntrySubItem target in Info2.Children)
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
                Trace.WriteLine("Is current business exist on change to new id " + (CurrentBusinesses != null));

                if (CurrentBusinesses != null)
                {
                    //Trace.WriteLine("XXXCurrent owner for full preview is not null in change to new id.");
                    RemoveItem.IsEnabled = false;
                    AddItem.IsEnabled = true;
                    for (int i = 0; i < CurrentBusinesses.Length; i++)
                    {

                        if (CurrentBusinesses[i] == uniqueId)
                        {
                            RemoveItem.IsEnabled = true;
                            AddItem.IsEnabled = false;
                            //Trace.WriteLine("FOUND IT ALREADY!");
                        }
                    }
                } //TODO: Make reset state reset the variables changed here
                else
                {
                    //Trace.WriteLine("THIS SHOULDNT FIRE");
                    AddItem.IsEnabled = true;
                }
            }
            else if (TypeOfMain == 3) // client
            {
                _clientFile = Sql.GetClientFileById(uniqueId);
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
                    }
                }
                Trace.WriteLine("Is current client exist on change to new id " + (CurrentClients != null));

                if (CurrentClients != null)
                {
                    //Trace.WriteLine("XXXCurrent owner for full preview is not null in change to new id.");
                    RemoveItem.IsEnabled = false;
                    AddItem.IsEnabled = true;
                    for (int i = 0; i < CurrentClients.Length; i++)
                    {

                        if (CurrentClients[i] == uniqueId)
                        {
                            RemoveItem.IsEnabled = true;
                            AddItem.IsEnabled = false;
                            //Trace.WriteLine("FOUND IT ALREADY!");
                        }
                    }
                } //TODO: Make reset state reset the variables changed here
                else
                {
                    //Trace.WriteLine("THIS SHOULDNT FIRE");
                    AddItem.IsEnabled = true;
                }
            }
            else
            {
                throw new Exception("Out of bounds type of main");
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
            Row1.IsEnabled = false;
            Row2.IsEnabled = false;
            Row3.IsEnabled = false;
            Row4.IsEnabled = false;

            RemoveItem.IsEnabled = false;
            AddItem.IsEnabled = false;

            CurrentOwners = null;
            CurrentBusinesses = null;
            CurrentClients = null;
            //AddItemLabel.Content = "Edit";
            _currentUniqueId = null;
            AddItemIcon.Kind = PackIconKind.AccountPlus;

            //Trace.WriteLine("THE LENGTH OF CURRENT OWNERS IN CHANGE ID IS ON RESET: " + _listOfCurrentOwners.Length);

        }

        private void Delete_OnClickItem_Click(object sender, RoutedEventArgs e)
        {

            if (TypeOfMain == 0)
            {

            }
            else if (TypeOfMain == 1) //owner
            {
                for (int i = 0; i < CurrentOwners.Length; i++)
                {
                    Trace.WriteLine(CurrentOwners[i]);
                }
                List<int> tempList = CurrentOwners.ToList();
                tempList.Remove((int)_currentUniqueId); //will probably never be null to be honest
                CurrentOwners = tempList.ToArray();
                Trace.WriteLine("----------------------");

                for (int i = 0; i < CurrentOwners.Length; i++)
                {
                    Trace.WriteLine(CurrentOwners[i]);
                }
            }
            else if (TypeOfMain == 2) //business
            {
                for (int i = 0; i < CurrentBusinesses.Length; i++)
                {
                    Trace.WriteLine(CurrentBusinesses[i]);
                }
                List<int> tempList = CurrentBusinesses.ToList();
                tempList.Remove((int)_currentUniqueId); //will probably never be null to be honest
                CurrentBusinesses = tempList.ToArray();
                Trace.WriteLine("----------------------");

                for (int i = 0; i < CurrentBusinesses.Length; i++)
                {
                    Trace.WriteLine(CurrentBusinesses[i]);
                }
            }
            else if (TypeOfMain == 3) // client
            {
                for (int i = 0; i < CurrentClients.Length; i++)
                {
                    Trace.WriteLine(CurrentClients[i]);
                }
                List<int> tempList = CurrentClients.ToList();
                tempList.Remove((int)_currentUniqueId); //will probably never be null to be honest
                CurrentClients = tempList.ToArray();
                Trace.WriteLine("----------------------");

                for (int i = 0; i < CurrentClients.Length; i++)
                {
                    Trace.WriteLine(CurrentClients[i]);
                }
            }
            else
            {
                throw new Exception("Out of bounds type of main");
            }





            

            RemoveItem.IsEnabled = false;
            AddItem.IsEnabled = true;
        }

        private void Add_OnClickItem_Click(object sender, RoutedEventArgs e)
        {

            if (TypeOfMain == 0)
            {

            }
            else if (TypeOfMain == 1) //owner
            {
                Trace.WriteLine((CurrentOwners != null) + ": exists");
                if (CurrentOwners != null)
                {
                    for (int i = 0; i < CurrentOwners.Length; i++)
                    {
                        Trace.WriteLine(CurrentOwners[i]);
                    }
                    List<int> tempList = CurrentOwners.ToList();
                    tempList.Add((int)_currentUniqueId); //will probably never be null to be honest
                    CurrentOwners = tempList.ToArray();
                    Trace.WriteLine("----------------------");

                    for (int i = 0; i < CurrentOwners.Length; i++)
                    {
                        Trace.WriteLine(CurrentOwners[i]);
                    }
                }
                else
                {
                    List<int> tempList = new List<int>();
                    tempList.Add((int)_currentUniqueId);
                    CurrentOwners = tempList.ToArray();
                    for (int i = 0; i < CurrentOwners.Length; i++)
                    {
                        Trace.WriteLine("Lets see what the array holds " + CurrentOwners[i]);
                    }
                }
            }
            else if (TypeOfMain == 2) //business
            {
                Trace.WriteLine((CurrentBusinesses != null) + ": exists");
                if (CurrentBusinesses != null)
                {
                    for (int i = 0; i < CurrentBusinesses.Length; i++)
                    {
                        Trace.WriteLine(CurrentBusinesses[i]);
                    }
                    List<int> tempList = CurrentBusinesses.ToList();
                    tempList.Add((int)_currentUniqueId); //will probably never be null to be honest
                    CurrentBusinesses = tempList.ToArray();
                    Trace.WriteLine("----------------------");

                    for (int i = 0; i < CurrentBusinesses.Length; i++)
                    {
                        Trace.WriteLine(CurrentBusinesses[i]);
                    }
                }
                else
                {
                    List<int> tempList = new List<int>();
                    tempList.Add((int)_currentUniqueId);
                    CurrentBusinesses = tempList.ToArray();
                    for (int i = 0; i < CurrentBusinesses.Length; i++)
                    {
                        Trace.WriteLine("Lets see what the array holds " + CurrentBusinesses[i]);
                    }
                }
            }
            else if (TypeOfMain == 3) // client
            {
                Trace.WriteLine((CurrentClients != null) + ": exists");
                if (CurrentClients != null)
                {
                    for (int i = 0; i < CurrentClients.Length; i++)
                    {
                        Trace.WriteLine(CurrentClients[i]);
                    }
                    List<int> tempList = CurrentClients.ToList();
                    tempList.Add((int)_currentUniqueId); //will probably never be null to be honest
                    CurrentClients = tempList.ToArray();
                    Trace.WriteLine("----------------------");

                    for (int i = 0; i < CurrentClients.Length; i++)
                    {
                        Trace.WriteLine(CurrentClients[i]);
                    }
                }
                else
                {
                    List<int> tempList = new List<int>();
                    tempList.Add((int)_currentUniqueId);
                    CurrentClients = tempList.ToArray();
                    for (int i = 0; i < CurrentClients.Length; i++)
                    {
                        Trace.WriteLine("Lets see what the array holds " + CurrentClients[i]);
                    }
                }
            }
            else
            {
                throw new Exception("Out of bounds type of main");
            }


            
            RemoveItem.IsEnabled = true;
            AddItem.IsEnabled = false;
        }
    }
}
