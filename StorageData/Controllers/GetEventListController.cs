using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageData.DBContext;

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
        public ActionResult<IEnumerable<string>> Post(string cameraId, DateTime beginPeriod, DateTime endPeriod)
        {
            var eventList = new List<string>();
            var listEvents = dbContext.EventAttributes.Where(item => item.Parameters.Name == "CameraId" && item.Value == cameraId).Select(item => item.Frames.EventId).Distinct();
            if (beginPeriod != DateTime.MinValue && endPeriod != DateTime.MinValue)
            {
                foreach (var itemOfListEvent in listEvents)
                {
                    var test = dbContext.EventAttributes.Where(item => item.Frames.EventId == itemOfListEvent && item.Parameters.Name == "DateTime" && Convert.ToDateTime(item.Value) >= beginPeriod);
                }
                return eventList;
            }
            else
            {
                foreach (var item in listEvents)
                {
                    eventList.Add(item.ToString());
                }
                return eventList;
            }
        }
    }
}
