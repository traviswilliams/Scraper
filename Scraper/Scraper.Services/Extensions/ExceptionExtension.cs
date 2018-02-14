using System;
using System.Text;

namespace Scraper.Services.Extensions
{
    public static class ExceptionExtension
    {
        public static string GetFullExceptionMessage(this Exception ex)
        {
            var message = new StringBuilder();

            var currentException = ex;

            while(ex != null)
            {
                message.AppendLine(ex.Message);

                if (ex.InnerException != null)
                    message.AppendLine("Inner Exception:");

                ex = ex.InnerException;
            }

            return message.ToString();
        }
    }
}
