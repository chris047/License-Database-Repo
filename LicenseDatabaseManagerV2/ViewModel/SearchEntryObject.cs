using System.Collections.Generic;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace LicenseDatabaseManagerV2.ViewModel
{
    public class SearchEntryObject
    {
        public SearchEntryObject(string searchCategoryName, string startingSearchText, bool isAdvanced)
        {
            SearchCategoryName = searchCategoryName;
            StartingSearchText = startingSearchText;
            IsEnabled = false;
            IsAdvanced = isAdvanced;
            HasBeenClicked = false;
        } // default full constructor 
        public SearchEntryObject(string searchCategoryName, string startingSearchText) 
        {
            SearchCategoryName = searchCategoryName;
            StartingSearchText = startingSearchText;
            IsEnabled = false;
            HasBeenClicked = false;
        } //this is the mostly full constructor that allows to set enabled
       //this is the semi full instructor that sets the search preference automatically to false, as only one should usually be enabled at startup.
       public SearchEntryObject(string searchCategoryName)
        {
            SearchCategoryName = searchCategoryName;
            StartingSearchText = "";
            IsEnabled = false;
            HasBeenClicked = false;
        } //this is the second most simplest of search entries, as it just asks for the category name and if its advanced or not.
       


        public string SearchCategoryName { get; private set; } //Name of the category searching, this will be used to name the label next to text box.
        public string? StartingSearchText { get;  set; } //What information will be shown as the example for the search box.
        // e.g. xx: 'bill jones' 
        //this would appear in grey and would disappear when box is clicked.

        public bool IsEnabled { get; set; } //All search options start disabled until the client enters information to use it.
        public bool IsAdvanced { get; private set; } //if the search category is advanced.
        public bool HasBeenClicked { get; set; } // Has the search entry been clicked into.


    }
}
