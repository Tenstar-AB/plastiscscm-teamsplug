﻿using System.Reflection;

namespace TeamsPlug
{
    internal class LogConfig
    {
        internal static string GetLogConfigFile(string basePath)
        {
            return Path.Combine(GetBasePath(basePath), LOG_CONFIG_FILE);
        }

        static string GetBasePath(string basePath)
        {
            if (string.IsNullOrEmpty(basePath))
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return basePath;
        }

        const string LOG_CONFIG_FILE = "teamsplug.log.conf";
    }
}
