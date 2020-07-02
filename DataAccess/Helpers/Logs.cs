using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DataAccess.Helpers
{
    public class Logs
    {
        private static bool salveLogOrdems = true;
        private static bool salveLogConexao = true;     
        private static bool salveLogConexaoAtivos = true;

        public static void CreateLogFolder()
        {
            bool existeDiretorio = Directory.Exists("C:\\CriptoStormLogs");
            if(!existeDiretorio)
            {
                Directory.CreateDirectory("C:\\CriptoStormLogs");
            }
        }

        public static void LogOrdem(string msg)
        {
            try
            {
                if (salveLogOrdems)
                {
                    var path = "C:\\CriptoStormLogs\\OrderLogs.txt";
                    using (var file = File.Exists(path) ? File.Open(path, FileMode.Append) : File.Open(path, FileMode.CreateNew))
                    {
                        using (StreamWriter stream = new StreamWriter(file))
                        {                               
                            stream.WriteLineAsync(msg + Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msgError = ex.Message;
            }
        }

        public static void LogConexao(string msg)
        {
            try
            {
                if (salveLogConexao)
                {
                    var path = "C:\\CriptoStormLogs\\ConnectionLogs.txt";
                    using (var file = File.Exists(path) ? File.Open(path, FileMode.Append) : File.Open(path, FileMode.CreateNew))
                    {
                        using (StreamWriter stream = new StreamWriter(file))
                        {
                            stream.WriteLineAsync(msg + Environment.NewLine);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static void LogConexaoAtivos(string msg)
        {
            try
            {
                if (salveLogConexaoAtivos)
                {
                    var path = "C:\\CriptoStormLogs\\AtivosConnectionLogs.txt";
                    using (var file = File.Exists(path) ? File.Open(path, FileMode.Append) : File.Open(path, FileMode.CreateNew))
                    {
                        using (StreamWriter stream = new StreamWriter(file))
                        {
                            stream.WriteLineAsync(msg + Environment.NewLine);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}