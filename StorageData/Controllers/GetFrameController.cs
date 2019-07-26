using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using StorageData.DBContext;
using StorageData.TransferData;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetFrameController : Controller
    {
        public Context dbContext;
        public Configuration configuration;
        public GetFrameController()
        {
            this.dbContext = new Context();
            this.configuration = new Configuration();
        }
        public ActionResult<IEnumerable<JsonFrame>> Post(string eventId)
        {
            var listFrame = new List<JsonFrame>();
            var directoryInfo = new DirectoryInfo($"{configuration.GetConfiguration().PathForStoreImage}\\{eventId}\\Frame");
            foreach (var pathFrame in directoryInfo.GetFiles("*.jpg"))
            {
                var jsonFrame = new JsonFrame();
                jsonFrame.FrameId = Guid.Parse(pathFrame.Name.Substring(0, pathFrame.Name.Length - 4));
                jsonFrame.BackgroundId = dbContext.EventAttributes.Where(item => item.Parameters.Name == "BackgroundId" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.DateTime = dbContext.EventAttributes.Where(item => item.Parameters.Name == "DateTime" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.Coordinate_X = dbContext.EventAttributes.Where(item => item.Parameters.Name == "Coordinate_X" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.Coordinate_Y = dbContext.EventAttributes.Where(item => item.Parameters.Name == "Coordinate_Y" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.Width = dbContext.EventAttributes.Where(item => item.Parameters.Name == "Width" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.Length = dbContext.EventAttributes.Where(item => item.Parameters.Name == "Length" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                using (var fileStream = new FileStream(pathFrame.ToString(), FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    jsonFrame.Data = (Convert.ToBase64String(buffer));
                }
                listFrame.Add(jsonFrame);
            }
            return listFrame;
        }
    }
}
