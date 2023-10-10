using IrisWebTest.Browsers;
using IrisWebTest.Configuration;
using IrisWebTest.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace IrisWebTest
{
    public class ContactPageTests
    {
        private readonly ThreadLocal<IWebDriver> driver = new();

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
            if (driver.Value != null)
            {
                driver.Value.Quit();
                driver.Value.Dispose();
            }
        }

        private static IEnumerable<object[]> BrowserTypes => EnvironmentConfiguration.BrowserTypes;

        private static IEnumerable<TestCaseData> InvalidEmailData
        {
            get
            {
                string[] badEmails = { "a", "a@", "a@aa", "a@a.a", "@a.com", "a@.com", "a@a.com.", "a..a@a.com", "a@a..com", "a@a a.com", "a@a)a.com" };
                foreach (var browserType in BrowserTypes)
                {
                    foreach (var email in badEmails)
                    {
                        yield return new TestCaseData(browserType[0], email);
                    }
                }
            }
        }

        [TestCaseSource(nameof(InvalidEmailData))]
        [Parallelizable(ParallelScope.All)]
        public void ShouldShowErrorOnInvalidEmail(BrowserType browserType, string email)
        {
            driver.Value = WebDriverFactory.Create(browserType);
            ContactPage contactPage = new ContactPage(driver.Value).Load();
            contactPage.SubmitForm(email);
            Assert.That(contactPage.EmailErrorText, Is.EqualTo("The Email-id address entered is invalid."));
        }
    }
}