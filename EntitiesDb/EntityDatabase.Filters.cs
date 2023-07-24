namespace EntitiesDb
{
    public sealed partial class EntityDatabase
    {
        public QueryFilter Any<T1>()
            where T1 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.Any<T1>();
        }

        public QueryFilter Any<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.Any<T1, T2>();
        }

        public QueryFilter Any<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.Any<T1, T2, T3>();
        }

        public QueryFilter Any<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.Any<T1, T2, T3, T4>();
        }

        public QueryFilter Any<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.Any<T1, T2, T3, T4, T5>();
        }

        public QueryFilter Any<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.Any<T1, T2, T3, T4, T5, T6>();
        }

        public QueryFilter IncludeDisabled()
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.IncludeDisabled();
        }

        public QueryFilter No<T1>()
            where T1 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.No<T1>();
        }

        public QueryFilter No<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.No<T1, T2>();
        }

        public QueryFilter No<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.No<T1, T2, T3>();
        }

        public QueryFilter No<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.No<T1, T2, T3, T4>();
        }

        public QueryFilter No<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.No<T1, T2, T3, T4, T5>();
        }

        public QueryFilter No<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.No<T1, T2, T3, T4, T5, T6>();
        }

        public QueryFilter With<T1>()
            where T1 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.With<T1>();
        }

        public QueryFilter With<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.With<T1, T2>();
        }

        public QueryFilter With<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.With<T1, T2, T3>();
        }

        public QueryFilter With<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.With<T1, T2, T3, T4>();
        }

        public QueryFilter With<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.With<T1, T2, T3, T4, T5>();
        }

        public QueryFilter With<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = RentQueryFilter();
            return queryFilter.With<T1, T2, T3, T4, T5, T6>();
        }
    }
}
