using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebOptimizer;

namespace System.Web.Optimization
{
    /// <summary>
    /// Static holder class for the default bundle collection
    /// </summary>
    public class BundleTable
    {
        private static BundleCollection bundles;
        private static bool? enableOptimizations;

        /// <summary>
        /// Gets the default bundle collection.
        /// </summary>
        [Obsolete("Use AddWebOptimizer().")]
        public static BundleCollection Bundles =>
            bundles ?? throw new InvalidOperationException("Call AddWebOptimizer() first.");

        internal static void SetBundles(BundleCollection value)
        {
            if (bundles != null)
                throw new InvalidOperationException("AddWebOptimizer() should not be called twice.");

            bundles = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets whether bundling and minification is enabled.
        /// </summary>
        [Obsolete("Use webOptimizer.enableTagHelperBundling")]
        public static bool EnableOptimizations
        {
            get => enableOptimizations ?? Options.EnableTagHelperBundling ?? !HttpContext.Current.IsDebuggingEnabled;
            set => enableOptimizations = value;
        }

        private static WebOptimizerOptions Options =>
            HttpContext.Current.GetRequestServices()
                .GetRequiredService<IOptions<WebOptimizerOptions>>().Value;
    }
}
