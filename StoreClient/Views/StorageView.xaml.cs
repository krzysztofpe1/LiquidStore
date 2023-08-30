using StoreClient.DatabaseModels;
using StoreClient.Utils;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoreClient.Views
{
    /// <summary>
    /// Interaction logic for StorageView.xaml
    /// </summary>
    public partial class StorageView : UserControl
    {
        #region Private vars
        private StoreRestClient _restClient;
        private ObservableCollection<STORAGE> _storageCache;
        #endregion
        #region Constructors
        public StorageView(StoreRestClient restClient)
        {
            _restClient = restClient;
            _storageCache = new ObservableCollection<STORAGE>();
            InitializeComponent();
            Initialize();
            RefreshAsync();
            HideUsedStorage(null, null);
            ShowUsedCheckbox.IsChecked = false;
        }
        #endregion
        #region Private Methods
        private void Initialize()
        {
            var converter = new StorageDataGridRowColorConverter(5, 10);

            Style rowStyle = new Style(typeof(DataGridRow));
            rowStyle.Setters.Add(new Setter(DataGridRow.BackgroundProperty, new Binding("Remaining") { Converter = converter }));
            StorageDataGrid.RowStyle = rowStyle;
        }
        private bool CheckCache(ObservableCollection<STORAGE> storageList)
        {
            if (_storageCache.Count != storageList.Count) return false;
            foreach (var twoItems in _storageCache.Zip(storageList, (cache, list) => new { Cache = cache, List = list }))
            {
                if (twoItems.Cache != twoItems.List) return false;
            }
            return true;
        }
        #endregion
        #region Public Methods
        public async Task RefreshAsync()
        {
            var storageList = new ObservableCollection<STORAGE>(await _restClient.GetStorage());
            if (CheckCache(storageList)) return;
            storageList.Add(new STORAGE());
            _storageCache = storageList;
            StorageDataGrid.ItemsSource = _storageCache;
            if (ShowUsedCheckbox.IsChecked.Value)
                ShowUsedStorage(null, null);
            else
                HideUsedStorage(null, null);
        }

        #endregion
        #region GUI Interactions
        private void StorageDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            TextBox element = e.EditingElement as TextBox;
            STORAGE storageItem = element.DataContext as STORAGE;
            var newItem = storageItem.Id == null;

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
            if (newItem && _storageCache.Count > 0)
            {
                _storageCache.RemoveAt(_storageCache.Count - 1);
                _storageCache.Add(storageItem);
                _storageCache.Add(new STORAGE());
            }
            StorageDataGrid.Focus();
        }
        private void ShowUsedStorage(object sender, RoutedEventArgs e)
        {
            StorageDataGrid.ItemsSource = _storageCache;
        }
        private void HideUsedStorage(object sender, RoutedEventArgs e)
        {
            StorageDataGrid.ItemsSource = _storageCache.Where(item => item.Remaining > 0 || item.Id == null).ToList();
        }
        private void StorageDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var item = ((STORAGE)StorageDataGrid.SelectedItem);
                if (!_restClient.DeleteStorageItem(item)) MessageBox.Show("Wprowadzenie zmian nie powiodło się!\nProszę odśwież zakładkę.", "Błąd komunikacji z serwerem", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        #endregion
    }
}
