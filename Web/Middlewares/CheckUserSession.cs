using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckUserSessionAttribute : ActionFilterAttribute
    {
        public static String LoginUrl { get; set; }
        public delegate bool CheckSessionDelegate(HttpSessionStateBase session);

        public static CheckSessionDelegate CheckSessionAlive;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if ((CheckSessionAlive == null) || (CheckSessionAlive(session)==false))
            {
                
                var url = new UrlHelper(filterContext.RequestContext);
                var loginUrl = url.Content(LoginUrl);
                session.RemoveAll();
                session.Clear();
                session.Abandon();

                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.HttpContext.Response.Redirect(loginUrl, false);
                filterContext.Result = new EmptyResult();
            }
            else
            {
                return;
            }    
        }
    }
}