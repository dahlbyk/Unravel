using System;
using WebOptimizer;

namespace System.Web.Optimization
{
    /// <summary>
    /// Bundle designed specifically for processing cascading stylesheets (CSS)
    /// </summary>
    public class StyleBundle : Bundle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="StyleBundle"/> from within a view or Web page.</param>
        public StyleBundle(string virtualPath)
            : base(virtualPath, "text/css; charset=UTF-8")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="StyleBundle"/> from within a view or Web page.</param>
        /// <param name="cdnPath">Ignored. Use <see cref="WebOptimizerOptions.CdnUrl"/>.</param>
        [Obsolete("cdnPath is ignored; use webOptimizer.cdnUrl.")]
        public StyleBundle(string virtualPath, string cdnPath)
            : this(virtualPath)
        {
        }
    }
}
