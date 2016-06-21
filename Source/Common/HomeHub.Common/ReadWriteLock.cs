namespace HomeHub.Common
{
    using System;
    using System.Threading;

    /// <summary>
    /// The read write lock.
    /// </summary>
    public class ReadWriteLock
    {
        /// <summary>
        /// The actual lock.
        /// </summary>
        private readonly ReaderWriterLock rwls;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteLock"/> class.
        /// </summary>
        public ReadWriteLock()
        {
            this.rwls = new ReaderWriterLock();
        }

        /// <summary>
        /// Enter a read lock
        /// </summary>
        public ReadLock Read => new ReadLock(this.rwls);

        /// <summary>
        /// Enter a write lock
        /// </summary>
        public WriteLock Write => new WriteLock(this.rwls);
    }
}
