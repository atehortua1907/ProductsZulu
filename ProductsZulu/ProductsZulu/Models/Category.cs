namespace ProductsZulu.Models
{
    using GalaSoft.MvvmLight.Command;
    using ProductsZulu.Services;
    using ProductsZulu.ViewModels;
    using System.Collections.Generic;
    using System.Windows.Input;
    using System;

    public class Category
    {
        #region Properties
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
        #endregion

        #region Services
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Commands
        public ICommand SelectCategoryCommand
        {
            get
            {
                return new RelayCommand(SelectCategory);
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(Edit);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }
        #endregion

        #region Constructors
        public Category()
        {
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Methods
        async void SelectCategory()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Products = new ProductsViewModel(Products);
            await navigationService.Navigate("ProductsView");//Pintamos la vista de Productos
        }

        async void Edit()
        {
            MainViewModel.GetInstance().EditCategory = new EditCategoryViewModel(this);//Edita la categoria escogida this
            await navigationService.Navigate("EditCategoryView");
        }

        async void Delete()
        {
            var response = await dialogService.ShowConfirm(
                "Confirm",
                "Are you sure to delete this record");

            if (!response)
            {
                return;
            }

            CategoriesViewModel.GetInstance().DeleteCategory(this);
        }

        public override int GetHashCode()
        {
            return CategoryId;
        }
        #endregion

    }

}
