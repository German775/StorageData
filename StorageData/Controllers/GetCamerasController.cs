using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageData.DBContext;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetCamerasController : Controller
    {
        public Context dbContext;

        public GetCamerasController()
        {
            this.dbContext = new Context();
        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> Post()
        {
            var cameraList = dbContext.FrameParameters.Where(item => item.Parameters.Name == "CameraId").Select(item => item.Value).Distinct().ToList();
            return cameraList;
        }
    }
}
