using System.Text.Json;
using System.Text;

namespace EyesOnMeCore
{
    public class PostHandling
    {

        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
        }
    }
}

