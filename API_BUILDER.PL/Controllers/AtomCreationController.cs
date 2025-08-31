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
    public class AtomCreationController : ApiController
    {
        private readonly LoggerIL _logger;

        public AtomCreationController(LoggerIL logger)
        {
            _logger = logger;

        }

        [HttpPost]
        public IResponseModel GetAtomDefinition(JObject Model)
        {
            var response = new ResponseModel();
            try
            {

                DataSet ds = new DataSet();
                AtomCreation_IL objIl = new AtomCreation_IL();
                string json = JsonConvert.SerializeObject(Model);
                AtomDefinitonModel definitonModel = JsonConvert.DeserializeObject<AtomDefinitonModel>(json);

                ds = objIl.GetAtomDefiniton(definitonModel);

                response.Data.Add(ds);
            }
            catch (Exception ex)
            {
                _logger.LogMessage("GetAtomDefinition EXCEPTION DTLS", ex);
                throw ex;
            }
            return response;

        }
        [HttpPost]
        public IResponseModel Post(JObject Model)
        {
            var response = new ResponseModel();
            try
            {

                DataSet ds = new DataSet();
                AtomCreation_IL objIl = new AtomCreation_IL();
                string json = JsonConvert.SerializeObject(Model);
                AtomCreationModel PostModel = JsonConvert.DeserializeObject<AtomCreationModel>(json);

                response.IsValid = objIl.Post(PostModel);

                response.Data.Add(ds);
            }
            catch (Exception ex)
            {
                _logger.LogMessage("GetAtomDefinition EXCEPTION DTLS", ex);
                throw ex;
            }
            return response;

        }

        [HttpPost]
        public DataSet getData()
        {
            //_logger.LogMessage("s");
            DataSet ds = new DataSet();
            AtomCreation_IL objIl = new AtomCreation_IL();
            ds = objIl.GetAtomData();
            return ds;

        }
    }
}
