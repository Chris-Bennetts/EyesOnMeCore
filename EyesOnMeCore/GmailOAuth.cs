using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using static OpenAI.GPT3.ObjectModels.SharedModels.IOpenAiModels;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Drive.v3;

namespace EyesOnMeCore
{
    public class GmailOAuth
    {
        [STAThread]
        public async Task Main()
        {

        }

        public async Task RunFull()
        {
            try
            {
                new GmailOAuth().RunOauthAttempt2().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
        }

        private async Task Runoauth()
        {
            UserCredential credential;


            ClientSecrets clientsecrets = new ClientSecrets
            {
                ClientId = "596717765407-52qi3h1otn9pdpfaeam1j9uluh90rthi.apps.googleusercontent.com",
                ClientSecret = "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",
            };
            FileDataStore filedatastore = new FileDataStore("Gmail.Test");

            string redirectUri = "/authorize/";
                        
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientsecrets,
                new[] { GmailService.Scope.GmailMetadata },
                "user",
                CancellationToken.None,
                filedatastore
                );
            

            var service = new GmailService(new BaseClientService.Initializer()
           {
                HttpClientInitializer = credential,
                ApplicationName = "Dissertation experiments",
            });

            var labels = service.ApplicationName;
            //UsersResource usersResource = new UsersResource(service);
            UsersResource.LabelsResource.ListRequest listrequest =  new UsersResource.LabelsResource.ListRequest(service, "me");
            listrequest.CreateRequest();
            Google.Apis.Gmail.v1.Data.ListLabelsResponse results = listrequest.Execute();
            Console.WriteLine(results.ToString());

        }

        private async Task RunOauthAttempt2()
        {

        }

    }
}
