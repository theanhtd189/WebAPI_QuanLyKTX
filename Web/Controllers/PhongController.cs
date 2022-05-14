using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class PhongController : Controller
    {
        // GET: Phong
        private Context db = new Context();

        public ActionResult Index(int page = 1, int limit = 10)
        {

            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("get", "phong", new { httproute = "DefaultApi", limit = limit, page = page });
                var _url = _host + _api;
                // client.BaseAddress = new Uri(_url);              
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PHONG>>();
                    readTask.Wait();
                    IEnumerable<PHONG> list = null;
                    list = readTask.Result;
                    ViewBag.CurrentPage = page;
                    var o_list = new Context().PHONGs.ToList();
                    ViewBag.TotalPage = Math.Ceiling((float)o_list.Count / 10);
                    ViewBag.TotalPage_List = o_list;
                    ViewBag.I = 1;
                    if (ViewBag.CurrentPage > 1)
                    {
                        ViewBag.Stt = (ViewBag.CurrentPage - 1) * 10 + 1; //số thứ tự tiếp theo 
                    }
                    return View(list.ToList());
                }
                else
                {
                    ViewBag.Msg = result.ReasonPhrase;
                    ViewBag.Url_Error = _url;
                    ViewBag.Code = (int)result.StatusCode;
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
        }
    }
}