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
        public ActionResult<IEnumerable<string>> Post(string cameraId)
        {
            var eventList = new List<string>();
            var keyCamera = dbContext.Parameters.Where(item => item.Name == "CameraId").Select(item => item.Id).First();
            var listEvents = dbContext.EventAttributes.Where(item => item.Parameters.Name == "CameraId").Select(item => item.Frames.EventId).Distinct();
            foreach (var item in listEvents)
            {
                eventList.Add(item.ToString());
            }
            return eventList;
        }
    }
}
