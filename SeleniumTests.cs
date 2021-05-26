using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Reflection;

namespace Lab3Task1KPI
{
	public class Tests
	{
		private static IWebDriver _driver;
		private string _url;
		private WebDriverWait _wait;

		[SetUp]
		public void Setup()
		{
			_url = "https://makeup.com.ua/";
			_driver = new ChromeDriver(Environment.CurrentDirectory);
			_wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(15));
		}

		[Test]
		public void OpenPage_CheckOpening_Correct()
		{
			var expected = "© MAKEUP 2009—2021";
			var actual = ActionsOnPage.OpenPage(_driver, _url);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SearchProduct_CheckSearching_Correct()
		{
			var productName = "парфюм";
			var result = ActionsOnPage.SearchProduct(_driver, _url, _wait, productName);
			result.Should().MatchRegex(@"Результаты поиска по запросу «?\w*?». Найдено\s?\w*? \d* товар\w*.");
		}

		[Test]
		public void AddProductToCart_CheckAdding_Correct()
		{
			var productName = "парфюм";
			var productTitle = ActionsOnPage.AddProductToCart(_driver, _url, _wait, productName).ToLower();
			productTitle.Should().Contain(productName);
		}

		[TearDown]
		public void TearDown()
		{
			_driver.Close();
		}
	}
}