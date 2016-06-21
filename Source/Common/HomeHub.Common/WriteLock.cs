namespace HomeHub.Common
{
    using System;
    using System.Threading;

    /// <summary>
    /// The write lock.
    /// </summary>
    public sealed class WriteLock : Disposable<ReaderWriterLockSlim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLock"/> class.
        /// </summary>
        /// <param name="readWriteLock">The lock</param>
        internal WriteLock(ReaderWriterLockSlim readWriteLock)
            : base(readWriteLock, (rw) => rw.ExitWriteLock())
        {
            readWriteLock.TryEnterWriteLock(TimeSpan.FromSeconds(10));
        }
    }
}
