namespace ProductsZulu.Services
{
    using ProductsZulu.Views;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using System;

    public class NavigationService
    {
        /// <summary>
        /// Drawing Views
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public async Task Navigate(string pageName)
        {
            switch (pageName)
            {
                case "CategoriesView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new CategoriesView()); 
                    break;
                case "ProductsView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new ProductsView());
                    break;
                case "NewCategoryView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new NewCategoryView());
                    break;
                case "EditCategoryView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new EditCategoryView());
                    break;
            }

        }

        public async Task Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
