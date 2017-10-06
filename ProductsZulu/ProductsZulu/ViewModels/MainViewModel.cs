using GalaSoft.MvvmLight.Command;
using ProductsZulu.Models;
using System.Windows.Input;
using System;
using ProductsZulu.Services;

namespace ProductsZulu.ViewModels
{
    public class MainViewModel
    {
        #region Services
        NavigationService navigationService;
        #endregion

        #region Properties
        public LoginViewModel Login { get; set; }
        public CategoriesViewModel Categories { get; set; }
        public TokenResponse Token { get; set; }
        public ProductsViewModel Products { get; set; }
        public NewCategoryViewModel NewCategory { get; set; }
        public EditCategoryViewModel EditCategory { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this; //La instancia es ud misma
            Login = new LoginViewModel(); //Por aquí empieza la app, cuando paso a otra pagina, es ahí donde instancio la otra clase
            navigationService = new NavigationService();
        }

        #endregion
        #region Sigleton
        static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

        #region Commands
        public ICommand NewCategoryCommand
        {
            get
            {
                return new RelayCommand(GoNewCategory);
            }
        }
        #endregion

        #region Methods
        async void GoNewCategory()
        {
            NewCategory = new NewCategoryViewModel();
            await navigationService.Navigate("NewCategoryView");
        }
        #endregion
    }
}
