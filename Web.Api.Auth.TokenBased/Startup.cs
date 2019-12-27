
using Owin;

namespace Web.Api.Auth.TokenBased
{
    public partial class Startup
    {
        public void Configururation(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}