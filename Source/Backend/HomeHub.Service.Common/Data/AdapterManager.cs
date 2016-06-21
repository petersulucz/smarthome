namespace HomeHub.Service.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;

    using HomeHub.Adapters.Common;

    using static HomeHub.Common.Trace.HomeHubEventSource;

    /// <summary>
    /// The adapter manager.
    /// </summary>
    public class AdapterManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterManager"/> class. 
        /// </summary>
        /// <param name="adapterFolder">
        /// The adapter Folder.
        /// </param>
        /// <exception cref="Exception">
        /// If we cant find the folder
        /// </exception>
        public AdapterManager(string adapterFolder)
        {
            Log.MethodEnter();

            if (false == Directory.Exists(adapterFolder))
            {
                throw new Exception("NO ADAPTERS FOLDER. LOAD ERROR");
            }

            var catalog = new AggregateCatalog();

            foreach (var folder in Directory.GetDirectories(adapterFolder))
            {
                Log.Info($"Loading adapters in folder {folder}");
                catalog.Catalogs.Add(new DirectoryCatalog(folder));
            }

            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);

            this.AdapterMap = new Dictionary<string, IHomeHubAdapter>();

            foreach (var adapter in this.Adapters)
            {
                Log.Info($"Founder adapter for {adapter.Manufacturer}");
                this.AdapterMap[adapter.Manufacturer] = adapter;
            }

            Log.MethodLeave();
        }

        /// <summary>
        /// Gets the adapter map.
        /// </summary>
        public Dictionary<string, IHomeHubAdapter> AdapterMap { get; }

        /// <summary>
        /// Gets the adapters.
        /// </summary>
        [ImportMany(typeof(IHomeHubAdapter))]
        internal IEnumerable<IHomeHubAdapter> Adapters { get; private set; }
    }
}
