using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StorageData.TransferData
{
    public class JsonData
    {
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public int Coordinate_X  { get; set; }
        public int Coordinate_Y { get; set; }
        public Guid BackgroundId { get; set; }
        public string CameraId { get; set; }
        public string Data { get; set; }
        public Guid EventId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
