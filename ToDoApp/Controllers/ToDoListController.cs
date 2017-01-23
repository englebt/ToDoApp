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
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoListController : ApiController
    {
        private ToDoContext db = new ToDoContext();

        // GET: api/ToDoList
        public IEnumerable<ToDoListDTO> GetToDoLists()
        {
            return db.ToDoLists.Where(u => u.UserName == User.Identity.Name)
                .OrderByDescending(u => u.Id)
                .AsEnumerable()
                .Select(todoList => new ToDoListDTO(todoList));
        }

        // GET: api/ToDoList/5
        [ResponseType(typeof(ToDoList))]
        public ToDoListDTO GetToDoList(int id)
        {
            ToDoList list = db.ToDoLists.Find(id);
            if (list == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            if (list.UserName != User.Identity.Name)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));

            return new ToDoListDTO(list);
        }

        // PUT: api/ToDoList/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutToDoList(int id, ToDoList toDoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != toDoList.Id)
                return BadRequest();

            db.Entry(toDoList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ToDoList
        [ResponseType(typeof(ToDoList))]
        public IHttpActionResult PostToDoList(ToDoList toDoList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.ToDoLists.Add(toDoList);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = toDoList.Id }, toDoList);
        }

        // DELETE: api/ToDoList/5
        [ResponseType(typeof(ToDoList))]
        public IHttpActionResult DeleteToDoList(int id)
        {
            ToDoList list = db.ToDoLists.Find(id);
            if (list == null)
                return NotFound();

            db.ToDoLists.Remove(list);
            db.SaveChanges();

            return Ok(list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        private bool ToDoListExists(int id)
        {
            return db.ToDoLists.Count(e => e.Id == id) > 0;
        }
    }
}