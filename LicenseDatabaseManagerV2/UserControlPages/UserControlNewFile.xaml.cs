using System;
using System.Collections.Generic;
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
using LicenseDatabaseManagerV2.Interfaces;
using LicenseDatabaseManagerV2.ViewModel;

namespace LicenseDatabaseManagerV2.UserControlPages
{
    /// <summary>
    /// Interaction logic for UserControlNewFile.xaml
    /// </summary>
    public partial class UserControlNewFile : UserControl, IReset
    {
        public UserControlNewFileSubItem NewGeneral = new UserControlNewFileSubItem("General");
        public UserControlNewFileSubItem NewOwner = new UserControlNewFileSubItem("Owner");
        public UserControlNewFileSubItem NewLicense = new UserControlNewFileSubItem("License");
        public UserControlNewFileSubItem NewClient = new UserControlNewFileSubItem("Client");
        public UserControlNewFile()
        {
            InitializeComponent();
            NewFileOptions.Visibility = Visibility.Visible;
            NewFilePopulation.Children.Clear();



        }




        private void OpenDataEntry(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("The new button has been clicked.");
            string? fileType = (sender as Button).Name.ToString();
            if (fileType.ToLower() == "general")
            {
                NewFilePopulation.Children.Add(NewGeneral);
            }
            else if (fileType.ToLower() == "owner")
            {
                NewFilePopulation.Children.Add(NewOwner);
            }
            else if (fileType.ToLower() == "license")
            {
                NewFilePopulation.Children.Add(NewLicense);
            }
            else if (fileType.ToLower() == "client")
            {
                NewFilePopulation.Children.Add(NewClient);
            }
            
            NewFileOptions.Visibility = Visibility.Collapsed;


        }
        public void ResetState()
        {
            NewFileOptions.Visibility = Visibility.Visible;
            NewGeneral.ResetState();
            NewOwner.ResetState();
            NewLicense.ResetState();
            NewClient.ResetState();
            NewFilePopulation.Children.Clear();
            Trace.WriteLine("Resetted state of New File");
        }
    }
}
