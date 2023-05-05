using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;

namespace EyesOnMeCore
{
    public class DatabaseAccess
    {
        public async Task<Dictionary<string, string[]>>  GetData(string queryString)
        {
            try
            {
                Dictionary<string, string[]> resultsdict = new Dictionary<string, string[]>();

                using (var connection = new System.Data.SqlClient.SqlConnection(
                "Server=tcp:cbennettsdevserver.database.windows.net,1433;" +
                "Database=EOUTesting ;User ID=CBennetts;" +
                "Password=azureVenice2013!;Encrypt=True;" +
                "TrustServerCertificate=False;Connection Timeout=30;"
                ))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);

                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    
                    while (reader.Read())
                    {
                        string[] temp = new string[] { reader[1].ToString(), reader[2].ToString(), reader[3].ToString()};
                        resultsdict.Add(reader[0].ToString(), temp);
                    }
                    reader.Close();
                }
                return resultsdict;
            }
            catch
            {
                return null;
            }
        }

        private static void ReadSingleRow(IDataRecord dataRecord)
        {
            //resultstring.Append(String.Format("{0}, {1}", dataRecord[0], dataRecord[1]));
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
