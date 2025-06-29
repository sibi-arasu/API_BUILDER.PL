using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_UTILITY.IL
{
    public interface LoggerIL
    {
        void LogMessage(string Msg);
        void LogMessage(string Msg,string FileName);
        void LogMessage(string Msg, string FileName, string Path);
        void LogMessage(string Msg,Exception err);
        void LogMessage(string Msg, string FileName,Exception err);


    }
}
