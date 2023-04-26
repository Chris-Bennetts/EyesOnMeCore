using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
namespace EyesOnMeCore
{
    /// <summary>
    /// Sample which demonstrates how to use the Books API.
    /// https://developers.google.com/books/docs/v1/getting_started
    ///// <summary>
    //internal class Program
    public class GmailOAuth
    {
        [STAThread]
        public async Task Main()
        {

        }

        [STAThread]
        public async Task RunFull()
        {
            Console.WriteLine("Books API Sample: List MyLibrary");
            Console.WriteLine("================================");
            try
            {
                new GmailOAuth().Runoauth().Wait();
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

        private async Task Runoauth()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "PUT_CLIENT_ID_HERE",
                    ClientSecret = "PUT_CLIENT_SECRETS_HERE"
                },
                new[] { GmailService.Scope.GmailMetadata },
                "user",
                CancellationToken.None,
                new FileDataStore("Gmail.Test"));

                //credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                //    GoogleClientSecrets.Load(stream).Secrets,
                //    new[] { GmailService.Scope.GmailMetadata },
                //    "user", CancellationToken.None, new FileDataStore("Gmail.Test"));
            }

            // Create the service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Dissertation experiments",
            });

            //var bookshelves = await service.Mylibrary.Bookshelves.List().ExecuteAsync();
            
        }
    }
}
