﻿using System.Collections.Generic;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace LicenseDatabaseManagerV2.ViewModel
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        // Only used by dashboard 
        //public ItemMenu(string header, UserControl screen, PackIconKind icon)
        //{
        //    Header = header;
        //    Screen = screen;
        //    Icon = icon;
        //}

        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
