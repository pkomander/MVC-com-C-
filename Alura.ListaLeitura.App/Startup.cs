using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    internal class Startup
    {
        // applicationBuilder constroi o pipiline de exibição para aplicação
        public void Configure(IApplicationBuilder app)
        {
            app.Run(Roteamento);
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