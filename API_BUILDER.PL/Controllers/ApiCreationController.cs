using API_BUILDER.IL;
using API_BUILDER.MODEL;
using API_BUILDER.PL.RESPONSE;
using DATA_UTILITY.IL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_BUILDER.PL.Controllers
{
    public class ApiCreationController : ApiController
    {
        private readonly LoggerIL _logger;

        public ApiCreationController(LoggerIL logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ResponseModel GetApiDtls()
        {
            ResponseModel model = new ResponseModel();
            return model;
        }
    }
}