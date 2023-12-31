﻿using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StoreClient.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        #region Private vars
        private StoreRestClient _restClient;
        private Thread _loginCheckerThread;
        #endregion
        #region Public props
        public bool LoggedIn { get; private set; }
        #endregion
        #region Contructors
        internal LoginView(StoreRestClient restClient)
        {
            _restClient = restClient;
            InitializeComponent();
            #if(DEBUG)
            LoggedIn = true;
            #endif
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// This is the method to call, right after Butoon click or Enter pressed.
        /// </summary>
        private void Login()
        {
            var login = loginTextBox.Text;
            var password = passwordTextBox.Password;
            _loginCheckerThread = new Thread(() => CheckLogin(login, password));
            _loginCheckerThread.Start();
        }
        private void CheckLogin(string login, string password)
        {
            Dispatcher.Invoke(() =>
            {
                loginButton.IsEnabled = false;
                loginTextBox.IsEnabled = false;
                passwordTextBox.IsEnabled = false;
            });
            if (login.Length > 0 && password.Length > 0)
            {
                Task<bool> task = _restClient.CreateSession(login, password);
                task.Wait();
                if (task.Result == true)
                {
                    LoggedIn = true;
                }
                else
                {
                    Dispatcher.Invoke(() => informationTextBlock.Text = "Logowanie nie powiodło się");
                }
            }
            else
            {
                Dispatcher.Invoke(() => informationTextBlock.Text = "Wypełnij formularz logowania!");
            }
            Dispatcher.Invoke(() =>
            {
                loginButton.IsEnabled = true;
                loginTextBox.IsEnabled = true;
                passwordTextBox.IsEnabled = true;
            });
        }
        #endregion
        #region GUI Interactions
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Login();
            }
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        #endregion
    }
}
