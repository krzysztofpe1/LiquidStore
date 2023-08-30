using System.Windows;
using System.Windows.Controls;

namespace StoreClient.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        #region Private Properties
        private StoreRestClient _restClient { get; set; }
        #endregion
        #region Views
        private StorageView _storageView;
        private OrdersView _ordersView;
        #endregion
        #region constructor
        public MainView(StoreRestClient restClient)
        {
            InitializeComponent();
            _restClient = restClient;
            _storageView = new StorageView(_restClient);
            _ordersView = new OrdersView(_restClient);
            PagableContent.Content = _storageView;
        }
        #endregion
        #region GUI Interactions
        private void StorageButton_Click(object sender, RoutedEventArgs e)
        {
            _storageView.RefreshAsync();
            PagableContent.Content = _storageView;
        }
        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            _ordersView.RefreshAsync((PagableContent.Content==_ordersView));
            PagableContent.Content = _ordersView;
        }
        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
