using EntitiesDb.Cache;
using EntitiesDb.Components;

namespace EntitiesDb.Queries
{
    public struct QueryFilter
    {
        internal EntityArchetype AnyFilter;
        internal EntityArchetype NoFilter;
        internal EntityArchetype WithFilter;

        private readonly EntityDatabase _entityDatabase;

        public QueryFilter(EntityDatabase entityDatabase)
        {
            AnyFilter = default;
            NoFilter = default;
            WithFilter = default;
            _entityDatabase = entityDatabase;
            AddtoFilter(ref NoFilter, ComponentRegistry.Type<Disabled>.Id);
        }

        public QueryFilter Any<T1>()
            where T1 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            AddtoFilter(ref AnyFilter, id1);
            return this;
        }

        public QueryFilter Any<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            AddtoFilter(ref AnyFilter, id1);
            AddtoFilter(ref AnyFilter, id2);
            return this;
        }

        public QueryFilter Any<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            AddtoFilter(ref AnyFilter, id1);
            AddtoFilter(ref AnyFilter, id2);
            AddtoFilter(ref AnyFilter, id3);
            return this;
        }

        public QueryFilter Any<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            AddtoFilter(ref AnyFilter, id1);
            AddtoFilter(ref AnyFilter, id2);
            AddtoFilter(ref AnyFilter, id3);
            AddtoFilter(ref AnyFilter, id4);
            return this;
        }

        public QueryFilter Any<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            var id5 = ComponentRegistry.Type<T5>.Id;
            AddtoFilter(ref AnyFilter, id1);
            AddtoFilter(ref AnyFilter, id2);
            AddtoFilter(ref AnyFilter, id3);
            AddtoFilter(ref AnyFilter, id4);
            AddtoFilter(ref AnyFilter, id5);
            return this;
        }

        public QueryFilter Any<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            var id5 = ComponentRegistry.Type<T5>.Id;
            var id6 = ComponentRegistry.Type<T6>.Id;
            AddtoFilter(ref AnyFilter, id1);
            AddtoFilter(ref AnyFilter, id2);
            AddtoFilter(ref AnyFilter, id3);
            AddtoFilter(ref AnyFilter, id4);
            AddtoFilter(ref AnyFilter, id5);
            AddtoFilter(ref AnyFilter, id6);
            return this;
        }

        public QueryFilter IncludeDisabled()
        {
            var disabledId = ComponentRegistry.Type<Disabled>.Id;
            RemoveFromFilter(ref NoFilter, disabledId);
            return this;
        }

        public QueryFilter No<T1>()
            where T1 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            AddtoFilter(ref NoFilter, id1);
            return this;
        }

        public QueryFilter No<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            AddtoFilter(ref NoFilter, id1);
            AddtoFilter(ref NoFilter, id2);
            return this;
        }

        public QueryFilter No<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            AddtoFilter(ref NoFilter, id1);
            AddtoFilter(ref NoFilter, id2);
            AddtoFilter(ref NoFilter, id3);
            return this;
        }

        public QueryFilter No<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            AddtoFilter(ref NoFilter, id1);
            AddtoFilter(ref NoFilter, id2);
            AddtoFilter(ref NoFilter, id3);
            AddtoFilter(ref NoFilter, id4);
            return this;
        }

        public QueryFilter No<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            var id5 = ComponentRegistry.Type<T5>.Id;
            AddtoFilter(ref NoFilter, id1);
            AddtoFilter(ref NoFilter, id2);
            AddtoFilter(ref NoFilter, id3);
            AddtoFilter(ref NoFilter, id4);
            AddtoFilter(ref NoFilter, id5);
            return this;
        }

        public QueryFilter No<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            var id5 = ComponentRegistry.Type<T5>.Id;
            var id6 = ComponentRegistry.Type<T6>.Id;
            AddtoFilter(ref NoFilter, id1);
            AddtoFilter(ref NoFilter, id2);
            AddtoFilter(ref NoFilter, id3);
            AddtoFilter(ref NoFilter, id4);
            AddtoFilter(ref NoFilter, id5);
            AddtoFilter(ref NoFilter, id6);
            return this;
        }

        public QueryFilter With<T1>()
            where T1 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            AddtoFilter(ref WithFilter, id1);
            return this;
        }

        public QueryFilter With<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            AddtoFilter(ref WithFilter, id1);
            AddtoFilter(ref WithFilter, id2);
            return this;
        }

        public QueryFilter With<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            AddtoFilter(ref WithFilter, id1);
            AddtoFilter(ref WithFilter, id2);
            AddtoFilter(ref WithFilter, id3);
            return this;
        }

        public QueryFilter With<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            AddtoFilter(ref WithFilter, id1);
            AddtoFilter(ref WithFilter, id2);
            AddtoFilter(ref WithFilter, id3);
            AddtoFilter(ref WithFilter, id4);
            return this;
        }

        public QueryFilter With<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            var id5 = ComponentRegistry.Type<T5>.Id;
            AddtoFilter(ref WithFilter, id1);
            AddtoFilter(ref WithFilter, id2);
            AddtoFilter(ref WithFilter, id3);
            AddtoFilter(ref WithFilter, id4);
            AddtoFilter(ref WithFilter, id5);
            return this;
        }

        public QueryFilter With<T1, T2, T3, T4, T5, T6>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            var id5 = ComponentRegistry.Type<T5>.Id;
            var id6 = ComponentRegistry.Type<T6>.Id;
            AddtoFilter(ref WithFilter, id1);
            AddtoFilter(ref WithFilter, id2);
            AddtoFilter(ref WithFilter, id3);
            AddtoFilter(ref WithFilter, id4);
            AddtoFilter(ref WithFilter, id5);
            AddtoFilter(ref WithFilter, id6);
            return this;
        }

        public void ForEach<T1>(ComponentFunc<T1> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new ComponentQuery<T1>(func), false);
        }

        public void ForEach<T1, T2>(ComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new ComponentQuery<T1, T2>(func), false);
        }

        public void ForEach<T1, T2, T3>(ComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new ComponentQuery<T1, T2, T3>(func), false);
        }

        public void ForEach<T1, T2, T3, T4>(ComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new ComponentQuery<T1, T2, T3, T4>(func), false);
        }

        public void ForEach<T1, T2, T3, T4, T5>(ComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new ComponentQuery<T1, T2, T3, T4, T5>(func), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, T6>(ComponentFunc<T1, T2, T3, T4, T5, T6> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new ComponentQuery<T1, T2, T3, T4, T5, T6>(func), false);
        }

        public void ForEach<T1, TState>(TState state, ComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new ComponentStateQuery<T1, TState>(func, state), false);
        }

        public void ForEach<T1, T2, TState>(TState state, ComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new ComponentStateQuery<T1, T2, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, TState>(TState state, ComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new ComponentStateQuery<T1, T2, T3, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new ComponentStateQuery<T1, T2, T3, T4, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new ComponentStateQuery<T1, T2, T3, T4, T5, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, T6, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new ComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), false);
        }

        public void ForEach(IdComponentFunc func)
        {
            Query(new IdComponentQuery(func), false);
        }

        public void ForEach<T1>(IdComponentFunc<T1> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new IdComponentQuery<T1>(func), false);
        }

        public void ForEach<T1, T2>(IdComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new IdComponentQuery<T1, T2>(func), false);
        }

        public void ForEach<T1, T2, T3>(IdComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new IdComponentQuery<T1, T2, T3>(func), false);
        }

        public void ForEach<T1, T2, T3, T4>(IdComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new IdComponentQuery<T1, T2, T3, T4>(func), false);
        }

        public void ForEach<T1, T2, T3, T4, T5>(IdComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new IdComponentQuery<T1, T2, T3, T4, T5>(func), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, T6>(IdComponentFunc<T1, T2, T3, T4, T5, T6> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new IdComponentQuery<T1, T2, T3, T4, T5, T6>(func), false);
        }

        public void ForEach<TState>(TState state, IdComponentStateFunc<TState> func)
        {
            Query(new IdComponentStateQuery<TState>(func, state), false);
        }

        public void ForEach<T1, TState>(TState state, IdComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new IdComponentStateQuery<T1, TState>(func, state), false);
        }

        public void ForEach<T1, T2, TState>(TState state, IdComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new IdComponentStateQuery<T1, T2, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, TState>(TState state, IdComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new IdComponentStateQuery<T1, T2, T3, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new IdComponentStateQuery<T1, T2, T3, T4, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new IdComponentStateQuery<T1, T2, T3, T4, T5, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, T6, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new IdComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), false);
        }

        public void ForEach(EntityFunc func) => Query(new EntityQuery(func), false);
        public void ForEach<TState>(TState state, EntityStateFunc<TState> func) => Query(new EntityStateQuery<TState>(func, state), false);

        public void ParallelForEach<T1>(ComponentFunc<T1> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new ComponentQuery<T1>(func), true);
        }

        public void ParallelForEach<T1, T2>(ComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new ComponentQuery<T1, T2>(func), true);
        }

        public void ParallelForEach<T1, T2, T3>(ComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new ComponentQuery<T1, T2, T3>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4>(ComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new ComponentQuery<T1, T2, T3, T4>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5>(ComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new ComponentQuery<T1, T2, T3, T4, T5>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, T6>(ComponentFunc<T1, T2, T3, T4, T5, T6> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new ComponentQuery<T1, T2, T3, T4, T5, T6>(func), true);
        }

        public void ParallelForEach<T1, TState>(TState state, ComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new ComponentStateQuery<T1, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, TState>(TState state, ComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new ComponentStateQuery<T1, T2, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, TState>(TState state, ComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new ComponentStateQuery<T1, T2, T3, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new ComponentStateQuery<T1, T2, T3, T4, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new ComponentStateQuery<T1, T2, T3, T4, T5, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, T6, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new ComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), true);
        }

        public void ParallelForEach(IdComponentFunc func)
        {
            Query(new IdComponentQuery(func), true);
        }

        public void ParallelForEach<T1>(IdComponentFunc<T1> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new IdComponentQuery<T1>(func), true);
        }

        public void ParallelForEach<T1, T2>(IdComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new IdComponentQuery<T1, T2>(func), true);
        }

        public void ParallelForEach<T1, T2, T3>(IdComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new IdComponentQuery<T1, T2, T3>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4>(IdComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new IdComponentQuery<T1, T2, T3, T4>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5>(IdComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new IdComponentQuery<T1, T2, T3, T4, T5>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, T6>(IdComponentFunc<T1, T2, T3, T4, T5, T6> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new IdComponentQuery<T1, T2, T3, T4, T5, T6>(func), true);
        }

        public void ParallelForEach<TState>(TState state, IdComponentStateFunc<TState> func)
        {
            Query(new IdComponentStateQuery<TState>(func, state), true);
        }

        public void ParallelForEach<T1, TState>(TState state, IdComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            ZeroCheck<T1>();
            Query(new IdComponentStateQuery<T1, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, TState>(TState state, IdComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            Query(new IdComponentStateQuery<T1, T2, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, TState>(TState state, IdComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            Query(new IdComponentStateQuery<T1, T2, T3, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            Query(new IdComponentStateQuery<T1, T2, T3, T4, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            Query(new IdComponentStateQuery<T1, T2, T3, T4, T5, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, T6, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            ZeroCheck<T1>();
            ZeroCheck<T2>();
            ZeroCheck<T3>();
            ZeroCheck<T4>();
            ZeroCheck<T5>();
            ZeroCheck<T6>();
            Query(new IdComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), true);
        }

        public void ParallelForEach(EntityFunc func) => Query(new EntityQuery(func), true);
        public void ParallelForEach<TState>(TState state, EntityStateFunc<TState> func) => Query(new EntityStateQuery<TState>(func, state), true);

        internal static void AddtoFilter(ref EntityArchetype archetype, int typeId)
        {
            var depth = typeId / 64;
            if (depth + 1 > archetype.Depth)
            {
                var newArchetype = ArchetypeCache.Rent(depth + 1);
                archetype.CopyTo(newArchetype);
                ArchetypeCache.Return(archetype);
                archetype = newArchetype;
            }

            archetype[depth] |= 1ul << (typeId % 64);
        }

        internal static void RemoveFromFilter(ref EntityArchetype archetype, int typeId)
        {
            var depth = typeId / 64;
            if (depth + 1 > archetype.Depth) return;
            archetype[depth] &= ~(1ul << (typeId % 64));

            if (depth + 1 == archetype.Depth &&
                archetype[depth] == 0)
            {
                var newArchetype = ArchetypeCache.Rent(depth);
                archetype.CopyTo(newArchetype);
                ArchetypeCache.Return(archetype);
                archetype = newArchetype;
            }
        }

        internal bool Contains(in EntityArchetype archetype)
        {
            return archetype.ContainsAny(in AnyFilter) &&
                (NoFilter.Depth == 0 || !archetype.ContainsAny(in NoFilter)) &&
                archetype.ContainsAll(in WithFilter);
        }

        internal unsafe void Query<T>(T query, bool parallel) where T : IQuery => _entityDatabase.Query(query, parallel, this);

        internal void Return()
        {
            ArchetypeCache.Return(AnyFilter);
            ArchetypeCache.Return(NoFilter);
            ArchetypeCache.Return(WithFilter);

            AnyFilter = default;
            NoFilter = default;
            WithFilter = default;
        }

        private static void ZeroCheck<T1>() where T1 : unmanaged
        {
            if (ComponentRegistry.Type<T1>.ZeroSize) throw new Exception($"Zero-size components cannot be iterated!");
        }
    }
}
