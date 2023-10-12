using StoreClient.DatabaseModels;
using StoreClient.Utils;
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

namespace StoreClient.Views
{
    /// <summary>
    /// Interaction logic for StorageItemWindow.xaml
    /// </summary>
    public partial class StorageItemWindow : Window
    {
        #region Private vars
        private StorageView _storageView;
        private StoreRestClient _restClient;
        private int? _itemId = null;
        #endregion
        public WindowExitingStatus ExitingStatus { get; private set; } = WindowExitingStatus.Waiting;
        public StorageItemWindow(string title, StoreRestClient restClient, StorageView storageView)
        {
            _restClient = restClient;
            _storageView = storageView;
            Title = title;
            InitializeComponent();
        }
        public StorageItemWindow(string title, StoreRestClient restClient, StorageView storageView, STORAGE storageItem)
        {
            _restClient = restClient;
            _storageView = storageView;
            Title = title;
            InitializeComponent();
            InitializeTextBoxes(storageItem);
            _itemId = storageItem.Id;
        }

        private void InitializeTextBoxes(STORAGE storageItem)
        {
            Brand.Text = storageItem.Brand;
            Name.Text = storageItem.Name;
            Volume.Text = storageItem.Volume.ToString();
            Cost.Text = storageItem.Cost.ToString();
            Remaining.Text = storageItem.Remaining.ToString();
        }
        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = new STORAGE()
                {
                    Id = _itemId,
                    Brand = Brand.Text,
                    Name = Name.Text,
                    Volume = int.Parse(Volume.Text),
                    Cost = double.Parse(Cost.Text),
                    Remaining = int.Parse(Remaining.Text)
                };
                if ((await _restClient.SaveStorageItem(item)) == null)
                {
                    Log.ShowUserErrorBox("Coś poszło nie tak, sprobuj poprawić błędy, jeżeli widzisz jakieś.");
                    return;
                }
            }
            catch { return; }
            ExitingStatus = WindowExitingStatus.Proceed;
            _storageView.RefreshAsync();
            this.Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ExitingStatus = WindowExitingStatus.Canceled;
            this.Close();
        }
        private void ValidateNumericIntField(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
                e.Handled = true;
        }
        private void ValidateNumericDoubleField(object sender, TextCompositionEventArgs e)
        {
            var temp = e.Text;
            if (temp == ",")
            {
                var textBox = (TextBox)sender;
                if (textBox.Text.Contains(","))
                    e.Handled = true;
            }
            else if (!int.TryParse(temp, out _))
                e.Handled = true;
            
        }
    }
}
