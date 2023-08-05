using AutomationTestStore.Contants;
using AutomationTestStore.Helpers;
using AutomationTestStore.Pages;
using AutomationTestStore.Pages.HomePage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace AutomationTestStore.StepDefinitions
{
    [Binding]
    internal class BaseSteps
    {
        BasePage basePage;
        HomePage homePage;
        DetailedInformationPage detailedInformationPage;
        ScenarioContext scenarioContext;
        BaseSteps(BasePage basePage, HomePage homePage, DetailedInformationPage detailedInformationPage, ScenarioContext scenarioContext)
        {
            this.basePage = basePage;
            this.homePage = homePage;
            this.detailedInformationPage = detailedInformationPage;
            this.scenarioContext = scenarioContext;
        }
        [Given(@"I open can open Chrome")]
        public void GivenIOpenCanOpenChrome()
        {
            basePage.NavigateToURL(JSONReader.GetUrlFromJSON());
        }

        [Then(@"I can see the home page of Automation test Store")]
        public void ThenICanSeeTheOfAutomationTestStore()
        {
            homePage.VerifyTheHomePageUrlIsDisplayed(JSONReader.GetUrlFromJSON());
        }

        [Then(@"I can see the message ""([^""]*)"" in the home page")]
        public void ThenICanSeeTheMessageInTheHomePage(string welcomeMsg)
        {
            homePage.VerifyWelcomeMsgInHomePageIsDisplayed(welcomeMsg);
        }

        [Then(@"I can hover my mouse to ""([^""]*)"" menu")]
        public void ThenICanHoverMyMouseToMenu(string item)
        {
            homePage.HoverOnAnItem(item);
        }

        [Then(@"I can see ""([^""]*)"" categories in ""([^""]*)"" menu")]
        public void ThenICanSeeCategoriesInMenu(string categories, string menu)
        {
            basePage.VerifyUserCanSeeTheCategoryWhenHoverOnMenu(categories, menu);
        }

        [Then(@"I can click on ""([^""]*)"" categories")]
        public void ThenICanClickOnCategories(string item)
        {
            homePage.ClickOnCategory(item);
        }

        [Then(@"I can see ""([^""]*)"" page")]
        public void ThenICanSeeT_ShirtsPage(string page)
        {
            basePage.VerifyUserCanSeeThePage(page);
        }

        [Then(@"I can select Sort by ""([^""]*)"" on T-Shirt page")]
        public void ThenICanSelectSortByOnT_ShirtPage(string sortType)
        {
            basePage.SelectItemInSortList(sortType);
        }

        [Then(@"I can verify that all items were sorted by ""([^""]*)""")]
        public void ThenICanVerifyThatAllItemsWereSortedBy(string sortType)
        {
            basePage.VerifyThePriceOnItemsBeingSortedCorrectly(sortType);
        }

        [Then(@"I add an ""([^""]*)"" item into card on ""([^""]*)"" page")]
        public void ThenIAddAnItemIntoCardOnPage(string item, string page)
        {
            basePage.AddItemIntoCartOnPage(item, page);
            scenarioContext["itemNameAddedToCart"] = item;
        }

        [Then(@"I can see the item detailed information")]
        public void ThenICanSeeTheItemDetailedInformation()
        {
            string itemName = (string)scenarioContext["itemNameAddedToCart"];
            string itemPrice = (string)scenarioContext["priceItem"];
            detailedInformationPage.VerifyAddedItemNameToCartBeingCorrectly(itemName);
            detailedInformationPage.VerifyAddedItemPriceToCartBeingCorrectly(itemPrice);
            detailedInformationPage.VerifyAddedItemTotalPriceToCartBeingCorrectly(itemPrice);
        }

    }
}
