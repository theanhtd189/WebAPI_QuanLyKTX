using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Middlewares;
using Web.Models;

namespace Web.Controllers
{
    public class HoaDonController : Controller
    {
        private Context db = new Context();

        public ActionResult DownloadExcel()
        {

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "Qawithexperts";
                excelPackage.Workbook.Properties.Title = "test Excel";
                excelPackage.Workbook.Properties.Subject = "Write in Excel";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                var p_list = db.HOADONs.ToList();

                ExcelPackage Ep = new ExcelPackage();
                ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
                Sheet.Cells["A1"].Value = "Mã hóa đơn";
                Sheet.Cells["B1"].Value = "Mã phiếu điện nước";
                Sheet.Cells["C1"].Value = "Ngày tạo hóa đơn";
                Sheet.Cells["D1"].Value = "Tiền điện nước";
                Sheet.Cells["E1"].Value = "Tiền phòng";
                Sheet.Cells["C1"].Value = "Tổng tiền";
                Sheet.Cells["D1"].Value = "Mã Phòng";
                Sheet.Cells["E1"].Value = "Tên học sinh thanh toán hóa đơn";
                Sheet.Cells["E1"].Value = "Tên nhân viên";
                int row = 2;
                foreach (var item in p_list)
                {

                    Sheet.Cells[string.Format("A{0}", row)].Value = item.mahd;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.maphieudiennuoc;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.ngaytao;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.tiendiennuoc;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.tienphong;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.tongtien;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.maphong;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.tenhs;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.tennv;
                    row++;
                }


                Sheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
                Response.BinaryWrite(Ep.GetAsByteArray());
                Response.End();

                return View("XuatExcel");
            }
        }

        // GET: HoaDon
        [CheckUserSession]
        public ActionResult Index(int page = 1, int limit = 10)
        {

            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("get", "HOADON", new { httproute = "DefaultApi", limit = limit, page = page });
                var _url = _host + _api;
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();
                //fetch
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

        // GET: HoaDon/Details/5
        [CheckUserSession]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON e = db.HOADONs.Find(id);
            if (e == null)
            {
                return HttpNotFound();
            }
            return View(e);
        }

        [CheckUserSession]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HOADON e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("post", "HOADON", new { httproute = "DefaultApi" });
                var _url = _host + _api;

                var postTask = client.PostAsJsonAsync<HOADON>(_url, e);
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

        [CheckUserSession]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("get", "HOADON", new { httproute = "DefaultApi", id = id });
                var _url = _host + _api;
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<HOADON>();
                    readTask.Wait();
                    var e = readTask.Result;
                    return View(e);
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
        public ActionResult Edit(HOADON e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("edit", "HOADON", new { httproute = "DefaultApi" });
                var _url = _host + _api;
                var responseTask = client.PutAsJsonAsync<HOADON>(_url, e);
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

        [CheckUserSession]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("delete", "HOADON", new { httproute = "DefaultApi", id = id });
                var _url = _host + _api;
                var deleteTask = client.DeleteAsync(_url);
                deleteTask.Wait();

                var result = deleteTask.Result;
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
    }
}
