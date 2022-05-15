using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult Details(int? id, bool? stt = null)
        {
            if (id != null)
            {
                try
                {
                    if (stt == null)
                    {
                        var e = db.HOCSINHs.FirstOrDefault(x => x.mahs == id);
                        return View(e);
                    }
                    else
                    if ((bool)stt)
                    {
                        var e = db.HOCSINH_NEW.FirstOrDefault(x => x.mahs == id);
                        return View("Detail_NEW", e);
                    }
                    else
                    {
                        var e = db.HOCSINH_OLD.FirstOrDefault(x => x.mahs == id);
                        return View("Detail_OLD", e);
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.Msg = ex.Message;
                    return View("~/Views/Shared/Error.cshtml");
                }                            
            }
            else
            {
                ViewBag.Msg = "ID không tồn tại";
                ViewBag.Code = "404";
                return View("~/Views/Shared/Error.cshtml");
            }    
        }

        public ActionResult Create(bool? stt=null)
        {
            if(stt == null)
            {
                return View();
            }
            else
                if (stt == true)
            {
                return View("Create_NEW");
            }
            else
            {
                return View("Create_OLD");
            }    
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HOCSINH e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("post", "HOCSINH", new { httproute = "DefaultApi" });
                var _url = _host + _api;

                var postTask = client.PostAsJsonAsync<HOCSINH>(_url, e);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNew(HOCSINH_NEW e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("post", "HOCSINH", new { httproute = "DefaultApi" }) + "/new/";
                var _url = _host + _api;

                var postTask = client.PostAsJsonAsync<HOCSINH_NEW>(_url, e);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new {stt="True"});
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOld(HOCSINH_OLD e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("post", "HOCSINH", new { httproute = "DefaultApi" }) + "/old/";
                var _url = _host + _api;

                var postTask = client.PostAsJsonAsync<HOCSINH_OLD>(_url, e);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { stt = "False" });
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

        [HttpGet]
        public ActionResult Edit(int? id, bool? stt=null)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _type = "/";
                if(stt == true)
                {
                    _type = "/new/";
                }
                else
                if(stt == false)
                {
                    _type = "/old/";
                }    
                var _api = Url.Action("get", "HOCSINH", new { httproute = "DefaultApi"}) + _type + id;
                var _url = _host + _api;
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    if(stt == null) {
                        var readTask = result.Content.ReadAsAsync<HOCSINH>();
                        readTask.Wait();
                        var e = readTask.Result;
                        if(e!=null)
                        return View(e);
                    }
                    else
                        if (stt == true)
                    {
                        var readTask = result.Content.ReadAsAsync<HOCSINH_NEW>();
                        readTask.Wait();
                        var e = readTask.Result;
                        if (e != null)
                            return View("Edit_NEW",e);
                    }
                    else
                    {
                        var readTask = result.Content.ReadAsAsync<HOCSINH_OLD>();
                        readTask.Wait();
                        var e = readTask.Result;
                        if (e != null)
                            return View("Edit_OLD",e);
                    }    
                }
                else
                {
                    ViewBag.Msg = result.ReasonPhrase;
                    ViewBag.Url_Error = _url;
                    ViewBag.Code = (int)result.StatusCode;
                }
            }     
            return View("~/Views/Shared/Error.cshtml");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HOCSINH e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("edit", "HOCSINH", new { httproute = "DefaultApi" });
                var _url = _host + _api;
                var responseTask = client.PutAsJsonAsync<HOCSINH>(_url, e);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Msg = result.ReasonPhrase;
                    ViewBag.Url_Error = _url;
                    ViewBag.Code = (int)result.StatusCode;
                    return RedirectToAction("Index", new { msg = ViewBag.Msg });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNew(HOCSINH_NEW e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("edit", "HOCSINH", new { httproute = "DefaultApi" })+"/new/";
                var _url = _host + _api;
                var responseTask = client.PutAsJsonAsync<HOCSINH_NEW>(_url, e);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new {stt="True"});
                }
                else
                {
                    ViewBag.Msg = result.ReasonPhrase;
                    ViewBag.Url_Error = _url;
                    ViewBag.Code = (int)result.StatusCode;
                    return RedirectToAction("Index", new { msg = ViewBag.Msg });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOld(HOCSINH_OLD e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("edit", "HOCSINH", new { httproute = "DefaultApi" })+"/old/";
                var _url = _host + _api;
                var responseTask = client.PutAsJsonAsync<HOCSINH_OLD>(_url, e);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { stt = "False" });
                }
                else
                {
                    ViewBag.Msg = result.ReasonPhrase;
                    ViewBag.Url_Error = _url;
                    ViewBag.Code = (int)result.StatusCode;
                    return RedirectToAction("Index", new { msg = ViewBag.Msg });
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id, bool? stt=null)
        {
            using (var client = new HttpClient())
            {
                var _type = "";
                if (stt == true)
                {
                    _type = "/new/";
                }
                else
                if(stt == false)
                {
                    _type = "/old/";
                }
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _url = _host + "/api/HOCSINH/DELETE/" + _type + id;
                var deleteTask = client.DeleteAsync(_url);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { stt=stt });
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