using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using percy_csharp_selenium;

namespace BstackNetCoreNunit
{
    [TestFixture("firefox","single","single_test",".NetCore Nunit")]
    public class SingleTest : BaseTest
    {
        public Percy percy;

        public SingleTest(String platform, String profile, String session_name, String build) : base(platform, profile, session_name, build) { }

        [Test]
        public void SingleTestCase ()
        {
            percy = new Percy();
            try
            {
                driver.Navigate().GoToUrl("https://bstackdemo.com/");
                percy.Snapshot(driver, "Login Page", null);
                driver.FindElement(By.Id("signin")).Click();
                driver.FindElement(By.CssSelector("#username input")).SendKeys("demouser");
                driver.FindElement(By.CssSelector("#username input")).SendKeys(Keys.Enter);
                driver.FindElement(By.CssSelector("#password input")).SendKeys("testingisfun99");
                driver.FindElement(By.CssSelector("#password input")).SendKeys(Keys.Enter);

                driver.FindElement(By.Id("login-btn")).Click();
                String verifyUser = driver.FindElement(By.ClassName("username")).Text;
                percy.Snapshot(driver, "Home Page", null);
                if (verifyUser.Equals("demouser"))
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Expected\"}}");

                }
                else
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Unexpected\"}}");
                }
                System.Threading.Thread.Sleep(5000);
                SessionDetails sessionDetails = new SessionDetails();
                sessionDetails.GetDetails(driver.SessionId.ToString());

            }
            catch(Exception e)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Something went wrong!\"}}");
                Console.WriteLine(e);
            }

        }
    }
}
