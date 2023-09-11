using System.Collections.Generic;
using System.Data.Common;
using System.Transactions;

namespace Mustang.DataAccess
{
    public static class TransactionScopeConnections
    {
        public static readonly Dictionary<Transaction, Dictionary<string, DatabaseConnectionWrapper>> TransactionConnections = new Dictionary<Transaction, Dictionary<string, DatabaseConnectionWrapper>>();

        public static DatabaseConnectionWrapper GetConnection(Database db)
        {
            Transaction currentTransaction = Transaction.Current;

            if (currentTransaction == null)
                return null;

            Dictionary<string, DatabaseConnectionWrapper> connectionList;
            DatabaseConnectionWrapper connection;

            lock (TransactionConnections)
            {
                if (!TransactionConnections.TryGetValue(currentTransaction, out connectionList))
                {
                    //连接池中没有当前事务的连接，则创建一个新连接
                    //一个事务可以有多个数据库连接
                    connectionList = new Dictionary<string, DatabaseConnectionWrapper>();
                    TransactionConnections.Add(currentTransaction, connectionList);

                    //重新绑定事务完成事件，用于知道该事务何时完成（释放事务及数据库连接）
                    currentTransaction.TransactionCompleted += OnTransactionCompleted;
                }
            }

            lock (connectionList)
            {
                // 下一步我们将看看是否已经有连接。如果没有，我们将创建一个新的连接并添加它。
                // 对事务的连接列表。
                // 此集合只应由创建事务范围的线程进行修改。
                // 尽管事务范围是活动的。
                //但是，没有任何文件来证实这一点，所以我们在安全方面犯了错误，并锁定。
                if (!connectionList.TryGetValue(db.ConnectionString, out connection))
                {
                    var dbConnection = db.GetNewOpenConnection();
                    connection = new DatabaseConnectionWrapper(dbConnection);
                    connectionList.Add(db.ConnectionString, connection);
                }
                connection.AddRef();
            }

            return connection;
        }

        static void OnTransactionCompleted(object sender, TransactionEventArgs e)
        {
            Dictionary<string, DatabaseConnectionWrapper> connectionList;

            lock (TransactionConnections)
            {
                if (!TransactionConnections.TryGetValue(e.Transaction, out connectionList))
                {
                    //当事务已经不存在的时候直接返回（不知道什么情况会有这种场景）
                    return;
                }

                //删除这个事务
                TransactionConnections.Remove(e.Transaction);
            }

            lock (connectionList)
            {
                //释放当前事务下的全部数据库连接
                foreach (var connectionWrapper in connectionList.Values)
                {
                    connectionWrapper.Dispose();
                }
            }
        }
    }
}
