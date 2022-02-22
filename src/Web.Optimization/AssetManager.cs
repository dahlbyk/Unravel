using System.Collections.Generic;
using System.Text;

namespace System.Web.Optimization
{
    public class AssetManager
    {
        public virtual IHtmlString RenderExplicit(string tagFormat, string[] paths)
        {
            var pathsToRender = DeterminePathsToRender(paths);

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

        private IEnumerable<string> DeterminePathsToRender(IEnumerable<string> paths)
        {
            // TODO: Get from IAssetPipeline
            foreach (var path in paths)
                yield return Normalize(path);
        }

        private static string Normalize(string path) =>
            path?.StartsWith("~/") == true ? path.Substring(1) : path;
    }
}
