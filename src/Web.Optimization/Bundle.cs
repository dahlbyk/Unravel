using System;
using System.Collections.Generic;

namespace System.Web.Optimization
{
    public abstract class Bundle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bundle"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="Bundle"/> from within a view or Web page.</param>
        /// <remarks>Constructor is <c>internal</c> because anything clever should be migrated directly to WebOptimizer.</remarks>
        internal Bundle(string virtualPath, string contentType)
        {
            Path = virtualPath;
            ContentType = contentType;
        }

        /// <summary>
        /// Virtual path used to reference the <see cref="Bundle"/> from within a view or Web page.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Content type of the bundle.
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// Files or file patterns to be included in the bundle.
        /// </summary>
        public List<string> Files { get; } = new List<string>();

        /// <summary>
        /// Specifies a set of files to be included in the <see cref="Bundle"/>.
        /// </summary>
        /// <param name="virtualPaths">The virtual path of the file or file pattern to be included in the bundle.</param>
        /// <returns>The <see cref="Bundle"/> object itself for use in subsequent method chaining.</returns>
        public virtual Bundle Include(params string[] virtualPaths)
        {
            if (virtualPaths == null)
                throw new ArgumentNullException(nameof(virtualPaths));

            foreach (var path in virtualPaths)
            {
                var normalizedPath = path;
                if (path?.StartsWith("~/") == true)
                    normalizedPath = path.Substring(1);

                Files.Add(normalizedPath);
            }

            return this;
        }
    }
}
