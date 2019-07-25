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

            return null;
        }
    }
}
