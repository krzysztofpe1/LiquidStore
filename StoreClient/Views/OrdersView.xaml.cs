using StoceClient.DatabaseModels;
using StoreClient.Controls;
using StoreClient.DatabaseModels;
using StoreClient.Utils;
using StoreClient.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace StoreClient.Views
{
    /// <summary>
    /// Logika interakcji dla klasy OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        #region Private vars
        private StoreRestClient _restClient;
        private List<ORDER> _ordersCache;
        #endregion
        #region Constructors
        internal OrdersView(StoreRestClient restClient)
        {
            _restClient = restClient;
            _ordersCache = new List<ORDER>();
            InitializeComponent();
            Initialize();
            RefreshAsync(true);
        }
        #endregion
        #region Private Methods
        private async Task Initialize()
        {
            var ordersList = await _restClient.GetOrders();
            _ordersCache = ordersList;
            ordersList.ForEach(order =>
            {
                var dataGrid = InitializeDataGridOfDetails(order.Details);
                dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
                Expander expander = new Expander()
                {
                    Header = order.Comment,
                    Tag = order.Id,
                    Content = dataGrid
                };
                OrdersListView.Items.Add(expander);
            });
        }
        private void PopulateOrdersListView()
        {
            OrdersListView.Items.Clear();
            _ordersCache.ForEach(order =>
            {
                var dataGrid = InitializeDataGridOfDetails(order.Details);
                dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
                Expander expander = new Expander()
                {
                    Header = order.Comment,
                    Tag = order.Id,
                    Content = dataGrid
                };
                OrdersListView.Items.Add(expander);
            });
        }
        private DataGrid InitializeDataGridOfDetails(List<ORDERDETAILS> details)
        {
            DataGrid dataGrid = new DataGrid();
            DataGridTextColumn textColumn = new DataGridTextColumn()
            {
                Header = "ID",
                Binding = new Binding("Id"),
                Width = 30
            };
            dataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Marka",
                Binding = new Binding("Brand"),
                Width = 100
            };
            dataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Nazwa",
                Binding = new Binding("Name"),
                Width = 100
            };
            dataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Objętość",
                Binding = new Binding("Volume"),
                Width = 60
            };
            dataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Stężenie",
                Binding = new Binding("Concentration"),
                Width = 60
            };
            dataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Status",
                Binding = new Binding("StatusMapping"),
                Width = 60
            };
            dataGrid.Columns.Add(textColumn);
            dataGrid.AutoGenerateColumns = false;
            dataGrid.ItemsSource = details;
            dataGrid.CanUserAddRows = false;
            dataGrid.CanUserDeleteRows = false;
            dataGrid.IsReadOnly = true;
            return dataGrid;
        }
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child != null && child is T t)
                {
                    return t;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }
        #endregion
        #region Public Methods
        public async Task RefreshAsync(bool forceRefresh = false)
        {
            var ordersList = await _restClient.GetOrders();
            if (!ShowDeliveredCheckBox.IsChecked.Value) ordersList = ordersList.Where(item =>
            {
                if (item.Details.Where(detail => detail.Status != OrderStatusMapping.SETTLED).Count() == 0) return false;
                return true;
            }).ToList();
            if (forceRefresh)
            {
                _ordersCache = ordersList;
                PopulateOrdersListView();
            }
            if (CheckCache(ordersList)) return;
            PopulateOrdersListView();
        }
        private bool CheckCache(List<ORDER> ordersList)
        {
            List<int> idsList = new List<int>();
            _ordersCache.ForEach(item => idsList.Add(item.Id.Value));
            foreach (var item in ordersList)
            {
                if (!idsList.Contains(item.Id.Value)) return false;
            }
            idsList = new List<int>();
            ordersList.ForEach(item => idsList.Add(item.Id.Value));
            foreach (var item in _ordersCache)
            {
                if (!idsList.Contains(item.Id.Value)) return false;
            }
            return true;
        }
        #endregion
        #region GUI Interactions
        private async void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersListView.SelectedItem == null)
                return;
            int orderId = int.Parse((OrdersListView.SelectedItem as Expander).Tag.ToString());
            var item = (await _restClient.GetOrders()).FirstOrDefault(order => order.Id == orderId);
            var window = new OrderItemWindow(_restClient, this, item);
            window.Show();
        }
        private void ShowCompletedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var task = RefreshAsync(true);
        }
        private void ShowCompletedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var tast = RefreshAsync(true);
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderItemWindow(_restClient, this);
            window.Show();
        }
        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var expanders = OrdersListView.SelectedItems;
            foreach (var expander in expanders)
            {
                var exp = (Expander)expander;
                var order = (await _restClient.GetOrders()).FirstOrDefault(item => item.Id == int.Parse(exp.Tag.ToString()));
                if (!(await _restClient.DeleteOrder(order)))
                    Log.ShowServerErrorBox($"Nie udało się usunąć zamówienia: {order.Comment}");
            }
            RefreshAsync(true);
        }
        #endregion
    }
}
