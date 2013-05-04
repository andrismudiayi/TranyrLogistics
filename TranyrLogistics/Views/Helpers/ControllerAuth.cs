using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TranyrLogistics.Views.Helpers
{
    public static class ControllerAuth
    {
        public static bool HasActionPermission(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            ControllerBase controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                ? htmlHelper.ViewContext.Controller
                : GetControllerByName(htmlHelper, controllerName);

            ControllerContext controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            ReflectedControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);

            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }

        static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            IController controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException("Controller not found during permission check.");
            }

            return (ControllerBase)controller;
        }

        static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
            {
                return false;
            }

            AuthorizationContext authContext = new AuthorizationContext(controllerContext, actionDescriptor);
            foreach (Filter authFilter in FilterProviders.Providers.GetFilters(authContext, actionDescriptor))
            {
                if (authFilter.Instance is System.Web.Mvc.AuthorizeAttribute)
                {
                    ((IAuthorizationFilter)authFilter.Instance).OnAuthorization(authContext);

                    if (authContext.Result != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}