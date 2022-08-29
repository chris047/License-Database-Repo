using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
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
using LicenseDatabaseManagerV2.UserControlPages;


namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlDataEntryResultsSubPreview.xaml
    /// </summary>
    public partial class UserControlDataEntryResultsSubItem : UserControl
    {


        private IUserControlCreate _mainContext;
        private IUserControllerScanner _context;
        private int _TypeOfSub;
       
        /*

        0: General File
        1: Owner File
        2: License File
        3: Client File


        */
        private static readonly MySqlCrudActions Sql = new(GetConnectionString());
        private readonly BusinessFile _businessFile = new();
        private readonly ClientFile _clientFile = new();
        private readonly OwnerFile _ownerFile = new();
        private readonly GeneralFile _generalFile = new();
        

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }
/*
        public UserControlDataEntryResultsSubItem(int uniqueId, IUserControllerScanner context, int typeOfSub, IUserControlCreate mainContext)
        {
            InitializeComponent(); //The 6 items below can be changed to any variable that one thinks is important.
            _mainContext = mainContext;
            _context = context;
            _TypeOfSub = typeOfSub;
           
            UniqueIdVariable.Content = uniqueId;


            if (_TypeOfSub == 0)
            {
                Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("DBA:", "Mr Buffalo's Liquids")));
                Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address:", "1234 Errent Way")));
                Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", "Thousand Oaks")));
                Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("License:", "232-34322")));
                Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", "8187141007")));
                Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Owner:", "Mr.Buffalo Jr.")));
            }
            else if (_TypeOfSub == 1)
            {
                if (uniqueId != -1)
                {
                    _ownerFile = Sql.GetOwnerFileById(uniqueId);
                    Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Name:", $"{_ownerFile.first_name} {_ownerFile.last_name}")));
                    Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address1:", $"{_ownerFile.address_line1_number} {_ownerFile.address_line1_street_name}")));
                    Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2:", $"{_ownerFile.address_line2}")));
                    Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", $"{_ownerFile.city_name}")));
                    Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("State:", $"{_ownerFile.state_code}")));
                    Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", $"({_ownerFile.area_code}) {_ownerFile.phone_number}")));
                }
                else
                {
                    Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Name:", "Demo")));
                    Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address1:", "Demo")));
                    Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2:", "Demo")));
                    Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", "Demo")));
                    Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("State:", "Demo")));
                    Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", "Demo")));
                }


            }
            else if (_TypeOfSub == 2)
            {
                _businessFile = Sql.GetBusinessFileById(uniqueId);
                Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("DBA:", _businessFile.dba)));
                Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("License:", _businessFile.license_number)));
                Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address:", $"{_businessFile.address_line1_number} {_businessFile.address_line1_street_name}")));
                Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2", $"{_businessFile.address_line2}")));
                Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", _businessFile.city_name)));
                Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", $"{_businessFile.area_code} {_businessFile.phone_number}")));
            }
            else if (_TypeOfSub == 3)
            {
                _clientFile = Sql.GetClientFileById(uniqueId);
                Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Name:", $"{_clientFile.business_name} {_clientFile.first_name} {_clientFile.last_name}")));
                Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address1:", $"{_clientFile.address_line1_number} {_clientFile.address_line1_street_name}")));
                Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2:", $"{_clientFile.address_line2}")));
                Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", $"{_clientFile.city_name}")));
                Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("State:", $"{_clientFile.state_code}")));
                Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", $"({_clientFile.area_code}) {_clientFile.phone_number}")));
            }
            else
            {
                throw new Exception("No valid type of sub given");
            }
            
        }
*/
        public UserControlDataEntryResultsSubItem(int uniqueId, int typeOfSub, IUserControlCreate mainContext) //When just displaying information
        {
            InitializeComponent(); //The 6 items below can be changed to any variable that one thinks is important.
            _TypeOfSub = typeOfSub;
            _mainContext = mainContext;
            UniqueIdVariable.Content = uniqueId;
            WholeItem.MouseEnter -= ChangeColorToSelected;
            WholeItem.MouseLeave -= ChangeColorToLeave;
            WholeItem.MouseLeftButtonUp -= ViewPreview;
            WholeItem.Cursor = null;

            if (_TypeOfSub == 0)
            {
                // This may not work; Using business file for now since it's the base for general file
                _businessFile = Sql.GetBusinessFileById(uniqueId);
                Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("DBA:", _businessFile.dba)));
                Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("License:", _businessFile.license_number)));
                Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address:", $"{_businessFile.address_line1_number} {_businessFile.address_line1_street_name}")));
                Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2", $"{_businessFile.address_line2}")));
                Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", _businessFile.city_name)));
                Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", $"{_businessFile.area_code} {_businessFile.phone_number}")));
            }
            else if (_TypeOfSub == 1)
            {
                _ownerFile = Sql.GetOwnerFileById(uniqueId);
                Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Name:", $"{_ownerFile.first_name} {_ownerFile.last_name}")));
                Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address1:", $"{_ownerFile.address_line1_number} {_ownerFile.address_line1_street_name}")));
                Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2:", $"{_ownerFile.address_line2}")));
                Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", $"{_ownerFile.city_name}")));
                Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("State:", $"{_ownerFile.state_code}")));
                Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", $"({_ownerFile.area_code}) {_ownerFile.phone_number}")));

            }
            else if (_TypeOfSub == 2)
            {
                _businessFile = Sql.GetBusinessFileById(uniqueId);
                Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("DBA:", _businessFile.dba)));
                Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("License:", _businessFile.license_number)));
                Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address:", $"{_businessFile.address_line1_number} {_businessFile.address_line1_street_name}")));
                Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2", $"{_businessFile.address_line2}")));
                Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", _businessFile.city_name)));
                Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", $"{_businessFile.area_code} {_businessFile.phone_number}")));
            }
            else if (_TypeOfSub == 3)
            {
                _clientFile = Sql.GetClientFileById(uniqueId);
                Spot1.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Name:", $"{_clientFile.business_name} {_clientFile.first_name} {_clientFile.last_name}")));
                Spot2.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address1:", $"{_clientFile.address_line1_number} {_clientFile.address_line1_street_name}")));
                Spot3.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Address2:", $"{_clientFile.address_line2}")));
                Spot4.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("City:", $"{_clientFile.city_name}")));
                Spot5.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("State:", $"{_clientFile.state_code}")));
                Spot6.Children.Add(new UserControlPreviewLabelsSubItem(new InfoSubItemPreview("Phone:", $"({_clientFile.area_code}) {_clientFile.phone_number}")));
            }
            else
            {
                throw new Exception("No valid type of sub given");
            }

        }


        private void ViewPreview(object sender, MouseButtonEventArgs e)
        {
            //Trace.WriteLine("Button has been clicked to show preview.");
            _context.PopulateFullPreview(UniqueIdVariable.Content);

        }

        private void ChangeColorToSelected(object sender, MouseEventArgs e)
        {
            WholeItem.Background = Brushes.LightGray;
        }

        private void ChangeColorToLeave(object sender, MouseEventArgs e)
        {
            WholeItem.Background = null;
        }

        private void RemoveEntry_OnClick(object sender, RoutedEventArgs e)
        {
            _mainContext.RemovePopulateEntryArray(_TypeOfSub, (int)UniqueIdVariable.Content);
        }
    }
}
