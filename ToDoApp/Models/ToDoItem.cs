using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsComplete { get; set; }

        public bool Destroy { get; set; }
    }
}