namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   An interface for configuring ASP.NET MVC services.
    /// </summary>
    public interface IAspNetMvcBuilder
    {
        /// <summary>
        ///   Gets the <see cref="IServiceCollection"/> where ASP.NET MVC services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
