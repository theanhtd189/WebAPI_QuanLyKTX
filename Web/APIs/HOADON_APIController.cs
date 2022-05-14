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
    [RoutePrefix("api/hoadon")]
    public class HOADON_APIController : ApiController
    {
        private Context db = new Context();

        [Route("get")]
        public List<HOADON> GetList(int? page = 1, int? limit = 0)
        {
            var links = db.HOADONs.OrderBy(x => x.mahd).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<HOADON>();

            if (limit <= 0)
            {
                result = db.HOADONs.ToList();
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();

            return result;
        }

        [Route("get/{id}")]
        public HOADON GetSingle(int id)
        {
            var result = db.HOADONs.FirstOrDefault(x => x.mahd == id);
            return result;
        }

        // PUT: api/HOADONs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHOADON(int id, HOADON hOADON)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hOADON.mahd)
            {
                return BadRequest();
            }

            db.Entry(hOADON).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HOADONExists(id))
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

        // POST: api/HOADONs
        [ResponseType(typeof(HOADON))]
        public IHttpActionResult PostHOADON(HOADON hOADON)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HOADONs.Add(hOADON);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hOADON.mahd }, hOADON);
        }

        // DELETE: api/HOADONs/5
        [ResponseType(typeof(HOADON))]
        public IHttpActionResult DeleteHOADON(int id)
        {
            HOADON hOADON = db.HOADONs.Find(id);
            if (hOADON == null)
            {
                return NotFound();
            }

            db.HOADONs.Remove(hOADON);
            db.SaveChanges();

            return Ok(hOADON);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HOADONExists(int id)
        {
            return db.HOADONs.Count(e => e.mahd == id) > 0;
        }
    }
}