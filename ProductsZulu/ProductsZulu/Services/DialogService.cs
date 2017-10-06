namespace ProductsZulu.Services
{
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class DialogService
    {
        public async Task ShowMessage(string tittle, string message)
        {
            await Application.Current.MainPage.DisplayAlert(
                tittle,
                message,
                "Accept");
        }

        /// <summary>
        /// Retorna un booleano con la respuesta de la opción escogida por el usuario
        /// </summary>
        /// <param name="tittle"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> ShowConfirm(string tittle, string message)
        {
            return await Application.Current.MainPage.DisplayAlert(
                tittle,
                message,
                "Yes",
                "No");
        }

    }

}
