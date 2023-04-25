using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Discovery.Data;
using Google.Apis.Services;

namespace EyesOnMeCore.Pages
{
    public class EmailCheckModel : PageModel
    {
        public void OnGet()
        {

        }
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Discovery API Sample");
        //    Console.WriteLine("====================");
        //    try
        //    {
        //        //new EmailCheckModel.RunCore().Wait();
        //        new Program().Run().Wait();
        //    }
        //    catch (AggregateException ex)
        //    {
        //        foreach (var e in ex.InnerExceptions)
        //        {
        //            Console.WriteLine("ERROR: " + e.Message);
        //        }
        //    }
        //    Console.WriteLine("Press any key to continue...");
        //    Console.ReadKey();
        //}

        //private async Task RunCore()
        //{
        //    // Create the service.
        //    var service = new DiscoveryService(new BaseClientService.Initializer
        //    {
        //        ApplicationName = "Discovery Sample",
        //        ApiKey = "[YOUR_API_KEY_HERE]",
        //    });

        //    // Run the request.
        //    Console.WriteLine("Executing a list request...");
        //    var result = await service.Apis.List().ExecuteAsync();

        //    // Display the results.
        //    if (result.Items != null)
        //    {
        //        foreach (DirectoryList.ItemsData api in result.Items)
        //        {
        //            Console.WriteLine(api.Id + " - " + api.Title);
        //        }
        //    }
        //}

    }
}
