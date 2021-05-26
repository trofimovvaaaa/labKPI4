using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace Lab3Task1KPI
{
	public static class ActionsOnPage
	{
        public static void main(string [] args)
		{
        }
        public static string OpenPage(this IWebDriver driver, string url)
        {
            //var windowHandles = driver.WindowHandles;
            //var script = string.Format("window.open('{0}', '_blank');", url);
            //((IJavaScriptExecutor)driver).ExecuteScript(script);
            //var newWindowHandles = driver.WindowHandles;
            //var openedWindowHandle = newWindowHandles.Except(windowHandles).Single();
            //driver.SwitchTo().Window(openedWindowHandle);
            driver.Navigate().GoToUrl(url);
            var footer = driver.FindElement(By.ClassName("footer-copy-right"));
            return footer.Text;
        }

        public static string SearchProduct(this IWebDriver driver, string url, WebDriverWait wait, string productName)
		{
            OpenPage(driver, url);
			//driver.Navigate().GoToUrl(url);

			wait.Until(driver => driver.FindElement(By.ClassName("search-button")));

            var inputSearchElement = driver.FindElement(By.ClassName("input"));
            var searchButtonElement = driver.FindElement(By.ClassName("search-button"));

            inputSearchElement.SendKeys(productName);
            searchButtonElement.Click();

            wait.Until(driver => driver.FindElement(By.ClassName("search-results")));
            var resultsElement = driver.FindElement(By.ClassName("search-results"));
            url = driver.Url;
            return resultsElement.Text;
        }

        public static string AddProductToCart(this IWebDriver driver, string url, WebDriverWait wait, string productName)
        {
            OpenPage(driver, url);
            SearchProduct(driver, url, wait, productName);
            var productButton = driver.FindElements(By.ClassName("simple-slider-list__image")).First();
            productButton.Click();
            var buyProductButton = driver.FindElements(By.CssSelector("div.button.buy")).First();
            buyProductButton.Click();
            wait.Until(driver => driver.FindElement(By.CssSelector("div.product__column")));
            var productTitle = driver.FindElements(By.CssSelector("div.product__column")).First().Text;
            return productTitle;
        }
    }
}
