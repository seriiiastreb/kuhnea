using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace K.Platform
{
    public class Logger
    {
        #region Static Members
        private static Logger msInstance;
        private static readonly log4net.ILog msLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static object msSyncObject = new object();
        #endregion Static Members

        #region Initialization
        private Logger()
        {
            // Code that runs on application startup
            log4net.Config.XmlConfigurator.Configure();
        }

        public static Logger Instance
        {
            get
            {

                if (msInstance == null)
                {
                    Monitor.Enter(msSyncObject);
                    if (msInstance == null)
                    {
                        try
                        {
                            msInstance = new Logger();
                        }
                        finally
                        {
                            Monitor.Exit(msSyncObject);
                        }
                    }
                }
                return msInstance;
            }
        }
        #endregion Initialization

        #region Log Info
        public static void LogInfo(string message)
        {
            Instance.WriteInfoMessage(message);
        }

        public static void LogInfo(string message, Exception exceptionObject)
        {
            Instance.WriteInfoMessage(message, exceptionObject);
        }

        private void WriteInfoMessage(string message)
        {
            msLogger.Info(message);
        }

        private void WriteInfoMessage(string message, Exception exceptionObject)
        {
            msLogger.Info(message, exceptionObject);
        }
        #endregion Log Info

        #region Log Warning
        public static void LogWarning(string message)
        {
            Instance.WriteWarningMessage(message);
        }

        public static void LogWarning(string message, Exception exceptionObject)
        {
            Instance.WriteWarningMessage(message, exceptionObject);
        }

        private void WriteWarningMessage(string message)
        {
            msLogger.Warn(message);
        }

        private void WriteWarningMessage(string message, Exception exceptionObject)
        {
            msLogger.Warn(message, exceptionObject);
        }
        #endregion Log Warning

        #region Log Error
        public static void LogError(string message)
        {
            Instance.WriteErrorMessage(message);
        }

        public static void LogError(string message, Exception exceptionObject)
        {
            Instance.WriteErrorMessage(message, exceptionObject);
        }

        private void WriteErrorMessage(string message)
        {
            msLogger.Error(message);
        }

        private void WriteErrorMessage(string message, Exception exceptionObject)
        {
            msLogger.Error(message, exceptionObject);
        }
        #endregion Log Error

        #region Log Fatal
        public static void LogFatal(string message)
        {
            Instance.WriteFatalMessage(message);
        }

        public static void LogFatal(string message, Exception exceptionObject)
        {
            Instance.WriteFatalMessage(message, exceptionObject);
        }

        private void WriteFatalMessage(string message)
        {
            msLogger.Fatal(message);
        }

        private void WriteFatalMessage(string message, Exception exceptionObject)
        {
            msLogger.Fatal(message, exceptionObject);
        }
        #endregion Log Fatal

        #region Log Debug
        public static void LogDebug(string message)
        {
            Instance.WriteDebugMessage(message);
        }

        public static void LogDebug(string message, Exception exceptionObject)
        {
            Instance.WriteDebugMessage(message, exceptionObject);
        }

        private void WriteDebugMessage(string message)
        {
            msLogger.Debug(message);
        }

        private void WriteDebugMessage(string message, Exception exceptionObject)
        {
            msLogger.Debug(message, exceptionObject);
        }
        #endregion Log Debug
    }
}
