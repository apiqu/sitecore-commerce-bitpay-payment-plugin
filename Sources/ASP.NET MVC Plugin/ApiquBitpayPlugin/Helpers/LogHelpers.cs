using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Commerce.Services;
using Sitecore.Data.Items;

namespace ApiquBitpayPlugin.Helpers
{
    public static class LogHelpers
    {
        /// <summary>
        /// Logs the errors.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <param name="owner">The owner.</param>
        public static void LogSystemMessages(IEnumerable<SystemMessage> messages, object owner)
        {
            var systemMessages = messages as IList<SystemMessage> ?? messages.ToList();
            if (!systemMessages.Any())
            {
                return;
            }

            foreach (var message in systemMessages)
            {
                Sitecore.Diagnostics.Log.Error(message.Message, owner);
            }
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="owner">The owner.</param>
        public static void LogException(Exception exception, object owner)
        {
            Sitecore.Diagnostics.Log.Error(exception.Message, exception, owner);
        }
        
    }
}