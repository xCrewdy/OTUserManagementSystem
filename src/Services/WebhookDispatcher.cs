using OTUserManagementSystem.src.Core.Interfaces;
using System.Text;
using System.Text.Json;

namespace OTUserManagementSystem.src.Services
{
    public class WebhookDispatcher : IWebhookDispatcher
    {
        private readonly IHttpClientFactory _http;
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly ILogger<WebhookDispatcher> _logger;

        public WebhookDispatcher(IHttpClientFactory http, IUserRepository repo, IConfiguration config, ILogger<WebhookDispatcher> logger)
        {
            _http = http;
            _repo = repo;
            _config = config;
            _logger = logger;
        }

        public async Task DispatchLoginEventAsync(OTUserManagementSystem.src.Core.Models.User loggingInUser)
        {
            var webhookUrl = _config["Webhook:Url"];
            if (string.IsNullOrEmpty(webhookUrl))
            {
                _logger.LogWarning("Webhook URL not configured; skipping dispatch");
                return;
            }

            var since = DateTime.Now.AddMinutes(-30);
            var active = await _repo.GetUsersLoggedInSinceAsync(since);

            var payload = new
            {
                @event = "user_logged_in",
                timestamp = DateTime.Now.ToString("o"),
                activeUsers = active.Select(u => new {
                    id = u.Id,
                    username = u.Username,
                    email = u.Email,
                    lastLoginAt = u.LastLoginAt?.ToString("o")
                })
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var client = _http.CreateClient();
                var resp = await client.PostAsync(webhookUrl, content);

                if (resp.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Webhook dispatched successfully. Url={Url}, ActiveCount={Count}", webhookUrl, active.Count);
                }
                else
                {
                    _logger.LogError("Webhook dispatch failed. Url={Url}, Status={Status}, Body={Body}", webhookUrl, (int)resp.StatusCode, await resp.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Webhook dispatch error for Url={Url}", webhookUrl);
            }
        }
    }
}
