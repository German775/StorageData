using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using StorageData.DBContext;
using StorageData.TransferData;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetEventListController : Controller
    {
        public Context dbContext;

        public GetEventListController()
        {
            this.dbContext = new Context();
        }


        [HttpPost]
        public ActionResult<IEnumerable<JsonEvent>> Post(string cameraId, DateTime beginPeriod, DateTime endPeriod, Guid lastEvent, string movingType)
        {
            const int quantityReceivedEvents = 10;
            var listEventsForCamera = dbContext.FrameParameters.Where(item => item.Parameters.Name == "CameraId" && item.Value == cameraId).Select(item => item.Frames.EventId).Distinct();
            var eventList = new List<JsonEvent>();
            var nextPageEvents = new List<JsonEvent>();

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
                }
                else
                {
                    eventList.Add(transferEvent);
                }
            }

            eventList = eventList.OrderByDescending(item => item.EventStartTime).ToList();

            if (lastEvent != Guid.Empty)
            {
                var indexLastEvent = eventList.FindIndex(item => item.EventId == lastEvent);
                if (movingType == "Next")
                {
                    nextPageEvents = eventList.GetRange(++indexLastEvent,  Math.Min(quantityReceivedEvents, eventList.Count - indexLastEvent)); 
                    if (nextPageEvents.Count == 0)
                    {
                        nextPageEvents = eventList.Take(quantityReceivedEvents).ToList();
                    }
                }
                else if(movingType == "Back")
                {
                    nextPageEvents = eventList.Skip(--indexLastEvent - quantityReceivedEvents).Take(quantityReceivedEvents).ToList();
                }
            }
            else
            {
                nextPageEvents = eventList.Take(quantityReceivedEvents).ToList();
            }
            return nextPageEvents;
        }
    }
}
