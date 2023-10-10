using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace IrisWebTest.Browsers
{
    internal class WebDriverFactory
    {
        public static IWebDriver Create(BrowserType browserType)
        {
            IWebDriver driver;

            switch (browserType)
            {
                case BrowserType.Chrome:
                    driver = new ChromeDriver();
                    break;

                case BrowserType.Firefox:
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                    service.Host = "::1";
                    driver = new FirefoxDriver(service);
                    break;

                default:
                    throw new NotImplementedException($"{browserType} not implemented");
            }

            return driver;
        }
    }
}
