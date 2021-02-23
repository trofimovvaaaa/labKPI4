using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace LW2
{
    public class LW2
    {
        IListSource _exceptionListSource;
        ITelemetryReporter _telemetryReporter;

        public int CounterCriticalExceptions { get; private set; }

        public int CounterNotCriticalExceptions { get; private set; }

        public int ReportFailures { get; private set; }

        public LW2() { }

        static void Main(string[] args)
        {

        }

        public LW2(IListSource src, ITelemetryReporter reporter)
        {
            _exceptionListSource = src;
            _telemetryReporter = reporter;
        }

        public bool IsCritical(Exception exception)
        {
            var criticalExceptions = _exceptionListSource.GetList();
            return criticalExceptions.Contains(exception.GetType());
        }

        public void CountExceptions(Exception exception)
        {
            if (IsCritical(exception))
            {
                CounterCriticalExceptions += 1;
                if (!_telemetryReporter.Report(exception.ToString()))
                {
                    ReportFailures += 1;
                }
            }
            else
            {
                CounterNotCriticalExceptions += 1;
            }
        }

        public IListSource ExceptionListSource
        {
            set { _exceptionListSource = value; }
        }

        public ITelemetryReporter TelemetryReporter
        {
            set { _telemetryReporter = value; }
        }
	}
}
