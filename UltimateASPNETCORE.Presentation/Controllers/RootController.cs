using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateASPNETCORE.Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }


    }
}
