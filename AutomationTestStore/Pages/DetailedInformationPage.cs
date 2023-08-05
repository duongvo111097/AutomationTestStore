using AutomationTestStore.Contants;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace AutomationTestStore.Pages
{
    [Binding]
    internal class DetailedInformationPage : BasePage
    {
        #region Declared variables
        ScenarioContext scenarioContext;
        #endregion

        #region Contructor
        public DetailedInformationPage(ScenarioContext scenarioContext) : base(scenarioContext) 
        {
            this.scenarioContext = scenarioContext;
        }
        #endregion

        #region Locators
        public By ItemName() { return By.ClassName("bgnone"); }
        public By ItemPrice() { return By.ClassName("productfilneprice"); }
        public By TotalPrice() { return By.XPath("//label[contains(text(),'Total Price')]//span[@class='total-price']"); }
        public By ProductQuantity() { return By.Id("product_quantity"); }
        #endregion

        #region Verification methods
        /// <summary>
        /// Verify added item name into cart with item name in detailed information page being correctly
        /// </summary>
        /// <param name="expected"></param>
        public void VerifyAddedItemNameToCartBeingCorrectly(string expected)
        {
            string actual = GetText(ItemName(), GeneralConstants.TIME_TO_WAIT_FOR_ELEMENT_LOADING_IN_MILLISECONDS).Trim();
            Assert.True(actual == expected, "The added item '{0}' name is not match with the item '{1}' name in detailed informatoin page", expected, actual);
        }

        /// <summary>
        /// Verify added item price into cart with item price in detailed information page being correctly
        /// </summary>
        /// <param name="expected"></param>
        public void VerifyAddedItemPriceToCartBeingCorrectly(string expected)
        {
            string actual = GetText(ItemPrice(), GeneralConstants.TIME_TO_WAIT_FOR_ELEMENT_LOADING_IN_MILLISECONDS).Trim();
            Assert.True(actual == expected, "The added item '{0}' price is not match with the item '{1}' price in detailed informatoin page", expected, actual);
        }

        /// <summary>
        /// Verify added item price into cart with item total price in detailed information page being correctly
        /// </summary>
        /// <param name="expected"></param>
        public void VerifyAddedItemTotalPriceToCartBeingCorrectly(string expected)
        {
            string actual = GetText(TotalPrice(), GeneralConstants.TIME_TO_WAIT_FOR_ELEMENT_LOADING_IN_MILLISECONDS).Trim();
            Assert.True(actual == expected, "The added item '{0}' price is not match with the item '{1}' totalprice in detailed informatoin page", expected, actual);
        }

        /// <summary>
        /// Verify total price coressponding with quantity of product in detailed information page being correctly
        /// </summary>
        /// <param name="expected"></param>
        public void VerifyTotalPriceBeingCalculatedCorectly()
        {
            float totalPrice = float.Parse(ExtractFloatNumberFromString(GetText(TotalPrice()).Trim()));
            int productQuantity = int.Parse(GetAttribute("value", ProductQuantity()));
            float calculatedPrice = totalPrice * productQuantity;
            Assert.True(totalPrice == calculatedPrice, "The total price {0} is not calculated corresponding with quantity {1} correctly", totalPrice, calculatedPrice);
        }
        #endregion
    }
}
