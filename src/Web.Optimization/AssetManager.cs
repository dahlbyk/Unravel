using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using WebOptimizer;

namespace System.Web.Optimization
{
    public class AssetManager
    {
        private readonly IAssetPipeline assetPipeline;

        public AssetManager(IAssetPipeline assetPipeline)
        {
            this.assetPipeline = assetPipeline;
        }

        public virtual IHtmlString RenderExplicit(string tagFormat, string[] paths)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var enableBundling = BundleTable.EnableOptimizations;
#pragma warning restore CS0618 // Type or member is obsolete

            var pathsToRender = DeterminePathsToRender(paths, enableBundling);

            var sb = new StringBuilder();

            foreach (var path in pathsToRender)
            {
                sb.AppendFormat(tagFormat, path);
                sb.AppendLine();
            }

            return new HtmlString(sb.ToString());
        }

        public virtual IHtmlString ResolveUrl(string virtualPath)
        {
            return new HtmlString(Normalize(virtualPath));
        }

        private IEnumerable<string> DeterminePathsToRender(IEnumerable<string> paths, bool enableBundling)
        {
            var httpContext = HttpContext.Current.GetRequestServices().GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>()?.HttpContext;

            // TODO: Get from IAssetPipeline
            foreach (var path in paths)
                yield return Normalize(path);
        }

        private static string Normalize(string path) =>
            path?.StartsWith("~/") == true ? path.Substring(1) : path;
    }
}
