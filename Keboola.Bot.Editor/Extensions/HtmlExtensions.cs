using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Keboola.Bot.Editor.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString NewLine2Br(this HtmlHelper htmlHelper, string text)
        {
            if (string.IsNullOrEmpty(text))
                return MvcHtmlString.Create(text);
            var builder = new StringBuilder();
            var lines = text.Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                    builder.Append("<br/>\n");
                builder.Append(HttpUtility.HtmlEncode(lines[i]));
            }
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}