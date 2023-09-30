using StoceClient.DatabaseModels;
using StoreClient.Controls;
using StoreClient.DatabaseModels;
using StoreClient.Views;
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
using System.Windows.Shapes;

namespace StoreClient.Windows
{
    /// <summary>
    /// Interaction logic for OrderItemWindow.xaml
    /// </summary>
    public partial class OrderItemAddWindow : Window
    {
        private StoreRestClient _restClient;
        private OrdersView _ordersView;
        public OrderItemAddWindow(StoreRestClient restClient, OrdersView ordersView)
        {
            _restClient = restClient;
            _ordersView = ordersView;
            InitializeComponent();
            AddOrderDetailsItemToList();
        }
        public void AddOrderDetailsItemToList()
        {
            OrderDetailsList.Items.Add(new OrderDetailsItemAddControl(_restClient));
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddOrderDetailsItemToList();
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = OrderDetailsList.SelectedItems;
            foreach (var item in selectedItems)
            {
                OrderDetailsList.Items.Remove(item);
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var listOfDetails = new List<ORDERDETAILS>();
            foreach(var item in OrderDetailsList.Items)
            {
                var myActualItem = (OrderDetailsItemAddControl)item;
                var productId = myActualItem.GetProductId();
                //var dbProduct = _restClient.GetStorage().Result.FirstOrDefault(dbItem => dbItem.Id == productId);

                var volume = myActualItem.GetVolume();
                var detailsItem = new ORDERDETAILS()
                {
                    /*Brand = dbProduct.Brand,
                    Name = dbProduct.Name,*/
                    Volume = volume
                };
                listOfDetails.Add(detailsItem);
            }
            var orderItem = new ORDER()
            {
                Comment = Comment.Text,
                Details = listOfDetails
            };

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
