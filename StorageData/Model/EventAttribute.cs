using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageData.Model
{
    public class EventAttribute
    {
        public Guid Id { get; set; }
        public Guid Frame_Id { get; set; }
        public Guid Parem_Id { get; set; }
        public string Value { get; set; }

        public virtual Frame Frames { get; set; }
        public virtual Parameter Parameters { get; set; }
    }
}
