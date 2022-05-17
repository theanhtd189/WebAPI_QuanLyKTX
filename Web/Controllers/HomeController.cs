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

        [CheckUserSession]
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

        [HttpGet,Route("LogIn")]
        public ActionResult Login() {
            try
            {
                return View("Login");
            }
            catch (Exception x)
            {
                ViewBag.Msg = x.Message;
                return View("~/View/Shared/Error.cshtml");
            }
            
        }
        [HttpPost,Route("Login")]
        public ActionResult Login(string email, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    using (HttpClient client=new HttpClient())
                    {
                        var host = Request.Url.Scheme +"://"+ Request.Url.Authority ;
                        var api = host + Url.Action("GetAccount","TaiKhoan",new { httproute= "DefaultApi", email=email, password=password });
                        var task = client.GetAsync(api);
                        task.Wait();

                        var result = task.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var content = result.Content.ReadAsAsync<TAIKHOAN>();
                            content.Wait();
                            var data = content.Result;
                            if (data != null)
                            {
                                Session["user_id"] = data.matk;
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ViewBag.Msg = "Tài khoản hoặc mật khẩu không đúng!";
                            }         
                        }
                        else
                        {
                            ViewBag.Msg = "Tài khoản hoặc mật khẩu không đúng!";
                            
                        }
                        return View("LogIn");
                    }
                }
                else
                {
                    ViewBag.Msg = "Tham số không tồn tại!";
                    return View("~/View/Shared/Error.cshtml");
                }    
                
            }
            catch (Exception x)
            {
                ViewBag.Msg = x.Message;
                return View("~/View/Shared/Error.cshtml");
            }

        }

        [HttpGet,Route("LogOut")]
        public ActionResult Logout() {
            if (Session["user_id"]!=null)
            {
                Session.Clear();               
            }
            return RedirectToAction("Index");
        }

        [Route("SignUp")]
        public ActionResult Signup() {
            return View();
        }

        [CheckUserSession]
        [HttpGet,Route("ThongTinTaiKhoan")]
        public ActionResult ThongTinTaiKhoan()
        {
            var _id = int.Parse(Session["user_id"].ToString());
            var e = db.TAIKHOANs.FirstOrDefault(x=>x.matk==_id);
            if (e != null)
            {
                return View(e);
            }
            else
            {
                ViewBag.Msg = "Session không hợp lệ!";
                return View("~/Shared/Error.cshtml");
            }    
        }

        [CheckUserSession]
        [HttpPost, Route("ThongTinTaiKhoan")]
        public ActionResult ThongTinTaiKhoan(TAIKHOAN e)
        {
            if (e != null)
            {
                var o = db.TAIKHOANs.FirstOrDefault(x=>x.matk==e.matk);
                if (e != null)
                {
                    if (e.hoten != o.hoten)
                        o.hoten = e.hoten;
                    if(o.email != e.email)
                        o.email = e.email;
                    if(!string.IsNullOrEmpty(e.pass) && o.pass != e.pass)
                        o.pass = e.pass;
                    if(o.cvu!=e.cvu)
                        o.cvu = e.cvu;
                    var stt = db.SaveChanges();
                    if (stt > 0)
                        ViewBag.Msg = "Lưu thành công!";
                    else
                        ViewBag.Msg = "Lỗi!";
                }
            }
            else
            {
                ViewBag.Msg = "Tham số không hợp lệ";
            }    
            return View();
        }

    }
}
