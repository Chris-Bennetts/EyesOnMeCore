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
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Drive.v3;

namespace EyesOnMeCore.Pages
{
    [GoogleScopedAuthorize(GmailService.ScopeConstants.GmailReadonly)]
    public class EmailCheckModel : PageModel
    {
        Dictionary<string, string[]> Requestdata = new Dictionary<string, string[]>();

        public async Task OnGet([FromServices] IGoogleAuthProvider auth)
        {
            //GmailOAuth gmailtest = new GmailOAuth();
            //gmailtest.RunFull();
            //Runoauth();
            try
            {
                GoogleCredential cred = await auth.GetCredentialAsync();
                GmailService service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred
                });
                //var yoda = await service.Users.Labels.List("me").ExecuteAsync();
                // var yada = await service.Users.Labels.Get("me", "CATEGORY_PROMOTIONS").ExecuteAsync();
                //var yeda = await service.Users.Messages.List("me").ExecuteAsync();
                var listRequest = service.Users.Messages.List("me");
                listRequest.LabelIds = "CATEGORY_PROMOTIONS";
                listRequest.IncludeSpamTrash = false;
                listRequest.MaxResults = 500;
                var listRequestResponse = await listRequest.ExecuteAsync();
                Dictionary<string, string> resultList = listRequestResponse.Messages.ToDictionary(yoda => yoda.Id.ToString() , yoda => yoda.ToString());
                
                foreach (KeyValuePair<string, string> messageid in resultList)
                {
                    var messageResource = await service.Users.Messages.Get("me",messageid.Key).ExecuteAsync();
                    Requestdata.Add(messageid.Key, new string[] { messageResource.ToString() });
                }


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            //GmailBaseServiceRequest gmailBaseServiceRequest = new Google.Apis.Gmail.v1.GmailBaseServiceRequest();
            //var files = await service.ExecuteAsync();
            //var fileNames = files.Files.Select(x => x.Name).ToList();
            //return View(fileNames);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Discovery API Sample");
            Console.WriteLine("====================");
            try
            {
                //new EmailCheckModel.RunCore().Wait();
                //new Program().Run().Wait();
                //DriveFileList(auth);

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

        private async Task RunCore(string usertext)
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

        //public async Task<IActionResult> DriveFileList([FromServices] IGoogleAuthProvider auth)
        //{
        //    GoogleCredential cred = await auth.GetCredentialAsync();
        //    var service = new DriveService(new BaseClientService.Initializer
        //    {
        //        HttpClientInitializer = cred
        //    });
        //    var files = await service.Files.List().ExecuteAsync();
        //    var fileNames = files.Files.Select(x => x.Name).ToList();
        //    return View(fileNames);
        //}

        public async Task RunScanAndSend(string textraw)
        { 
            RunCore(textraw);
            LegalRequestModel legalcheck = new LegalRequestModel();
            legalcheck.RunRequests(textraw);



        }

        //public async Task DriveFileList([FromServices] IGoogleAuthProvider auth)
        //{;
        //    GoogleCredential cred = await auth.GetCredentialAsync();
        //    var service = new DriveService(new BaseClientService.Initializer
        //    {
        //        HttpClientInitializer = cred
        //    });
        //    var files = await service.Files.List().ExecuteAsync();
        //    var fileNames = files.Files.Select(x => x.Name).ToList();
        //}

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
