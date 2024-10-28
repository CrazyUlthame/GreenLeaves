using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebAPI.Services
{
    public class Http
    {
        private readonly HttpClient _httpClient;
        public Http(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> PostDataAsync(string url, object data)
        {
            // Serializa el objeto a JSON
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Realiza la solicitud POST
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode(); // Lanza una excepción si la respuesta no es exitosa

            return await response.Content.ReadAsStringAsync();
        }
    }
}
