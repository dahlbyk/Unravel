using System;
using System.Web.Optimization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebOptimizer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UnravelWebOptimizationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds WebOptimizer to the specified <see cref="IServiceCollection"/>.
        /// Also sets <see cref="BundleTable.Bundles"/> to a <see cref="BundleCollection"/> that wraps the current pipeline.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureBundles">Configure asset pipeline with legacy <see cref="BundleCollection"/>.</param>
        public static IServiceCollection AddWebOptimizer(this IServiceCollection services, Action<BundleCollection> configureBundles)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureBundles == null)
            {
                throw new ArgumentNullException(nameof(configureBundles));
            }

            services.TryAddSingleton<AssetManager>();

            return services.AddWebOptimizer(ConfigureAssetPipeline);

            void ConfigureAssetPipeline(IAssetPipeline assetPipline)
            {
                var bundles = new BundleCollection(assetPipline);
                BundleTable.SetBundles(bundles);
                configureBundles(bundles);
            }
        }
    }
}
