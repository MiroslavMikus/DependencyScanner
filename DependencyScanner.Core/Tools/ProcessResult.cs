using System;

namespace DependencyScanner.Core.Tools
{
    public class ProcessResult
    {
        public int ReturnCode { get; }
        public string Output { get; }
        public string Errors { get; }
        public Exception Exception { get; set; }

        public ProcessResult(int returnCode, string output, string errors)
        {
            ReturnCode = returnCode;
            Output = output;
            Errors = errors;
        }

        public ProcessResult(Exception exception)
        {
            Exception = exception;
        }
    }
}
