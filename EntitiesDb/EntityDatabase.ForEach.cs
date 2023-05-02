using EntitiesDb.Queries;

namespace EntitiesDb
{
    public sealed partial class EntityDatabase
    {
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

        public void ForEach<T1, T2, T3, T4, T5, T6>(ComponentFunc<T1, T2, T3, T4> func)
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

        public void ForEach<T1, T2, T3, T4, T5, T6>(IdComponentFunc<T1, T2, T3, T4> func)
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

        public void ForEach(EntityFunc func) => Query(new EntityQuery(func), false);
        public void ForEach(IdEntityFunc func) => Query(new IdEntityQuery(func), false);

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

        public void ParallelForEach<T1, T2, T3, T4, T5, T6>(ComponentFunc<T1, T2, T3, T4> func)
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

        public void ParallelForEach<T1, T2, T3, T4, T5, T6>(IdComponentFunc<T1, T2, T3, T4> func)
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

        public void ParallelForEach(EntityFunc func) => Query(new EntityQuery(func), true);
        public void ParallelForEach(IdEntityFunc func) => Query(new IdEntityQuery(func), true);
    }
}
