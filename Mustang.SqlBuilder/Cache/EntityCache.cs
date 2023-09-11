using System.Collections.Concurrent;

namespace Mustang.SqlBuilder
{
    public class EntityCache
    {
        private static readonly ConcurrentDictionary<string, EntityContext> _entityContextDic = new ConcurrentDictionary<string, EntityContext>();

        public static EntityContext GetEntityContext<T>(T entity)
        {
            return _entityContextDic.GetOrAdd(entity.ToString(), EntityHelper.GetEntityContext(entity));
        }

        //public static EntityContext Get(string entityName)
        //{
        //    _entityContextDic.TryGetValue(entityName, out var entityContext);

        //    return entityContext;
        //}
    }
}
