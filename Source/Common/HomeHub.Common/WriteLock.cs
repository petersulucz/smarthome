namespace HomeHub.Common
{
    using System;
    using System.Threading;

    /// <summary>
    /// The write lock.
    /// </summary>
    public sealed class WriteLock : Disposable<ReaderWriterLock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLock"/> class.
        /// </summary>
        /// <param name="readWriteLock">The lock</param>
        internal WriteLock(ReaderWriterLock readWriteLock)
            : base(readWriteLock,
                (rw) =>
                    {
                        if (rw.IsReaderLockHeld)
                        {
                            rw.ReleaseReaderLock();
                        }
                        else if (rw.IsWriterLockHeld)
                        {
                            rw.ReleaseWriterLock();
                        }
                    })
        {
            if (readWriteLock.IsReaderLockHeld)
            {
                readWriteLock.UpgradeToWriterLock(TimeSpan.FromSeconds(10));
            }
            else
            {
                readWriteLock.AcquireWriterLock(TimeSpan.FromSeconds(10));
            }
        }
    }
}
