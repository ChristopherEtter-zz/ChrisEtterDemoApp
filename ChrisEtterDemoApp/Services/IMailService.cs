namespace ChrisEtterDemoApp.Services
{
    public interface IMailService
    {
        void SendMessage(string to, string name, string email, string subject, string message);
    }
}