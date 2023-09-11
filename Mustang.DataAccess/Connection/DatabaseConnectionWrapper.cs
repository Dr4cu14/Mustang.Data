using System;
using System.Data.Common;
using System.Threading;

namespace Mustang.DataAccess
{
    public class DatabaseConnectionWrapper : IDisposable
    {
        private int refCount;

        public DatabaseConnectionWrapper(DbConnection connection)
        {
            Connection = connection;
            refCount = 1;
        }

        public DbConnection Connection { get; private set; }

        public bool IsDisposed
        {
            get { return refCount == 0; }
        }

        #region IDisposable Members

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "As designed. This is a reference counting disposable.")]
        public void Dispose()
        {
            Dispose(true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "As designed. This is a reference counting disposable.")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                int count = Interlocked.Decrement(ref refCount);
                if (count == 0)
                {
                    Connection.Close();
                    Connection.Dispose();
                    Connection = null;
                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion

        public DatabaseConnectionWrapper AddRef()
        {
            Interlocked.Increment(ref refCount);
            return this;
        }
    }
}
