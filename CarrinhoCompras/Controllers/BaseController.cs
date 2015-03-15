using System;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using CarrinhoCompras.Hubs;

namespace CarrinhoCompras.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected readonly Lazy<IHubContext> CarrinhoComprasHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<CarrinhoComprasHub>());
        protected readonly Lazy<IHubContext> AdminHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<AdminHub>());
    }
}