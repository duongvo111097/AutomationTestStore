@AddItem
Feature: Verify user can add to Card an Item on T-shirts and see the item detailed information

Scenario: Verify user can add to Card an Item on T-shirts and see the item detailed information
	Given I open can open Chrome
	Then I can see the home page of Automation test Store
	And I can see the message "Welcome to the Automation Test Store!" in the home page
	And I can hover my mouse to "Apparel & accessories" menu
	Then I can see "Shoes, T-shirts" categories in "Apparel & accessories" menu
	And I can click on "T-shirts" categories
	Then I can see "T-shirts" page
	And I can select Sort by "Price Low > High" on T-Shirt page
	Then I can verify that all items were sorted by "Price Low > High"
	And I add an "Casual 3/4 Sleeve Baseball T-Shirt" item into card on "T-shirts" page
	And I can see the item detailed information