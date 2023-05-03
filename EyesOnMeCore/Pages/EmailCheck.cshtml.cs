using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Drive.v3;
using Google.Protobuf.WellKnownTypes;
using Google.Apis.Gmail.v1.Data;
using System.Text.RegularExpressions;

namespace EyesOnMeCore.Pages
{
    [GoogleScopedAuthorize(GmailService.ScopeConstants.GmailReadonly)]
    public class EmailCheckModel : PageModel
    {
        Dictionary<string, string[]> requestlist = new Dictionary<string, string[]>();

        public async Task OnGet([FromServices] IGoogleAuthProvider auth)
        {
            try
            {
                GoogleCredential cred = await auth.GetCredentialAsync();
                GmailService service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred
                });

                var listRequest = service.Users.Messages.List("me");
                listRequest.LabelIds = "CATEGORY_PROMOTIONS";
                listRequest.IncludeSpamTrash = false;
                listRequest.MaxResults = 500;
                var listRequestResponse = await listRequest.ExecuteAsync();
                Dictionary<string, string> resultList = listRequestResponse.Messages.ToDictionary(messageList => messageList.Id.ToString() , messageList => messageList.ToString());
                
                var toMessageResource = await service.Users.Messages.Get("me", resultList.First().Key).ExecuteAsync();
                string toFull = toMessageResource.Payload.Headers[24].Value.ToString();
                toFull = Regex.Match(toFull, $@"^.*?(?=>)").ToString();
                string[] returnData = toFull.Split('<');

                foreach (KeyValuePair<string, string> messageid in resultList)
                {
                    var messageResource = await service.Users.Messages.Get("me",messageid.Key).ExecuteAsync();

                    string fromFull = messageResource.Payload.Headers[24].Value.ToString();
                    fromFull = Regex.Match(fromFull, $@"^.*?(?=>)").ToString();
                    string[] requestData = fromFull.Split('<');
                    //string[] request = new string[] { datarequested, target, purpose, subject, return };
                    requestlist.Add(Guid.NewGuid().ToString(),new string[] {
                        "All Marketing Related Data",
                        requestData[1],
                        "Request",
                        returnData[0],
                        returnData[1]
                    });
                }

                LegalRequestModel legalRequest = new LegalRequestModel();

                await legalRequest.GenerateRequests(requestlist);
                await legalRequest.SendRequests(requestlist);
                await legalRequest.SaveRequests(requestlist);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
