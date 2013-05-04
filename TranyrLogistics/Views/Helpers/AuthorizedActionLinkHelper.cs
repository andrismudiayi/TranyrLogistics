using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TranyrLogistics.Views.Helpers
{
    public static class ActionLinkHelper
    {
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName = null, object routeValues = null, object htmlAttributes = null)
        {
            if (!ControllerAuth.HasActionPermission(htmlHelper, actionName, controllerName))
            {
                return MvcHtmlString.Empty;
            }

            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString SecureActionImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            if (!ControllerAuth.HasActionPermission(htmlHelper, actionName, controllerName))
            {
                return MvcHtmlString.Empty;
            }

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