using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace EyesOnMeCore
{
    public class DatabaseAccess
    {
        public string[] GetData(string queryString)
        {
            string[] resultstring = new string[0];
            using (var connection = new System.Data.SqlClient.SqlConnection(
            "Server=tcp:cbennettsdevserver.database.windows.net,1433;" +
            "Database=EOUTesting ;User ID=CBennetts;" +
            "Password=azureVenice2013!;Encrypt=True;" +
            "TrustServerCertificate=False;Connection Timeout=30;"
            ))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    resultstring.Append(reader.GetString(0));
                }
                reader.Close();
            }
            return resultstring;
        }

        public int SetData(string queryString)
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(
            "Server=tcp:cbennettsdevserver.database.windows.net,1433;" +
            "Database=EOUTesting ;User ID=CBennetts;" +
            "Password=azureVenice2013!;Encrypt=True;" +
            "TrustServerCertificate=False;Connection Timeout=30;"
            ))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();
                int rowsEffected = command.ExecuteNonQuery();
                return rowsEffected;
            }
        }
    }
}
