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
    [RoutePrefix("api/taikhoan")]
    public class TAIKHOAN_APIController : ApiController
    {
        private Context db = new Context();

        [Route("get")]
        public List<TAIKHOAN> GetList(int? page = 1, int? limit = 0)
        {
            var links = db.TAIKHOANs.OrderBy(x => x.matk).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<TAIKHOAN>();

            if (limit <= 0)
            {
                result = db.TAIKHOANs.ToList();
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();

            return result;
        }

        [Route("get/{id}")]
        public TAIKHOAN GetSingle(int id)
        {
            var result = db.TAIKHOANs.FirstOrDefault(x => x.matk == id);
            return result;
        }      

        // PUT: api/TAIKHOAN_API/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTAIKHOAN(int id, TAIKHOAN tAIKHOAN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tAIKHOAN.matk)
            {
                return BadRequest();
            }

            db.Entry(tAIKHOAN).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TAIKHOANExists(id))
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

        // POST: api/TAIKHOAN_API
        [ResponseType(typeof(TAIKHOAN))]
        public IHttpActionResult PostTAIKHOAN(TAIKHOAN tAIKHOAN)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TAIKHOANs.Add(tAIKHOAN);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tAIKHOAN.matk }, tAIKHOAN);
        }

        // DELETE: api/TAIKHOAN_API/5
        [ResponseType(typeof(TAIKHOAN))]
        public IHttpActionResult DeleteTAIKHOAN(int id)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return NotFound();
            }

            db.TAIKHOANs.Remove(tAIKHOAN);
            db.SaveChanges();

            return Ok(tAIKHOAN);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TAIKHOANExists(int id)
        {
            return db.TAIKHOANs.Count(e => e.matk == id) > 0;
        }
    }
}