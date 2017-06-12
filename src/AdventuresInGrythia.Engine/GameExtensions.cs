using System.Reflection;
using AdventuresInGrythia.Engine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AdventuresInGrythia.Engine
{
    public static class GameExtensions
    {
        public static IServiceCollection AddAdventuresInGrythiaGame(this IServiceCollection services)
        {
            services.AddTransient<Game>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(Game))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

        public static IApplicationBuilder UseAdventuresInGrythia(this IApplicationBuilder app,
                                                              Game game)
        {
            return app.UseMiddleware<GameMiddleware>(game);
        }
    }
}
