using AutomationTestStore.Contants;
using Newtonsoft.Json.Bson;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace AutomationTestStore.Pages
{
    [Binding]
    internal class BasePage
    {
        #region Declare variables
        IWebDriver driver;
        ScenarioContext scenarioContext;
        #endregion

        #region Constructor
        public BasePage(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            driver = (IWebDriver)scenarioContext["driver"];
        }
        #endregion

        #region Locators
        string categoryMenuStr = "//ul[contains(@class,'categorymenu')]//a[contains(text(),'{0}')]";
        string subCategoryMenuStr = "//following-sibling::div//a[contains(text(),'{1}')]";
        public By CategoryMenu(string item) { return By.XPath(string.Format(categoryMenuStr, item)); }
        public By SubCategoryMenu(string item) { return By.XPath(string.Format("//div//a[contains(text(),'{0}')]", item)); }
        public By SubCategoryMenu(string category, string menu) { return By.XPath(string.Format(AppendString(categoryMenuStr, subCategoryMenuStr), menu, category)); }
        public By PageTitle(string page) { return By.XPath(string.Format("//span[@class='maintext' and text()='T-shirts']", page)); }
        public By TxtPriceAllItems() { return By.XPath("//div[@class='fixed_wrapper']//following-sibling::div//div[@class='oneprice']"); }
        public By TxtPriceItem(string item) { return By.XPath(string.Format("//div//a[text()='{0}']//parent::div//parent::div//following-sibling::div//div[@class='oneprice']", item)); }
        public By SortSelectOption() { return By.Id("sort"); }
        public By BtnAddToCartOnPage(string item, string page) { return By.XPath(string.Format("//span[@class='maintext' and text()='{0}']//following::div//a[text()='{1}']//parent::div//parent::div//following-sibling::div//a", page, item)); }
        #endregion

        #region General action methods
        /// <summary>
        /// Navigate to url
        /// </summary>
        /// <param name="url"></param>
        public void NavigateToURL(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Find element with 30 seconds
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public IWebElement FindElement(By locator)
        {
            WaitForLoad();
            IWebElement element = FluentWaitForElement(locator, TimeSpan.FromSeconds(GeneralContants.TIME_TO_WAIT_FOR_ELEMENT_LOADING_IN_SECOND), TimeSpan.FromMilliseconds(GeneralContants.TIME_TO_POLLING_INTERVAL_IN_MILLISECONDS));
            ScrollIntoElementByJS(element);
            return element;
        }

        /// <summary>
        /// Find element with time span
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public IWebElement FindElement(By locator, TimeSpan timeSpan)
        {
            WaitForLoad();
            IWebElement element = FluentWaitForElement(locator, timeSpan, TimeSpan.FromMilliseconds(GeneralContants.TIME_TO_POLLING_INTERVAL_IN_MILLISECONDS));
            ScrollIntoElementByJS(element);
            return element;
        }

        /// <summary>
        /// Find multiple elements
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> FindAllElements(By locator)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
            return driver.FindElements(locator);
        }

        /// <summary>
        /// Click on element
        /// </summary>
        /// <param name="locator"></param>
        public void ClickOnElement(By locator)
        {
            bool staleElement = true;
            bool notInterceptedElement = true;
            int count = 0;
            while (staleElement && notInterceptedElement && count < 5)
            {
                try
                {
                    FindElement(locator).Click();
                    staleElement = false;
                    notInterceptedElement = false;
                    count++;
                    break;
                }
                catch (ElementClickInterceptedException)
                {
                    notInterceptedElement = true;
                    count++;
                }
                catch (ElementNotInteractableException)
                {
                    FindElement(locator).Click();
                    count++;

                }
                catch (StaleElementReferenceException)
                {
                    staleElement = true;
                    count++;
                }
                catch (NullReferenceException)
                {
                    throw new(string.Format("Locator is null: {0}", locator));
                }
            }
        }

        /// <summary>
        /// Wait for web loading having finished
        /// </summary>
        public void WaitForLoad()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, GeneralContants.TIME_TO_WAIT_FOR_WEB_LOADING_IN_SECOND));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString().Trim() == "complete");
        }

        /// <summary>
        /// Wait for element is displayed
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timesout"></param>
        /// <param name="pollingInterval"></param>
        /// <returns></returns>
        /// <exception cref="NoSuchElementException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="WebDriverTimeoutException"></exception>
        public IWebElement FluentWaitForElement(By locator, TimeSpan timesout, TimeSpan pollingInterval)
        {
            IWebElement element = FluentWaitSettings(timesout, pollingInterval).Until(x =>
            {
                IWebElement fluentWaitElement;
                try { fluentWaitElement = x.FindElement(locator); }
                catch (NoSuchElementException ex) { throw new NoSuchElementException(@"Element is not found: ", ex); }
                catch (NullReferenceException ex) { throw new NullReferenceException(@"Null Reference Exception", ex); }
                catch (WebDriverTimeoutException ex) { throw new WebDriverTimeoutException(@"WebDriver Timeout", ex); }
                return fluentWaitElement;
            });
            return element;
        }

        /// <summary>
        /// Scroll into element
        /// </summary>
        /// <param name="element"></param>
        public void ScrollIntoElementByJS(IWebElement element)
        {
            try
            {
                String scrollElementIntoMiddle = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                                            + "var elementTop = arguments[0].getBoundingClientRect().top;"
                                            + "window.scrollBy(0, elementTop-(viewPortHeight/2));";

                ((IJavaScriptExecutor)driver).ExecuteScript(scrollElementIntoMiddle, element);
            }

            catch (WebDriverException ex) { throw ex; }
        }

        /// <summary>
        /// Setting for fluentwait
        /// </summary>
        /// <param name="timesout"></param>
        /// <param name="pollingInterval"></param>
        /// <returns></returns>
        public DefaultWait<IWebDriver> FluentWaitSettings(TimeSpan timesout, TimeSpan pollingInterval)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = timesout;
            fluentWait.PollingInterval = pollingInterval;
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            return fluentWait;
        }

        /// <summary>
        /// Get current url
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUrl()
        {
            WaitForLoad();
            return driver.Url;
        }

        /// <summary>
        /// Get text from element
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public string GetText(By locator)
        {
            string text = "";
            int loop = 5;
            int i = 0;
            IWebElement element;
            while (i < loop)
            {
                element = FindElement(locator, TimeSpan.FromSeconds(10));
                if (!String.IsNullOrEmpty(element.Text))
                {
                    text = element.Text;
                    break;
                }
                i++;
            }

            return text;
        }

        /// <summary>
        /// Get text from element with stopwatch
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public string GetText(By locator, int timesoutMS = 10000)
        {
            IWebElement element = FindElement(locator, TimeSpan.FromSeconds(GeneralContants.TIME_TO_WAIT_FOR_ELEMENT_LOADING_IN_SECOND));
            string text = element.Text;
            var timer = new Stopwatch();
            timer.Start();
            while (timer.ElapsedMilliseconds < timesoutMS && String.IsNullOrEmpty(text))
            {
                text = FindElement(locator, TimeSpan.FromSeconds(GeneralContants.TIME_TO_WAIT_FOR_ELEMENT_LOADING_IN_SECOND)).Text;
                if(!String.IsNullOrEmpty(text))
                    break;
            }
            timer.Stop();
            return text;
        }

        /// <summary>
        /// Using mouse hover on element
        /// </summary>
        /// <param name="locator"></param>
        public void HoverOnElement(By locator)
        {
            Actions actions = new Actions(driver);

            actions.MoveToElement(FindElement(locator)).Perform();
        }

        /// <summary>
        /// Check element is displayed and enabled
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public bool IsDisplayed(By locator)
        {
            try
            {
                IWebElement element = FindElement(locator);
                return element.Displayed && element.Enabled;
            }
            catch (WebDriverException)
            {
                return false;
            }
        }

        /// <summary>
        /// Concat the multiple strings
        /// </summary>
        /// <param name="strArry"></param>
        /// <returns></returns>
        public string AppendString(params string[] strArry)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            foreach (string str in strArry)
            {
                builder.Append(str);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Select item in option list by text
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="text"></param>
        public void SelectItemInOptionByText(By locator, string text)
        {
            IWebElement element = FindElement(locator);
            if (!element.Equals(null))
            {
                new SelectElement(element).SelectByText(text);
            }
        }

        /// <summary>
        /// Gets attribute of element
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="locator"></param>
        /// <returns></returns>
        public string GetAttribute(string attribute, By locator)
        {
            return FindElement(locator).GetAttribute(attribute);
        }
        #endregion

        #region Wrapper actions
        /// <summary>
        /// Select item in sort element
        /// </summary>
        /// <param name="text"></param>
        public void SelectItemInSortList(string text)
        {
            SelectItemInOptionByText(SortSelectOption(), text);
        }

        /// <summary>
        /// Extract float number in string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string ExtractFloatNumberFromString(string str)
        {
            return Regex.Split(str, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").First();
        }

        /// <summary>
        /// Click on button add to cart
        /// </summary>
        /// <param name="item"></param>
        /// <param name="page"></param>
        public void AddItemIntoCartOnPage(string item, string page)
        {
            scenarioContext["priceItem"] = GetText(TxtPriceItem(item));
            ClickOnElement(BtnAddToCartOnPage(item, page));
        }

        /// <summary>
        /// Hover on an item in menu
        /// </summary>
        /// <param name="item"></param>
        public void HoverOnAnItem(string item)
        {
            HoverOnElement(CategoryMenu(item));
        }

        /// <summary>
        /// Click on category when hover on an item in menu
        /// </summary>
        /// <param name="item"></param>
        public void ClickOnCategory(string item)
        {
            ClickOnElement(SubCategoryMenu(item));
        }
        #endregion

        #region Verification methods
        /// <summary>
        /// Verify the price on items being sorted correctly
        /// </summary>
        /// <param name="sortType"></param>
        public void VerifyThePriceOnItemsBeingSortedCorrectly(string sortType)
        {
            IWebElement[] listElement = FindAllElements(TxtPriceAllItems()).ToArray();
            float previousNumber, followingNumber;
            switch (sortType.Trim())
            {
                case "Price Low > High":
                    for (int i = 0; i < listElement.Length - 1; i++)
                    {
                        previousNumber = float.Parse(ExtractFloatNumberFromString(listElement[i].Text));
                        followingNumber = float.Parse(ExtractFloatNumberFromString(listElement[i + 1].Text));
                        Assert.True(previousNumber < followingNumber, string.Format("The previous number {0} is bigger than following number {1}", previousNumber, followingNumber));
                    }
                    break;
            }
        }

        /// <summary>
        /// Verify user can see the category when hover on menu
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="menu"></param>
        public void VerifyUserCanSeeTheCategoryWhenHoverOnMenu(string categories, string menu)
        {
            string[] categoryArr = categories.Split(",");
            foreach (string category in categoryArr)
            {
                Assert.True(IsDisplayed(SubCategoryMenu(category.Trim(), menu)), string.Format("The {0} category in {1} menu is not displayed", category, menu));
            }
        }

        /// <summary>
        /// Verify the page being displayed
        /// </summary>
        /// <param name="page"></param>
        public void VerifyUserCanSeeThePage(string page)
        {
            Assert.True(IsDisplayed(PageTitle(page)), string.Format("The page {0} is not displayed", page));
        }
        #endregion
    }
}
