using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System;

namespace Lab3Task2KPI
{
    [TestFixture]
    class TodomvcTests
    {
        private IWebDriver driver;
        private Homepage homepage;

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver(Environment.CurrentDirectory);
            homepage = new Homepage(driver);
        }

        [TearDown]
        public void TearDownTest()
        {
            driver.Close();
        }

        [TestCase("Task")]
        [TestCase("Another task")]
        [TestCase("And another task")]
        public void AddTodo(string taskName)
        {
            homepage.GoToPage()
                .AddTask(taskName);
            var tasks = homepage.GetAllTasks();
            Assert.That(tasks.Select(t => t.Text).Contains(taskName));
        }

        [TestCase("Task")]
        [TestCase("Another task")]
        public void RemoveTodo(string taskName)
        {
            homepage.GoToPage()
                .AddTask(taskName)
                .DeleteTask(taskName);
            var tasks = homepage.GetAllTasks();
            Assert.That(!tasks.Any());
        }

        [TestCase("Some task")]
        public void ChangeTodoStatus(string taskName)
        {
            homepage.GoToPage()
                .AddTask(taskName)
                .ChangeTaskStatus(taskName)
                .SwitchTab(Homepage.Tabs.Completed);
            var tasks = homepage.GetAllTasks();
            Assert.That(tasks.Any(t => t.Text.Contains(taskName)));
        }
    }
}