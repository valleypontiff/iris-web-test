using Microsoft.Extensions.Configuration;

namespace IrisWebTest.Configuration
{
    internal class ConfigHelper
    {
        public static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static EnvironmentConfiguration GetEnvironmentConfiguration()
        {
            var config = new EnvironmentConfiguration();
            IConfigurationRoot rootConfig = GetConfigurationRoot();
            rootConfig
                .GetSection("Environment")
                .Bind(config);

            // validate some settings
            if (config.BaseUrl == null)
            {
                throw new Exception("BaseUrl cannot be null");
            }

            if (config.Browsers == null || config.Browsers.Length == 0)
            {
                throw new Exception("No browsers configured");
            }

            return config;
        }
    }
}
