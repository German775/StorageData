﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using StorageData.DBContext;
using StorageData.TransferData;
using System.Globalization;

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
        public ActionResult<IEnumerable<JsonFrame>> Post(Guid eventId, DateTime dataPastDetect)
        {
            var listFrame = new List<JsonFrame>();
            var dateFrames = dbContext.Frames.Where(item => item.EventId == eventId).OrderBy(item => item.Timestamp).Select(item => item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)).Distinct();
            if (dataPastDetect == DateTime.MinValue)
            {
                var dateTest = dateFrames.First();
                var frames = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Type" && item.Value == "Frame" && item.Frames.EventId == eventId && item.Frames.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) == dateTest).Select(item => item.Frames.Id);

                foreach (var frame in frames) {
                    var jsonFrame = new JsonFrame();
                    jsonFrame.FrameId = frame;
                    jsonFrame.BackgroundId = dbContext.FrameParameters.Where(item => item.Parameters.Name == "BackgroundId" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.DateTime = dbContext.Frames.Where(item => item.Id == jsonFrame.FrameId).Select(item => item.Timestamp).First().ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    jsonFrame.Coordinate_X = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Coordinate_X" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.Coordinate_Y = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Coordinate_Y" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.Width = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Width" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.Height = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Height" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();

                    using (var fileStream =
                        new FileStream(
                            Path.Combine(new[]
                            {
                                configuration.GetConfiguration().PathForStoreImage, eventId.ToString(), "Frame",
                                $"{jsonFrame.FrameId}.jpg"
                            }), FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, (int)fileStream.Length);
                        jsonFrame.Data = (Convert.ToBase64String(buffer));
                    }
                    listFrame.Add(jsonFrame);
                }
            }

            /*
            var listFrame = new List<JsonFrame>();
            var directoryInfo = new DirectoryInfo($"{configuration.GetConfiguration().PathForStoreImage}\\{eventId}\\Frame");
            foreach (var pathFrame in directoryInfo.GetFiles("*.jpg"))
            {
                var jsonFrame = new JsonFrame();
                jsonFrame.FrameId = Guid.Parse(pathFrame.Name.Substring(0, pathFrame.Name.Length - 4));
                jsonFrame.BackgroundId = dbContext.FrameParameters.Where(item => item.Parameters.Name == "BackgroundId" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.DateTime = dbContext.Frames.Where(item => item.Id == jsonFrame.FrameId).Select(item => item.Timestamp).First().ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);       
                jsonFrame.Coordinate_X = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Coordinate_X" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.Coordinate_Y = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Coordinate_Y" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.Width = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Width" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                jsonFrame.Height = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Height" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                using (var fileStream = new FileStream(pathFrame.ToString(), FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    jsonFrame.Data = (Convert.ToBase64String(buffer));
                }
                listFrame.Add(jsonFrame);
            }
            return listFrame.OrderBy(parameter => parameter.DateTime).ToList();
            */
            return listFrame.ToList();
        }
    }
}
