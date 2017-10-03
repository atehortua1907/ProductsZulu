using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProductsZulu.Backend.Startup))]
namespace ProductsZulu.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
