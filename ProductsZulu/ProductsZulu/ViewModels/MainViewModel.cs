using ProductsZulu.Models;

namespace ProductsZulu.ViewModels
{
    public class MainViewModel
    {
        #region Properties
        public LoginViewModel Login { get; set; }
        public CategoriesViewModel Categories { get; set; }
        public TokenResponse Token { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this; //La instancia es ud misma
            Login = new LoginViewModel(); //Por aquí empieza la app, cuando paso a otra pagina, es ahí donde instancio la otra clase
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
    }
}
