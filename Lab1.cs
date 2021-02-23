using System;
using System.Collections.Generic;

namespace Lab1KPI4
{
	public class Lab1
	{
        public int CounterCriticalExceptions { get; private set; }

        public int CounterNotCriticalExceptions { get; private set; }

        static void Main(string[] args)
        {
            
        }

        public bool IsCritical(Exception exception)
        {
            var criticalExceptions = new List<Type>()
              {
                typeof(DivideByZeroException),
                typeof(OutOfMemoryException),
                typeof(StackOverflowException),
                typeof(InsufficientMemoryException),
                typeof(InsufficientExecutionStackException)
              };
            return criticalExceptions.Contains(exception.GetType());
        }

        public void CountExceptions(Exception exception)
        {
            if (IsCritical(exception))
            {
                CounterCriticalExceptions += 1;
            }
            else
            {
                CounterNotCriticalExceptions += 1;
            }
        }
	}
}
