using StoceClient.DatabaseModels;
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
    /// Interaction logic for OrderItemEditWindow.xaml
    /// </summary>
    public partial class OrderItemEditWindow : Window
    {
        #region Private vars
        private readonly StoreRestClient _restClient;
        private readonly OrdersView _ordersView;
        private ORDER _orderItem;
        #endregion
        #region Constructor
        public OrderItemEditWindow(StoreRestClient restClient, OrdersView ordersView, ORDER orderItem)
        {
            InitializeComponent();
            _restClient = restClient;
            _ordersView = ordersView;
            _orderItem = orderItem;
        }
        #endregion
        #region GUI Interactions
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
