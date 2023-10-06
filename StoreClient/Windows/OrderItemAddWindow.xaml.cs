using StoceClient.DatabaseModels;
using StoreClient.Controls;
using StoreClient.DatabaseModels;
using StoreClient.Utils;
using StoreClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace StoreClient.Windows
{
    /// <summary>
    /// Interaction logic for OrderItemWindow.xaml
    /// </summary>
    public partial class OrderItemAddWindow : Window
    {
        #region Private vars
        private StoreRestClient _restClient;
        private OrdersView _ordersView;
        private ObservableCollection<OrderDetailsItemAddControl> _ordersDetailsList;
        #endregion
        #region Contructor
        public OrderItemAddWindow(StoreRestClient restClient, OrdersView ordersView)
        {
            _restClient = restClient;
            _ordersView = ordersView;
            _ordersDetailsList = new ObservableCollection<OrderDetailsItemAddControl>();
            InitializeComponent();
            OrderDetailsList.ItemsSource = _ordersDetailsList;
            AddOrderDetailsItemToList();
        }
        #endregion
        #region Private Methods
        private void AddOrderDetailsItemToList()
        {
            _ordersDetailsList.Add(new OrderDetailsItemAddControl(_restClient));
        }
        #endregion
        #region GUI Interactions
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddOrderDetailsItemToList();
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = OrderDetailsList.SelectedItems.Cast<OrderDetailsItemAddControl>().ToList();
            foreach (var item in selectedItems)
            {
                _ordersDetailsList.Remove(item);
            }
        }
        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (Comment.Text == "")
            {
                Log.ShowUserErrorBox("Wpisz nazwę zamówienia.");
                return;
            }
            var listOfDetails = new List<ORDERDETAILS>();
            foreach(var item in OrderDetailsList.Items)
            {
                var myActualItem = (OrderDetailsItemAddControl)item;
                var productId = myActualItem.GetProductId();
                var dbProduct = (await _restClient.GetStorage()).FirstOrDefault(dbItem => dbItem.Id == productId);

                var volume = myActualItem.GetVolume();
                var detailsItem = new ORDERDETAILS()
                {
                    Brand = dbProduct.Brand,
                    Name = dbProduct.Name,
                    Volume = volume
                };
                listOfDetails.Add(detailsItem);
            }
            var orderItem = new ORDER()
            {
                Comment = Comment.Text,
                Details = listOfDetails
            };
            if (!_restClient.SaveOrder(ref orderItem))
            {
                Log.ShowServerErrorBox("Coś poszło nie tak podczas przesyłania zamówienia na serwer. Spróbuj ponownie.");
                return;
            }
            _ordersView.RefreshAsync(true);
            Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
