﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication.Google;
using static OpenAI.GPT3.ObjectModels.SharedModels.IOpenAiModels;

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
            Console.WriteLine("GMAIL API TEST");
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

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientsecrets,
                new[] { GmailService.Scope.GmailMetadata },
                "user",
                CancellationToken.None,
                filedatastore
                );

            //Create the service.
           var service = new GmailService(new BaseClientService.Initializer()
           {
                HttpClientInitializer = credential,
                ApplicationName = "Dissertation experiments",
            });

            var labels = service.ApplicationName;
            //UsersResource usersResource = new UsersResource(service);
            UsersResource.LabelsResource.ListRequest listrequest =  new UsersResource.LabelsResource.ListRequest(service, "me");
            listrequest.CreateRequest();
            listrequest.Execute();
        }
    }
}