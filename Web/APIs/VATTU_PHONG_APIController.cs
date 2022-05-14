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

namespace Web.APIs
{
    public class VATTU_PHONG_APIController : ApiController
    {
        private Context db = new Context();

        // GET: api/VATTU_PHONG
        public IQueryable<VATTU_PHONG> GetVATTU_PHONG()
        {
            return db.VATTU_PHONG;
        }

        // GET: api/VATTU_PHONG/5
        [ResponseType(typeof(VATTU_PHONG))]
        public IHttpActionResult GetVATTU_PHONG(int id)
        {
            VATTU_PHONG vATTU_PHONG = db.VATTU_PHONG.Find(id);
            if (vATTU_PHONG == null)
            {
                return NotFound();
            }

            return Ok(vATTU_PHONG);
        }

        // PUT: api/VATTU_PHONG/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVATTU_PHONG(int id, VATTU_PHONG vATTU_PHONG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vATTU_PHONG.ID)
            {
                return BadRequest();
            }

            db.Entry(vATTU_PHONG).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VATTU_PHONGExists(id))
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

        // POST: api/VATTU_PHONG
        [ResponseType(typeof(VATTU_PHONG))]
        public IHttpActionResult PostVATTU_PHONG(VATTU_PHONG vATTU_PHONG)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VATTU_PHONG.Add(vATTU_PHONG);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VATTU_PHONGExists(vATTU_PHONG.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vATTU_PHONG.ID }, vATTU_PHONG);
        }

        // DELETE: api/VATTU_PHONG/5
        [ResponseType(typeof(VATTU_PHONG))]
        public IHttpActionResult DeleteVATTU_PHONG(int id)
        {
            VATTU_PHONG vATTU_PHONG = db.VATTU_PHONG.Find(id);
            if (vATTU_PHONG == null)
            {
                return NotFound();
            }

            db.VATTU_PHONG.Remove(vATTU_PHONG);
            db.SaveChanges();

            return Ok(vATTU_PHONG);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VATTU_PHONGExists(int id)
        {
            return db.VATTU_PHONG.Count(e => e.ID == id) > 0;
        }
    }
}