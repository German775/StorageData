using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageData.Model
{
    public class Frame
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ICollection<FrameParameter> EventAttributes { get; set; }
    }
}
