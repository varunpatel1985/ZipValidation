using System;
using System.IO;
using System.Reflection;

namespace API.Logs
{
    public class Logger
    {
        private string m_exePath = @"C:\varun\docs\Angular\ZipValidation\API\Logs";
        public Logger(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
          
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}: {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(),logMessage);               
                ;
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}