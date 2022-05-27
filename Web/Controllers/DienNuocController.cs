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
using Web.Middlewares;
using System.Text;
using OfficeOpenXml;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Web.Controllers
{
    public class DienNuocController : Controller
    {
        private Context db = new Context();

        // xuất file
        //public ActionResult Csv()
        //{
        //    var p_list = new Context().PHIEU_DIENNUOC.ToList();
        //    var builder = new StringBuilder();
        //    builder.AppendLine("Mã phiếu,Mã phòng,Ngày tạo phiếu,Số điện,Giá điện,Số nước,Giá nước,Tổng tiền");
        //    foreach (var phieu in p_list)
        //    {
        //        builder.AppendLine($"{phieu.maphieu},{phieu.maphong},{phieu.ngaytaophieu},{phieu.sodien},{phieu.giadien},{phieu.sonuoc},{phieu.gianuoc},{phieu.tongtien}");
        //    }

        //    return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");
        //}

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

                var p_list = db.PHIEU_DIENNUOC.ToList();

                ExcelPackage Ep = new ExcelPackage();
                ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
                Sheet.Cells["A1"].Value = "Mã phiếu";
                Sheet.Cells["B1"].Value = "Mã phòng";
                Sheet.Cells["C1"].Value = "Ngày tạo phiếu";
                Sheet.Cells["D1"].Value = "Số điện";
                Sheet.Cells["E1"].Value = "Giá điện";
                Sheet.Cells["C1"].Value = "Số nước";
                Sheet.Cells["D1"].Value = "Giá nước";
                Sheet.Cells["E1"].Value = "Tổng tiền";
                int row = 2;
                foreach (var item in p_list)
                {

                    Sheet.Cells[string.Format("A{0}", row)].Value = item.maphieu;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.maphong;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.ngaytaophieu;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.sodien;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.giadien;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.sonuoc;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.gianuoc;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.tongtien;
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
       

        // GET: DienNuoc
        [CheckUserSession]
        public ActionResult Index(int page = 1, int limit = 10)
        {

            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("get", "diennuoc", new { httproute = "DefaultApi", limit = limit, page = page });
                var _url = _host + _api;
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PHIEU_DIENNUOC>>();
                    readTask.Wait();
                    IEnumerable<PHIEU_DIENNUOC> list = null;
                    list = readTask.Result;
                    ViewBag.CurrentPage = page;
                    var o_list = new Context().PHIEU_DIENNUOC.ToList();
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

        [CheckUserSession]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEU_DIENNUOC e = db.PHIEU_DIENNUOC.Find(id);
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
        public ActionResult Create(PHIEU_DIENNUOC e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("post", "DIENNUOC", new { httproute = "DefaultApi" });
                var _url = _host + _api;

                var postTask = client.PostAsJsonAsync<PHIEU_DIENNUOC>(_url, e);
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
                var _api = Url.Action("get", "DIENNUOC", new { httproute = "DefaultApi", id = id });
                var _url = _host + _api;
                var responseTask = client.GetAsync(_url);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PHIEU_DIENNUOC>();
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
        public ActionResult Edit(PHIEU_DIENNUOC e)
        {
            using (var client = new HttpClient())
            {
                var _host = Request.Url.Scheme + "://" + Request.Url.Authority;
                var _api = Url.Action("edit", "DIENNUOC", new { httproute = "DefaultApi" });
                var _url = _host + _api;
                var responseTask = client.PutAsJsonAsync<PHIEU_DIENNUOC>(_url, e);
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
                var _api = Url.Action("delete", "DIENNUOC", new { httproute = "DefaultApi", id = id });
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
