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
    /// Interaction logic for StorageView.xaml
    /// </summary>
    public partial class StorageView : UserControl
    {
        private StoreRestClient _restClient;
        public StorageView(StoreRestClient client)
        {
            _restClient = client;
            InitializeComponent();
            Initialize();
            RefreshAsync();
        }
        private void Initialize()
        {
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "ID";
            textColumn.Binding = new Binding("Id");
            StorageDataGrid.Columns.Add(textColumn);
            textColumn= new DataGridTextColumn();
            textColumn.Header = "Marka";
            textColumn.Binding = new Binding("Brand");
            StorageDataGrid.Columns.Add(textColumn);
            textColumn= new DataGridTextColumn();
            textColumn.Header= "Nazwa";
            textColumn.Binding = new Binding("Name");
            StorageDataGrid.Columns.Add(textColumn);
            textColumn= new DataGridTextColumn();
            textColumn.Header = "Objętość";
            textColumn.Binding = new Binding("Volume");
            StorageDataGrid.Columns.Add(textColumn);
            textColumn= new DataGridTextColumn();
            textColumn.Header = "Koszt";
            textColumn.Binding = new Binding("Cost");
            StorageDataGrid.Columns.Add(textColumn);
            textColumn= new DataGridTextColumn();
            textColumn.Header = "Pozostało";
            textColumn.Binding = new Binding("Remaining");
            StorageDataGrid.Columns.Add(textColumn);
        }
        public async Task RefreshAsync()
        {
            var storageList = await _restClient.GetStorage();
            StorageDataGrid.Items.Clear();
            storageList.ForEach(item =>
            {
                //Console.WriteLine($"ID: {item.Id} Brand: {item.Brand} Name: {item.Name} Volume: {item.Volume} Cost: {item.Cost} Remaining: {item.Remaining}");
                StorageDataGrid.Items.Add(item);
            });
        }
    }
}
