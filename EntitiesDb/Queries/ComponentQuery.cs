using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EntitiesDb
{
    internal unsafe readonly struct ComponentQuery<T1> : IQuery
        where T1 : unmanaged
    {
        private readonly ComponentFunc<T1> _componentFunc;

        public ComponentQuery(ComponentFunc<T1> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1)
                );
                list1++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
        }
    }

    internal unsafe readonly struct ComponentQuery<T1, T2> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
    {
        private readonly ComponentFunc<T1, T2> _componentFunc;

        public ComponentQuery(ComponentFunc<T1, T2> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2)
                );
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

    internal unsafe readonly struct ComponentQuery<T1, T2, T3> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        private readonly ComponentFunc<T1, T2, T3> _componentFunc;

        public ComponentQuery(ComponentFunc<T1, T2, T3> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3)
                );
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

    internal unsafe readonly struct ComponentQuery<T1, T2, T3, T4> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        private readonly ComponentFunc<T1, T2, T3, T4> _componentFunc;

        public ComponentQuery(ComponentFunc<T1, T2, T3, T4> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4)
                );
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

    internal unsafe readonly struct ComponentQuery<T1, T2, T3, T4, T5> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        private readonly ComponentFunc<T1, T2, T3, T4, T5> _componentFunc;

        public ComponentQuery(ComponentFunc<T1, T2, T3, T4, T5> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5)
                );
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

    internal unsafe readonly struct ComponentQuery<T1, T2, T3, T4, T5, T6> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        private readonly ComponentFunc<T1, T2, T3, T4, T5, T6> _componentFunc;

        public ComponentQuery(ComponentFunc<T1, T2, T3, T4, T5, T6> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            var list6 = chunk.GetList<T6>(job.Archetype.GetListOffset(typeof(T6)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5),
                    ref Unsafe.AsRef<T6>(list6)
                );
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

    internal unsafe readonly struct ComponentStateQuery<T1, TState> : IQuery
        where T1 : unmanaged
    {
        private readonly ComponentStateFunc<T1, TState> _componentFunc;
        private readonly TState _state;

        public ComponentStateQuery(ComponentStateFunc<T1, TState> componentFunc, TState state)
        {
            _componentFunc = componentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    _state
                );
                list1++;
            }
        }

        public IEnumerable<ComponentMetaData> GetDelegateMetaData()
        {
            yield return ComponentMetaData<T1>.Instance;
        }
    }

    internal unsafe struct ComponentStateQuery<T1, T2, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
    {
        private readonly ComponentStateFunc<T1, T2, TState> _componentFunc;
        private readonly TState _state;

        public ComponentStateQuery(ComponentStateFunc<T1, T2, TState> componentFunc, TState state)
        {
            _componentFunc = componentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    _state
                );
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

    internal unsafe struct ComponentStateQuery<T1, T2, T3, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        private readonly ComponentStateFunc<T1, T2, T3, TState> _componentFunc;
        private readonly TState _state;

        public ComponentStateQuery(ComponentStateFunc<T1, T2, T3, TState> componentFunc, TState state)
        {
            _componentFunc = componentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    _state
                );
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

    internal unsafe struct ComponentStateQuery<T1, T2, T3, T4, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        private readonly ComponentStateFunc<T1, T2, T3, T4, TState> _componentFunc;
        private readonly TState _state;

        public ComponentStateQuery(ComponentStateFunc<T1, T2, T3, T4, TState> componentFunc, TState state)
        {
            _componentFunc = componentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    _state
                );
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

    internal unsafe struct ComponentStateQuery<T1, T2, T3, T4, T5, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        private readonly ComponentStateFunc<T1, T2, T3, T4, T5, TState> _componentFunc;
        private readonly TState _state;

        public ComponentStateQuery(ComponentStateFunc<T1, T2, T3, T4, T5, TState> componentFunc, TState state)
        {
            _componentFunc = componentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5),
                    _state
                );
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

    internal unsafe struct ComponentStateQuery<T1, T2, T3, T4, T5, T6, TState> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        private readonly ComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> _componentFunc;
        private readonly TState _state;

        public ComponentStateQuery(ComponentStateFunc<T1, T2, T3, T4, T5, T6, TState> componentFunc, TState state)
        {
            _componentFunc = componentFunc;
            _state = state;
        }

        public void EnumerateChunk(in EnumerationJob job)
        {
            var chunk = job.Archetype.GetChunk(job.ChunkIndex);
            var count = job.Archetype.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Archetype.GetListOffset(typeof(T1)));
            var list2 = chunk.GetList<T2>(job.Archetype.GetListOffset(typeof(T2)));
            var list3 = chunk.GetList<T3>(job.Archetype.GetListOffset(typeof(T3)));
            var list4 = chunk.GetList<T4>(job.Archetype.GetListOffset(typeof(T4)));
            var list5 = chunk.GetList<T5>(job.Archetype.GetListOffset(typeof(T5)));
            var list6 = chunk.GetList<T6>(job.Archetype.GetListOffset(typeof(T6)));
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1),
                    ref Unsafe.AsRef<T2>(list2),
                    ref Unsafe.AsRef<T3>(list3),
                    ref Unsafe.AsRef<T4>(list4),
                    ref Unsafe.AsRef<T5>(list5),
                    ref Unsafe.AsRef<T6>(list6),
                    _state
                );
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
