using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StoreClient.Utils
{

    internal class StorageDataGridRowColorConverter : IValueConverter
    {
        #region Private vars
        private int _redValue = 5;
        private int _greenValue = 10;
        #endregion
        #region Constructors
        public StorageDataGridRowColorConverter(int redColorBelowValue, int greenColorAboveValue)
        {
            _redValue = redColorBelowValue;
            _greenValue = greenColorAboveValue;
        }
        #endregion
        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue;
            if (value != null && int.TryParse(value.ToString(), out intValue))
            {
                if (intValue <= _redValue)
                    return new SolidColorBrush(Color.FromArgb(25, 255, 0, 0));
                if (intValue >= _greenValue)
                    return new SolidColorBrush(Color.FromArgb(25, 0, 255, 0));
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
