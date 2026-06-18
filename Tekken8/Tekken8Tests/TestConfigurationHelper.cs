using Microsoft.Extensions.Configuration;

namespace Tests.Helpers
{
    public class TestConfigurationHelper
    {
        public IConfiguration Configuration { get; }

        public TestConfigurationHelper()
        {
            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {
                        "ConnectionStrings:DefaultConnection",
                        "Server=mssqlstud.fhict.local;Database=dbi439179_tekkentest;User ID=dbi439179_tekkentest;Password=TKDB;TrustServerCertificate=True"
                    }
                })
                .Build();
        }
    }
}