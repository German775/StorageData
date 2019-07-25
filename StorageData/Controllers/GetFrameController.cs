using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using StorageData.DBContext;
using StorageData.TransferData;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetFrameController : Controller
    {
        public Context dbContext;
        public Configuration configuration;
        public GetFrameController()
        {
            this.dbContext = new Context();
            this.configuration = new Configuration();
        }
        public ActionResult<IEnumerable<JsonFrame>> Post(string eventId)
        {
            //var image = new List<string>();
            //var directoryInfo = new DirectoryInfo($"{configuration.GetConfiguration().PathForStoreImage}\\{eventId}\\Frame");
            //foreach (var pathFrame in directoryInfo.GetFiles("*.jpg"))
            //{
            //    using (var fileStream = new FileStream(pathFrame.ToString(), FileMode.Open, FileAccess.Read))
            //    {
            //        var buffer = new byte[fileStream.Length];
            //        fileStream.Read(buffer, 0, (int)fileStream.Length);
            //        image.Add(Convert.ToBase64String(buffer));
            //    }
            //}
            //return image;

            var image = new List<JsonFrame>();
            var directoryInfo = new DirectoryInfo($"{configuration.GetConfiguration().PathForStoreImage}\\{eventId}\\Frame");
            foreach (var pathFrame in directoryInfo.GetFiles("*.jpg"))
            {
                string path = pathFrame.Name.ToString();
                string frameId = path.Substring(path.Length-4, path.Length-1);

                //D:\StorageImages\7640dd91-ed16-4696-915c-21abd2fdb320\Frame\d299be9c-5da3-4f13-b0ac-c99929285dd4.jpg}

                using (var fileStream = new FileStream(pathFrame.ToString(), FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    //image.Add(Convert.ToBase64String(buffer));
                }
            }
            return null;

        }
    }
}
