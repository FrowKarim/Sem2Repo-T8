using Tests.Helpers;

namespace Tekken8Tests
{
    public abstract class IntegrationTestBase
    {
        protected IntegrationTestBase()
        {
            DatabaseHelper.ResetDatabase();
        }
    }
}