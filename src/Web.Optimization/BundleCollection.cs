using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using WebOptimizer;

namespace System.Web.Optimization
{
    /// <summary>
    /// The main entry point for Web optimization bundling and is exposed to developers via <see cref="BundleTable.Bundles"/>.
    /// </summary>
    public class BundleCollection : IAssetPipeline
    {
        private readonly IAssetPipeline assetPipline;

        /// <summary>
        /// Construct a <see cref="BundleCollection"/> that wraps an <see cref="IAssetPipeline"/>.
        /// </summary>
        /// <param name="assetPipline">The underlying <see cref="IAssetPipeline"/>.</param>
        public BundleCollection(IAssetPipeline assetPipline)
        {
            this.assetPipline = assetPipline ?? throw new ArgumentNullException(nameof(assetPipline));
        }

        /// <inheritdoc/>
        public IReadOnlyList<IAsset> Assets => assetPipline.Assets;

        /// <summary>
        /// Adds a bundle to the collection
        /// </summary>
        /// <param name="bundle">The bundle to add to the collection</param>
        public void Add(Bundle bundle)
        {
            if (bundle == null)
            {
                throw new ArgumentNullException("bundle");
            }

            assetPipline
                .AddBundle(bundle.Path, bundle.ContentType, bundle.Files.ToArray())
                .UseContentRoot();
        }

        /// <inheritdoc/>
        public IAsset AddBundle(IAsset asset)
        {
            return assetPipline.AddBundle(asset);
        }

        /// <inheritdoc/>
        public IEnumerable<IAsset> AddBundle(IEnumerable<IAsset> asset)
        {
            return assetPipline.AddBundle(asset);
        }

        /// <inheritdoc/>
        public IAsset AddBundle(string route, string contentType, params string[] sourceFiles)
        {
            return assetPipline.AddBundle(route, contentType, sourceFiles);
        }

        /// <inheritdoc/>
        public IEnumerable<IAsset> AddFiles(string contentType, params string[] sourceFiles)
        {
            return assetPipline.AddFiles(contentType, sourceFiles);
        }

        /// <inheritdoc/>
        public bool TryGetAssetFromRoute(string route, out IAsset asset)
        {
            return assetPipline.TryGetAssetFromRoute(route, out asset);
        }
    }
}
