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

        public Context dataBase;

        public GetEventListController()
        {
            this.dataBase = new Context();
        }

        [HttpPost]
        public IEnumerable<Guid> Post(int cameraId)
        {
            return null;
        }

        [HttpPost]
        public IEnumerable<Guid> Post()
        {
            //List<Guid> eventList = dataBase.Datas.Select(item => item.FileId).ToList();
            //return eventList;
            return null;
        }
    }
}
