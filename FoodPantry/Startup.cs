using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FoodPantry.Startup))]

namespace FoodPantry
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}