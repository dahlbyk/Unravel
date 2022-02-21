using System;
using WebOptimizer;

namespace System.Web.Optimization
{
    /// <summary>
    /// Bundle designed specifically for processing JavaScript
    /// </summary>
    public class ScriptBundle : Bundle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="ScriptBundle"/> from within a view or Web page.</param>
        public ScriptBundle(string virtualPath)
            : base(virtualPath, "text/javascript; charset=UTF-8")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="ScriptBundle"/> from within a view or Web page.</param>
        /// <param name="cdnPath">Ignored. Use <see cref="WebOptimizerOptions.CdnUrl"/>.</param>
        [Obsolete("cdnPath is ignored; use webOptimizer.cdnUrl.")]
        public ScriptBundle(string virtualPath, string cdnPath)
            : this(virtualPath)
        {
        }
    }
}
