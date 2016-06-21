namespace HomeHub.Common
{
    using System.Threading;

    /// <summary>
    /// The read lock.
    /// </summary>
    public class ReadLock : Disposable<ReaderWriterLockSlim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLock"/> class.
        /// </summary>
        /// <param name="readWriteLock">The read write lock</param>
        internal ReadLock(ReaderWriterLockSlim readWriteLock)
            : base(readWriteLock, (rw) => rw.ExitReadLock())
        {
            readWriteLock.EnterReadLock();
        }
    }
}
