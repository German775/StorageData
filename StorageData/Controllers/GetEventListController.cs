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
        public ActionResult<IEnumerable<Guid>> Post(string cameraId, DateTime beginPeriod, DateTime endPeriod)
        {
            IEnumerable<Guid> listEvents;
            if (beginPeriod != DateTime.MinValue && endPeriod != DateTime.MinValue)
            {
                listEvents = dbContext.FrameParameters.Where(item => item.Parameters.Name == "CameraId" && item.Value == cameraId && item.Frames.Timestamp >= beginPeriod && item.Frames.Timestamp <= endPeriod).Select(item => item.Frames.EventId).Distinct();
            }
            else
            {
                listEvents = dbContext.FrameParameters.Where(item => item.Parameters.Name == "CameraId" && item.Value == cameraId).Select(item => item.Frames.EventId).Distinct();
            }
            return listEvents.ToList();
        }
    }
}
