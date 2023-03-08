using System.ComponentModel.DataAnnotations;
using System.Transactions;
using TodoApp_DomainEntities;

namespace TodoAppWeb.Models.ViewModels
{
    public class TodoListViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please add title")]
        public string Title { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }

        public IEnumerable<TodoViewModel> Todos { get; set; } = Enumerable.Empty<TodoViewModel>();
    }
}
