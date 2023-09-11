using System;
using System.Data;

namespace Mustang.DataAccess
{

    public class RefCountingDataReader : DataReaderWrapper
    {
        private readonly DatabaseConnectionWrapper _connectionWrapper;


        public RefCountingDataReader(DatabaseConnectionWrapper connection, IDataReader innerReader): base(innerReader)
        {
            if (connection == null)
                throw new ArgumentException("connection");

            if (innerReader == null)
                throw new ArgumentException("innerReader");

            _connectionWrapper = connection;
            _connectionWrapper.AddRef();
        }

        public override void Close()
        {
            if (!IsClosed)
            {
                base.Close();
                _connectionWrapper.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsClosed)
                {
                    base.Dispose(true);
                    _connectionWrapper.Dispose();
                }
            }
        }
    }
}
