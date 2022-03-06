namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   An interface for configuring ASP.NET Web API services.
    /// </summary>
    public interface IAspNetWebApiBuilder
    {
        /// <summary>
        ///   Gets the <see cref="IServiceCollection"/> where ASP.NET Web API services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
