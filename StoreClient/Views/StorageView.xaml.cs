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
        public StorageView(StoreRestClient restClient)
        {
            _restClient = restClient;
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
            /*StorageDataGrid.Items.Clear();
            storageList.ForEach(item =>
            {
                //Console.WriteLine($"ID: {item.Id} Brand: {item.Brand} Name: {item.Name} Volume: {item.Volume} Cost: {item.Cost} Remaining: {item.Remaining}");
                StorageDataGrid.Items.Add(item);
            });*/
            StorageDataGrid.ItemsSource = storageList;
        }

        private void StorageDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;
            TextBox element = e.EditingElement as TextBox;
            STORAGE storageItem = element.DataContext as STORAGE;
            var propName = ((BindingExpression)((DataGridCell)element.Parent).BindingGroup.BindingExpressions[0]).ResolvedSourcePropertyName;
            var propInfo = storageItem.GetType().GetProperties().ToList().FirstOrDefault(prop => prop.Name == propName);
            var propType = propInfo.PropertyType;
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
            _restClient.SaveStorageItem(storageItem);
        }
    }
}
