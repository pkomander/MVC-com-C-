using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    internal class Startup
    {
        // applicationBuilder constroi o pipiline de exibição para aplicação
        public void Configure(IApplicationBuilder app)
        {
            app.Run(LivrosParaLer);
        }

        public Task LivrosParaLer(HttpContext context)
        {
            // quando a request for chamanda sera mandada a resposta no contex.response
            var _repo = new LivroRepositorioCSV();
            return context.Response.WriteAsync(_repo.ParaLer.ToString());
            
        }
    }
}