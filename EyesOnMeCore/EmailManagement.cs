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
            var sender = "EyePrivate@86d6667f-d86d-4883-81e8-229a7ed1918a.azurecomm.net";

            var htmlContent = request.Value[0] + "\r\n<p><small><i> This request was generated automatically from eyesonme.azurewebsites.net A student project built to explore data privacy tools.\r\n for more information, contact Christopher.Mac.Bennetts@Gmail.com</p>";
            var recipient = request.Value[1];
            string purpose = request.Value[2];
            var subject = request.Value[3] + " Subject Action Request";
            string returnmail = request.Value[4];


            try
            {
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    Azure.WaitUntil.Completed,
                    sender,
                    recipient,
                    subject,
                    htmlContent);
                EmailSendResult statusMonitor = emailSendOperation.Value;

                string operationId = emailSendOperation.Id;
            }
            catch (RequestFailedException ex)
            {
                
            }
        }
    }
}
