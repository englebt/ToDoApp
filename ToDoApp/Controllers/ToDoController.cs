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

        // POST: api/ToDo
        public async Task<IHttpActionResult> PostToDoItems(IEnumerable<ToDoItem> items)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach(ToDoItem item in items)
            {
                ToDoItem dbItem = db.ToDoItems.Find(item.Id);

                if (dbItem != null)
                {
                    if (item.Destroyed)
                        db.ToDoItems.Remove(dbItem);
                    else 
                    {
                        dbItem.Title = item.Title;
                        dbItem.IsComplete = item.IsComplete;
                    }
                }
            }

            await db.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/ToDo/5
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult DeleteToDoItem(int id)
        {
            ToDoItem toDoItem = db.ToDoItems.Find(id);
            if (toDoItem == null)
                return NotFound();

            db.ToDoItems.Remove(toDoItem);
            db.SaveChanges();

            return Ok();
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