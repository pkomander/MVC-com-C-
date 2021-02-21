using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    internal class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }



        // applicationBuilder constroi o pipiline de exibição para aplicação
        public void Configure(IApplicationBuilder app)
        {
            var builder = new RouteBuilder(app);
            builder.MapRoute("Livros/ParaLer", LivrosParaLer);
            builder.MapRoute("Livros/Lendo", LivrosLendo);
            builder.MapRoute("Livros/Lidos", LivrosLidos);
            builder.MapRoute("Cadastro/NovoLivro/{nome}/{autor}", NovoLivroParaLer);
            builder.MapRoute("Livros/Detalhes/{id:int}", ExibeDetalhes); //colocando uma restrição para o tipo inteiro no elemento
            var rotas = builder.Build();

            app.UseRouter(rotas);

            //app.Run(Roteamento);
        }

        public Task ExibeDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            var repo = new LivroRepositorioCSV();
            var livro = repo.Todos.First(l => l.Id == id);
            return context.Response.WriteAsync(livro.Detalhes());
        }

        public Task NovoLivroParaLer(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = Convert.ToString(context.GetRouteValue("nome")),
                Autor = Convert.ToString(context.GetRouteValue("autor")),
            };

            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso");
        }

        // httpcontext tem duas classes: resquest e response
        public Task Roteamento(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            // caminhos, caso eles existam, aparecerar a rota com o conteudo do caminho
            // no caso de livros para ler ira aparecer todos os livros que o leito falta ler na lista
            var caminhosAtendidos = new Dictionary<string, RequestDelegate>
            {
                { "/Livros/ParaLer", LivrosParaLer },
                {"/Livros/Lendo", LivrosLendo },
                { "Livros/Lidos", LivrosLidos }
            };


            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {
                var metodo = caminhosAtendidos[context.Request.Path];
                return metodo.Invoke(context);
            }

            context.Response.StatusCode = 404;
            return context.Response.WriteAsync("Caminho inexistente");
        }

        // Livros para ler e o metodo para ser chamado no run, no caso chama todos os livros pendentes para ler
        public Task LivrosParaLer(HttpContext context)
        {
            // quando a request for chamanda sera mandada a resposta no contex.response
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
            
        }

        public Task LivrosLendo(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lendo.ToString());

        }

        public Task LivrosLidos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.Lidos.ToString());

        }
    }
}