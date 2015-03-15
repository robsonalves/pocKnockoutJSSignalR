using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarrinhoCompras.Models;

namespace CarrinhoCompras
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RepositorioMemoria<Livro>.RepositorioMem = new ConcurrentDictionary<int, Livro>();
            RepositorioMemoria<Livro>.RepositorioMem.TryAdd(1, new Livro { Autor = "Assis, Machado", Id = 1, Preco = 100, Quantidade = 160, Titulo = "Helena" });
            RepositorioMemoria<Livro>.RepositorioMem.TryAdd(2, new Livro { Autor = "Alencar, josé", Id = 2, Preco = 150, Quantidade = 500, Titulo = "O guarani" });
            RepositorioMemoria<Livro>.RepositorioMem.TryAdd(3, new Livro { Autor = "Riordan, Rick", Id = 3, Preco = 200, Quantidade = 1000, Titulo = "Heroes of Olympus V.4 - The Mark Of Athena" });
            RepositorioMemoria<Livro>.RepositorioMem.TryAdd(4, new Livro { Autor = "Collins, Suzanne", Id = 4, Preco = 60, Quantidade = 2000, Titulo = "Hunger Games V.2 - Catching Fire" });
            RepositorioMemoria<Livro>.RepositorioMem.TryAdd(5, new Livro { Autor = "Riordan, Rick", Id = 5, Preco = 90, Quantidade = 800, Titulo = "Heroes of Olympus V.5 - The blood of Olympus" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
