using System.ComponentModel.DataAnnotations;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.ViewModels
{
    public class TodoViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public DateTime CreationDate { get; set; }

        public TodoStatus Status { get; set; }

        public long ListId { get; set; }
    }
}
