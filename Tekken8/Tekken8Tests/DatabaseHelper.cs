using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Tests.Helpers
{
    public static class DatabaseHelper
    {
        public static void ResetDatabase()
        {
            var configuration = new TestConfigurationHelper().Configuration;
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string sql = @"
                DELETE FROM Comment;
                DELETE FROM [User];

                DBCC CHECKIDENT ('Comment', RESEED, 0);
                DBCC CHECKIDENT ('User', RESEED, 0);
            ";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}