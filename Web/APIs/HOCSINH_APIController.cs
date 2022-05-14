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

        [HttpGet]
        [Route("get/new")]
        public List<HOCSINH_NEW> ChuaCoPhong(int? page = 1, int? limit = 0)
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

        [HttpGet]
        [Route("get/old")]
        public List<HOCSINH_OLD> XoaKhoiPhong(int? page = 1, int? limit = 0)
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

        [Route("get/{id:int}")]
        public HOCSINH GetSingle(int id)
        {
            var result = db.HOCSINHs.FirstOrDefault(x => x.mahs == id);
            return result;
        }

        // GET: api/HOCSINHs
        [ResponseType(typeof(HOCSINH))]
        [Route("api/hocsinhs/get/{id?}")]
        public IHttpActionResult Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                HOCSINH hOCSINH = db.HOCSINHs.FirstOrDefault(x=>x.mahs.ToString()==(id));
                if (hOCSINH == null)
                {
                    return NotFound();
                }

                return Ok(hOCSINH);
            }
            else
            {
                return NotFound();
            }    
        }

        // GET: api/HOCSINHs/5
     /*   [ResponseType(typeof(HOCSINH))]
        [Route("api/hocsinhs/get/{id}")]
        public IHttpActionResult GetHOCSINH(int id)
        {
            HOCSINH hOCSINH = db.HOCSINHs.Find(id);
            if (hOCSINH == null)
            {
                return NotFound();
            }

            return Ok(hOCSINH);
        }*/

        // PUT: api/HOCSINHs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHOCSINH(int id, HOCSINH hOCSINH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hOCSINH.mahs)
            {
                return BadRequest();
            }

            db.Entry(hOCSINH).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HOCSINHExists(id))
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

        // POST: api/HOCSINHs
        [ResponseType(typeof(HOCSINH))]
        public IHttpActionResult PostHOCSINH(HOCSINH hOCSINH)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HOCSINHs.Add(hOCSINH);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hOCSINH.mahs }, hOCSINH);
        }

        // DELETE: api/HOCSINHs/5
        [ResponseType(typeof(HOCSINH))]
        public IHttpActionResult DeleteHOCSINH(int id)
        {
            HOCSINH hOCSINH = db.HOCSINHs.Find(id);
            if (hOCSINH == null)
            {
                return NotFound();
            }

            db.HOCSINHs.Remove(hOCSINH);
            db.SaveChanges();

            return Ok(hOCSINH);
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