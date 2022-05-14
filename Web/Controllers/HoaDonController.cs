using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HoaDonController : Controller
    {
        private Context db = new Context();

        // GET: HoaDon
        public ActionResult index(int page = 1, int limit = 10)
        {

            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("get", "HOADON", new { httproute = "DefaultApi", limit = limit, page = page });
                var _url = _host + _api;
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<HOADON>>();
                    readTask.Wait();
                    IEnumerable<HOADON> list = null;
                    list = readTask.Result;
                    ViewBag.CurrentPage = page;
                    var o_list = new Context().HOADONs.ToList();
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

        // GET: HoaDon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hOADON = db.HOADONs.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            return View(hOADON);
        }

        // GET: HoaDon/Create
        public ActionResult Create()
        {
            ViewBag.phong = new SelectList(db.PHONGs, "maphong", "tenphong");
            return View();
        }

        // POST: HoaDon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mahd,ngayhoadon,phong,tiendiennuoc,tongtien,tienphong,tenhs,tennv,maphieudiennuoc")] HOADON hOADON)
        {
            if (ModelState.IsValid)
            {
                db.HOADONs.Add(hOADON);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.phong = new SelectList(db.PHONGs, "maphong", "tenphong", hOADON.maphong);
            return View(hOADON);
        }

        // GET: HoaDon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hOADON = db.HOADONs.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            ViewBag.phong = new SelectList(db.PHONGs, "maphong", "tenphong", hOADON.maphong);
            return View(hOADON);
        }

        // POST: HoaDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mahd,ngayhoadon,phong,tiendiennuoc,tongtien,tienphong,tenhs,tennv,maphieudiennuoc")] HOADON hOADON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOADON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.phong = new SelectList(db.PHONGs, "maphong", "tenphong", hOADON.maphong);
            return View(hOADON);
        }

        // GET: HoaDon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hOADON = db.HOADONs.Find(id);
            if (hOADON == null)
            {
                return HttpNotFound();
            }
            return View(hOADON);
        }

        // POST: HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HOADON hOADON = db.HOADONs.Find(id);
            db.HOADONs.Remove(hOADON);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
