using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HocSinhController : Controller
    {
        // GET: HocSinh
        private Context db = new Context();


        /// <summary>
        /// Stt của học sinh: null(đã có phòng) - true(học sinh mới) - false(rời phòng)
        /// Mặc định show list học sinh đã có phòng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="stt"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int limit = 10, bool? stt=null) 
        {

            using (var client = new HttpClient())
            {
                string _stt = "/in";
                if (stt != null)
                {
                    if ((bool)stt==true)
                    {
                        _stt = "/new";
                    }                       
                    else
                        _stt = "/old";
                }    
                   
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("get"+_stt, "hocsinh", new { httproute = "DefaultApi", limit = limit, page = page });
                var _url = _host + _api;
                // client.BaseAddress = new Uri(_url);              
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<HOCSINH>>();
                    readTask.Wait();
                    IEnumerable<HOCSINH> list = null;
                    list = readTask.Result;
                    ViewBag.CurrentPage = page;
                    var total = 1;
                    if (stt == null)
                    {
                        total = (int)Math.Ceiling((float)db.HOCSINHs.ToList().Count / 10);
                    }
                    else
                    if (stt == true)
                    {

                        total = (int)Math.Ceiling((float)db.HOCSINH_NEW.ToList().Count / 10);
                    }
                    else
                    {
                        total = (int)Math.Ceiling((float)db.HOCSINH_OLD.ToList().Count / 10);
                    }    
                    ViewBag.TotalPage = total;
                    ViewBag.I = 1;
                    ViewBag.Stt = stt;
                    if (ViewBag.CurrentPage > 1)
                    {
                        ViewBag.I = (ViewBag.CurrentPage - 1) * 10 + 1; //số thứ tự tiếp theo 
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