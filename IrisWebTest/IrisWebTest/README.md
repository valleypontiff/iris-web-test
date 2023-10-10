# Iris Web Test Automation

A simple example of testing Iris's website using C# with NUnit and Selenium. This consists of a data-driven test of the email validation done on the Contact Us page. The data for the test is entirely made up of strings that are not valid email addresses.

## Dependencies

The project was made in VS 2022 and targets .NET 7. It doesn't specifically target Windows 10, but that's the only OS it was run on.

Chrome or Firefox (or both) are required.

## Configuring

Configuration is held in appsettings.json.

- BaseUrl is the root URL of the Iris website.
- Browsers is the array of browsers to run the tests against. Currently only supports Chrome and Firefox.
- SleepTime: sometimes a page needs a bit of time do something, but there isn't an easy and precise way to wait for that thing, so the tests sleep instead.
- WaitTime is the maximum of amount of time that the tests will wait for certain things to happen, like waiting for the page to update after a form is submitted.
