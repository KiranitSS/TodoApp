using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp_DomainEntities
{
    public interface IEntity<TKey> where TKey : struct
    {
        TKey Id { get; set; }

        bool IsNew();
    }
}
