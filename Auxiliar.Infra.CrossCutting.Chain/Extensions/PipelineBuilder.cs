using Auxiliar.Infra.CrossCutting.Chain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Auxiliar.Infra.CrossCutting.Chain.Extensions
{
    public static class PipelineBuilder
    {
        #region Metodos Publicos

        public static T ConfigureChain<T>(this IServiceCollection services, T handler) where T : ChainBase
        {
            services.AddSingleton(handler);
            return handler;
        }

        public static T Next<T>(this T handler, T next) where T : ChainBase
        {
            handler.Next = next;
            return (T)handler.Next;
        }

        #endregion Metodos Publicos
    }
}