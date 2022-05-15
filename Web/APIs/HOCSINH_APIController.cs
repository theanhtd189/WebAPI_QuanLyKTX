using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web.Models;
using PagedList;

namespace Web.APIs
{
    [RoutePrefix("api/hocsinh")]

    public class HOCSINH_APIController : ApiController
    {
        private Context db = new Context();

        [AcceptVerbs("GET", "POST")]
        [Route("get/in"),Route("get")]
        public List<HOCSINH> CoPhong(int? page = 1, int? limit = 0)
        {
            var source = db.HOCSINHs.ToList();
            var links = source.OrderBy(x => x.mahs).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<HOCSINH>();

            if (limit <= 0)
            {
                result = links;
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();

            return result;
        }

        [Route("get/{id:int}")]
        public HOCSINH GetSingle(int id)
        {
            var result = db.HOCSINHs.FirstOrDefault(x => x.mahs == id);
            return result;
        }

        [HttpGet]
        [Route("get/new")]
        public List<HOCSINH_NEW> Get(int? page = 1, int? limit = 0)
        {
            var source = db.HOCSINH_NEW.ToList();
            var links = source.OrderBy(x => x.mahs).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<HOCSINH_NEW>();

            if (limit <= 0)
            {
                result = links;
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();

            return result;
        }

        [Route("get/new/{id:int}")]
        public HOCSINH_NEW GetNew(int id)
        {
            var result = db.HOCSINH_NEW.FirstOrDefault(x => x.mahs == id);
            return result;
        }

        [HttpGet]
        [Route("get/old")]
        public List<HOCSINH_OLD> GetOld(int? page = 1, int? limit = 0)
        {
            var source = db.HOCSINH_OLD.ToList();
            var links = source.OrderBy(x => x.mahs).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<HOCSINH_OLD>();

            if (limit <= 0)
            {
                result = links;
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();

            return result;
        }

        [Route("get/old/{id:int}")]
        public HOCSINH_OLD GetOld(int id)
        {
            var result = db.HOCSINH_OLD.FirstOrDefault(x => x.mahs == id);
            return result;
        }      

        [Route("edit/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Edit(HOCSINH e)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(e).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HOCSINHExists(e.mahs))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("edit/new")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult EditNew(HOCSINH_NEW e)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(e).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException x)
            {
                throw x;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("edit/old")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult EditOld(HOCSINH_OLD e)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(e).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException x)
            {
                throw x;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("post")]
        [HttpPost]
        [ResponseType(typeof(HOCSINH))]
        public IHttpActionResult Create(HOCSINH e)
        {
            try
            {
                if (!ModelState.IsValid || e == null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.HOCSINHs.Add(e);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }

            return CreatedAtRoute("DefaultApi", new { id = e.mahs }, e);
        }

        [Route("post/new")]
        [HttpPost]
        [ResponseType(typeof(HOCSINH_NEW))]
        public IHttpActionResult CreateNew(HOCSINH_NEW e)
        {
            try
            {
                if (!ModelState.IsValid || e == null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.HOCSINH_NEW.Add(e);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }

            return CreatedAtRoute("DefaultApi", new { id = e.mahs }, e);
        }

        [Route("post/old")]
        [HttpPost]
        [ResponseType(typeof(HOCSINH_OLD))]
        public IHttpActionResult CreateOld(HOCSINH_OLD e)
        {
            try
            {
                if (!ModelState.IsValid || e == null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.HOCSINH_OLD.Add(e);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }

            return CreatedAtRoute("DefaultApi", new { id = e.mahs }, e);
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            HOCSINH e = db.HOCSINHs.Find(id);
            if (e == null)
            {
                return NotFound();
            }

            db.HOCSINHs.Remove(e);
            db.SaveChanges();

            return Ok(e);
        }

        [Route("delete/new/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteNew(int id)
        {
            HOCSINH_NEW e = db.HOCSINH_NEW.Find(id);
            if (e == null)
            {
                return NotFound();
            }

            db.HOCSINH_NEW.Remove(e);
            db.SaveChanges();

            return Ok(e);
        }

        [Route("delete/old/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOld(int id)
        {
            HOCSINH_OLD e = db.HOCSINH_OLD.Find(id);
            if (e == null)
            {
                return NotFound();
            }

            db.HOCSINH_OLD.Remove(e);
            db.SaveChanges();

            return Ok(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HOCSINHExists(int id)
        {
            return db.HOCSINHs.Count(e => e.mahs == id) > 0;
        }
    }
}