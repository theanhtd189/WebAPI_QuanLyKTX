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


        [Route("edit/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult Edit(TAIKHOAN e)
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
                if (!TAIKHOANExists(e.matk))
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
        [ResponseType(typeof(TAIKHOAN))]     
        public IHttpActionResult Create(TAIKHOAN e)
        {
            try
            {
                if (!ModelState.IsValid || e==null)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.TAIKHOANs.Add(e);
                    db.SaveChanges();
                }               
            }
            catch (DbUpdateException ex)
            {
                if (TAIKHOANExists(e.matk))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = e.matk }, e);
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ResponseType(typeof(TAIKHOAN))]
        public IHttpActionResult Delete(int id)
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