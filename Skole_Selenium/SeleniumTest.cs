using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Skole_Selenium
{
    [TestFixtureSource(typeof(MyFixtureData), "FixtureParms")]
    class SeleniumTest
    {
        private string msg;

        public SeleniumTest(string msg)
        {
            this.msg = msg;
        }
        IWebDriver driver;
        [SetUp]
        public void Initialize()
        {
            driver = new FirefoxDriver();
        }
        [Test]
        public void OpenAppTest()
        {
            driver.Url = "http://skoleprojektwebchat.azurewebsites.net/SeleniumWebChat.html";
            IWebElement frame = driver.FindElement(By.XPath("//iframe"));
            driver.SwitchTo().Frame(frame);
            IWebElement element = driver.FindElement(By.XPath("//input[@class='wc-shellinput']"));
            element.SendKeys(msg);
            IWebElement button = driver.FindElement(By.XPath("//label[@class='wc-send']"));
            button.Click();
            Thread.Sleep(3000);
            var msgs = driver.FindElements(By.XPath("//div[@class='wc-message-wrapper list']"));
            Assert.AreEqual(msgs.Count, 2);
            var msgfrombot = driver.FindElements(By.XPath("//div[@class='wc-message wc-message-from-bot']"));
            Assert.AreEqual(msgfrombot.Count, 1);
            var msgtextfrombot = driver.FindElement(By.XPath("//div[@class='wc-message wc-message-from-bot']/div[1]//p")).Text;
            Assert.AreEqual(msgtextfrombot, "You said " + msg);
        }
        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }



        public class MyFixtureData
        {
            public static IEnumerable FixtureParms
            {
                get
                {
                    yield return new TestFixtureData("hello");
                    yield return new TestFixtureData("bye");
                    yield return new TestFixtureData("hi");
                }
            }
        }
    }
}
