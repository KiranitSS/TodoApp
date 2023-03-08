using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp_DomainEntities
{
    public class ReminderList : EntityBase
    {
        public ICollection<Reminder> TodoIds { get; set; } = new HashSet<Reminder>();
    }
}
