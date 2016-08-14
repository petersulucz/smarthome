using System.ServiceProcess;

namespace HomeHub.Administration.Service
{
    using System.Threading;

    using HomeHub.Administration.Service.Endpoint;

    using static HomeHub.Common.Trace.HomeHubEventSource;

    public partial class AdministrationService : ServiceBase
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        private EndpointManager endpointManager = null;

        public AdministrationService()
        {
            this.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ThreadPool.QueueUserWorkItem(this.StartService);
        }

        /// <summary>
        /// The start
        /// </summary>
        /// <param name="input">dummy.</param>
        public void StartService(object input)
        {
            Log.Startup();
            this.endpointManager = new EndpointManager(this.tokenSource.Token, 8001);
            this.endpointManager.Open();
        }

        /// <summary>
        /// The stop.
        /// </summary>
        public void StopService()
        {
            Log.Shutdown();
            this.endpointManager.Close();
            this.tokenSource.Cancel();
        }

        protected override void OnStop()
        {
            this.StopService();
        }
    }
}
