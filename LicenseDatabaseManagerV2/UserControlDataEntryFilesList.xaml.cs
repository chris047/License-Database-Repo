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

namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlDataEntryFilesList.xaml
    /// </summary>


    /*
    0: General File
    1: Owner File
    2: License File
    3: Client File

    */


    public partial class UserControlDataEntryFilesList : UserControl, IReset
    {
        public int[]? _uniqueIdArray = Array.Empty<int>();
        public int TypeOfFile;
        private IUserControlAddition _context;
        private IUserControlCreate _mainContext;
        private bool isEmpty = true; //Tells usercontrol if they even need to make initial subscanners

        public UserControlDataEntryFilesList(int typeOfMain, IUserControlAddition context, IUserControlCreate mainContext)
        {
            InitializeComponent();
            TypeOfFile = typeOfMain;
            _context = context;
            _mainContext = mainContext;
            PopulateState(_uniqueIdArray);
            if(typeOfMain == 0)
            {
                AddButtonLabel.Content = "Add General File";
            }
            else if (typeOfMain == 1)
            {
                AddButtonLabel.Content = "Add Owner File";
            }
            else if (typeOfMain == 2)
            {
                AddButtonLabel.Content = "Add Business File";
            }
            else if(typeOfMain == 3)
            {
                AddButtonLabel.Content = "Add Client File";
            }
            else
            {
                throw new Exception("Unknown typeOfMain!");
            }
        }
        public UserControlDataEntryFilesList(int typeOfMain,int[]? uniqueIdArray, IUserControlAddition context, IUserControlCreate mainContext)
        {
            InitializeComponent();
            TypeOfFile = typeOfMain;
            _context = context;
            _mainContext = mainContext;
            _uniqueIdArray = (int[])uniqueIdArray.Clone();
            isEmpty = false;
            PopulateState(_uniqueIdArray);
            if (typeOfMain == 0)
            {
                AddButtonLabel.Content = "Add General File";
            }
            else if (typeOfMain == 1)
            {
                AddButtonLabel.Content = "Add Owner File";
            }
            else if (typeOfMain == 2)
            {
                AddButtonLabel.Content = "Add Business File";
            }
            else if (typeOfMain == 3)
            {
                AddButtonLabel.Content = "Add Client File";
            }
            else
            {
                throw new Exception("Unknown typeOfMain!");
            }
            
        }

        public void PopulateState(int[]? dataArray)
        {
            _uniqueIdArray = (int[]) dataArray.Clone();
            Trace.WriteLine(TypeOfFile + " IS THE TYPE OF FILE");
            if (!isEmpty)
            {
                if (TypeOfFile == 0)
                {

                }
                else if (TypeOfFile == 1) //owner
                {
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        Trace.WriteLine(dataArray.Length);
                        UniformFileList.Children.Add(new UserControlDataEntryResultsSubItem(dataArray[i], 1,_mainContext));
                    }
                }
                else if (TypeOfFile == 2) //business
                {
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        Trace.WriteLine(dataArray.Length);
                        UniformFileList.Children.Add(new UserControlDataEntryResultsSubItem(dataArray[i], 2, _mainContext));
                    }
                }
                else if (TypeOfFile == 3) //client
                {
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        Trace.WriteLine(dataArray.Length);
                        UniformFileList.Children.Add(new UserControlDataEntryResultsSubItem(dataArray[i], 3, _mainContext));
                    }
                }
                else
                {
                   
                }
            }
            else
            {
                //UniformFileList.Children.Add(new UserControlScannerResultsSubItem(-1, 1));
            }
        }
        public void RepopulateState(int[]? dataArray)
        {
            this.ResetState();
            _uniqueIdArray = (int[])dataArray?.Clone();
            Trace.WriteLine(TypeOfFile + " IS THE TYPE OF FILE repop");
            if (dataArray != null) 
            {
                if (TypeOfFile == 0)
                {

                }
                else if (TypeOfFile == 1)//owner
                {
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        Trace.WriteLine(dataArray.Length);
                        UniformFileList.Children.Add(new UserControlDataEntryResultsSubItem(dataArray[i], 1,_mainContext));
                    }
                }
                else if (TypeOfFile == 2) //business
                {
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        Trace.WriteLine(dataArray.Length);
                        UniformFileList.Children.Add(new UserControlDataEntryResultsSubItem(dataArray[i], 2, _mainContext));
                    }
                }
                else if (TypeOfFile == 3) //client
                {
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        Trace.WriteLine("WUT?");
                        Trace.WriteLine(dataArray.Length);
                        UniformFileList.Children.Add(new UserControlDataEntryResultsSubItem(dataArray[i], 3, _mainContext));
                    }
                }
                else
                {

                }
            }
            else
            {
                //UniformFileList.Children.Add(new UserControlScannerResultsSubItem(-1, 1));
            }
        }

        public void ResetState()
        {
            /*
            foreach (IReset subPreview in UniformFileList.Children) //resetting the data entry input starting values
            {
                subPreview.ResetState();
            }
            */
            UniformFileList.Children.Clear(); //TODO: Check to see if memory is still being taken up.


        }
        private void AddFile_OnClick(object sender, RoutedEventArgs e)
        {
            _context.ChangeAdditionSearchVisibility(TypeOfFile);
        }
    }
}
