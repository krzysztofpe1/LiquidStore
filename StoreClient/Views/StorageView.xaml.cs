using StoreClient.DatabaseModels;
using StoreClient.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
            _storageCache = storageList;
            StorageDataGrid.ItemsSource = _storageCache;
            if (ShowUsedCheckbox.IsChecked.Value)
                ShowUsedStorage(null, null);
            else
                HideUsedStorage(null, null);
        }

        #endregion
        #region GUI Interactions
        private void ShowUsedStorage(object sender, RoutedEventArgs e)
        {
            StorageDataGrid.ItemsSource = _storageCache;
        }
        private void HideUsedStorage(object sender, RoutedEventArgs e)
        {
            StorageDataGrid.ItemsSource = _storageCache.Where(item => item.Remaining > 0 || item.Id == null).ToList();
        }
        private void StorageDataGridRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (StorageDataGrid.SelectedItems.Count != 1) return;
            var item = (STORAGE)StorageDataGrid.SelectedItem;
            if(item.Id == null) return;
            var window = new StorageItemWindow("Zmień wartości pól Produktu w Magazynie", _restClient, this, item);
            window.Show();
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new StorageItemWindow("Dodaj produkt do Magazynu", _restClient, this);
            window.Show();
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (StorageDataGrid.SelectedCells.Count == 0)
                Log.ShowUserErrorBox("Nie zaznaczyłeś przdmiotów do usunięcia.");
            else
            {
                var listOfItemsToDelete = StorageDataGrid.SelectedCells.ToList();
                var mbResult = MessageBox.Show("Na pewno chcesz usunąć element/y?", "Usunąć elementy?", MessageBoxButton.YesNo);
                if (mbResult == MessageBoxResult.Yes)
                {
                    listOfItemsToDelete.ForEach(item =>
                    {
                        _restClient.DeleteStorageItem((STORAGE)item.Item);
                    });
                    RefreshAsync();
                }
            }
        }
        #endregion
    }
}
