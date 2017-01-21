using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ToDoItemDTO
    {
        [Key]
        public int ToDoItemId { get; set; }
        
        [Required]
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public int ToDoListId { get; set; }

        #region Constructor

        public ToDoItemDTO() { }

        public ToDoItemDTO(ToDoItem item)
        {
            ToDoItemId = item.Id;
            Title = item.Title;
            IsComplete = item.IsComplete;
            ToDoListId = item.ToDoListId;
        }

        #endregion

        public ToDoItem ToEntity()
        {
            return new ToDoItem()
            {
                Id = ToDoItemId,
                Title = Title,
                IsComplete = IsComplete,
                ToDoListId = ToDoListId
            };
        }
    }
}