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
    [RoutePrefix("api/diennuoc")]
    public class PHIEU_DIENNUOC_APIController : ApiController
    {
        private Context db = new Context();

        [Route("get")]
        public List<PHIEU_DIENNUOC> GetList(int? page = 1, int? limit = 0)
        {
            var links = db.PHIEU_DIENNUOC.OrderBy(x => x.maphieu).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<PHIEU_DIENNUOC>();

            if (limit <= 0)
            {
                result = db.PHIEU_DIENNUOC.ToList();
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();

            return result;
        }

        [Route("get/{id}")]
        public PHIEU_DIENNUOC GetSingle(int id)
        {
            var result = db.PHIEU_DIENNUOC.FirstOrDefault(x => x.maphieu == id);
            return result;
        }

        [Route("edit/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Edit(PHIEU_DIENNUOC e)
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
                if (!PHIEU_DIENNUOCExists(e.maphieu))
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
        [ResponseType(typeof(PHIEU_DIENNUOC))]
        public IHttpActionResult Create(PHIEU_DIENNUOC e)
        {
            try
            {
                if (!ModelState.IsValid || e == null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.PHIEU_DIENNUOC.Add(e);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
                if (PHIEU_DIENNUOCExists(e.maphieu))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = e.maphieu }, e);
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ResponseType(typeof(PHIEU_DIENNUOC))]
        public IHttpActionResult Delete(int id)
        {
            PHIEU_DIENNUOC PHIEU_DIENNUOC = db.PHIEU_DIENNUOC.Find(id);
            if (PHIEU_DIENNUOC == null)
            {
                return NotFound();
            }

            db.PHIEU_DIENNUOC.Remove(PHIEU_DIENNUOC);
            db.SaveChanges();

            return Ok(PHIEU_DIENNUOC);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PHIEU_DIENNUOCExists(int id)
        {
            return db.PHIEU_DIENNUOC.Count(e => e.maphieu == id) > 0;
        }
    }
}