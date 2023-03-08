using System.ComponentModel.DataAnnotations;

namespace TodoApp_DomainEntities
{
    public enum TodoStatus
    {
        [Display(Name = "Not Started")]
        NotStarted = 0,

        [Display(Name = "In Progress")]
        InProgress = 1,

        [Display(Name = "Completed")]
        Completed = 2
    }
}
