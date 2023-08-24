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

namespace StoreClient.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private StoreRestClient _restClient;
        private Thread _loginCheckerThread;
        public bool LoggedIn { get; private set; }
        public LoginView(StoreRestClient restClient)
        {
            _restClient = restClient;
            InitializeComponent();
            #if(DEBUG)
            LoggedIn = true;
            #endif
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            loginTextBox.IsEnabled = false;
            passwordTextBox.IsEnabled = false;
            var login = loginTextBox.Text;
            var password = passwordTextBox.Password;
            _loginCheckerThread = new Thread(()=>CheckLogin(login, password));
            _loginCheckerThread.Start();
        }
        private void CheckLogin(string login, string password)
        {
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
                    Dispatcher.Invoke(()=> informationTextBlock.Text = "Logowanie nie powiodło się");
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
    }
}
