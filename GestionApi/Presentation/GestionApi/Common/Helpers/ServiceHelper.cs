namespace GestionApi.Common.Helpers
{
    public static class ServiceHelper
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Resuelve una instancia del tipo especificado.
        /// Si hay múltiples implementaciones registradas, devuelve la última registrada.
        /// </summary>
        /// <typeparam name="T">El tipo del servicio a resolver.</typeparam>
        /// <returns>Una instancia del tipo especificado.</returns>
        public static T Resolve<T>() => ServiceProvider.GetService<T>();

        /// <summary>
        /// Resuelve todas las instancias del tipo especificado.
        /// </summary>
        /// <typeparam name="T">El tipo del servicio a resolver.</typeparam>
        /// <returns>Una colección de instancias del tipo especificado.</returns>
        public static IEnumerable<T> ResolveAll<T>() => ServiceProvider.GetServices<T>();
    }
}
