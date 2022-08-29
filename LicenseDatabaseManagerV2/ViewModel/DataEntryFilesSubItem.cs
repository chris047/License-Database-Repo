using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseDatabaseManagerV2.ViewModel
{
    public class DataEntryFilesSubItem
    {
        public DataEntryFilesSubItem(string inputCategoryName)
        {
            InputCategoryName = inputCategoryName;
            Searchable = false;
            Mandatory = false;
        }
        public DataEntryFilesSubItem(string inputCategoryName, int[] list)
        {
            InputCategoryName = inputCategoryName;
            Searchable = false;
            Mandatory = false;
            if (list.Length >= 1)
            {
                for (int i = 0; i < list.Length; i++) //adding array of integers into list.
                {
                    FilesList.Add(list[i]);
                }
            }
        }
        public DataEntryFilesSubItem(string inputCategoryName, bool searchable)
        {
            InputCategoryName = inputCategoryName;
            Searchable = searchable;
            Mandatory = false;
        }
        public DataEntryFilesSubItem(string inputCategoryName, bool searchable, bool mandatory)
        {
            InputCategoryName = inputCategoryName;
            Searchable = searchable;
            Mandatory = mandatory;
        }



        public bool Searchable { get; set; }
        public string InputCategoryName { get; private set; } //Name of the category searching, this will be used to name the label next to text box.

        public List<int> FilesList = new List<int>();
        public bool IsEnabled { get; set; } //All data options start disabled until the client enters information to use it.
        public bool HasBeenClicked { get; set; } // Has the search entry been clicked into.
        public bool Mandatory { get; set; } // Is it mandatory for item to be saved

    }
}
