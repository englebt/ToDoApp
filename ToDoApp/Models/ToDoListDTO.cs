using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ToDoListDTO
    {
        public int ToDoListId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public bool _destroy { get; set; }

        public List<ToDoItemDTO> ToDoItems { get; set; }

        #region Constructor

        public ToDoListDTO()
        {
            ToDoItems = new List<ToDoItemDTO>();
        }

        public ToDoListDTO(ToDoList list)
        {
            ToDoListId = list.Id;
            UserId = list.UserId;
            Title = list.Title;
            _destroy = list.Destroy;
            foreach (ToDoItem item in list.ToDoItems)
                ToDoItems.Add(new ToDoItemDTO(item));
        }

        #endregion

        public ToDoList ToEntity()
        {
            ToDoList list = new ToDoList()
            {
                Id = ToDoListId,
                UserId = UserId,
                Title = Title,
                Destroy = _destroy
            };
            foreach (ToDoItemDTO itemDTO in ToDoItems)
                list.ToDoItems.Add(itemDTO.ToEntity());

            return list;
        }
    }
}