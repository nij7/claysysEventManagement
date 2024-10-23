using log4net;

namespace EventManagement
{
    public class ErrorLog
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ErrorLog));

        public void LogInfo(string message)
        {
            log.Info(message);
        }

        public void LogDebug(string message)
        {
            log.Debug(message);
        }

        public void LogError(string message)
        {
            log.Error(message);
        }

        public void LogWarn(string message)
        {
            log.Warn(message);
        }
    }
}
