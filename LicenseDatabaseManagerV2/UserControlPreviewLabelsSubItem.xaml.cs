using System;
using System.Collections.Generic;
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
using LicenseDatabaseManagerV2.ViewModel;

namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlPreviewLabelsSubItem.xaml
    /// </summary>
    public partial class UserControlPreviewLabelsSubItem : UserControl
    {
        public UserControlPreviewLabelsSubItem(InfoSubItemPreview infoSubItemPreview)
        {
            InitializeComponent();
            this.DataContext = infoSubItemPreview;
            
        }
    }
}
