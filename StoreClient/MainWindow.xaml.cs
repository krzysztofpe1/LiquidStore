using StoreClient.Views;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace StoreClient
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private vars
        private LoginView loginView;
        private MainView mainView;
        private Thread _loginWatcherThread;
        private StoreRestClient _restClient;
        #endregion
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
#if(!DEBUG)
            _restClient = new StoreRestClient("http://84.10.162.134:5000");
#else
            _restClient = new StoreRestClient("http://localhost:5000");
#endif
            loginView = new LoginView(_restClient);
            
            MainContent.Content = loginView;
            _loginWatcherThread = new Thread(LoginWatcher);
            _loginWatcherThread.SetApartmentState(ApartmentState.STA);
            _loginWatcherThread.Start();
        }
#endregion
        #region Private Methods
        private void LoginWatcher()
        {
            while (!loginView.LoggedIn)
            {
                Thread.Sleep(500);
            }
            this.Dispatcher.Invoke(() =>
            {
                mainView = new MainView(_restClient);
                MainContent.Content = mainView;
            });
        }
        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            if(!loginView.LoggedIn)_loginWatcherThread.Abort();
        }
        #endregion
    }
}
