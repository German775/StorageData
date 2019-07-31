using Microsoft.AspNetCore.Mvc;
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
            storage.AddData(jsonData);
        }
    }
}
