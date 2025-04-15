using System;
using EclipseWorksChallange.Infraestructure.Persistence;
using EclipseWorksChallange.Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EclipseWorksChallange.Infraestructure.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IHistoricoTarefaRepository, HistoricoTarefaRepository>();
            services.AddScoped<ITarefasRepository, TarefasRepository>();
            services.AddScoped<IProjetosRepository, ProjetosRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }

        public static IServiceCollection AddSQLServerConnection(this IServiceCollection services, IConfiguration configuration)
        {
            //Secrets manager
            services.AddDbContext<EclipseWorksSQLServerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EclipseWorks")));

            return services;
        }
    }
}
