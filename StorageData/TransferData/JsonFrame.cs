using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageData.TransferData
{
    public class JsonFrame
    {
        public string Coordinate_X { get; set; }
        public string Coordinate_Y { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
        public string DateTime { get; set; }
        public string BackgroundId { get; set; }
        public string Data { get; set; }
        public Guid FrameId { get; set; }
    }
}
