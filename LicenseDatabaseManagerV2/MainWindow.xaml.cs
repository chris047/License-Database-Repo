using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using MaterialDesignThemes.Wpf;
using System.Windows.Automation;
using LicenseDatabaseManagerV2.ViewModel;
using System.Diagnostics;
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.UserControlPages;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using DataAccessLibrary.Models.BusinessData;
using DataAccessLibrary.Models.OwnerData;
using DataAccessLibrary.Models.SharedData;
using LicenseDatabaseManagerV2.UserControlPages.Printing;
using LicenseDatabaseManagerV2.UserControlPages.Settings;

//using Org.BouncyCastle.Math.EC;

namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IReset OpenPage;
        public MainWindow()
        {
            InitializeComponent();

            // Probably not going to use dashboard 
            //var item0 = new ItemMenu("Dashboard", new UserControl(), PackIconKind.ViewDashboard);

            var menuScanners = new List<SubItem>();
            /* THIS HAS BEEN TEMPORARILY REMOVED
            menuScanners.Add(new SubItem("Create New File", new UserControlNewFile()));
            menuScanners.Add(new SubItem("General File Search", new UserControlGeneralSearch()));
            menuScanners.Add(new SubItem("Owner Search", new UserControlOwnerSearch()));
            menuScanners.Add(new SubItem("License Search", new UserControlBusinessSearch()));
            menuScanners.Add(new SubItem("Client Search", new UserControlClientSearch()));
            */
            //var testvar = new SubItem("Create New File", new UserControlNewFile());
            //menuScanners.Add(testvar);
            menuScanners.Add(new SubItem("General File Search", new UserControlGeneralSearch()));
            menuScanners.Add(new SubItem("Business Search", new UserControlBusinessSearch()));
            menuScanners.Add(new SubItem("Owner Search", new UserControlOwnerSearch()));
            menuScanners.Add(new SubItem("Client Search", new UserControlClientSearch()));


            var item1 = new ItemMenu("Scanners", menuScanners, PackIconKind.Search);

            
            var menuDatabases = new List<SubItem>();
            menuDatabases.Add(new SubItem("Create General File", new UserControlGeneralCreate()));
            menuDatabases.Add(new SubItem("Create Business File", new UserControlBusinessCreate()));
            menuDatabases.Add(new SubItem("Create Owner File", new UserControlOwnerCreate()));
            menuDatabases.Add(new SubItem("Create Client File", new UserControlClientCreate()));
            //menuDatabases.Add(new SubItem("Past Affiliations1"));
            var item2 = new ItemMenu("Databases", menuDatabases, PackIconKind.Database);


            var menuPrinting = new List<SubItem>();
            menuPrinting.Add(new SubItem("Printing", new UserControlPrintingMain()));
           
            var item3 = new ItemMenu("Printing", menuPrinting, PackIconKind.Printer);


            var menuSettings = new List<SubItem>();
            menuSettings.Add(new SubItem("Settings", new UserControlSettings()));

            var item4 = new ItemMenu("Settings", menuSettings, PackIconKind.CogOutline);




            /*
            var menuPrintouts = new List<SubItem>();
            menuPrintouts.Add(new SubItem("Names and Addresses1"));
            menuPrintouts.Add(new SubItem("Past Affiliations1"));
            var item3 = new ItemMenu("Printouts", menuPrintouts, PackIconKind.Printer);

            var menuAccountsReceivable = new List<SubItem>();
            menuAccountsReceivable.Add(new SubItem("Names and Addresses1"));
            menuAccountsReceivable.Add(new SubItem("Past Affiliations1"));
            var item4 = new ItemMenu("Accounts Receivable", menuAccountsReceivable, PackIconKind.Money);

            var menuSettings = new List<SubItem>();
            menuSettings.Add(new SubItem("Names and Addresses"));
            menuSettings.Add(new SubItem("Past Affiliations"));
            var item5 = new ItemMenu("Settings", menuSettings, PackIconKind.Settings);
            */

            Menu.Children.Add(new UserControlMenuItem(item1,this));
            Menu.Children.Add(new UserControlMenuItem(item2,this));
            Menu.Children.Add(new UserControlMenuItem(item3, this));
            Menu.Children.Add(new UserControlMenuItem(item4, this));
            //Menu.Children.Add(new UserControlMenuItem(item3,this));
            //Menu.Children.Add(new UserControlMenuItem(item4,this));
            //Menu.Children.Add(new UserControlMenuItem(item5,this));

            // MySQL Testing under this line------------------------------------------



            MySqlCrudActions sql = new MySqlCrudActions(GetConnectionString());

            //CreateNewBusiness(sql);
            //CreateNewOwner(sql);
            //ReadAllOwners(sql);
            //ReadOwnerFile( sql, 1 );
            //SearchForOwner(sql);

        }

        private static string GetConnectionString(string connectionStringName = "LicenseDatabaseManagerDatabase")
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            return connectionString;
        }

        //private static void ReadAllOwners(MySqlCrudActions sql)
        //{
        //    var rows = sql.GetAllOwners();

        //    foreach (var row in rows)
        //    {
        //        Trace.WriteLine($"{ row.idowner }: { row.first_name } { row.last_name } { row.socsec }");
        //    }
        //}

        //private static void ReadOwnerFile(MySqlCrudActions sql, int ownerId)
        //{
        //    var owner = sql.GetOwnerFileById(ownerId);

        //    Trace.WriteLine(
        //        $"Owner ID: {owner.idowner}\n{owner.first_name} {owner.last_name} {owner.socsec}\n" +
        //        $"{owner.address_line1_number} {owner.address_line1_street_name} {owner.address_line2}\n" +
        //        $"{owner.city_code} {owner.city_name} " +
        //        $"{owner.county_name} {owner.county_region} " +
        //        $"{owner.state_code} \n" +
        //        $"{owner.zip}\n" +
        //        $"({ owner.area_code }) {owner.phone_number}\n" +
        //        $"{ owner.title }\n" +
        //        $"{ owner.stock } { owner.from_date } { owner.to_date }\n" +
        //        $"{ owner.blind_text }");
        //}

        //private static void CreateNewOwner(MySqlCrudActions sql)
        //{


        //    OwnerFile owner = new OwnerFile()
        //    {
        //            first_name = "Roger",
        //            last_name = "Cibrian",
        //            socsec = "123-45-6788",
        //            address_line1_number = "8128",
        //            address_line1_street_name = "Darby Ave.",
        //            city_code = "NR",
        //            city_name = "Northridge",
        //            county_name = "Los Angeles",
        //            zip = "91335",
        //            state_code = "CA",
        //            area_code = "818",
        //            phone_number = "324-4666",
        //            title = "Executive",
        //            stock = "25%",
        //            from_date = "2004-03-15",
        //            blind_text = "A test of text."
        //    };

        //    sql.CreateOwnerFile(owner);
        //}

        //private static void CreateNewBusiness(MySqlCrudActions sql)
        //{
        //    BusinessFile business = new BusinessFile()
        //    {
        //        dba = "An essential business1",
        //        //business_formerlies = new List<BusinessFormerly>()
        //        //{
        //        //    new()
        //        //    {
        //        //        idbusiness_formerly = 0,
        //        //        business_idbusiness = 0,
        //        //        business_idbusiness_old = 0,
        //        //        date_changed = null
        //        //    },

        //        //    new()
        //        //    {
        //        //        idbusiness_formerly = 0,
        //        //        business_idbusiness = 0,
        //        //        business_idbusiness_old = 0,
        //        //        date_changed = null
        //        //    },
        //        //},

        //        address_line1_number = "1234",
        //        address_line1_street_name = "Balboa Blvd",
        //        //address_line2 = "",

        //        city_name = "Reseda",
        //        city_code = "RE",

        //        county_name = "Los Angeles",
        //        //county_region = "South",

        //        state_code = "CA",

        //        zip = "91335",

        //        area_code = "818",
        //        phone_number = "123-4567",

        //        license_number = "1",
        //        establishment = "establishment name",
        //        entity = "entity name",
        //        active = true,
        //        business_activity_codes = new List<BusinessActivityCode>()
        //        {
        //            new()
        //            {
        //                activity_code_name = "AL"
        //            },
        //            new()
        //            {
        //                activity_code_name = "BW"
        //            }

        //        }, //= new List<BusinessActivityCode>();
        //        activity_date = "2022-02-18",

        //        //user_iduser = "",
        //        worked_date = "2022-04-18",
        //        completed_date = "2022-04-18",
        //        //List<Client> clients = "", //= new List<Client>();
        //        memo_text = "memo sample"
        //    };

        //    sql.CreateBusinessFile(business);
        //}

        // End MySQL Testing -----------------------------------------------------
        public void OpenNewPage(object sender)
        {
            var screen = ((UserControl)sender);
            
           
            if (screen != null)
            {
                
               /* 
                if (OpenPage != null)
                {
                    OpenPage.DataContext = null;
                    OpenPage.BindingGroup = null;
                    GC.Collect();

                }
                OpenPage = new UserControlOwnerSearch();
                */
               
                
                MainPageGrid.Children.Clear();
                
                MainPageGrid.Children.Add(screen);
                if (OpenPage != null)
                {
                    OpenPage.ResetState();
                    OpenPage = null;
                    Trace.WriteLine("PAGE IS NOT NULL MAKING NULL");
                }
                OpenPage = sender as IReset;


            }
            else
            {
                Trace.WriteLine("This shouldn't fire.");
            }
        }
    }
}
