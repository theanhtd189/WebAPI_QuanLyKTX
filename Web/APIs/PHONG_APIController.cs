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
    [RoutePrefix("api/phong")]
    public class PHONG_APIController : ApiController
    {

        private Context db = new Context();


        [Route("get")]
        public List<PHONG> GetList(int? page=1, int? limit = 0)
        {
            var links = db.PHONGs.OrderBy(x => x.maphong).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<PHONG>();

            if(limit<=0)
            {
                result = db.PHONGs.ToList();
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();               

            return result;
        }

        [Route("get/{id}")]
        public PHONG GetSingle(int id)
        {
            var result = db.PHONGs.FirstOrDefault(x=>x.maphong==id);
            return result;
        }


        // GET: api/PHONGs/5
        [ResponseType(typeof(PHONG))]
        public IHttpActionResult GetPHONG(int id)
        {
            PHONG pHONG = db.PHONGs.Find(id);
            if (pHONG == null)
            {
                return NotFound();
            }

            return Ok(pHONG);
        }

        // PUT: api/PHONGs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPHONG(int id, PHONG pHONG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pHONG.maphong)
            {
                return BadRequest();
            }

            db.Entry(pHONG).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PHONGExists(id))
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

        // POST: api/PHONGs
        [ResponseType(typeof(PHONG))]
        public IHttpActionResult PostPHONG(PHONG pHONG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PHONGs.Add(pHONG);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pHONG.maphong }, pHONG);
        }

        // DELETE: api/PHONGs/5
        [ResponseType(typeof(PHONG))]
        public IHttpActionResult DeletePHONG(int id)
        {
            PHONG pHONG = db.PHONGs.Find(id);
            if (pHONG == null)
            {
                return NotFound();
            }

            db.PHONGs.Remove(pHONG);
            db.SaveChanges();

            return Ok(pHONG);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PHONGExists(int id)
        {
            return db.PHONGs.Count(e => e.maphong == id) > 0;
        }
    }
}