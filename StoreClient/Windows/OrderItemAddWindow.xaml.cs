using StoreClient.Controls;
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
    }
}
