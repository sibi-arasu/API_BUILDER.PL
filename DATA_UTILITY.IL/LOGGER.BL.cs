using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_UTILITY.IL
{
    public class LOGGER:LoggerIL
    {
        public void LogMessage(string message)
        {
            string CurrentDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\LOGS"));
            if (!Directory.Exists(CurrentDirectory))
            {
                Directory.CreateDirectory(CurrentDirectory);
            }
            string FilePath = Path.Combine(CurrentDirectory, $"GEN_LOG_{DateTime.Now.ToString("dd-MM-yyyy")}"  +".txt");
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine($"{DateTime.Now:HH:mm:ss} - {message}");
            }
        }
        public void LogMessage(string Msg, string FileName)
        {
            string currentDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\LOGS"));
            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }
            string filePath = Path.Combine(currentDir, FileName);
            
            WriteToFile(filePath, Msg);

        }
        public void LogMessage(string Msg, string FileName, string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            string filePath = System.IO.Path.Combine(Path, FileName);
            WriteToFile(filePath, Msg);

        }
        public void LogMessage(string Msg, Exception err)
        {
            string currentDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\LOGS"));
            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }
            string filePath = System.IO.Path.Combine(currentDir, $"GEN_LOG_{DateTime.Now:dd-MM-yyyy}.txt");
            string fullMessage = $"{Msg}\nException: {err.Message}\nStackTrace: {err.StackTrace}";
            WriteToFile(filePath, fullMessage);


        }
        public void LogMessage(string Msg, string FileName, Exception err)
        {
            string currentDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\LOGS"));
            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }
            string filePath = System.IO.Path.Combine(currentDir, FileName);
            string fullMessage = $"{Msg}\nException: {err.Message}\nStackTrace: {err.StackTrace}";
            WriteToFile(filePath, fullMessage);

        }
        private void WriteToFile(string filePath, string content)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{DateTime.Now:HH:mm:ss} - {content}");
            }
        }

    }
}
