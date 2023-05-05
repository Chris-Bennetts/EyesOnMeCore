using System.Text.Json;
using System.Text;

namespace EyesOnMeCore
{
    public class PostHandling
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            try
            {
                using HttpResponseMessage response = await client.GetAsync("https://localhost:7147/PostHandling");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
            }
        }
    }
}

