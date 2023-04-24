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


        public async void OnGet()
        {
            
            string examplerequest = $@"

                [Name and address of the organisation]

                [Your name and full postal address]

                [Your contact number]

                [Your email address]

                [The date]

                Dear Sir or Madam

                Subject access request

                [Include your full name and other relevant details to help identify you].

                Please supply the personal data you hold about me, which I am entitled to receive under data protection law, held in:

                [Give specific details of where to search for the personal data you want, for example:

                    my personnel file;
                    emails between ‘person A’ and ‘person B’ (from 1 June 2017 to 1 Sept 2017)
                    my medical records (between 2014 and 2017) held by ‘Dr C’ at ‘hospital D’;
                    the CCTV camera situated at (‘location E’) on 23 May 2017 between 11am and 5pm; and
                    financial statements (between 2013 and 2017) held in account number xxxxx.]

                If you need any more information, please let me know as soon as possible.

                [If relevant, state whether you would prefer to receive the data in a particular electronic format, or printed out].

                It may be helpful for you to know that data protection law requires you to respond to a request for personal data within one calendar month.

                If you do not normally deal with these requests, please pass this letter to your data protection officer or relevant staff member.

                If you need advice on dealing with this request, the Information Commissioner’s Office can assist you. Its website is ico.org.uk, or it can be contacted on 0303 123 1113.

                Yours faithfully

                [Signature]
                ";
            try 
            {
                string UserID = User.Identity.Name;

                string[] requestdataraw = databaseaccess.GetData($"SELECT TOP 100 * FROM [dbo].[DataManagementRequest] WITH (NOLOCK) WHERE RequestUserID = '{UserID}'");

                foreach (string request in requestdataraw)
                {
                    string[] requestdetails = request.Split(',');
                    requestlist.Add(requestdetails[0], requestdetails);
                }
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
