using EclipseWorksChallange.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EclipseWorksChallange.Application.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IProjetosService, ProjetosService>();
            services.AddTransient<ITarefasService, TarefasService>();
            services.AddTransient<IHistoricoTarefaService, HistoricoTarefaService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IRelatoriosService, RelatoriosService>();

            return services;
        }
    }
}
