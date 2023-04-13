using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.Tokenizer;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3;
using Azure.Core;

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

            string APIKEy = "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";
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "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"
            });
            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromUser(
$@"Refer to the subject of the request as [DEFAULT SUBJECT] and the email adress that they should respond to as [DEFAULT EMAIL]. 
Do not include any text for the user to fill, e.g. '[your address here]' and list the email as being from  [DEFAULT SUBJECT]
Generate a GDPR Subject Action Request email to be sent to + {target}, the purpose of this request is to {purpose} all relevant data.
ask for this to be done for the data: {datarequested}.")
                    },
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = 3000
            });
            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            }
            else 
            {
                return "FAILED";
            }
        }


    }
}
