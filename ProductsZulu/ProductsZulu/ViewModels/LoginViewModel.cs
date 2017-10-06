namespace ProductsZulu.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using ProductsZulu.Services;
    using System.ComponentModel;
    using System.Windows.Input;

    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Atributes
        string _email;
        bool _isEnabled;
        bool _isToggled;
        bool _isRunning;
        string _password;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Properties
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if (_isToggled != value)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsToggled)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            Email = "jzuluaga55@gmail.com";
            Password = "123456";

            IsEnabled = true;
            IsToggled = true;
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter an email");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a password");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var tokenResponse = await apiService.GetToken("http://productszuluapi.azurewebsites.net", Email, Password);

            if (tokenResponse == null)
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage(
                    "Error",
                    "The Service in not available, please try latter.");
                Password = null;
                return;
            }

            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage(
                    "Error",
                    tokenResponse.ErrorDescription);
                Password = null;
                return;
            }

            //Voy a la siguiente pagina
            //Estamos garantizando que antes de pintar la CategoriesView, estamos
            //instanciando la CategoriesViewModel y ademas la ligamos a la MainViewModel
            //(Fundamental para el patron MVVM)
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = tokenResponse;
            mainViewModel.Categories = new CategoriesViewModel();
            await navigationService.Navigate("CategoriesView");//Pintamos la vista de categories

            Email = null;
            Password = null;

            IsEnabled = true;
            IsRunning = false;

        }
        #endregion

    }
}
