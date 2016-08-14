using System;
using System.Linq;

namespace HomeHub.Common.Trace
{
    using System.Diagnostics.Tracing;
    using System.Threading;

    public class ConsoleListener : EventListener
    {
        /// <summary>
        /// The base console color to return to
        /// </summary>
        private const ConsoleColor OriginalColor = ConsoleColor.White;

        /// <summary>Called whenever an event has been written by an event source for which the event listener has enabled events.</summary>
        /// <param name="eventData">The event arguments that describe the event.</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Get a pretty color
            switch (eventData.Level)
            {
                case EventLevel.Critical:
                case EventLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case EventLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case EventLevel.LogAlways:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                default:
                    // Just for consistency
                    Console.ForegroundColor = ConsoleListener.OriginalColor;
                    break;
            }

            // Do the string formatting
            var message = String.Empty;
            if (false == String.IsNullOrWhiteSpace(eventData.Message))
            {
                message = String.Format(eventData.Message, eventData.Payload.ToArray());
            }

            // Write to console
            Console.WriteLine($"[{DateTime.Now}-Thread:{Thread.CurrentThread.ManagedThreadId}] - {message}");

            // Reset color
            Console.ForegroundColor = OriginalColor;
        }
    }

}
