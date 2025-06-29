using DATA_UTILITY.IL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_BUILDER.PL.Controllers
{
    public class AtomCreationController : ApiController
    {
        private readonly LoggerIL _logger;

        public AtomCreationController(LoggerIL logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("{controller}/{action}")]
        public string getData()
        {
            _logger.LogMessage("s");
            return "s";
        }
    }
}
