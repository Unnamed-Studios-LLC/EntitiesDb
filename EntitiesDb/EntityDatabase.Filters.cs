using EntitiesDb.Components;
using EntitiesDb.Queries;

namespace EntitiesDb
{
    public partial class EntityDatabase
    {
        public QueryFilter Any<T1>()
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.Any<T1>();
            return queryFilter;
        }

        public QueryFilter Any<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.Any<T1, T2>();
            return queryFilter;
        }

        public QueryFilter Any<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.Any<T1, T2, T3>();
            return queryFilter;
        }

        public QueryFilter Any<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.Any<T1, T2, T3, T4>();
            return queryFilter;
        }

        public QueryFilter Any<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.Any<T1, T2, T3, T4, T5>();
            return queryFilter;
        }

        public QueryFilter Any<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.Any<T1, T2, T3, T4, T5, T6>();
            return queryFilter;
        }

        public QueryFilter IncludeDisabled()
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.IncludeDisabled();
            return queryFilter;
        }

        public QueryFilter No<T1>()
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.No<T1>();
            return queryFilter;
        }

        public QueryFilter No<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.No<T1, T2>();
            return queryFilter;
        }

        public QueryFilter No<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.No<T1, T2, T3>();
            return queryFilter;
        }

        public QueryFilter No<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.No<T1, T2, T3, T4>();
            return queryFilter;
        }

        public QueryFilter No<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.No<T1, T2, T3, T4, T5>();
            return queryFilter;
        }

        public QueryFilter No<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.No<T1, T2, T3, T4, T5, T6>();
            return queryFilter;
        }

        public QueryFilter With<T1>()
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.With<T1>();
            return queryFilter;
        }

        public QueryFilter With<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.With<T1, T2>();
            return queryFilter;
        }

        public QueryFilter With<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.With<T1, T2, T3>();
            return queryFilter;
        }

        public QueryFilter With<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.With<T1, T2, T3, T4>();
            return queryFilter;
        }

        public QueryFilter With<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.With<T1, T2, T3, T4, T5>();
            return queryFilter;
        }

        public QueryFilter With<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.With<T1, T2, T3, T4, T5, T6>();
            return queryFilter;
        }
    }
}
