using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;

namespace EyesOnMeCore.Pages
{
    public class EmailCheckModel : PageModel
    {
        public void OnGet()
        {
            GmailOAuth gmailtest = new GmailOAuth();
            gmailtest.RunFull();
            //Runoauth();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Discovery API Sample");
            Console.WriteLine("====================");
            try
            {
                //new EmailCheckModel.RunCore().Wait();
                //new Program().Run().Wait();

            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task RunCore()
        {
           // Create the service.
           var service = new DiscoveryService(new BaseClientService.Initializer
           {
               ApplicationName = "Discovery Sample",
               ApiKey = "[YOUR_API_KEY_HERE]",
           });

           // Run the request.
           Console.WriteLine("Executing a list request...");
            var result = await service.Apis.List().ExecuteAsync();

            // Display the results.
            if (result.Items != null)
            {
                foreach (DirectoryList.ItemsData api in result.Items)
                {
                    Console.WriteLine(api.Id + " - " + api.Title);
                }
            }
        }

        private async Task Runoauth()
        {
            UserCredential credential;

            //using (var stream = new FileStream("%APPDATA%\\Microsoft\\UserSecrets\\aspnet-EyesOnMeCore-B5EE1EA0-8DA3-482D-9523-A140E75D734E\r\n\\secrets.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new[] { GmailService.Scope.GmailMetadata },
            //        "user", CancellationToken.None, new FileDataStore("Gmail.Test"));
            //}

            ClientSecrets clientsecrets = new ClientSecrets
            {
                ClientId = "596717765407-52qi3h1otn9pdpfaeam1j9uluh90rthi.apps.googleusercontent.com",
                ClientSecret = "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"
            };
            FileDataStore filedatastore = new FileDataStore("Gmail.Test");

            string redirectUri = "http://localhost:7147/authorize/";


            //credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //    clientsecrets,
            //    new[] { GmailService.Scope.GmailMetadata },
            //    "user",
            //    CancellationToken.None,
            //    filedatastore
            //    );

            //Create the service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                //HttpClientInitializer = credential,
                ApplicationName = "Dissertation experiments",
            });

            var labels = service.ApplicationName;
            //UsersResource usersResource = new UsersResource(service);
            UsersResource.LabelsResource.ListRequest listrequest = new UsersResource.LabelsResource.ListRequest(service, "me");
            listrequest.CreateRequest();
            listrequest.Execute();
        }

    }
}
