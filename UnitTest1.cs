using System;
using System.Collections.Generic;
using Lab1KPI4;
using NUnit.Framework;

namespace Project1.UnitTests
{
    [TestFixture]
    public class Lab1Tests
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
            try
            {
                // act
                throw instance;
            }
            catch (Exception e)
            {
                // assert
                Assert.AreEqual(expectedResult, new Lab1().IsCritical(e));
                return;
            }
        }

        [Test]
        public void CountExceptions_CounterValues_Correct()
        {
            // arrange
            var criticalExceptions = new List<Type>()
                  {
                    typeof(DivideByZeroException),
                    typeof(OutOfMemoryException),
                    typeof(StackOverflowException),
                    typeof(InsufficientMemoryException),
                    typeof(InsufficientExecutionStackException)
                  };
            var nonCriticalExceptions = new List<Type>()
                  {
                    typeof(ArgumentNullException),
                    typeof(ArgumentOutOfRangeException),
                    typeof(NullReferenceException),
                    typeof(AccessViolationException),
                    typeof(IndexOutOfRangeException),
                    typeof(InvalidOperationException)
                  };

            var lab1 = new Lab1();

            // act
            foreach (var item in criticalExceptions)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab1.CountExceptions(instance);
            }
            foreach (var item in nonCriticalExceptions)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab1.CountExceptions(instance);
            }

            // assert
            Assert.AreEqual(lab1.CounterCriticalExceptions, criticalExceptions.Count);
            Assert.AreEqual(lab1.CounterNotCriticalExceptions, nonCriticalExceptions.Count);
        }

        [Test]
        public void CountExceptions_InitCounters_Zero()
        {
            // arrange
            var lab1 = new Lab1();

            // act: nothing

            // assert
            Assert.AreEqual(lab1.CounterCriticalExceptions, 0);
            Assert.AreEqual(lab1.CounterNotCriticalExceptions, 0);
        }
    }
}