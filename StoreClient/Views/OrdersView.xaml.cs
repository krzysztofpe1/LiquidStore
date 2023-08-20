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
        public OrdersView(StoreRestClient restClient)
        {
            _restClient = restClient;
            InitializeComponent();
            Initialize();
            RefreshAsync();
        }
        private void Initialize()
        {
            /*DataGridTextColumn textColumn = new DataGridTextColumn()
            {
                Header = "ID",
                Binding = new Binding("Id")
            };
            OrdersDataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Komentarz",
                Binding = new Binding("Comment")
            };
            OrdersDataGrid.Columns.Add(textColumn);*/
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

            details.ForEach(detail =>
            {
                dataGrid.Items.Add(detail);
            });
            return dataGrid;
        }
        public async Task RefreshAsync()
        {
            /*OrdersDataGrid.Items.Clear();
            var ordersList = await _restClient.GetOrders();
            ordersList.ForEach(item =>
            {
                var index = OrdersDataGrid.Items.Add(item);
            });*/
            OrdersListView.Items.Clear();
            var ordersList = await _restClient.GetOrders();
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
    }
}
