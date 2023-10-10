using IrisWebTest.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace IrisWebTest.Pages
{
    /// <summary>
    /// Contains interactions with the Contact page
    /// </summary>
    internal class ContactPage : LoadableComponent<ContactPage>
    {
        private readonly Uri contactUrl;

        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private readonly TimeSpan calmDownTime;

        private readonly By emailBoxBy = By.Name("your-email");
        private IWebElement EmailBox => driver.FindElement(emailBoxBy);

        private readonly By emailErrorBy = By.XPath("//span[@data-name='your-email']/span");
        private IWebElement EmailError => driver.FindElement(emailErrorBy);

        private readonly By sendMessageButtonBy = By.XPath("//input[@type='submit']");
        private IWebElement SendMessageButton => driver.FindElement(sendMessageButtonBy);

        private readonly By spinnerBy = By.ClassName("wpcf7-spinner");

        private readonly By cookieCancelButtonBy = By.Id("cookie_action_close_header_reject");
        private IWebElement CookieCancelButton => driver.FindElement(cookieCancelButtonBy);

        public ContactPage(IWebDriver driver)
        {
            EnvironmentConfiguration config = ConfigHelper.GetEnvironmentConfiguration();

#pragma warning disable CS8604 // Possible null reference argument (it's validated in ConfigHelper so shouldn't be possible for it to be null here).
            contactUrl = new Uri(config.BaseUrl, "contact");
#pragma warning restore CS8604 // Possible null reference argument.

            this.driver = driver;
            wait = new WebDriverWait(driver, config.WaitTimeout);
            calmDownTime = config.SleepTime;
        }

        public void SubmitForm(string email)
        {
            EmailBox.SendKeys(email);
            bool unsubmitted = true;
            do
            {
                try
                {
                    SendMessageButton.Click();
                    unsubmitted = false;
                }
                catch (ElementClickInterceptedException)
                {
                    // the cookie prompt gets in the way; scroll down until the Send button can be interacted with
                    driver.ExecuteJavaScript("window.scrollBy(0,100)");
                }
            } while (unsubmitted);

            // we're checking completion by waiting for the progress spinner to disappear
            // which means first we have to wait for it to appear
            // ideally, we'd use a wait statement, but if it appears and disappears too quickly, waiting for visibility will timeout
            // instead, sleep for a second or two and then wait for invisibility
            // a better solution would probably be to wait for a success or error message to appear
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(spinnerBy));
            Thread.Sleep(1000);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(spinnerBy));
        }

        public string EmailErrorText
        {
            get
            {
                try
                {
                    return EmailError.Text;
                }
                catch (NoSuchElementException)
                {
                    return string.Empty;
                }
            }
        }

        protected override bool EvaluateLoadedStatus()
        {
            return EmailBox.Displayed;
        }

        protected override void ExecuteLoad()
        {
            driver.Navigate().GoToUrl(contactUrl);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(sendMessageButtonBy));

            // the cookie prompt always comes up, but it takes longer, so we'll wait for it, too
            // if we're in an env where the cookie prompt isn't displayed, we'll need to make changes
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(cookieCancelButtonBy));

            // give the page a few seconds to calm down; this is especially needed when running a bunch of browsers in parallel
            Thread.Sleep(calmDownTime);
        }
    }
}
