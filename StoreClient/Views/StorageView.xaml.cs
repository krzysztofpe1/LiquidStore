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
    /// Interaction logic for StorageView.xaml
    /// </summary>
    public partial class StorageView : UserControl
    {
        private StoreRestClient _restClient;
        private List<STORAGE> _storageCache;
        public StorageView(StoreRestClient restClient)
        {
            _restClient = restClient;
            _storageCache = new List<STORAGE>();
            InitializeComponent();
            Initialize();
            RefreshAsync();
        }
        private void Initialize()
        {
            DataGridTextColumn textColumn;
            textColumn = new DataGridTextColumn()
            {
                Header = "ID",
                Binding = new Binding("Id")
            };
            StorageDataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Marka",
                Binding = new Binding("Brand")
            };
            StorageDataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Nazwa",
                Binding = new Binding("Name")
            };
            StorageDataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Objętość",
                Binding = new Binding("Volume")
            };
            StorageDataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Koszt",
                Binding = new Binding("Cost")
            };
            StorageDataGrid.Columns.Add(textColumn);
            textColumn = new DataGridTextColumn()
            {
                Header = "Pozostało",
                Binding = new Binding("Remaining")
            };
            StorageDataGrid.Columns.Add(textColumn);
        }
        public async Task RefreshAsync()
        {
            var storageList = await _restClient.GetStorage();
            if (CheckCache(storageList)) return;
            _storageCache = storageList;
            StorageDataGrid.ItemsSource = _storageCache;
        }

        private void StorageDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            TextBox element = e.EditingElement as TextBox;
            STORAGE storageItem = element.DataContext as STORAGE;

            var propName = ((BindingExpression)((DataGridCell)element.Parent).BindingGroup.BindingExpressions[0]).ResolvedSourcePropertyName;
            var propInfo = storageItem.GetType().GetProperties().ToList().FirstOrDefault(prop => prop.Name == propName);
            var propType = propInfo.PropertyType;
            var initialValue = propInfo.GetValue(storageItem);

            if (propType == typeof(string))
                propInfo.SetValue(storageItem, element.Text);
            else if (propType == typeof(int))
                propInfo.SetValue(storageItem, int.Parse(element.Text));
            else if (propType == typeof(float))
                propInfo.SetValue(storageItem, float.Parse(element.Text));
            else if (propType == typeof(double))
                propInfo.SetValue(storageItem, double.Parse(element.Text));
            else if (propType == typeof(Enum))
                propInfo.SetValue(storageItem, int.Parse(element.Text));

            if (!_restClient.SaveStorageItem(ref storageItem))
            {
                propInfo.SetValue(storageItem, initialValue);
                if (initialValue != null)
                    element.Text = initialValue.ToString();
                else
                    element.Text = string.Empty;
            }
            e.Row.Item=storageItem;
        }
        private bool CheckCache(List<STORAGE> storageList)
        {
            if (_storageCache.Count != storageList.Count) return false;
            storageList.Sort();
            _storageCache.Sort();
            foreach (var twoItems in _storageCache.Zip(storageList, (cache, list) => new { Cache = cache, List = list }))
            {
                if (twoItems.Cache != twoItems.List) return false;
            }
            return true;
        }
    }
}
