using Microsoft.Owin;
using Owin;
using SistemaInventario.Models;
using System;

[assembly: OwinStartupAttribute(typeof(SistemaInventario.Startup))]
namespace SistemaInventario
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
