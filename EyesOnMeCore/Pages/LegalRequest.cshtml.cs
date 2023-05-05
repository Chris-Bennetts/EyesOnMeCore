using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Azure.Communication.Email;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Web;
using Azure.Core;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace EyesOnMeCore.Pages
{
    public class LegalRequestModel : PageModel
    {
        public Dictionary<string, string[]> requestlist = new Dictionary<string, string[]>(); /*{ get; set; }*/


        SignInManager<IdentityUser> signInManager;
        UserManager<IdentityUser> userManager;
        EmailManagement emailmanager = new EmailManagement();
        DatabaseAccess databaseaccess = new DatabaseAccess();


        public async Task OnGet()
        {
            try 
            {
                string UserID = User.Identity.Name;

                requestlist = await databaseaccess.GetData($"SELECT TOP 100 * FROM [dbo].[DataManagementRequest] WITH (NOLOCK) WHERE RequestUserID = '{UserID}'");

                //foreach (KeyValuePair<string, string[]> request in requestdataraw)
                //{
                //    string[] requestdetails = request.Value;
                //    requestlist.Add(requestdetails[0], requestdetails);
                //}
            }
            catch
            {

            }
        }


        //public partial class Default : System.Web.UI.Page
        //{
        //    string name = Request.Form["txtName"];
        //}

        public async Task<bool> RunRequests(string rawtext)
        {
            bool succeeded = false;

            string[] request = rawtext.Split('|'); 

            try
            {

                //string[] request = new string[] { datarequested, target, purpose, subject, return };
                Guid id = Guid.NewGuid();
                requestlist.Add(id.ToString(), request);

                await GenerateRequests(requestlist);
                await SendRequests(requestlist);
                await SaveRequests(requestlist);
                
                succeeded = true;
            }
            catch
            {
                succeeded = false;
            }
            return succeeded;

        }

        public async Task<bool> SaveRequests(Dictionary<string, string[]> requestlist)
        {
            bool overallresult = false;
            foreach (KeyValuePair < string,string[]> request in requestlist)
            { 
                string requeststring;
                requeststring = $"INSERT INTO [dbo].[DataManagementRequest] VALUES ('{request.Key}', '{request.Value[4]}', '{request.Value[0]}', '{request.Value[1]}');";

                int result = databaseaccess.SetData(requeststring);
                if(result != 0)
                {
                    overallresult = true;
                }
            }
            if (overallresult)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task GenerateRequests(Dictionary<string, string[]> requestlist)
        {
            AIAPI aiapi = new AIAPI();
            foreach(KeyValuePair<string, string[]> request in requestlist)
            {
                string requestrun = await Task.Run(() => aiapi.SendRequest(request));
                request.Value[0] = requestrun;
            }
        }

        public async Task SendRequests(Dictionary<string, string[]> requestlist)
        {
            string ComsServiceConnectionString = "endpoint=https://eoucomsservice.communication.azure.com/;accesskey=zKICzTtdwWjJop44CaZ0nyxSHbXTN2zqGjUMtlcb0lSaitop+dW0CxG4XargvHJBlFGg1pUyqF5kCZ7w7PBdcw==";
            EmailClient emailClient = new EmailClient(ComsServiceConnectionString);

            foreach (KeyValuePair<string, string[]> request in requestlist)
            {
                await emailmanager.SendMail(request, emailClient);
            }
        }
    }
}
