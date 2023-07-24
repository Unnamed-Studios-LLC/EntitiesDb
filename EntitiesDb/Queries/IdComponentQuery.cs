using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EntitiesDb
{
    internal unsafe struct IdComponentQuery : IQuery
    {
        private readonly IdComponentFunc _idComponentFunc;

        public IdComponentQuery(IdComponentFunc idComponentFunc)
        {
            _idComponentFunc = idComponentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds
                );
                entityIds++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData() => Array.Empty<ComponentMetaData>();
    }

    internal unsafe struct IdComponentQuery<T1> : IQuery
        where T1 : unmanaged
    {
        private readonly IdComponentFunc<T1> _idComponentFunc;

        public IdComponentQuery(IdComponentFunc<T1> idComponentFunc)
        {
            _idComponentFunc = idComponentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1)
                );
                entityIds++;
                list1++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
        }
    }

    internal unsafe struct IdComponentQuery<T1, T2> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
    {
        private readonly IdComponentFunc<T1, T2> _idComponentFunc;

        public IdComponentQuery(IdComponentFunc<T1, T2> idComponentFunc)
        {
            _idComponentFunc = idComponentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2)
                );
                entityIds++;
                list1++;
                list2++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
        }
    }

    internal unsafe struct IdComponentQuery<T1, T2, T3> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        private readonly IdComponentFunc<T1, T2, T3> _idComponentFunc;

        public IdComponentQuery(IdComponentFunc<T1, T2, T3> idComponentFunc)
        {
            _idComponentFunc = idComponentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3)
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
        }
    }

    internal unsafe struct IdComponentQuery<T1, T2, T3, T4> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        private readonly IdComponentFunc<T1, T2, T3, T4> _idComponentFunc;

        public IdComponentQuery(IdComponentFunc<T1, T2, T3, T4> idComponentFunc)
        {
            _idComponentFunc = idComponentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4)
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
                list4++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
            yield return ComponentMetaData<T4>.Instance;
        }
    }

    internal unsafe struct IdComponentQuery<T1, T2, T3, T4, T5> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        private readonly IdComponentFunc<T1, T2, T3, T4, T5> _idComponentFunc;

        public IdComponentQuery(IdComponentFunc<T1, T2, T3, T4, T5> idComponentFunc)
        {
            _idComponentFunc = idComponentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5)
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
                list4++;
                list5++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
            yield return ComponentMetaData<T4>.Instance;
            yield return ComponentMetaData<T5>.Instance;
        }
    }

    internal unsafe struct IdComponentQuery<T1, T2, T3, T4, T5, T6> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        private readonly IdComponentFunc<T1, T2, T3, T4, T5, T6> _idComponentFunc;

        public IdComponentQuery(IdComponentFunc<T1, T2, T3, T4, T5, T6> idComponentFunc)
        {
            _idComponentFunc = idComponentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            var list6 = chunk.GetList<T6>(job.Archetype.GetListOffset(typeof(T6)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5),
                    ref Unsafe.AsRef<T6>(list6)
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
                list4++;
                list5++;
                list6++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
            yield return ComponentMetaData<T4>.Instance;
            yield return ComponentMetaData<T5>.Instance;
            yield return ComponentMetaData<T6>.Instance;
        }
    }

    internal unsafe struct IdComponentStateQuery<TState> : IQuery
    {
        private readonly IdComponentStateFunc<TState> _idComponentFunc;
        private readonly TState _state;

        public IdComponentStateQuery(IdComponentStateFunc<TState> idComponentFunc, TState state)
        {
            _idComponentFunc = idComponentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    _state
                );
                entityIds++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData() => Array.Empty<ComponentMetaData>();
    }

    internal unsafe struct IdComponentStateQuery<T1, TState> : IQuery
        where T1 : unmanaged
    {
        private readonly IdComponentStateFunc<T1, TState> _idComponentFunc;
        private readonly TState _state;

        public IdComponentStateQuery(IdComponentStateFunc<T1, TState> idComponentFunc, TState state)
        {
            _idComponentFunc = idComponentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    _state
                );
                entityIds++;
                list1++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
        }
    }

    internal unsafe struct IdComponentStateQuery<T1, T2, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
    {
        private readonly IdComponentStateFunc<T1, T2, TState> _idComponentFunc;
        private readonly TState _state;

        public IdComponentStateQuery(IdComponentStateFunc<T1, T2, TState> idComponentFunc, TState state)
        {
            _idComponentFunc = idComponentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    _state
                );
                entityIds++;
                list1++;
                list2++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
        }
    }

    internal unsafe struct IdComponentStateQuery<T1, T2, T3, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        private readonly IdComponentStateFunc<T1, T2, T3, TState> _idComponentFunc;
        private readonly TState _state;

        public IdComponentStateQuery(IdComponentStateFunc<T1, T2, T3, TState> idComponentFunc, TState state)
        {
            _idComponentFunc = idComponentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    _state
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
        }
    }

    internal unsafe struct IdComponentStateQuery<T1, T2, T3, T4, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        private readonly IdComponentStateFunc<T1, T2, T3, T4, TState> _idComponentFunc;
        private readonly TState _state;

        public IdComponentStateQuery(IdComponentStateFunc<T1, T2, T3, T4, TState> idComponentFunc, TState state)
        {
            _idComponentFunc = idComponentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    _state
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
                list4++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
            yield return ComponentMetaData<T4>.Instance;
        }
    }

    internal unsafe struct IdComponentStateQuery<T1, T2, T3, T4, T5, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        private readonly IdComponentStateFunc<T1, T2, T3, T4, T5, TState> _idComponentFunc;
        private readonly TState _state;

        public IdComponentStateQuery(IdComponentStateFunc<T1, T2, T3, T4, T5, TState> idComponentFunc, TState state)
        {
            _idComponentFunc = idComponentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5),
                    _state
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
                list4++;
                list5++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
            yield return ComponentMetaData<T4>.Instance;
            yield return ComponentMetaData<T5>.Instance;
        }
    }

    internal unsafe struct IdComponentStateQuery<T1, T2, T3, T4, T5, T6, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        private readonly IdComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> _idComponentFunc;
        private readonly TState _state;

        public IdComponentStateQuery(IdComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> idComponentFunc, TState state)
        {
            _idComponentFunc = idComponentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var entityIds = (uint*)chunk.Data;
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            var list6 = chunk.GetList<T6>(job.Archetype.GetListOffset(typeof(T6)));
            for (int i = 0; i < count; i++)
            {
                _idComponentFunc.Invoke(
                    *entityIds,
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5),
                    ref Unsafe.AsRef<T6>(list6),
                    _state
                );
                entityIds++;
                list1++;
                list2++;
                list3++;
                list4++;
                list5++;
                list6++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
            yield return ComponentMetaData<T2>.Instance;
            yield return ComponentMetaData<T3>.Instance;
            yield return ComponentMetaData<T4>.Instance;
            yield return ComponentMetaData<T5>.Instance;
            yield return ComponentMetaData<T6>.Instance;
        }
    }
}
