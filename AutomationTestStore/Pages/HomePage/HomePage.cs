using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomationTestStore.Pages.HomePage
{
    [Binding]
    internal class HomePage : BasePage
    {
        #region Contructor
        public HomePage(ScenarioContext scenarioContext) : base(scenarioContext) { }
        #endregion

        #region Locators
        private By WelcomeMsg() { return By.CssSelector(".welcome_msg > h4"); }
        #endregion

        #region Verification methods
        /// <summary>
        /// Verify Welcome message in Home page being displayed correctly
        /// </summary>
        /// <param name="expectedMsg"></param>
        public void VerifyWelcomeMsgInHomePageBeingDisplayed(string expectedMsg)
        {
            string actualMsg = GetText(WelcomeMsg());
            Assert.True(actualMsg.Contains(expectedMsg), string.Format("Expected message '{0}' is not equal with actual messge '{1}'", expectedMsg, actualMsg));
        }

        /// <summary>
        /// Verify url of Home page being displayed correctly
        /// </summary>
        /// <param name="url"></param>
        public void VerifyTheHomePageUrlIsDisplayed(string url)
        {
            string currentUrl = GetCurrentUrl();
            Assert.True(url.Equals(currentUrl), string.Format("Expected url '{0}' is not equal with actual url '{1}'",
                        url, currentUrl));
        }
        #endregion
    }
}
