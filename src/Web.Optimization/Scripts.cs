using Microsoft.Extensions.DependencyInjection;

namespace System.Web.Optimization
{
    /// <summary>
    /// Helper class for rendering script elements.
    /// </summary>
    public static class Scripts
    {
        private static AssetManager Manager =>
            Unravel.Application.Services.GetRequiredService<AssetManager>();

        /// <summary>
        /// Default format string for defining how link tags are rendered.
        /// </summary>
        public static string DefaultTagFormat { get; set; } = @"<script src=""{0}""></script>";

        /// <summary>
        /// Renders link tags for a set of paths.
        /// </summary>
        /// <param name="paths">Set of virtual paths for which to generate link tags.</param>
        /// <returns>HTML string containing the link tag or tags for the bundle.</returns>
        /// <remarks>
        /// Render generates multiple link tags for each item in the bundle when
        /// <see cref="BundleTable.EnableOptimizations" /> is set to false. When optimizations are enabled,
        /// Render generates a single link tag to a version-stamped URL which represents the entire bundle.
        /// </remarks>
        public static IHtmlString Render(params string[] paths)
        {
            return RenderFormat(DefaultTagFormat, paths);
        }

        /// <summary>
        /// Renders link tags for a set of paths based on a format string.
        /// </summary>
        /// <param name="tagFormat">Format string for defining the rendered link tags. For more
        /// details on format strings, see http://msdn.microsoft.com/en-us/library/txafckwd.aspx</param>
        /// <param name="paths">Set of virtual paths for which to generate link tags.</param>
        /// <returns>HTML string containing the link tag or tags for the bundle.</returns>
        /// <remarks>
        /// RenderFormat generates link tags for the supplied paths using the specified format string. It
        /// generates multiple link tags for each item in the bundle when
        /// <see cref="BundleTable.EnableOptimizations" /> is set to false. When optimizations are enabled,
        /// it generates a single link tag to a version-stamped URL which represents the entire bundle.
        /// </remarks>
        public static IHtmlString RenderFormat(string tagFormat, params string[] paths)
        {
            if (String.IsNullOrEmpty(tagFormat))
            {
                throw ExceptionUtil.ParameterNullOrEmpty(nameof(tagFormat));
            }
            if (paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
            }
            foreach (string path in paths)
            {
                if (String.IsNullOrEmpty(path))
                {
                    throw ExceptionUtil.ParameterNullOrEmpty(nameof(paths));
                }
            }

            return Manager.RenderExplicit(tagFormat, paths);
        }

        /// <summary>
        /// Generates a version-stamped url for a bundle
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        /// <remarks>If the supplied virtual path identifies a bundle, a version-stamped url is returned. If
        /// the virutal path is not a bundle, the resolved virutal path url is returned.
        /// </remarks>
        public static IHtmlString Url(string virtualPath)
        {
            return Manager.ResolveUrl(virtualPath);
        }
    }
}
