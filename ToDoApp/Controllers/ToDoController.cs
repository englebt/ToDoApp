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
        public IEnumerable<ToDoListDTO> GetToDoLists()
        {
            return db.ToDoLists.Include(l => l.ToDoItems)
                .Where(l => l.UserId == User.Identity.Name)
                .OrderByDescending(l => l.Id)
                .AsEnumerable()
                .Select(list => new Models.ToDoListDTO(list));
        }

        // POST: api/ToDo
        [HttpPost]
        public async Task<IHttpActionResult> PostToDoLists(IEnumerable<ToDoListDTO> toDoLists)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            UpdateLists(toDoLists);

            await db.SaveChangesAsync();

            return Ok(toDoLists);
        }

        //// GET: api/ToDo
        //public IEnumerable<ToDoItemDTO> GetToDoItems()
        //{
        //    return db.ToDoItems.Where(i => i.UserId == User.Identity.Name)
        //        .OrderByDescending(i => i.Id)
        //        .AsEnumerable()
        //        .Select(itemList => new ToDoItemDTO(itemList));
        //}

        //// POST: api/ToDo
        //[HttpPost]
        //public async Task<IHttpActionResult> PostToDoItems(IEnumerable<ToDoItemDTO> toDoItems)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    foreach(ToDoItemDTO itemDTO in toDoItems)
        //    {
        //        ToDoItem item = db.ToDoItems.Find(itemDTO.ToDoItemId);

        //        if (item != null)   // existing item
        //        {
        //            if (itemDTO._destroy) // verify we haven't removed it from the VM
        //                db.ToDoItems.Remove(item);
        //            else // Update database with changes to existing item
        //            {
        //                item.Title = itemDTO.Title;
        //                item.IsComplete = itemDTO.IsComplete;
        //            }
        //        }
        //        else
        //            db.ToDoItems.Add(itemDTO.ToEntity());
        //    }

        //    await db.SaveChangesAsync();

        //    return Ok(toDoItems);
        //}

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

        private void UpdateLists(IEnumerable<ToDoListDTO> toDoLists)
        {
            foreach (ToDoListDTO listDTO in toDoLists)
            {
                ToDoList list = db.ToDoLists.Find(listDTO.ToDoListId);

                if (list != null)
                {
                    if (listDTO._destroy)
                    {
                        foreach (ToDoItem item in list.ToDoItems)
                            db.ToDoItems.Remove(item);

                        db.ToDoLists.Remove(list);
                    }
                    else
                    {
                        list.Title = listDTO.Title;
                        foreach (ToDoItemDTO itemDTO in listDTO.ToDoItems)
                        {
                            ToDoItem item = db.ToDoItems.Find(itemDTO.ToDoItemId);

                            if (item != null)   // existing item
                            {
                                if (itemDTO._destroy) // verify we haven't removed it from the VM
                                    db.ToDoItems.Remove(item);
                                else // Update database with changes to existing item
                                {
                                    item.Title = itemDTO.Title;
                                    item.IsComplete = itemDTO.IsComplete;
                                }
                            }
                            else
                                db.ToDoItems.Add(itemDTO.ToEntity());
                        }
                    }
                }
                else
                    db.ToDoLists.Add(listDTO.ToEntity());
            }
        }
    }
}