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

namespace StoreClient.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private StorageView storageView;
        public MainView()
        {
            InitializeComponent();
            storageView = new StorageView();
            PagableContent.Content = storageView;
        }

        private void StorageButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
