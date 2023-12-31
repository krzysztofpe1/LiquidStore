﻿using StoreClient.DatabaseModels;
using StoreClient.Utils;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StoreClient.Controls
{
    public partial class OrderDetailsItemAddControl : UserControl
    {
        #region Private cars
        private StoreRestClient _restClient;
        private ORDERDETAILS _item;
        private int? _previousStatus;
        #endregion
        #region Constructor
        public OrderDetailsItemAddControl(StoreRestClient restClient, ORDERDETAILS item = null)
        {
            _restClient = restClient;
            _item = item;
            InitializeComponent();
            InitializeItemChoiceList();
            if (item == null)
            {
                Status.IsEnabled = false;
                Status.SelectedIndex = 0;
            }
            else
            {
                PopulateInputs(item);


            }
        }
        #endregion
        #region Public Methods
        internal int? GetDetailID()
        {
            if (_item == null)
                return null;
            return _item.Id;
        }
        public int GetStorageId()
        {
            return int.Parse(((ComboBoxItem)ItemChoice.SelectedItem).Tag.ToString());
        }
        public int GetVolume()
        {
            if (Volume.Text == "")
                return 0;
            return int.Parse(Volume.Text);
        }
        public int GetConcentration()
        {
            if (Concentration.Text == "")
                return 0;
            return int.Parse(Concentration.Text);
        }
        public int GetStatus()
        {
            return Status.SelectedIndex + 1;
        }
        /// <summary>
        /// Automatically sets the Status to DELIVERED or to previous Status state
        /// </summary>
        /// <param name="status">true = owner; false = non-owner</param>
        public void SetOrderStatus(bool status)
        {
            if (status)
            {
                _previousStatus = Status.SelectedIndex;
                Status.SelectedIndex = 3;
            }
            else if (_previousStatus != null)
            {
                Status.SelectedIndex = _previousStatus.Value;
            }
        }
        #endregion
        #region Private Methods
        private async void PopulateInputs(ORDERDETAILS item)
        {
            //ItemsChoice [Brand + Name]
            string choice = item.Brand + " " + item.Name;
            if (ItemChoice.SelectedItem == null || ((ComboBoxItem)ItemChoice.SelectedItem).Content.ToString() != choice)
            {
                var storageItem = (await _restClient.GetStorage()).FirstOrDefault(storage => storage.Brand + " " + storage.Name == choice);
                if (storageItem == null)
                {
                    storageItem = new STORAGE()
                    {
                        Brand = item.Brand,
                        Name = item.Name,
                        Volume = 0,
                        Cost = 0,
                        Remaining = 0
                    };
                    if ((await _restClient.SaveStorageItem(storageItem)) == null)
                    {
                        Log.ShowServerErrorBox("Błąd połączenia podczas dodawania przedmiotów do listy.\nNie można dodać pustego przedmiotu do magazynu.\nLepiej zamknij okno, bo zrobi się nieprzyjemnie!");
                        return;
                    }
                }
                var ICItem = ItemChoice.Items.Cast<ComboBoxItem>().FirstOrDefault(icItem => icItem.Content.ToString() == choice);
                var ICIndex = ItemChoice.Items.IndexOf(ICItem);
                if(ICIndex == -1)
                {
                    ICIndex = ItemChoice.Items.Add(new ComboBoxItem()
                    {
                        Content = choice,
                        Tag = storageItem.Id
                    });
                }
                ItemChoice.SelectedIndex = ICIndex;
            }
            //Volume
            var CBItem = Volume.Items.Cast<ComboBoxItem>().FirstOrDefault(cbi => cbi.Content.ToString() == item.Volume.ToString());
            var volumeIndex = Volume.Items.IndexOf(CBItem);
            Volume.SelectedIndex = volumeIndex;
            //Concentration
            Concentration.Text = item.Concentration.ToString();
            //Status
            var statusItem = Status.Items.Cast<ComboBoxItem>().FirstOrDefault(si => si.Content.ToString() == item.StatusMapping);
            var statusIndex = Status.Items.IndexOf(statusItem);
            Status.SelectedIndex = statusIndex;

        }
        private async void InitializeItemChoiceList()
        {
            var storage = (await _restClient.GetStorage()).Where(item => item.Remaining>=5);
            string toSearch = (_item == null) ? string.Empty : _item.Brand + " " + _item.Name;
            foreach (var item in storage)
            {
                string result = item.Brand + " " + item.Name;
                ComboBoxItem listItem = new ComboBoxItem()
                {
                    Content = result,
                    Tag = item.Id
                };
                var index = ItemChoice.Items.Add(listItem);
                //doesn't work for some reason
                if(toSearch!=string.Empty && result==toSearch)
                    ItemChoice.SelectedIndex = index;
            }
        }
        #endregion
        #region GUI Interactions
        private void ValidateNumericIntField(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
                e.Handled = true;
        }
        #endregion
    }
}
