using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.Tokenizer;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3;
using Azure.Core;
using Microsoft.AspNetCore.DataProtection;

namespace EyesOnMeCore
{
    public class AIAPI
    {
        public async Task<string> SendRequest(KeyValuePair<string, string[]> request)
        {
            string datarequested = request.Value[0];
            string target = request.Value[1];
            string purpose = request.Value[2];
            string subject = request.Value[3];
            string returnmail = request.Value[4];

            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "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"
            });
            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromUser(
$@"Refer to the subject of the request as [DEFAULT SUBJECT],to the target of the email as [DEFAULT TARGET], and the email adress that they should respond to as [DEFAULT EMAIL]. 
Do not include any text for the user to fill, e.g. '[your address here]' and list the email as being from  [DEFAULT SUBJECT]
Generate a GDPR Subject Action Request email to be sent to + {target}, the purpose of this request is to {purpose} all relevant data.
ask for this to be done for the data: {datarequested}.")
                    },
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = 3000
            });
            if (completionResult.Successful)
            {
                string result =  completionResult.Choices.First().Message.Content;
                string[] targetname = target.Split('@');
                result = result.Replace("[DEFAULT TARGET]", targetname[0]);
                result =  result.Replace("[DEFAULT SUBJECT]", subject);
                result = result.Replace("Your Name", subject);
                result =  result.Replace("[DEFAULT EMAIL]", returnmail);

                return result;
            }
            else 
            {
                return "FAILED";
            }
        }


    }
}
