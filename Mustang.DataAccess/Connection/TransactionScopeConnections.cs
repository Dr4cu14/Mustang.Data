using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Transactions;

namespace Mustang.DataAccess
{
    public static class TransactionScopeConnections
    {
        public static readonly ConcurrentDictionary<Transaction, ConcurrentDictionary<string, DatabaseConnectionWrapper>> TransactionConnections = new ();

        public static DatabaseConnectionWrapper GetConnection(Database db)
        {
            Transaction currentTransaction = Transaction.Current;

            if (currentTransaction == null)
                return null;

            if (!TransactionConnections.TryGetValue(currentTransaction, out var connectionList))
            {
                connectionList = new ConcurrentDictionary<string, DatabaseConnectionWrapper>();
                TransactionConnections.TryAdd(currentTransaction, connectionList);

                currentTransaction.TransactionCompleted += OnTransactionCompleted;
            }

            if (!connectionList.TryGetValue(db.ConnectionString, out var connection))
            {
                var dbConnection = db.GetNewOpenConnection();
                connection = new DatabaseConnectionWrapper(dbConnection);
                connectionList.TryAdd(db.ConnectionString, connection);
            }
            connection.AddRef();

            return connection;
        }

        static void OnTransactionCompleted(object sender, TransactionEventArgs e)
        {
            if (e.Transaction != null)
            {
                if (TransactionConnections.TryGetValue(e.Transaction, out var connectionList))
                {
                    foreach (var connectionWrapper in connectionList.Values)
                    {
                        connectionWrapper.Dispose();
                    }

                    //删除这个事务
                    TransactionConnections.TryRemove(e.Transaction, out connectionList);
                }
            }
        }
    }
}
