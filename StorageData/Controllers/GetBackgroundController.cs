using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageData.DBContext;
using System.IO;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetBackgroundController : Controller
    {
        public Context dbContext;
        public Configuration configuration;

        public GetBackgroundController()
        {
            this.dbContext = new Context();
            this.configuration = new Configuration();
        }
        [HttpPost]
        public string Post(string backgroundId)
        {
            var frameId = dbContext.EventAttributes.Where(item => item.Parameters.Name == "BackgroundId" && item.Value == backgroundId).Select(item => item.Frames.Id).First();
            var eventId = dbContext.EventAttributes.Where(item => item.Parameters.Name == "BackgroundId" && item.Value == backgroundId).Select(item => item.Frames.EventId).First();
            using (var fileStream = new FileStream($"{configuration.GetConfiguration().PathForStoreImage}//{eventId}//Background//{frameId}.jpg".ToString(), FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                return(Convert.ToBase64String(buffer));
            };
        }
    }
}
