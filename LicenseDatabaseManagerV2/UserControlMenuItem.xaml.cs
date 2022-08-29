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
using System.Diagnostics;



namespace LicenseDatabaseManagerV2
{
    /// <summary>
    /// Interaction logic for UserControlMenuItem.xaml
    /// </summary>
    public partial class UserControlMenuItem : UserControl
    {
        private MainWindow _context;
        public UserControlMenuItem(ItemMenu itemMenu, MainWindow context)
        {
            InitializeComponent();
            _context = context;
            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
            this.DataContext = itemMenu;
        }
        /* Using textBlock as button was unruly to say the least.
        private void MoveToNewPage(object sender, MouseButtonEventArgs e)
        {
            _context.OpenNewPage(((TextBlock)sender).Tag);
            Trace.WriteLine("Has been clicked on button");
        }
        */
        private void MoveToNewPageMain(object sender, MouseButtonEventArgs e)
        {
            _context.OpenNewPage(((SubItem) ((ListView) sender).SelectedItem).Screen);
           
        }
    }
}
