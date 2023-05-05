using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace EyesOnMeCore.Pages
{
    public class AccountModel : PageModel
    {
        public void OnGet()
        {
             AccountData accountdata = new AccountData();
            //accountdata.Connect();
        }
    }

    public class AccountData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Test { get; set; }
        public string Email { get; set; }
        public string PassHash { get; set; }

        public string PassSalt { get; set; }



        //public void Connect()
        //{
        //    string queryString = "SELECT TOP (1000) * FROM [dbo].[Users]";
        //    DatabaseAccess databaseaccess = new DatabaseAccess();
        //    string[] accountinfo = databaseaccess.GetData(queryString);
        //}
    }
}
