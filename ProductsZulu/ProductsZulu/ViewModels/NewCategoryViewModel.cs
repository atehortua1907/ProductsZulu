namespace ProductsZulu.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using ProductsZulu.Models;
    using ProductsZulu.Services;
    using System.ComponentModel;
    using System.Windows.Input;

    public class NewCategoryViewModel:INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        #endregion


        #region Atributes
        bool _isEnabled;
        bool _isRunning;
        string _description;
        #endregion


        #region Properties
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

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }
        #endregion

        #region Constructor
        public NewCategoryViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        #endregion

        #region Methods
        async void Save()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You Must Enter a Category Description");
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

            var category = new Category
            {
                Description = Description
            };

            var mainViewModel = MainViewModel.GetInstance(); //Lo mismo que decir "Instanceate aquí, te necesito aquí"
            
            var responsePost = await apiService.Post(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                category);

            if (!responsePost.IsSuccess)
            {
                IsEnabled = true;
                IsRunning = false;
                await dialogService.ShowMessage(
                    "Error",
                    responsePost.Message);
                return;
            }

            category = (Category)responsePost.Result;
            var categoriesViewModel = CategoriesViewModel.GetInstance();
            categoriesViewModel.AddCategory(category);

            await navigationService.Back();//Devolvemos a la página anterior

            IsRunning = false;
            IsEnabled = true;

        }
        #endregion

    }
}