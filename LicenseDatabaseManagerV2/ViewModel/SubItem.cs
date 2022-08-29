using System;
using System.Windows.Controls;

namespace LicenseDatabaseManagerV2.ViewModel
{
    public class SubItem
    {
        public SubItem(string name, UserControl screen)
        {
            Name = name;
            Screen = screen; //Page location allows one to set a page in project to open via Uri
        }
        public string Name { get; private set; }
        public UserControl Screen { get; private set; }
    }
}