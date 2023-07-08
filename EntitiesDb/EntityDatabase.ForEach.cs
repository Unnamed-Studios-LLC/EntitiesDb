using EntitiesDb.Queries;

namespace EntitiesDb
{
    public sealed partial class EntityDatabase
    {
        public void ForEach<T1>(ComponentFunc<T1> func)
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2>(ComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3>(ComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3, T4>(ComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3, T4, T5>(ComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3, T4, T5, T6>(ComponentFunc<T1, T2, T3, T4, T5, T6> func)
			where T1 : unmanaged
			where T2 : unmanaged
			where T3 : unmanaged
			where T4 : unmanaged
			where T5 : unmanaged
			where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, TState>(TState state, ComponentStateFunc<T1, TState> func)
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(state, func);
        }

        public void ForEach<T1, T2, TState>(TState state, ComponentStateFunc<T1, T2, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(state, func);
        }

        public void ForEach<T1, T2, T3, TState>(TState state, ComponentStateFunc<T1, T2, T3, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(state, func);
        }

        public void ForEach<T1, T2, T3, T4, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(state, func);
        }

        public void ForEach<T1, T2, T3, T4, T5, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(state, func);
        }

        public void ForEach<T1, T2, T3, T4, T5, T6, TState>(TState state, ComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(state, func);
        }

        public void ForEach<T1>(IdComponentFunc<T1> func)
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2>(IdComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3>(IdComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3, T4>(IdComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3, T4, T5>(IdComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach<T1, T2, T3, T4, T5, T6>(IdComponentFunc<T1, T2, T3, T4, T5, T6> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ForEach(EntityFunc func)
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ForEach(func);
        }

        public void ParallelForEach<T1>(ComponentFunc<T1> func)
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2>(ComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3>(ComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3, T4>(ComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5>(ComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, T6>(ComponentFunc<T1, T2, T3, T4, T5, T6> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1>(IdComponentFunc<T1> func)
            where T1 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2>(IdComponentFunc<T1, T2> func)
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3>(IdComponentFunc<T1, T2, T3> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, T6>(IdComponentFunc<T1, T2, T3, T4> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5>(IdComponentFunc<T1, T2, T3, T4, T5> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach<T1, T2, T3, T4, T5, T6>(IdComponentFunc<T1, T2, T3, T4, T5, T6> func)
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
            where T6 : unmanaged
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }

        public void ParallelForEach(EntityFunc func)
        {
            var queryFilter = new QueryFilter(this);
            queryFilter.ParallelForEach(func);
        }
    }
}
