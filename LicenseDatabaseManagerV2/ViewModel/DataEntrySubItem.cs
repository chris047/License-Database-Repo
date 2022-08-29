using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseDatabaseManagerV2.ViewModel
{
    public class DataEntrySubItem
    {


        public DataEntrySubItem(string inputCategoryName, string dataEntryText)
        {
            InputCategoryName = inputCategoryName;
            DataEntryText = dataEntryText;
            StartingText = dataEntryText;
            Searchable = false;
            Mandatory = false;
        }
        public DataEntrySubItem(string inputCategoryName, string dataEntryText, bool searchable)
        {
            InputCategoryName = inputCategoryName;
            DataEntryText = dataEntryText;
            StartingText = dataEntryText;
            Searchable = searchable;
            Mandatory = false;
        }
        public DataEntrySubItem(string inputCategoryName, string dataEntryText, bool searchable, bool mandatory)
        {
            InputCategoryName = inputCategoryName;
            DataEntryText = dataEntryText;
            StartingText = dataEntryText;
            Searchable = searchable;
            Mandatory = mandatory;
        }
        

        public bool Searchable { get; set; }
        public string InputCategoryName { get; private set; } //Name of the category searching, this will be used to name the label next to text box.
        public string DataEntryText { get; set; } //What information will be shown as the example for the search box.
        
        public String StartingText { get; set; }
        // e.g. xx: 'bill jones' 
        //this would appear in grey and would disappear when box is clicked.
        public bool IsEnabled { get; set; } //All data options start disabled until the client enters information to use it.
        public bool Mandatory { get; set; } // Is it mandatory for item to be saved
        public bool HasBeenClicked { get; set; } // Has the search entry been clicked into.

    }
    



}
