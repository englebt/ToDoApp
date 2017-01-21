﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ToDoListDTO
    {
        [Key]
        public int ToDoListId { get; set; }

        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string Title { get; set; }

        public virtual List<ToDoItemDTO> ToDoItems { get; set; }

        #region Constructor

        public ToDoListDTO() { }

        public ToDoListDTO(ToDoList list)
        {
            ToDoListId = list.Id;
            UserId = list.UserId;
            Title = list.Title;

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
                ToDoItems = new List<ToDoItem>()
            };

            foreach (ToDoItemDTO item in ToDoItems)
                list.ToDoItems.Add(item.ToEntity());

            return list;
        }
    }
}