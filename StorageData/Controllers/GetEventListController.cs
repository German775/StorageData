using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageData.DBContext;
using StorageData.TransferData;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetEventListController : Controller
    {
        public Context dbContext;
        public List<JsonEvent> eventList;

        public GetEventListController()
        {
            this.dbContext = new Context();
            this.eventList = new List<JsonEvent>();
        }


        [HttpPost]
        public ActionResult<IEnumerable<JsonEvent>> Post(string cameraId, DateTime beginPeriod, DateTime endPeriod)
        {
            var listEventsForCamera = dbContext.FrameParameters.Where(item => item.Parameters.Name == "CameraId" && item.Value == cameraId).Select(item => item.Frames.EventId).Distinct();
            foreach (var eventForCamera in listEventsForCamera)
            {
                var transferEvent = new JsonEvent();
                transferEvent.EventId = eventForCamera;
                transferEvent.EventStartTime = dbContext.Frames.Where(item => item.EventId == eventForCamera).Min(item => item.Timestamp);
                transferEvent.EventEndTime = dbContext.Frames.Where(item => item.EventId == eventForCamera).Max(item => item.Timestamp);
                if (beginPeriod != DateTime.MinValue && endPeriod != DateTime.MinValue)
                {
                    if (transferEvent.EventStartTime >= beginPeriod && transferEvent.EventEndTime <= endPeriod)
                    {
                        eventList.Add(transferEvent);
                    }
                    else continue;
                }
                else
                {
                    eventList.Add(transferEvent);
                }
            }
            return eventList.OrderByDescending(item => item.EventEndTime).ToList();
        }
    }
}
