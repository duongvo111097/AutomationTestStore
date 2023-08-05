using AutomationTestStore.Contants;
using AutomationTestStore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationTestStore.Driver
{
    [Binding]
    public class InitDriver
    {
        private IWebDriver driver;
        static string browser = JSONReader.GetBrowserFromJSON();
        ScenarioContext scenarioContext;
        public InitDriver(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void InitWebDriver(ScenarioContext scenarioContext)
        {
            switch (browser.ToLower().Trim())
            {
                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    driver = new FirefoxDriver();
                    driver.Manage().Window.Maximize();
                    scenarioContext["driver"] = driver;
                    break;
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    driver = new ChromeDriver();
                    driver.Manage().Window.Maximize();
                    scenarioContext["driver"] = driver;
                    break;
            }
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
