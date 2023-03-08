using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp_DomainEntities
{
    public class Todo : EntityBase, IMarkableDeleted
    {
        [Required(ErrorMessage = "Please, enter title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please, enter due date")]
        public DateTime DueDate { get; set; }

        public DateTime CreationDate { get; init; } = DateTime.Now.Date;

        public TodoStatus Status { get; set; } = TodoStatus.NotStarted;

        public bool IsDeleted { get; set; }

        public long ListId { get; set; }
    }
}
