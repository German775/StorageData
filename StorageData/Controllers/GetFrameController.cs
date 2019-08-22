using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using StorageData.DBContext;
using StorageData.TransferData;
using System.Globalization;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetFrameController : Controller
    {
        public Context dbContext;

        public GetFrameController()
        {
            dbContext = new Context(); // TODO: Get created IContext outside as constructor param
        }
        public ActionResult<IEnumerable<JsonFrame>> Post(Guid eventId, DateTime datePastDetect)
        {
            const int quantityReceivedImage = 50;
            var listFrame = new List<JsonFrame>();
            var dateFrames = dbContext.Frames.Where(item => item.EventId == eventId).OrderBy(item => item.Timestamp).Select(item => item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture)).Distinct().ToList();
            List<string> dates;
            if (datePastDetect.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture) == DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture) || datePastDetect == null)
            {
                dates = dateFrames.Take(quantityReceivedImage).ToList();
            }
            else
            {
                var indexActualDate = dateFrames.FindIndex(item => item == datePastDetect.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture));
                if (++indexActualDate >= dateFrames.Count)
                {
                    return new List<JsonFrame>();
                }
                dates = dateFrames.Skip(++indexActualDate).Take(quantityReceivedImage).ToList();
            }

            foreach (var date in dates)
            {
                var frames = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Type" && item.Value == "Frame" && item.Frames.EventId == eventId && item.Frames.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture) == date).Select(item => item.Frames.Id);
                foreach (var frame in frames)
                {
                    var jsonFrame = new JsonFrame();
                    jsonFrame.FrameId = frame;
                    jsonFrame.BackgroundId = dbContext.FrameParameters.Where(item => item.Parameters.Name == "BackgroundId" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.DateTime = dbContext.Frames.Where(item => item.Id == jsonFrame.FrameId).Select(item => item.Timestamp).First().ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
                    jsonFrame.Coordinate_X = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Coordinate_X" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.Coordinate_Y = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Coordinate_Y" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.Width = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Width" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();
                    jsonFrame.Height = dbContext.FrameParameters.Where(item => item.Parameters.Name == "Height" && item.Frames.Id == jsonFrame.FrameId).Select(item => item.Value).First();

                    using (var fileStream =
                        new FileStream(
                            Path.Combine(new[]
                            {
                                Configuration.GetConfiguration().PathForStoreImage, eventId.ToString(), "Frame",
                                $"{jsonFrame.FrameId}.jpg"
                            }), FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, (int) fileStream.Length);
                        jsonFrame.Data = Convert.ToBase64String(buffer);
                    }

                    listFrame.Add(jsonFrame);
                }
            }
            return listFrame.ToList();
        }
    }
}
