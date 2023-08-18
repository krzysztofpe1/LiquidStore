using StoreClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace StoreClient
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private properties
        private LoginView loginView;
        private MainView mainView;
        private Thread _loginWatcherThread;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            loginView = new LoginView();
            
            MainContent.Content = loginView;
            _loginWatcherThread = new Thread(LoginWatcher);
            _loginWatcherThread.SetApartmentState(ApartmentState.STA);
            _loginWatcherThread.Start();
        }
        private void LoginWatcher()
        {
            while (!loginView.LoggedIn)
            {
                Thread.Sleep(500);
            }
            this.Dispatcher.Invoke(() =>
            {
                mainView = new MainView();
                MainContent.Content = mainView;
            });
        }
    }
}
