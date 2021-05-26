using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3Task2KPI
{
    class Homepage
    {
        private readonly string homeUrl = "https://todomvc.com/examples/angularjs";

        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private IWebElement TodoInput => wait.Until(driver => driver.FindElement(By.ClassName("new-todo")));

        public Homepage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        public Homepage GoToPage()
        {
            driver.Navigate().GoToUrl(homeUrl);
            return this;
        }

        public Homepage AddTask(string taskName)
        {
            TodoInput.SendKeys(taskName);
            TodoInput.SendKeys(Keys.Enter);
            return this;
        }

        public Homepage DeleteTask(string taskName)
        {
            var task = GetAllTasks().FirstOrDefault(t => t.Text == taskName);
            if (task == null)
            {
                throw new ArgumentNullException(taskName);
            }

            var actions = new Actions(driver);
            var selectedTask = GetAllTasks().First(t => t.Text.Contains(taskName));
            var deleteButton = driver.FindElement(By.XPath($"//div[.//label[contains(text(),'{taskName}')]]//button"));
            actions.MoveToElement(selectedTask);
            actions.Click(deleteButton);
            actions.Build().Perform();

            return this;
        }

        public Homepage ChangeTaskStatus(string taskName)
        {
            IWebElement taskStatusButton = driver.FindElement(By.XPath($"//div[.//label[contains(text(),'{taskName}')]]//input[@ng-change='toggleCompleted(todo)']"));
            taskStatusButton.Click();
            return this;
        }

        public Homepage SwitchTab(Tabs tab)
        {
            IWebElement tabElement = tab switch
            {
                Tabs.All => driver.FindElement(By.XPath("//a[contains(text(), 'All')]")),
                Tabs.Active => driver.FindElement(By.XPath("//a[contains(text(), 'Active')]")),
                Tabs.Completed => driver.FindElement(By.XPath("//a[contains(text(), 'Completed')]")),
            };
            tabElement.Click();
            return this;
        }

        public IEnumerable<IWebElement> GetAllTasks()
        {
            return driver.FindElements(By.XPath(@"//label[contains(@class, 'ng-binding')]"));
        }

        public enum Tabs
        {
            All, Active, Completed
        }
    }
}