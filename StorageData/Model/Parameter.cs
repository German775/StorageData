using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageData.Model
{
    public class Parameter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<EventAttribute> EventAttributes { get; set; }
    }
}
