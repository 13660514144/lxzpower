using log4net;
using log4net.Config;
using log4net.Repository;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

namespace HelperTools
{
    public class LogHelper
    {
        private static ILoggerRepository repository { get; set; }
        private static ILog _log;
        private static ILog log
        {
            get
            {
                if (_log == null)
                {
                    Configure();
                }
                return _log;
            }
        }

        public static void Configure(string repositoryName = "NETCoreRepository", string configFile = "log4net.config")
        {
            repository = LogManager.CreateRepository(repositoryName);
            XmlConfigurator.Configure(repository, new FileInfo(configFile));
            _log = LogManager.GetLogger(repositoryName, "");
        }

        public static void Info(string msg)
        {
            log.Info(msg);
        }

        public static void Warn(string msg)
        {
            log.Warn(msg);
        }

        public static void Error(string msg)
        {
            log.Error(msg);
        }
        public static void Error(System.Exception ex)
        {
            JObject Err = new JObject();
            Err.Add(new JProperty("CurrentTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")));
            Err.Add(new JProperty("ErrTrace", ex.StackTrace));
            Err.Add(new JProperty("HTTP", ex.HelpLink));
            Err.Add(new JProperty("ErrSource", ex.TargetSite.ToString()));
            Err.Add(new JProperty("TargetSite", ex.Source));
            Err.Add(new JProperty("ErrMessage", ex.Message));            
            log.Error(JsonConvert.SerializeObject(Err));
        }
    }
}