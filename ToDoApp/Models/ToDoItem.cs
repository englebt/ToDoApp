using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsComplete { get; set; }

        //[ForeignKey("ToDoList")]
        //public int ToDoListId { get; set; }
        //public virtual ToDoList ToDoList { get; set; }
    }
}