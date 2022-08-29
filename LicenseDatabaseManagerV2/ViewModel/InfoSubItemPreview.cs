using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace LicenseDatabaseManagerV2.ViewModel
{
    public class InfoSubItemPreview //Simple label maker for preview.
    {
        public InfoSubItemPreview(string varName, string varValue)
        {
            VarName = varName;
            VarValue = varValue;
        }
        public string VarName { get; set; } // Variable 
        public string VarValue { get; set; }
    }
    




}
