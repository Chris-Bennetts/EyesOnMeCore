using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Drive.v3;
using Google.Protobuf.WellKnownTypes;
using Google.Apis.Gmail.v1.Data;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Google.Apis.Gmail.v1.UsersResource;

namespace EyesOnMeCore.Pages
{
    [GoogleScopedAuthorize(GmailService.ScopeConstants.GmailReadonly)]
    public class EmailCheckModel : PageModel
    {
        Dictionary<string, string[]> requestlist = new Dictionary<string, string[]>();

        public async Task OnGet([FromServices] IGoogleAuthProvider auth)
        {
            try
            {
                GoogleCredential cred = await auth.GetCredentialAsync();
                GmailService service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred
                });

                var listRequest = service.Users.Messages.List("me");
                listRequest.LabelIds = "CATEGORY_PROMOTIONS";
                listRequest.IncludeSpamTrash = false;
                listRequest.MaxResults = 500;
                var listRequestResponse = await listRequest.ExecuteAsync();
                Dictionary<string, string> resultList = listRequestResponse.Messages.ToDictionary(messageList => messageList.Id.ToString() , messageList => messageList.ToString());
                
                var toMessageResource = await service.Users.Messages.Get("me", resultList.First().Key).ExecuteAsync();
                //Dictionary<string, string> keyValuePairs = toMessageResource.Payload.Headers.ToDictionary(messageList => messageList.Name.ToString(), messageList => messageList.Value.ToString());
                //string toFull = keyValuePairs["To"];
                string toFull = toMessageResource.Payload.Headers[20].Value.ToString();
                //toFull = Regex.Match(toFull, $@"^.*?(?=>)").ToString();

                Dictionary<string, string> namedict = toMessageResource.Payload.Headers.ToDictionary(messageList => Guid.NewGuid().ToString(), messageList => messageList.Name.ToString());
                Dictionary<string, string> valuedict = toMessageResource.Payload.Headers.ToDictionary(messageList => Guid.NewGuid().ToString(), messageList => messageList.Value.ToString());

                int postion = 0;
                int count = 0;
                foreach (KeyValuePair<string, string> keylocation in namedict)
                {
                    if (keylocation.Value == "To")
                    {
                        postion = count;
                    }
                    count += 1;
                }

                toFull = valuedict.Values.ElementAt(postion);

                string[] returnData = toFull.Split('@');

                foreach (KeyValuePair<string, string> messageid in resultList)
                {
                    var messageResource = await service.Users.Messages.Get("me",messageid.Key).ExecuteAsync();

                    string fromFull = messageResource.Payload.Headers[16].Value.ToString();
                    //Dictionary<string, string> fromKeyValuePairs = toMessageResource.Payload.Headers.ToDictionary(messageList => messageList.Name.ToString(), messageList => messageList.Value.ToString());
                    //string fromFull = fromKeyValuePairs["From"];
                    Dictionary<string, string> fromnamedict = toMessageResource.Payload.Headers.ToDictionary(messageList => Guid.NewGuid().ToString(), messageList => messageList.Name.ToString());
                    Dictionary<string, string> fromvaluedict = toMessageResource.Payload.Headers.ToDictionary(messageList => Guid.NewGuid().ToString(), messageList => messageList.Value.ToString());
                    
                    int frompostion = 0;
                    int fromcount = 0;
                    foreach (KeyValuePair<string,string> keylocation in fromnamedict)
                    {
                        if (keylocation.Value == "From")
                        {
                            frompostion = fromcount;
                        }
                        fromcount += 1;
                    }

                    fromFull = fromvaluedict.Values.ElementAt(frompostion);
                    fromFull = Regex.Match(fromFull, $@"^.*?(?=>)").ToString();
                    string[] requestData = fromFull.Split('<');
                    //string[] request = new string[] { datarequested, target, purpose, subject, return };
                    requestlist.Add(Guid.NewGuid().ToString(),new string[] {
                        "All Marketing Related Data",
                        requestData[1],
                        "Request",
                        returnData[0],
                        toFull,
                    });
                }

                LegalRequestModel legalRequest = new LegalRequestModel();

                await legalRequest.GenerateRequests(requestlist);
                await legalRequest.SendRequests(requestlist);
                await legalRequest.SaveRequests(requestlist);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
    }
}
