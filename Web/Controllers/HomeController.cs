using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DSHocSinh()
        {
            IEnumerable<Web.Models.HOCSINH> m = db.HOCSINHs;
            ViewBag.Title = "Danh sách học sinh";
            return View("~/Views/Home/View.cshtml",m);
        }

    }
}
