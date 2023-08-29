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
            RefreshAsync(true);
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

            // Add Status column
            DataGridTemplateColumn statusColumn = new DataGridTemplateColumn();
            statusColumn.Header = "Status";

            // Cell Template (Non-editing mode)
            FrameworkElementFactory nonEditingFactory = new FrameworkElementFactory(typeof(TextBlock));
            nonEditingFactory.SetBinding(TextBlock.TextProperty, new Binding("StatusMapping"));
            statusColumn.CellTemplate = new DataTemplate() { VisualTree = nonEditingFactory };

            // Cell Editing Template (Editing mode)
            FrameworkElementFactory editingFactory = new FrameworkElementFactory(typeof(ComboBox));
            editingFactory.SetBinding(ComboBox.ItemsSourceProperty, new Binding("StatusOptions"));
            editingFactory.SetBinding(ComboBox.SelectedItemProperty, new Binding("StatusMapping"));

            statusColumn.CellEditingTemplate = new DataTemplate() { VisualTree = editingFactory };

            dataGrid.Columns.Add(statusColumn);

            dataGrid.AutoGenerateColumns = false;
            dataGrid.ItemsSource = details;
            dataGrid.CellEditEnding += OrderDetailsDataGrid_CellEditEnding;
            return dataGrid;
        }

        private void OrderDetailsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var currentDataGrid = sender as DataGrid;
            var oderId = ((ORDERDETAILS)currentDataGrid.Items[0]).OrderId;
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Column is DataGridTemplateColumn templateColumn && templateColumn.CellEditingTemplate != null)
                {
                    ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
                    if (contentPresenter == null) return;
                    ComboBox comboBox = FindVisualChild<ComboBox>(contentPresenter);
                    if (comboBox == null) return;
                    string newValue = comboBox.SelectedItem as string;
                    if (newValue == null) return;
                    if (e.Row.DataContext is ORDERDETAILS editedItem)
                    {
                        string initialValue = editedItem.StatusMapping;
                        editedItem.StatusMapping = newValue;
                        if(editedItem.OrderId == null)editedItem.OrderId = oderId;
                        if(!_restClient.SaveOrderDetailsItem(ref editedItem))
                        {
                            editedItem.StatusMapping = initialValue;
                            //((string)comboBox.SelectedItem) = initialValue;
                        }
                    }

                }
                else
                {
                    TextBox element = e.EditingElement as TextBox;
                    ORDERDETAILS odItem = element.DataContext as ORDERDETAILS;

                    var propName = ((BindingExpression)((DataGridCell)element.Parent).BindingGroup.BindingExpressions[0]).ResolvedSourcePropertyName;
                    var propInfo = odItem.GetType().GetProperties().ToList().FirstOrDefault(prop => prop.Name == propName);
                    var propType = propInfo.PropertyType;
                    var initialValue = propInfo.GetValue(odItem);

                    if (propType == typeof(string))
                        propInfo.SetValue(odItem, element.Text);
                    else if (propType == typeof(int))
                        propInfo.SetValue(odItem, int.Parse(element.Text));
                    else if (propType == typeof(float))
                        propInfo.SetValue(odItem, float.Parse(element.Text));
                    else if (propType == typeof(double))
                        propInfo.SetValue(odItem, double.Parse(element.Text));
                    else if (propType == typeof(Enum))
                        propInfo.SetValue(odItem, int.Parse(element.Text));

                    if (odItem.OrderId == null) odItem.OrderId = oderId;
                    if (!_restClient.SaveOrderDetailsItem(ref odItem))
                    {
                        propInfo.SetValue(odItem, initialValue);
                        if (initialValue != null)
                            element.Text = initialValue.ToString();
                        else
                            element.Text = string.Empty;
                    }
                }
            }
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
        private void PopulateOrdersListView()
        {
            OrdersListView.Items.Clear();
            _ordersCache.ForEach(order =>
            {
                Expander expander = new Expander()
                {
                    Header = order.Comment,
                    Content = InitializeDataGridOfDetails(order.Details)
                };
                OrdersListView.Items.Add(expander);
            });
        }

        public async Task RefreshAsync(bool forceRefresh=false)
        {
            var ordersList = await _restClient.GetOrders();
            if (!ShowDeliveredCheckBox.IsChecked.Value) ordersList = ordersList.Where(item =>
            {
                if(item.Details.Where(detail => detail.Status != OrderStatusMapping.DELIVERED).Count() == 0) return false;
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

        private void ShowDeliveredCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var task = RefreshAsync(true);
        }

        private void ShowDeliveredCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var tast = RefreshAsync(true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = new ORDER() { Comment = "NOWE ZAMÓWIENIE" };
            if (_restClient.SaveOrder(item))
            {
                _ordersCache.Add(item);
                var task = RefreshAsync(true);
            }
            else MessageBox.Show("Nie można było dodać zamówienia!", "Błąd serwera", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
