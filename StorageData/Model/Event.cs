using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageData.Model
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTime DateTime_Start { get; set; }
        public DateTime DateTime_End { get; set; }

        public virtual ICollection<Frame> Frames { get; set; }
    }
}
