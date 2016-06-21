namespace HomeHub.Common
{
    using System;

    /// <summary>
    /// The disposable.
    /// </summary>
    /// <typeparam name="T">
    /// The type of obj to keep
    /// </typeparam>
    public abstract class Disposable<T> : IDisposable
    {
        /// <summary>
        /// The disposable object.
        /// </summary>
        private readonly T disposable;

        /// <summary>
        /// The on dispose.
        /// </summary>
        private readonly Action<T> onDispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposable{T}"/> class. 
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="onDispose">The action to perform on dispose.</param>
        internal Disposable(T obj, Action<T> onDispose)
        {
            this.disposable = obj;
            this.onDispose = onDispose;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.onDispose(this.disposable);
        }
    }
}
