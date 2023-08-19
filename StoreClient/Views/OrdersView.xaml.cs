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
            DataGridTextColumn textColumn = new DataGridTextColumn()
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
            OrdersDataGrid.Columns.Add(textColumn);
        }
        public async Task RefreshAsync()
        {
            OrdersDataGrid.Items.Clear();
            var ordersList = await _restClient.GetOrders();
            ordersList.ForEach(item =>
            {
                OrdersDataGrid.Items.Add(item);
            });
        }
    }
}
