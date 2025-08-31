using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace API_BUILDER.PL.RESPONSE
{
    public interface IResponseModel
    {
        List<string> ValidationMsg { get; set; }
        List<string> ErrorMsg { get; set; }
        List<object> Data { get; set; }
        bool IsValid { get; set; }
    }
    public class ResponseModel : IResponseModel
    {
        public List<string> ValidationMsg { get; set; } = new List<string>();
        public List<string> ErrorMsg { get; set; } = new List<string>();
        public List<object> Data { get; set; } = new List<object>();
        public bool IsValid { get; set; } = true;
    }
}