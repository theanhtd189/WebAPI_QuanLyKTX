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
    [RoutePrefix("api/vattu")]
    public class VATTU_APIController : ApiController
    {
        private Context db = new Context();

        [Route("get")]
        public List<VATTU> GetList(int? page = 1, int? limit = 0)
        {
            var links = db.VATTUs.OrderBy(x => x.mavt).ToList();
            int pageNumber = (page ?? 1);
            var result = new List<VATTU>();

            if (limit <= 0)
            {
                result = db.VATTUs.ToList();
            }
            else
                result = links.ToPagedList(pageNumber, (int)limit).ToList();

            return result;
        }

        [Route("get/{id}")]
        public VATTU GetSingle(int id)
        {
            var result = db.VATTUs.FirstOrDefault(x => x.mavt == id);
            return result;
        }

        // GET: api/VATTU_API/5
        [ResponseType(typeof(VATTU))]
        public IHttpActionResult GetVATTU(int id)
        {
            VATTU vATTU = db.VATTUs.Find(id);
            if (vATTU == null)
            {
                return NotFound();
            }

            return Ok(vATTU);
        }

        // PUT: api/VATTU_API/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVATTU(int id, VATTU vATTU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vATTU.mavt)
            {
                return BadRequest();
            }

            db.Entry(vATTU).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VATTUExists(id))
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

        // POST: api/VATTU_API
        [ResponseType(typeof(VATTU))]
        public IHttpActionResult PostVATTU(VATTU vATTU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VATTUs.Add(vATTU);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vATTU.mavt }, vATTU);
        }

        // DELETE: api/VATTU_API/5
        [ResponseType(typeof(VATTU))]
        public IHttpActionResult DeleteVATTU(int id)
        {
            VATTU vATTU = db.VATTUs.Find(id);
            if (vATTU == null)
            {
                return NotFound();
            }

            db.VATTUs.Remove(vATTU);
            db.SaveChanges();

            return Ok(vATTU);
        }
        [Route("edit/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Edit(VATTU e)
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
                if (!VATTUExists(e.mavt))
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
        [ResponseType(typeof(VATTU))]
        public IHttpActionResult Create(VATTU e)
        {
            try
            {
                if (!ModelState.IsValid || e == null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.VATTUs.Add(e);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
                if (VATTUExists(e.mavt))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = e.mavt }, e);
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ResponseType(typeof(VATTU))]
        public IHttpActionResult Delete(int id)
        {
            VATTU VATTU = db.VATTUs.Find(id);
            if (VATTU == null)
            {
                return NotFound();
            }

            db.VATTUs.Remove(VATTU);
            db.SaveChanges();

            return Ok(VATTU);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VATTUExists(int id)
        {
            return db.VATTUs.Count(e => e.mavt == id) > 0;
        }
    }
}