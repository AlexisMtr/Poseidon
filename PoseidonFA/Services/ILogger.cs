using Microsoft.Azure.WebJobs.Host;
using System;

namespace PoseidonFA.Services
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message, Exception exception = null);
        void Warning(string message);
    }

    public class TraceWriterWrapper : ILogger
    {
        private readonly TraceWriter traceWriter;

        public TraceWriterWrapper(TraceWriter traceWriter)
        {
            this.traceWriter = traceWriter;
        }

        public void Error(string message, Exception exception = null)
        {
            traceWriter.Error(message, exception);
        }

        public void Info(string message)
        {
            traceWriter.Info(message);
        }

        public void Warning(string message)
        {
            traceWriter.Warning(message);
        }
    }
}
