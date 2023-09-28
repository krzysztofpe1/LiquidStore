using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StoreClient.Utils
{
    internal static class Log
    {
        public static void ShowUserErrorBox(string message)
        {
            MessageBox.Show(message, "Błąd klienta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public static void ShowServerErrorBox(string message)
        {
            MessageBox.Show(message, "Błąd serwera", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
