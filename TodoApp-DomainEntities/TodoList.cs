using System.ComponentModel.DataAnnotations;

namespace TodoApp_DomainEntities
{
    public class TodoList : EntityBase
    {
        [Required(ErrorMessage = "Please, enter title")]
        public string Title { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public ICollection<Todo> Todos { get; } = new HashSet<Todo>();

        public bool IsDeleted { get; set; }
    }
}
