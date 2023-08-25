using StoceClient.DatabaseModels;
using StoreClient.DatabaseModels;
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
    /// Logika interakcji dla klasy OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        private StoreRestClient _restClient;
        private List<ORDER> _ordersCache;
        public OrdersView(StoreRestClient restClient)
        {
            _restClient = restClient;
            _ordersCache = new List<ORDER>();
            InitializeComponent();
            Initialize();
            RefreshAsync();
        }
        private async Task Initialize()
        {
            var ordersList = await _restClient.GetOrders();
            _ordersCache = ordersList;
            ordersList.ForEach(order =>
            {
                Expander expander = new Expander()
                {
                    Header = order.Comment,
                    Content = InitializeDataGridOfDetails(order.Details)
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
                Width = 100
            };
            dataGrid.Columns.Add(textColumn);

            dataGrid.AutoGenerateColumns = false;
            dataGrid.ItemsSource = details;
            return dataGrid;
        }
        public async Task RefreshAsync()
        {
            var ordersList = await _restClient.GetOrders();
            if (CheckCache(ordersList)) return;
            _ordersCache = ordersList;

            OrdersListView.Items.Clear();
            ordersList.ForEach(order =>
            {
                Expander expander = new Expander()
                {
                    Header = order.Comment,
                    Content = InitializeDataGridOfDetails(order.Details)
                };
                OrdersListView.Items.Add(expander);
            });
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
    }
}
