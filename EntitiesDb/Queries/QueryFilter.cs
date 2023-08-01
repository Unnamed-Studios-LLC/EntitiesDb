using System;
using System.Collections.Generic;

namespace EntitiesDb
{
    public class QueryFilter : IQueryable
    {
        public bool Parallel = false;
        public readonly HashSet<Type> AnyTypes = new ();
        public readonly HashSet<Type> NoTypes = new ();
        public readonly HashSet<Type> WithTypes = new();

        private readonly EntityDatabase _entityDatabase;

        internal QueryFilter(EntityDatabase entityDatabase)
        {
            _entityDatabase = entityDatabase;
        }

        public static void AssertDelegateComponent<T>(bool isBuffer) where T : unmanaged
        {
            var metaData = ComponentMetaData<T>.Instance;
            if (metaData.ZeroSize) throw new ForEachTagException(metaData.Type);
            if (metaData.Bufferable && !isBuffer) throw new ForEachBufferableException(metaData.Type);
            if (!metaData.Bufferable && isBuffer) throw new ForEachBufferableException(metaData.Type);
        }

        public void Clear()
        {
            AnyTypes.Clear();
            NoTypes.Clear();
            WithTypes.Clear();
            NoTypes.Add(typeof(Disabled));
            Parallel = false;
        }

        public QueryFilter GetQueryFilter() => this;

        public unsafe void Query<TEnumerator>(TEnumerator enumerator) where TEnumerator : IQueryEnumerator
        {
            _entityDatabase.Query(enumerator, this);
        }
    }
}
