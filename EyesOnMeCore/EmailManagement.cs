using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email;
using Azure.Core;

namespace EyesOnMeCore
{
    public class EmailManagement
    {
        public async Task Main(string[] args)
        {
        }

        public async Task SendMail(KeyValuePair<string, string[]> request, EmailClient emailClient)
        {
            //var subject = "Welcome to Azure Communication Service Email APIs.";
            //var htmlContent = "<html><body><h1>Quick send email test</h1><br/><h4>This email message is sent from Azure Communication Service Email.</h4><p>This mail was send using .NET SDK!!</p></body></html>";
            var sender = "EyePrivate@86d6667f-d86d-4883-81e8-229a7ed1918a.azurecomm.net";
            //var recipient = "CMBennetts@proton.me";


            var htmlContent = request.Value[0];
            var recipient = request.Value[1];
            string purpose = request.Value[2];
            var subject = request.Value[3] + "Subject Action Request";
            string returnmail = request.Value[4];


            try
            {
                Console.WriteLine("Sending email...");
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    Azure.WaitUntil.Completed,
                    sender,
                    recipient,
                    subject,
                    htmlContent);
                EmailSendResult statusMonitor = emailSendOperation.Value;
                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }
        }
    }
}
