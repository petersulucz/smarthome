namespace HomeHub.Administration.Service
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.ServiceProcess;
    using System.Threading;

    using HomeHub.Common.Trace;

    /// <summary>
    /// The program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string [] args)
        {
            var service = new AdministrationService() { CanStop = true, };
            if (args.Any())
            {
                var listener = new ConsoleListener();
                listener.EnableEvents(HomeHubEventSource.Log, EventLevel.Informational);

                ThreadPool.QueueUserWorkItem(service.StartService);
                Console.WriteLine("Services started. Waiting. Press enter to exit.");
                Console.Read();
                service.StopService();
            }
            else
            {
                ServiceBase.Run(service);
            }
        }
    }
}
