namespace OTUserManagementSystem.src.Core.Interfaces
{
    public interface IWebhookDispatcher
    {
        Task DispatchLoginEventAsync(OTUserManagementSystem.src.Core.Models.User loggingInUser);
    }
}
