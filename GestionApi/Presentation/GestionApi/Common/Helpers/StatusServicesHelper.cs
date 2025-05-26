using Microsoft.Data.SqlClient;

namespace GestionApi.Common.Helpers
{
    public static class StatusServicesHelper
    {
        public static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }
}
