using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseDatabaseManagerV2.ViewModel
{
    public class RadioButtonSubItem
    {
        public RadioButtonSubItem(string name, string value, string group, bool selected) //Simple class that stores the name of the radio button and the value.
        {
            NameOfRadioButton = name;
            RadioVariable = value;
            RadioGroup = group;
            StartingChecked = selected;
        }

        public string NameOfRadioButton { get; set; }
        public string RadioVariable { get; set; }
        public string RadioGroup { get; set; }
        public bool StartingChecked { get; set; }
    }
}
