using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageData.TransferModel;
using Newtonsoft.Json;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataController : Controller
    {
        /*
        [HttpPost]
        public void Post([FromBody]JsonData data)
        {
            var storage = new Storage();
            var configuration = new Configuration();
            storage.AddData(configuration.GetConfiguration().PathForStoreImage, data.FileId, data.Type, data.TimeStamp, data.EventId, null);
        }
        */

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async void Post([FromForm]IFormFile myfiles, string datas)
        {
            //Test
            var storage = new Storage();
            var configuration = new Configuration();
            var data = new JsonData();
            data = JsonConvert.DeserializeObject<JsonData>(datas);
            storage.AddData(configuration.GetConfiguration().PathForStoreImage, data.FileId, data.Type, data.TimeStamp, data.EventId, null);
        }
    }
}
