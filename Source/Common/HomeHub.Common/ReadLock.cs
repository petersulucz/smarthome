namespace HomeHub.Common
{
    using System;
    using System.Threading;

    /// <summary>
    /// The read lock.
    /// </summary>
    public class ReadLock : Disposable<ReaderWriterLock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLock"/> class.
        /// </summary>
        /// <param name="readWriteLock">The read write lock</param>
        internal ReadLock(ReaderWriterLock readWriteLock)
            : base(readWriteLock,
                (rw) =>
                    {
                        rw.ReleaseLock();
                    })
        {
            readWriteLock.AcquireReaderLock(TimeSpan.FromSeconds(10));
        }
    }
}
