using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using XLeech.Core.Model;

namespace XLeech.Core.Service
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ChatGPTService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration;
        }

        public async Task<string> GetChatCompletionAsync(string question)
        {
            var httpClient = _httpClientFactory.CreateClient("ChtpGPT");

            ChatCompletionRequest completionRequest = new()
            {
                Model = "gpt-3.5-turbo",
                MaxTokens = 1000,
                Messages = new List<Message>() {
                                new Message()
                                {
                                    Role = "user",
                                    Content = question,

                                }
                            }
            };

            using var httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            httpReq.Headers.Add("Authorization", $"Bearer {_configuration["OpenAIKey"]}");

            string requestString = JsonSerializer.Serialize(completionRequest);
            httpReq.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

            using HttpResponseMessage? httpResponse = await httpClient.SendAsync(httpReq);
            httpResponse.EnsureSuccessStatusCode();

            var completionResponse = httpResponse.IsSuccessStatusCode ? JsonSerializer.Deserialize<ChatCompletionResponse>(await httpResponse.Content.ReadAsStringAsync()) : null;

            return completionResponse.Choices?[0]?.Message?.Content;

        }
    }
}
