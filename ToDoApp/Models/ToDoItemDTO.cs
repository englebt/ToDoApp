﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ToDoApp.Models
{
    public class ToDoItemDTO
    {
        public int ToDoItemId { get; set; }

        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public bool _destroy { get; set; }

        #region Constructor

        public ToDoItemDTO() { }

        public ToDoItemDTO(ToDoItem item)
        {
            ToDoItemId = item.Id;
            UserId = item.UserId;
            Title = item.Title;
            IsComplete = item.IsComplete;
            _destroy = item.Destroy;
        }

        #endregion

        public ToDoItem ToEntity()
        {
            return new ToDoItem()
            {
                Id = ToDoItemId,
                UserId = UserId, 
                Title = Title,
                IsComplete = IsComplete,
                Destroy = _destroy,
            };
        }
    }
}