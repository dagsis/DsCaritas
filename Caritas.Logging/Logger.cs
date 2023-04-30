using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caritas.Logging
{
    public static class Logger
    {
        private static string GetFileName()
        {
            return "log_" + (
                DateTime.Now.Day + "" +
                DateTime.Now.Month + "" +
                DateTime.Now.Year) + ".log";
        }

        public static void WriteLog(string message, string stackTrace)
        {
            string path = Environment.CurrentDirectory + @"\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string content = "";
            content += DateTime.Now + "_" + "Message:" + message +
                                           " StackTrace:" + stackTrace.Trim() +
                  Environment.NewLine;

            using (StreamWriter streamWriter = new StreamWriter(path + "/" + GetFileName(), true))
            {
                streamWriter.WriteLine(content);
                streamWriter.Close();
            }
        }
    }
}
