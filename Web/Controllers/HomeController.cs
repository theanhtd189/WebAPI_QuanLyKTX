using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Middlewares;
using Web.Models;

namespace Web.Controllers
{
    [HandleError]
    
    public class HomeController : Controller
    {
        private Context db = new Context();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("Error")]
        public ActionResult Error(string msg)
        {
            Session["Msg"] = ViewBag.Msg = msg;
            return View("~/View/Shared/Error.cshtml");
        }

        [HttpGet,Route("Account/LogIn")]
        public ActionResult Login() {
            Session["user"] = "ta1o9er";
            return RedirectToAction("Index");
        }

        [HttpGet,Route("Account/LogOut")]
        public ActionResult Logout() {
            return View();
        }

        [Route("Account/SignUp")]
        public ActionResult Signup() {
            return View();
        }

    }
}
