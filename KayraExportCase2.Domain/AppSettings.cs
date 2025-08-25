using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Domain
{
    public class AppSettings
    {
        public static AppSettings? instance;
        public static void Initialize(AppSettings appSettings)
        {
            instance = appSettings;
        }
        public LoggingSettings? Logging { get; set; }
        public string? AllowedHosts { get; set; }
        public ConnectionStrings? ConnectionStrings { get; set; }
        public string JWTSecret { get; set; } = "";

        
    }

    public class LoggingSettings
    {
        public LogLevelSettings? LogLevel { get; set; }
    }

    public class LogLevelSettings
    {
        public string? Default { get; set; }
        public string? MicrosoftAspNetCore { get; set; }
    }

    public class ConnectionStrings
    {
        public string? DefaultConnection { get; set; }
    }

}
