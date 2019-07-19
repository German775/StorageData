using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageData.TransferModel
{
    public class JsonData
    {
        public int FileId { get; set; }
        public string Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public int EventId { get; set; }
        public byte[] Data { get; set; }
    }
}
