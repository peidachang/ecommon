﻿using ECommon.Configurations;
using ECommon.Logging;

namespace ECommon.Log4Net
{
    /// <summary>ENode configuration class Log4Net extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>Use Log4Net as the logger.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseLog4Net(this Configuration configuration)
        {
            return UseLog4Net(configuration, "log4net.config");
        }
        /// <summary>Use Log4Net as the logger.
        /// </summary>
        /// <returns></returns>
        public static Configuration UseLog4Net(this Configuration configuration, string configFile)
        {
            configuration.SetDefault<ILoggerFactory, Log4NetLoggerFactory>(new Log4NetLoggerFactory(configFile));
            return configuration;
        }
    }
}