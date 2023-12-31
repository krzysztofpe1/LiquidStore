﻿using StoceClient.DatabaseModels;
using StoreClient.Controls;
using StoreClient.DatabaseModels;
using StoreClient.Utils;
using StoreClient.Views;
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
using System.Windows.Shapes;

namespace StoreClient.Windows
{
    /// <summary>
    /// Interaction logic for OrderItemWindow.xaml
    /// </summary>
    public partial class OrderItemWindow : Window
    {
        #region Private vars
        private StoreRestClient _restClient;
        private OrdersView _ordersView;
        private ObservableCollection<OrderDetailsItemAddControl> _orderDetailsList;
        private ORDER _item;
        private List<ORDERDETAILS> _deletedItems;
        #endregion
        #region Contructor
        public OrderItemWindow(StoreRestClient restClient, OrdersView ordersView, ORDER item = null)
        {
            _restClient = restClient;
            _ordersView = ordersView;
            _item = item;
            _orderDetailsList = new ObservableCollection<OrderDetailsItemAddControl>();
            _deletedItems = new List<ORDERDETAILS>();
            InitializeComponent();
            OrderDetailsList.ItemsSource = _orderDetailsList;
            if(item == null)
                AddOrderDetailsItemToList();
            else
            {
                Comment.Text = item.Comment;
                foreach(var detail in item.Details)
                {
                    AddOrderDetailsItemToList(detail);
                }
            }
        }
        #endregion
        #region Private Methods
        private void AddOrderDetailsItemToList(ORDERDETAILS item = null)
        {
            _orderDetailsList.Add(new OrderDetailsItemAddControl(_restClient, item));
        }
        #endregion
        #region GUI Interactions
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddOrderDetailsItemToList();
        }
        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = OrderDetailsList.SelectedItems.Cast<OrderDetailsItemAddControl>().ToList();
            foreach (var item in selectedItems)
            {
                if (item.GetDetailID() != null)
                    _deletedItems.Add(await _restClient.GetOrderDetailsItem(item.GetDetailID().Value));
                _orderDetailsList.Remove(item);
            }
        }
        private void OwnersButton_Checked(object sender, RoutedEventArgs e)
        {
            foreach(var item in _orderDetailsList)
            {
                item.SetOrderStatus(true);
            }
        }
        private void OwnersButton_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in _orderDetailsList)
            {
                item.SetOrderStatus(false);
            }
        }
        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (Comment.Text == "")
            {
                Log.ShowUserErrorBox("Wpisz nazwę zamówienia.");
                return;
            }
            var listOfDetails = new List<ORDERDETAILS>();
            foreach(var item in OrderDetailsList.Items)
            {
                var myActualItem = (OrderDetailsItemAddControl)item;
                var itemID = myActualItem.GetDetailID();
                var storageID = myActualItem.GetStorageId();
                var dbStorageItem = (await _restClient.GetStorage()).FirstOrDefault(dbItem => dbItem.Id == storageID);

                var volume = myActualItem.GetVolume();
                var concentration = myActualItem.GetConcentration();
                var status = myActualItem.GetStatus();
                var detailsItem = new ORDERDETAILS()
                {
                    Id = itemID,
                    Brand = dbStorageItem.Brand,
                    Name = dbStorageItem.Name,
                    Volume = volume,
                    Concentration = concentration,
                    Status = (OrderStatusMapping)status
                };
                listOfDetails.Add(detailsItem);
            }
            if (_item == null)
            {
                _item = new ORDER()
                {
                    Comment = Comment.Text,
                    Details = listOfDetails
                };
            }
            else
            {
                _item.Comment = Comment.Text;
                _item.Details = listOfDetails;
            }
            if ((await _restClient.SaveOrder(_item)) == null)
            {
                Log.ShowServerErrorBox("Coś poszło nie tak podczas przesyłania zamówienia na serwer. Spróbuj ponownie.");
                return;
            }
            foreach(var item in _deletedItems)
            {
                if(!(await _restClient.DeleteOrderDetailsItem(item)))
                    Log.ShowServerErrorBox($"Nie można było usunąć przemdiotu z zamówienia: {item.Brand + " " + item.Name}");
            }
            _ordersView.RefreshAsync(true);
            Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
