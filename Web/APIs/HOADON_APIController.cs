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

        [Route("edit/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Edit(HOADON e)
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
                if (!HOADONExists(e.mahd))
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


        [Route("post")]
        [ResponseType(typeof(HOADON))]
        public IHttpActionResult Create(HOADON e)
        {
            try
            {
                if (!ModelState.IsValid || e == null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.HOADONs.Add(e);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                if (HOADONExists(e.mahd))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = e.mahd }, e);
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ResponseType(typeof(HOADON))]
        public IHttpActionResult Delete(int id)
        {
            HOADON HOADON = db.HOADONs.Find(id);
            if (HOADON == null)
            {
                return NotFound();
            }

            db.HOADONs.Remove(HOADON);
            db.SaveChanges();

            return Ok(HOADON);
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