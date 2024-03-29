﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageData.DBContext;
using System.IO;
using StorageData.TransferData;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetBackgroundController : Controller
    {
        public Context dbContext;

        public GetBackgroundController()
        {
            this.dbContext = new Context();
        }

        [HttpPost]
        public IEnumerable<JsonBackground> Post(Guid eventId)
        {
            var backgroundList = new List<JsonBackground>();

            var framesId = dbContext.FrameParameters.Where(item => item.Frames.EventId == eventId && item.Parameters.Name == "Type" && item.Value == "Background").Select(item => item.Frames.Id);
            foreach (var frame in framesId)
            {
                var background = new JsonBackground();
                background.BackgroundId = dbContext.FrameParameters.Where(item => item.Parameters.Name == "BackgroundId" && Convert.ToString(item.Frames.Id) == Convert.ToString(frame)).Select(item => item.Value).First().ToString();
                background.Width = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Width" && item.Frames.Id == frame).Select(item => item.Value).First().ToString();
                background.Height = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Height" && item.Frames.Id == frame).Select(item => item.Value).First().ToString();
                using (var fileStream =
                    new FileStream(
                        Path.Combine(new[]
                        {
                            Configuration.GetConfiguration().PathForStoreImage,
                            eventId.ToString(),
                            "Background",
                            $"{frame}.jpg"
                        }), FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    background.Data = Convert.ToBase64String(buffer);
                }

                backgroundList.Add(background);
            }
            return backgroundList;
        }
    }
}