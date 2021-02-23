using System.ComponentModel;
using System;

namespace LW2
{
    public class LW2Factory
    {
        private IListSource _listSource;
        private ITelemetryReporter _telemetryReporter;

        public LW2Factory() { }

        public LW2Factory WithListSource(IListSource src)
        {
            _listSource = src;
            return this;
        }

        public LW2Factory WithTelemetryReporter(ITelemetryReporter rep)
        {
            _telemetryReporter = rep;
            return this;
        }

        public LW2 Build()
        {
            return new LW2(_listSource, _telemetryReporter);
        }
    }
}
