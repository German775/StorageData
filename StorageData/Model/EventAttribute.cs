using System;

namespace StorageData.Model
{
    public class EventAttribute
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public virtual Frame Frames { get; set; }
        public virtual Parameter Parameters { get; set; }
    }
}
