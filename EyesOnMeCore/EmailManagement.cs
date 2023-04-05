using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Azure;
using Azure.Communication.Email;

namespace EyesOnMeCore
{
    public class EmailManagement
    {
        EmailClient emailClient = new EmailClient(Environment.GetEnvironmentVariable("endpoint=https://eoutest.communication.azure.com/;accesskey=wD0WPzCupLwH1aYcMPN4J1fweeAMG22mxWlubzyi2SeH4YHnM1pn9RWqh6SMeEJdWpedZ+m1CfHUhBMpnmh91w=="));

        public async Task Main(string[] args)
        {
        }

        public async Task SendMail(KeyValuePair<string, string[]> mailparams)
        {
            var subject = "Welcome to Azure Communication Service Email APIs.";
            var htmlContent = "<html><body><h1>Quick send email test</h1><br/><h4>This email message is sent from Azure Communication Service Email.</h4><p>This mail was send using .NET SDK!!</p></body></html>";
            var sender = "DoNotReply@86d6667f-d86d-4883-81e8-229a7ed1918a.azurecomm.net";
            var recipient = "Christopher.Mac.Bennetts@Gmail.com";

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
