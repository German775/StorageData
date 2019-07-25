using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageData.Model;
using Newtonsoft.Json;
using System.IO;
using StorageData.Service;
using StorageData.TransferData;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetDataController : Controller
    {
        [HttpPost]
        public void Post([FromBody]JsonData jsonData)
        {
            var storage = new Storage();
            var configuration = new Configuration();
            storage.AddData(jsonData);
        }
    }
}
