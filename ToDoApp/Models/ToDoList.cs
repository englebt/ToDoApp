using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ToDoList
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsComplete { get; set; }
        //public virtual List<ToDoItem> ToDoItems { get; set; }
    }
}