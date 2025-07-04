using System.Text;
using System.Text.Json;

namespace TeamsPlug
{
    internal class TeamsAPI
    {
        // Does the web reqeuests to Teams API
        private HttpClient sharedClient;
        internal TeamsAPI(string flowUrl)
        {
            sharedClient = new()
            {
                BaseAddress = new Uri(flowUrl),
            };
        }
        internal async Task PostMessage(string message, string recipient)
        {
            await PostAsync(sharedClient, message, recipient);
        }
        private async Task PostAsync(HttpClient httpClient, string message, string recipient)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    message,
                    recipient,
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync(string.Empty, jsonContent);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }
    }
}
