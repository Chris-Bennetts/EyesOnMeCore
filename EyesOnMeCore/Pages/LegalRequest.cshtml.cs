using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace EyesOnMeCore.Pages
{
    public class LegalRequestModel : PageModel
    {
        public Dictionary<string, string[]> RequestList { get; set; }

        SignInManager<IdentityUser> signInManager;
        UserManager<IdentityUser> userManager;
        EmailManagement emailmanager = new EmailManagement();

        public void OnGet()
        {
           
            DatabaseAccess databaseaccess = new DatabaseAccess();

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

            string UserID = "0001";
            //if (SignInManager.IsSignedIn(User))
            //{

            //}
            databaseaccess.GetData($"SELECT TOP 100 * FROM [dbo].[DataManagementRequest] WITH (NOLOCK) WHHERE RequestUserID = {UserID}");
        }
        public void SendRequests(Dictionary<string, string[]> requestlist)
        {
           
            foreach(KeyValuePair<string, string[]> request in requestlist)
            {
                emailmanager.SendMail(request);
            }
        }

    }
}
