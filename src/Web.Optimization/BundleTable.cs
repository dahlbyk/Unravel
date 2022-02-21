namespace System.Web.Optimization
{
    /// <summary>
    /// Static holder class for the default bundle collection
    /// </summary>
    public class BundleTable
    {
        private static BundleCollection bundles;

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
    }
}
