using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StorageData.Controllers
{
    [Route("api/[controller]")]
    public class GetBackgroundController : Controller
    {
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int eventId)
        {
        }
    }
}
