using System;
using System.Collections.Generic;

namespace EntitiesDb
{
    public readonly ref struct QueryFilter
    {
        public readonly HashSet<Type> AnyTypes;
        public readonly HashSet<Type> NoTypes;
        public readonly HashSet<Type> WithTypes;

        private readonly EntityDatabase _entityDatabase;

        internal QueryFilter(EntityDatabase entityDatabase,
            HashSet<Type> anyTypes,
            HashSet<Type> noTypes,
            HashSet<Type> withTypes)
        {
            _entityDatabase = entityDatabase;
            AnyTypes = anyTypes;
            NoTypes = noTypes;
            WithTypes = withTypes;
            NoTypes.Add(typeof(Disabled));
        }

        public QueryFilter Any(Type type)
        {
            AnyTypes.Add(type);
            return this;
        }

        public QueryFilter Any(params Type[] types)
        {
            foreach (var type in types) AnyTypes.Add(type);
            return this;
        }

        public QueryFilter Any<T1>()
            where T1 : unmanaged
        {
            AnyTypes.Add(typeof(T1));
            return this;
        }

        public QueryFilter Any<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            AnyTypes.Add(typeof(T1));
            AnyTypes.Add(typeof(T2));
            return this;
        }

        public QueryFilter Any<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            AnyTypes.Add(typeof(T1));
            AnyTypes.Add(typeof(T2));
            AnyTypes.Add(typeof(T3));
            return this;
        }

        public QueryFilter Any<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            AnyTypes.Add(typeof(T1));
            AnyTypes.Add(typeof(T2));
            AnyTypes.Add(typeof(T3));
            AnyTypes.Add(typeof(T4));
            return this;
        }

        public QueryFilter Any<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            AnyTypes.Add(typeof(T1));
            AnyTypes.Add(typeof(T2));
            AnyTypes.Add(typeof(T3));
            AnyTypes.Add(typeof(T4));
            AnyTypes.Add(typeof(T5));
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
            AnyTypes.Add(typeof(T1));
            AnyTypes.Add(typeof(T2));
            AnyTypes.Add(typeof(T3));
            AnyTypes.Add(typeof(T4));
            AnyTypes.Add(typeof(T5));
            AnyTypes.Add(typeof(T6));
            return this;
        }

        public QueryFilter IncludeDisabled()
        {
            NoTypes.Remove(typeof(Disabled));
            return this;
        }

        public QueryFilter No(Type type)
        {
            NoTypes.Add(type);
            return this;
        }

        public QueryFilter No(params Type[] types)
        {
            foreach (var type in types) NoTypes.Add(type);
            return this;
        }

        public QueryFilter No<T1>()
            where T1 : unmanaged
        {
            NoTypes.Add(typeof(T1));
            return this;
        }

        public QueryFilter No<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            NoTypes.Add(typeof(T1));
            NoTypes.Add(typeof(T2));
            return this;
        }

        public QueryFilter No<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            NoTypes.Add(typeof(T1));
            NoTypes.Add(typeof(T2));
            NoTypes.Add(typeof(T3));
            return this;
        }

        public QueryFilter No<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            NoTypes.Add(typeof(T1));
            NoTypes.Add(typeof(T2));
            NoTypes.Add(typeof(T3));
            NoTypes.Add(typeof(T4));
            return this;
        }

        public QueryFilter No<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            NoTypes.Add(typeof(T1));
            NoTypes.Add(typeof(T2));
            NoTypes.Add(typeof(T3));
            NoTypes.Add(typeof(T4));
            NoTypes.Add(typeof(T5));
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
            NoTypes.Add(typeof(T1));
            NoTypes.Add(typeof(T2));
            NoTypes.Add(typeof(T3));
            NoTypes.Add(typeof(T4));
            NoTypes.Add(typeof(T5));
            NoTypes.Add(typeof(T6));
            return this;
        }

        public QueryFilter With(Type type)
        {
            WithTypes.Add(type);
            return this;
        }

        public QueryFilter With(params Type[] types)
        {
            foreach (var type in types) WithTypes.Add(type);
            return this;
        }

        public QueryFilter With<T1>()
            where T1 : unmanaged
        {
            WithTypes.Add(typeof(T1));
            return this;
        }

        public QueryFilter With<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            WithTypes.Add(typeof(T1));
            WithTypes.Add(typeof(T2));
            return this;
        }

        public QueryFilter With<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            WithTypes.Add(typeof(T1));
            WithTypes.Add(typeof(T2));
            WithTypes.Add(typeof(T3));
            return this;
        }

        public QueryFilter With<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            WithTypes.Add(typeof(T1));
            WithTypes.Add(typeof(T2));
            WithTypes.Add(typeof(T3));
            WithTypes.Add(typeof(T4));
            return this;
        }

        public QueryFilter With<T1, T2, T3, T4, T5>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            WithTypes.Add(typeof(T1));
            WithTypes.Add(typeof(T2));
            WithTypes.Add(typeof(T3));
            WithTypes.Add(typeof(T4));
            WithTypes.Add(typeof(T5));
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
            WithTypes.Add(typeof(T1));
            WithTypes.Add(typeof(T2));
            WithTypes.Add(typeof(T3));
            WithTypes.Add(typeof(T4));
            WithTypes.Add(typeof(T5));
            WithTypes.Add(typeof(T6));
            return this;
        }

        public void ForEach<T1>(ComponentFunc<T1> func)
            where T1 : unmanaged
        {
            Query(new ComponentQuery<T1>(func), false);
        }

        public void ForEach<T1, T2>(ComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new ComponentQuery<T1, T2>(func), false);
        }

        public void ForEach<T1, T2, T3>(ComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new ComponentQuery<T1, T2, T3>(func), false);
        }

        public void ForEach<T1, T2, T3, T4>(ComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new ComponentQuery<T1, T2, T3, T4>(func), false);
        }

        public void ForEach<T1, T2, T3, T4, T5>(ComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new ComponentQuery<T1, T2, T3, T4, T5, T6>(func), false);
        }

        public void ForEach<T1, TState>(TState state, ComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            Query(new ComponentStateQuery<T1, TState>(func, state), false);
        }

        public void ForEach<T1, T2, TState>(TState state, ComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new ComponentStateQuery<T1, T2, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, TState>(TState state, ComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new ComponentStateQuery<T1, T2, T3, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new ComponentStateQuery<T1, T2, T3, T4, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new ComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), false);
        }

        public void ForEach(IdComponentFunc func)
        {
            Query(new IdComponentQuery(func), false);
        }

        public void ForEach<T1>(IdComponentFunc<T1> func)
            where T1 : unmanaged
        {
            Query(new IdComponentQuery<T1>(func), false);
        }

        public void ForEach<T1, T2>(IdComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new IdComponentQuery<T1, T2>(func), false);
        }

        public void ForEach<T1, T2, T3>(IdComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new IdComponentQuery<T1, T2, T3>(func), false);
        }

        public void ForEach<T1, T2, T3, T4>(IdComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new IdComponentQuery<T1, T2, T3, T4>(func), false);
        }

        public void ForEach<T1, T2, T3, T4, T5>(IdComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new IdComponentQuery<T1, T2, T3, T4, T5, T6>(func), false);
        }

        public void ForEach<TState>(TState state, IdComponentStateFunc<TState> func)
        {
            Query(new IdComponentStateQuery<TState>(func, state), false);
        }

        public void ForEach<T1, TState>(TState state, IdComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, TState>(func, state), false);
        }

        public void ForEach<T1, T2, TState>(TState state, IdComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, T2, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, TState>(TState state, IdComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, T2, T3, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, T2, T3, T4, TState>(func, state), false);
        }

        public void ForEach<T1, T2, T3, T4, T5, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new IdComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), false);
        }

        public void ParallelForEach<T1>(ComponentFunc<T1> func)
            where T1 : unmanaged
        {
            Query(new ComponentQuery<T1>(func), true);
        }

        public void ParallelForEach<T1, T2>(ComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new ComponentQuery<T1, T2>(func), true);
        }

        public void ParallelForEach<T1, T2, T3>(ComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new ComponentQuery<T1, T2, T3>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4>(ComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new ComponentQuery<T1, T2, T3, T4>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5>(ComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new ComponentQuery<T1, T2, T3, T4, T5, T6>(func), true);
        }

        public void ParallelForEach<T1, TState>(TState state, ComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            Query(new ComponentStateQuery<T1, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, TState>(TState state, ComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new ComponentStateQuery<T1, T2, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, TState>(TState state, ComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new ComponentStateQuery<T1, T2, T3, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new ComponentStateQuery<T1, T2, T3, T4, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new ComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), true);
        }

        public void ParallelForEach(IdComponentFunc func)
        {
            Query(new IdComponentQuery(func), true);
        }

        public void ParallelForEach<T1>(IdComponentFunc<T1> func)
            where T1 : unmanaged
        {
            Query(new IdComponentQuery<T1>(func), true);
        }

        public void ParallelForEach<T1, T2>(IdComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new IdComponentQuery<T1, T2>(func), true);
        }

        public void ParallelForEach<T1, T2, T3>(IdComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new IdComponentQuery<T1, T2, T3>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4>(IdComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new IdComponentQuery<T1, T2, T3, T4>(func), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5>(IdComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new IdComponentQuery<T1, T2, T3, T4, T5, T6>(func), true);
        }

        public void ParallelForEach<TState>(TState state, IdComponentStateFunc<TState> func)
        {
            Query(new IdComponentStateQuery<TState>(func, state), true);
        }

        public void ParallelForEach<T1, TState>(TState state, IdComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, TState>(TState state, IdComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, T2, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, TState>(TState state, IdComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, T2, T3, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            Query(new IdComponentStateQuery<T1, T2, T3, T4, TState>(func, state), true);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, TState>(TState state, IdComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
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
            Query(new IdComponentStateQuery<T1, T2, T3, T4, T5, T6, TState>(func, state), true);
        }

        internal unsafe void Query<TQuery>(TQuery query, bool parallel) where TQuery : IQuery
        {
            _entityDatabase.Query(query, parallel, this);
        }
    }
}
