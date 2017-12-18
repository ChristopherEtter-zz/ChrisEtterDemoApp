using Microsoft.Extensions.Logging;

namespace ChrisEtterDemoApp.Services
{
    public class NullMailService : IMailService
    {
        private ILogger<NullMailService> _logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            _logger = logger;
        }

        public void SendMessage(string to, string name, string email, string subject, string message)
        {
            //Log Message
            _logger.LogInformation(string.Format("To: {0} From: {1} Email: {2} Subject: {3} Message: {4}", to, name, email, subject, message));
        }

        
    }
}
