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

namespace StoreClient.Controls
{
    /// <summary>
    /// Interaction logic for OrderDetailsItemAddControl.xaml
    /// </summary>
    public partial class OrderDetailsItemAddControl : UserControl
    {
        private StoreRestClient _restClient { get; }
        public OrderDetailsItemAddControl(StoreRestClient restClient)
        {
            _restClient = restClient;
            InitializeComponent();
            InitializeItemChoiceList();
        }
        private async Task InitializeItemChoiceList()
        {
            var storage = (await _restClient.GetStorage()).Where(item => item.Remaining > 5);
            foreach (var item in storage)
            {
                string result = item.Brand + " " + item.Name;
                ComboBoxItem listItem = new ComboBoxItem()
                {
                    Content = result,
                    Tag = item.Id
                };
                ItemChoice.Items.Add(listItem);
            }
        }
        public int GetProductId()
        {
            return int.Parse(((ComboBoxItem)ItemChoice.SelectedItem).Tag.ToString());
        }
        public int GetVolume()
        {
            return int.Parse(Volume.Text);
        }
    }
}
