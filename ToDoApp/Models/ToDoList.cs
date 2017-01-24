using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class ToDoList
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public bool Destroy { get; set; }
        
        public virtual List<ToDoItem> ToDoItems { get; set; }

        #region Constructor 

        public ToDoList()
        {
            ToDoItems = new List<ToDoItem>();
        }

        #endregion
    }
}