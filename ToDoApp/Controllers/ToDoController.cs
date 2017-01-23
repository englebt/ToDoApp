using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Authorize]
    public class ToDoController : ApiController
    {
        private ToDoContext db = new ToDoContext();

        // GET: api/ToDo
        public IEnumerable<ToDoItemDTO> GetToDoItems()
        {
            return db.ToDoItems.Where(i => i.UserId == User.Identity.Name)
                .OrderByDescending(i => i.Id)
                .AsEnumerable()
                .Select(itemList => new ToDoItemDTO(itemList));
        }

        // GET: api/ToDo/5
        [ResponseType(typeof(ToDoItemDTO))]
        public async Task<IHttpActionResult> GetToDoItem(int id)
        {
            ToDoItem item = await db.ToDoItems.SingleOrDefaultAsync(i=>i.Id == id);

            if (item == null)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            if (item.UserId != User.Identity.Name)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));

            return Ok(item);
        }

        // PUT: api/ToDo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutToDoItem(int id, ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != toDoItem.Id)
                return BadRequest();

            db.Entry(toDoItem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ToDo
        [ResponseType(typeof(ToDoItem))]
        public async Task<IHttpActionResult> PostToDoItem(ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.ToDoItems.Add(toDoItem);
            await db.SaveChangesAsync();

            ToDoItemDTO itemDTO = new ToDoItemDTO(toDoItem);

            return CreatedAtRoute("DefaultApi", new { id = toDoItem.Id }, itemDTO);
        }

        // DELETE: api/ToDo/5
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult DeleteToDoItem(int id)
        {
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            db.ToDoItems.Remove(toDoItem);
            db.SaveChanges();

            return Ok(toDoItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        private bool ToDoItemExists(int id)
        {
            return db.ToDoItems.Count(e => e.Id == id) > 0;
        }
    }
}