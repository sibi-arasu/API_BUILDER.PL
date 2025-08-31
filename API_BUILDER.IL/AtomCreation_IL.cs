using API_BUILDER.MODEL;
using DATA_UTILITY.IL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace API_BUILDER.MODEL
{

    public class AtomDefinitonModel
    {
        public string AtomDefintionID { get; set; }
        public bool IsQueryBased { get; set; }
        public string Query { get; set; }
        public bool IsSpBased { get; set; }
        public string SpName { get; set; }

    }
    public class AtomParams
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class AtomCreationModel
    {
        /// <summary>
        /// UNIQUE ID based on the def id, atom will be created
        /// </summary>
        public string AtomDefinitonID { get; set; }
        public string AtomID { get; set; }
        public string AtomName { get; set;}
        public List<AtomParams> InputParam { get; set; }
        public List<AtomParams> OutputParam { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }


    }
}

namespace API_BUILDER.IL
{
    public class AtomCreation_IL
    {
        public DataSet GetAtomData()
        {
            DataSet ds = new DataSet();
            CmdExecuter cmd = new CmdExecuter();
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT * FROM ATOM_DTLS");
            ds = cmd.ExecuteQuery(sbQuery.ToString());
            return ds;
        }

        public DataSet GetAtomDefiniton(AtomDefinitonModel objModel)
        {
            try
            {
                DataSet ds = new DataSet();
                CmdExecuter cmd = new CmdExecuter();
                StringBuilder sbQuery = new StringBuilder();
                if (objModel.IsQueryBased)
                {
                    sbQuery.Append($"EXEC sp_describe_first_result_set N'{objModel.Query ?? string.Empty}', NULL, 0");
                }
                else
                {
                    sbQuery.Append($"EXEC sp_describe_first_result_set N'{objModel.SpName ?? string.Empty}', NULL, 0");
                    sbQuery.AppendLine();
                    sbQuery.Append("SELECT p.name AS ParameterName,TYPE_NAME(p.user_type_id) AS ParameterType, ");
                    sbQuery.Append("p.is_output AS IsOutput, p.max_length, p.is_nullable FROM sys.parameters p ");
                    sbQuery.Append($"WHERE object_id = OBJECT_ID(N'{objModel.SpName ?? string.Empty}')");
                }
                ds = cmd.ExecuteQuery(sbQuery.ToString());

                return ds;
            }
            catch
            {
                throw;
            }
        }

        public bool Post(AtomCreationModel model)
        {
            bool isValid = false;

            DataSet ds = new DataSet();
            CmdExecuter cmd = new CmdExecuter();
            StringBuilder sbQuery = new StringBuilder();
            ArrayList list = new ArrayList();
            if (string.IsNullOrEmpty(model.AtomID))
            {
               model.AtomID = CHelperFns.GetRandomId();
            }
            if (string.IsNullOrEmpty(model.AtomDefinitonID))
            {
                model.AtomDefinitonID = CHelperFns.GetRandomId();
            }

            sbQuery.Append(" INSERT INTO ATOM_DTLS  (AtomID, AtomDefinitonID, AtomName, CreatedBy, CreatedOn) VALUES(");
            sbQuery.Append($"'{model.AtomID}', '{model.AtomDefinitonID}','{model.AtomName}','{model.CreatedBy}', ");
            sbQuery.Append($"'{model.CreatedOn?.ToString("yyyy-MM-dd HH:mm:ss") ?? "GETDATE()"}' )");
            list.Add(sbQuery.ToString());
            sbQuery.Clear();

            foreach (var param in model.InputParam ?? new List<AtomParams>())
            {
                
                sbQuery.Append("INSERT INTO ATOM_PARAMS (AtomID, ParamKey, ParamValue, ParamType) VALUES( ");
                sbQuery.Append($"'{model.AtomID}','{param.Key}','{param.Value}', INPUT )");
                list.Add(sbQuery.ToString());
                sbQuery.Clear();
            }
            foreach (var param in model.OutputParam ?? new List<AtomParams>())
            {
                sbQuery.Append("INSERT INTO ATOM_PARAMS (AtomID, ParamKey, ParamValue, ParamType) VALUES( ");
                sbQuery.Append($"'{model.AtomID}','{param.Key}','{param.Value}', OUTPUT )");
                list.Add(sbQuery.ToString());
                sbQuery.Clear();
            }

            ds = cmd.ExecuteQueryWithReader(list);
            
            return isValid;
        }
    }
}
