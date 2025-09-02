using API_BUILDER.IL.APICREATION_MODEL;
using DATA_UTILITY.IL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace API_BUILDER.IL.APICREATION_MODEL
{
    public class ApiCreationModel
    {
        public string ApiID { get; set; }
        public string ApiName { get; set; }
        public string MethodName { get; set; }
        public string AtomID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}

namespace API_BUILDER.IL
{
    public class ApiCreationService
    {
        private readonly string _controllersPath = @"C:\APIBuilder\Controllers";
        private readonly string _projectPath = @"C:\APIBuilder\APIBuilder.csproj";
        private readonly string _dllPath = @"C:\APIBuilder\bin\APIBuilder.dll";
        private readonly string _outputPath = "";// ConfigurationManager.AppSettings["OutputPath"];
        private readonly CmdExecuter _cmd = new CmdExecuter();

        public bool CreateApi(ApiCreationModel model)
        {
            if (string.IsNullOrEmpty(model.ApiID))
                model.ApiID = CHelperFns.GetRandomId();

            if (DoesMethodExist(model.ApiID, model.MethodName))
            {
                Console.WriteLine($"Method '{model.MethodName}' already exists in API '{model.ApiName}'.");
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Update existing method");
                Console.WriteLine("2. Create new version");

                var choice = Console.ReadLine();
                if (choice == "2")
                {
                    model.ApiName = $"{model.ApiName}V2";
                    model.ApiID = CHelperFns.GetRandomId();
                }
            }

            InsertApiMetadata(model);
            GenerateControllerOrAppendMethod(model);
            BuildAndCopyDll();

            return true;
        }

        private bool DoesMethodExist(string apiId, string methodName)
        {
            string query = $"SELECT COUNT(*) FROM API_METHODS WHERE ApiID = '{apiId}' AND MethodName = '{methodName}'";
            return Convert.ToInt32(_cmd.ExecuteQuery(query)) > 0;
        }

        private void InsertApiMetadata(ApiCreationModel model)
        {
            string methodID = CHelperFns.GetRandomId();
            var list = new ArrayList();
            var sb = new StringBuilder();

            sb.Append("INSERT INTO API_DTLS (ApiID, ApiName, CreatedBy, CreatedOn) VALUES(");
            sb.Append($"'{model.ApiID}', '{model.ApiName}', '{model.CreatedBy}', ");
            sb.Append($"'{model.CreatedOn:yyyy-MM-dd HH:mm:ss}')");
            list.Add(sb.ToString());
            sb.Clear();

            sb.Append("INSERT INTO API_METHODS (MethodID, ApiID, MethodName, AtomID, CreatedBy, CreatedOn) VALUES(");
            sb.Append($"'{methodID}', '{model.ApiID}', '{model.MethodName}', '{model.AtomID}', ");
            sb.Append($"'{model.CreatedBy}', '{model.CreatedOn:yyyy-MM-dd HH:mm:ss}')");
            list.Add(sb.ToString());

            _cmd.ExecuteQueryWithReader(list);
        }

        private void GenerateControllerOrAppendMethod(ApiCreationModel model)
        {
            string controllerName = $"{model.ApiName}Controller.cs";
            string controllerPath = Path.Combine(_controllersPath, controllerName);

            string methodCode = GenerateMethodCode(model.MethodName, model.AtomID);

            if (!File.Exists(controllerPath))
            {
                string controllerCode = GenerateNewController(model.ApiName, methodCode);
                File.WriteAllText(controllerPath, controllerCode);
            }
            else
            {
                AppendMethodToController(controllerPath, methodCode);
            }
        }

        private string GenerateMethodCode(string methodName, string atomId)
        {
            return $@"
        [HttpGet]
        [Route(""api/{methodName.ToLower()}"")]
        public IHttpActionResult {methodName}()
        {{
            var atom = new APIBuilder.Models.Atom_{atomId}();
            return Ok(atom);
        }}";
        }

        private string GenerateNewController(string apiName, string methodCode)
        {
            return $@"
using System.Web.Http;
using APIBuilder.Models;

namespace API_BUILDER.PL.Controllers
{{
    public class {apiName}Controller : ApiController
    {{
        {methodCode}
    }}
}}";
        }

        private void AppendMethodToController(string filePath, string methodCode)
        {
            var lines = File.ReadAllLines(filePath).ToList();
            int index = lines.FindLastIndex(line => line.Trim() == "}");
            lines.Insert(index, methodCode);
            File.WriteAllLines(filePath, lines);
        }

        private void BuildAndCopyDll()
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c msbuild \"{_projectPath}\" /p:Configuration=Release /t:Rebuild";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();

            File.Copy(_dllPath, Path.Combine(_outputPath, "APIBuilder.dll"), true);
        }
    }

}
