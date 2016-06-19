namespace HomeHub.Common.Trace
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// The home hub event source.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HomeHubEventSource : EventSource
    {
        public class Keywords
        {
            /// <summary>
            /// The general.
            /// </summary>
            public const EventKeywords General = (EventKeywords)1;

            /// <summary>
            /// The data.
            /// </summary>
            public const EventKeywords Data = (EventKeywords)2;

            /// <summary>
            /// The diagnostic.
            /// </summary>
            public const EventKeywords Diagnostic = (EventKeywords)4;

            /// <summary>
            /// The performance
            /// </summary>
            public const EventKeywords Perf = (EventKeywords)8;
        }

        /// <summary>
        /// The tasks.
        /// </summary>
        public class Tasks
        {
            /// <summary>
            /// The page.
            /// </summary>
            public const EventTask Page = (EventTask)1;

            /// <summary>
            /// The db query.
            /// </summary>
            public const EventTask DbQuery = (EventTask)2;
        }

        /// <summary>
        /// The event source.
        /// </summary>
        private static HomeHubEventSource src = new HomeHubEventSource();

        /// <summary>
        /// Prevents a default instance of the <see cref="HomeHubEventSource"/> class from being created. 
        /// </summary>
        private HomeHubEventSource()
        {
            // nothin
        }

        public static HomeHubEventSource Log => src;

        [Event(1, Message = "Startup", Keywords = Keywords.General, Level = EventLevel.Informational)]
        public void Startup()
        {
            this.WriteEvent(1);
        }

        [Event(2, Message = "Shutdown", Keywords = Keywords.General, Level = EventLevel.Informational)]
        public void Shutdown()
        {
            this.WriteEvent(2);
        }

        [Event(3, Keywords = Keywords.Diagnostic, Level = EventLevel.Critical)]
        public void Critical(string message)
        {
            this.WriteEvent(3, message);
        }

        [Event(4, Keywords = Keywords.Diagnostic, Level = EventLevel.Error)]
        public void Error(string message)
        {
            this.WriteEvent(4, message);
        }

        [Event(5, Keywords = Keywords.Diagnostic, Level = EventLevel.Warning)]
        public void Warning(string message)
        {
            this.WriteEvent(5, message);
        }

        [Event(6, Keywords = Keywords.Diagnostic, Level = EventLevel.Informational)]
        public void Info(string message)
        {
            this.WriteEvent(6, message);
        }

        [Event(7, Keywords = Keywords.Diagnostic, Level = EventLevel.Verbose)]
        public void Verbose(string message)
        {
            this.WriteEvent(7, message);
        }

        [Event(8, Keywords = Keywords.General, Level = EventLevel.Verbose)]
        public void MethodEnter()
        {
            var stack = new StackTrace();
            var method = stack.GetFrame(1).GetMethod().Name;
            this.WriteEvent(8, method);
        }

        [Event(9, Keywords = Keywords.General, Level = EventLevel.Verbose)]
        public void MethodLeave()
        {
            var stack = new StackTrace();
            var method = stack.GetFrame(1).GetMethod().Name;
            this.WriteEvent(9, method);
        }

        [Event(10, Keywords = Keywords.Data, Level = EventLevel.Verbose)]
        public void FetchingData(string info)
        {
            this.WriteEvent(10, info);
        }
    }
}
