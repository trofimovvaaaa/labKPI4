using System;
using System.ComponentModel;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace LW2.UnitTests
{
    public class TestData
    {
        public static List<Type> CriticalExceptions = new List<Type>()
        {
            typeof(DivideByZeroException),
            typeof(OutOfMemoryException),
            typeof(StackOverflowException),
            typeof(InsufficientMemoryException),
            typeof(InsufficientExecutionStackException)
        };
        public static List<Type> NonCriticalExceptions = new List<Type>()
        {
            typeof(ArgumentNullException),
            typeof(ArgumentOutOfRangeException),
            typeof(NullReferenceException),
            typeof(AccessViolationException),
            typeof(IndexOutOfRangeException),
            typeof(InvalidOperationException)
        };
    }

    [SetUpFixture]
    public class TestSetup
    {
        public static System.ComponentModel.IListSource ListSource;
        public static ITelemetryReporter NormalTelemetryReporter;
        public static ITelemetryReporter FailingTelemetryReporter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ListSource = Substitute.For<IListSource>();
            ListSource.GetList().Returns(TestData.CriticalExceptions);

            NormalTelemetryReporter = Substitute.For<ITelemetryReporter>();
            NormalTelemetryReporter.Report(Arg.Any<String>()).Returns(true);
            FailingTelemetryReporter = Substitute.For<ITelemetryReporter>();
            FailingTelemetryReporter.Report(Arg.Any<String>()).Returns(false);
        }
    }
    [TestFixture]
    public class LW2Tests
    {
        [TestCase(typeof(DivideByZeroException), true)]
        [TestCase(typeof(OutOfMemoryException), true)]
        [TestCase(typeof(StackOverflowException), true)]
        [TestCase(typeof(InsufficientMemoryException), true)]
        [TestCase(typeof(InsufficientExecutionStackException), true)]
        [TestCase(typeof(ArgumentNullException), false)]
        [TestCase(typeof(ArgumentOutOfRangeException), false)]
        [TestCase(typeof(NullReferenceException), false)]
        [TestCase(typeof(AccessViolationException), false)]
        [TestCase(typeof(IndexOutOfRangeException), false)]
        [TestCase(typeof(InvalidOperationException), false)]
        public void IsCritical_CriticalityCheck_Correct(Type exceptionType, bool expectedResult)
        {
            // arrange
            var instance = (Exception)Activator.CreateInstance(exceptionType);
            // 1. Use a LW2Factory
            var lab2 = new LW2Factory()
                .WithListSource(TestSetup.ListSource)
                .WithTelemetryReporter(TestSetup.NormalTelemetryReporter)
                .Build();

            try
            {
                // act
                throw instance;
            }
            catch (Exception e)
            {
                // assert
                Assert.AreEqual(expectedResult, lab2.IsCritical(e));
                return;
            }
        }

        [Test]
        public void CountExceptions_CounterValues_Correct()
        {
            // arrange
            // 2. Use constructor
            var lab2 = new LW2(TestSetup.ListSource, TestSetup.NormalTelemetryReporter);

            // act
            foreach (var item in TestData.CriticalExceptions)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab2.CountExceptions(instance);
            }
            foreach (var item in TestData.NonCriticalExceptions)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab2.CountExceptions(instance);
            }

            // assert
            Assert.AreEqual(lab2.CounterCriticalExceptions, TestData.CriticalExceptions.Count);
            Assert.AreEqual(lab2.CounterNotCriticalExceptions, TestData.NonCriticalExceptions.Count);
        }

        [Test]
        public void CountExceptions_InitCounters_Zero()
        {
            // arrange
            // 3. Use property access
            var lab2 = new LW2();
            lab2.ExceptionListSource = TestSetup.ListSource;
            lab2.TelemetryReporter = TestSetup.NormalTelemetryReporter;

            // act: nothing

            // assert
            Assert.AreEqual(lab2.CounterCriticalExceptions, 0);
            Assert.AreEqual(lab2.CounterNotCriticalExceptions, 0);
        }

        [Test]
        public void TelemetryReport_FailureCounter_Correct()
        {
            // arrange
            var lab20 = new LW2(TestSetup.ListSource, TestSetup.NormalTelemetryReporter);
            var lab21 = new LW2(TestSetup.ListSource, TestSetup.FailingTelemetryReporter);

            // act
            foreach (var item in TestData.CriticalExceptions)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab20.CountExceptions(instance);
                lab21.CountExceptions(instance);
            }

            // assert
            Assert.AreEqual(lab20.ReportFailures, 0);
            Assert.AreEqual(lab21.ReportFailures, TestData.CriticalExceptions.Count);
        }

        [Test]
        public void TelemetryReport_InitCounter_Zero()
        {
            // arrange
            var lab2 = new LW2(TestSetup.ListSource, TestSetup.FailingTelemetryReporter);

            // act: nothing

            // assert
            Assert.AreEqual(lab2.ReportFailures, 0);
        }
    }
}
