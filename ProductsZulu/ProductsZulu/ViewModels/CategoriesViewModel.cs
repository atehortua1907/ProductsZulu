namespace ProductsZulu.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using ProductsZulu.Models;
    using ProductsZulu.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class CategoriesViewModel:INotifyPropertyChanged
    {
        #region Services
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        ObservableCollection<Category> _categories;
        List<Category> categoriesList;
        bool _isRefreshing;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
        }

        public ObservableCollection<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Categories)));
                }
            }
        }
        #endregion


        #region Constructors
        public CategoriesViewModel()
        {
            instance = this;
            apiService = new ApiService();
            dialogService = new DialogService();
            LoadCategories();
        }
        #endregion

        #region Sigleton
        static CategoriesViewModel instance;
        public static CategoriesViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CategoriesViewModel();
            }
            return instance;
        }
        #endregion

        #region Methods
        public void AddCategory(Category category)
        {
            IsRefreshing = true;
            categoriesList.Add(category);
            Categories = new ObservableCollection<Category>(
                categoriesList.OrderBy(C => C.Description));
            IsRefreshing = false;
        }

        public void UpdateCategory(Category category)
        {
            IsRefreshing = true;
            var oldCategory = categoriesList
                .Where(c=>c.CategoryId == category.CategoryId)
                .FirstOrDefault();
            oldCategory = category;
            Categories = new ObservableCollection<Category>(
                categoriesList.OrderBy(C => C.Description));
            IsRefreshing = false;
        }

        public async Task DeleteCategory(Category category)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;

                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }


            var mainViewModel = MainViewModel.GetInstance(); //Lo mismo que decir "Instanceate aquí, te necesito aquí"

            var responsePut = await apiService.Delete<Category>(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                category);

            if (!responsePut.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    responsePut.Message);
                return;
            }

            categoriesList.Remove(category);
            Categories = new ObservableCollection<Category>(
                categoriesList.OrderBy(C => C.Description));

            IsRefreshing = false;
        }

        async void LoadCategories()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var response = await apiService.GetList<Category>(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            categoriesList = (List<Category>)response.Result;
            Categories = new ObservableCollection<Category>(
                categoriesList.OrderBy(C=>C.Description));
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadCategories);
            }
        }
        #endregion
    }
}
