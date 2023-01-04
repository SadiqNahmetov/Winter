

namespace FinalProject.Services.Interfaces
{
    public interface IEmailService
    {
        void Send(string to, string subject, string body, string from = null);
    }
}
