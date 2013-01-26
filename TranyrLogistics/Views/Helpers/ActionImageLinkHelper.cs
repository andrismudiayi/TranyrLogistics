using System.Collections.Generic;
using System.Web.Mvc;

namespace TranyrLogistics.Views.Helpers
{
    public static class ActionImageLinkHelper
    {
        public static MvcHtmlString ActionImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imgSrc);
            imgTag.MergeAttributes((IDictionary<string, string>)imgHtmlAttributes, true);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgTag.ToString();
            imglink.MergeAttributes((IDictionary<string, string>)htmlAttributes, true);

            return new MvcHtmlString(imglink.ToString());
        }
    }
}