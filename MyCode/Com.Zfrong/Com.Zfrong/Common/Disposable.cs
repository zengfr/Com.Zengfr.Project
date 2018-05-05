using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong
{
    public abstract class Disposable : MarshalByRefObject, IDisposable
    {
        #region Private Fields.

        private volatile bool _isDisposed;

        #endregion

        #region Public Interface.

        /// <summary>
        /// Gets a value indicating if this instance has been disposed of.
        /// </summary>
        public virtual bool IsDisposed
        {

            get { return _isDisposed; }
            private set { _isDisposed = value; }
        }

        #endregion

        #region Protected Interface.

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        /// <param name="disposing">True if being called explicitly, otherwise; false
        /// to indicate being called implicitly by the GC.</param>
        protected virtual void Dispose(bool disposing)
        {

            if (!this.IsDisposed)
            {
                this.IsDisposed = true;
                // No point calling SuppressFinalize if were are being called from
                // the finalizer.
                if (disposing)
                    GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Helper method to throw a <see cref="System.ObjectDisposedException"/>
        /// if this instance has been disposed of.
        /// </summary>
        protected virtual void CheckDisposed()
        {

            if (this.IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        #endregion

        #region Explicit Interface.

        void IDisposable.Dispose()
        {

            Dispose(true);
        }

        #endregion
    }
}
