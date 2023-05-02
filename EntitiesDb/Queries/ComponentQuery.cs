using System.Runtime.CompilerServices;
using EntitiesDb.Components;
using EntitiesDb.Data;

namespace EntitiesDb.Queries
{
    internal unsafe struct ComponentQuery<T1> : IQuery
        where T1 : unmanaged
    {
        private readonly ComponentFunc<T1> _componentFunc;

        public ComponentQuery(ComponentFunc<T1> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void CopyIndices(EntityGroup group, Span<int> indices) =>
            group.GetComponentListIndex<T1>(indices);

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var count = job.Group.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Group.ListOffsets[indices[0]]);
            for (int i = 0; i < count; i++)
            {
                _componentFunc.Invoke(
                    ref Unsafe.AsRef<T1>(list1)
                );
                list1++;
            }
        }

        public IEnumerable<int> GetRequiredIds()
        {
            yield return ComponentRegistry.Type<T1>.Id;
        }
    }

    internal unsafe struct ComponentQuery<T1, T2> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
    {
        private readonly ComponentFunc<T1, T2> _componentFunc;

        public ComponentQuery(ComponentFunc<T1, T2> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void CopyIndices(EntityGroup group, Span<int> indices) =>
            group.GetComponentListIndex<T1, T2>(indices);

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var count = job.Group.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Group.ListOffsets[indices[0]]);
            var list2 = chunk.GetList<T2>(job.Group.ListOffsets[indices[1]]);
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

        public IEnumerable<int> GetRequiredIds()
        {
            yield return ComponentRegistry.Type<T1>.Id;
            yield return ComponentRegistry.Type<T2>.Id;
        }
    }

    internal unsafe struct ComponentQuery<T1, T2, T3> : IQuery
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        private readonly ComponentFunc<T1, T2, T3> _componentFunc;

        public ComponentQuery(ComponentFunc<T1, T2, T3> componentFunc)
        {
            _componentFunc = componentFunc;
        }

        public void CopyIndices(EntityGroup group, Span<int> indices) =>
            group.GetComponentListIndex<T1, T2, T3>(indices);

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var count = job.Group.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Group.ListOffsets[indices[0]]);
            var list2 = chunk.GetList<T2>(job.Group.ListOffsets[indices[1]]);
            var list3 = chunk.GetList<T3>(job.Group.ListOffsets[indices[2]]);
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

        public IEnumerable<int> GetRequiredIds()
        {
            yield return ComponentRegistry.Type<T1>.Id;
            yield return ComponentRegistry.Type<T2>.Id;
            yield return ComponentRegistry.Type<T3>.Id;
        }
    }

    internal unsafe struct ComponentQuery<T1, T2, T3, T4> : IQuery
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

        public void CopyIndices(EntityGroup group, Span<int> indices) =>
            group.GetComponentListIndex<T1, T2, T3, T4>(indices);

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var count = job.Group.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Group.ListOffsets[indices[0]]);
            var list2 = chunk.GetList<T2>(job.Group.ListOffsets[indices[1]]);
            var list3 = chunk.GetList<T3>(job.Group.ListOffsets[indices[2]]);
            var list4 = chunk.GetList<T4>(job.Group.ListOffsets[indices[3]]);
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

        public IEnumerable<int> GetRequiredIds()
        {
            yield return ComponentRegistry.Type<T1>.Id;
            yield return ComponentRegistry.Type<T2>.Id;
            yield return ComponentRegistry.Type<T3>.Id;
            yield return ComponentRegistry.Type<T4>.Id;
        }
    }

    internal unsafe struct ComponentQuery<T1, T2, T3, T4, T5> : IQuery
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

        public void CopyIndices(EntityGroup group, Span<int> indices) =>
            group.GetComponentListIndex<T1, T2, T3, T4, T5>(indices);

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var count = job.Group.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Group.ListOffsets[indices[0]]);
            var list2 = chunk.GetList<T2>(job.Group.ListOffsets[indices[1]]);
            var list3 = chunk.GetList<T3>(job.Group.ListOffsets[indices[2]]);
            var list4 = chunk.GetList<T4>(job.Group.ListOffsets[indices[3]]);
            var list5 = chunk.GetList<T5>(job.Group.ListOffsets[indices[4]]);
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

        public IEnumerable<int> GetRequiredIds()
        {
            yield return ComponentRegistry.Type<T1>.Id;
            yield return ComponentRegistry.Type<T2>.Id;
            yield return ComponentRegistry.Type<T3>.Id;
            yield return ComponentRegistry.Type<T4>.Id;
            yield return ComponentRegistry.Type<T5>.Id;
        }
    }

    internal unsafe struct ComponentQuery<T1, T2, T3, T4, T5, T6> : IQuery
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

        public void CopyIndices(EntityGroup group, Span<int> indices) =>
            group.GetComponentListIndex<T1, T2, T3, T4, T5, T6>(indices);

        public void EnumerateChunk(in EnumerationJob job, Span<int> indices)
        {
            var chunk = job.Group.GetChunk(job.ChunkIndex);
            var count = job.Group.GetChunkLength(job.ChunkIndex);
            var list1 = chunk.GetList<T1>(job.Group.ListOffsets[indices[0]]);
            var list2 = chunk.GetList<T2>(job.Group.ListOffsets[indices[1]]);
            var list3 = chunk.GetList<T3>(job.Group.ListOffsets[indices[2]]);
            var list4 = chunk.GetList<T4>(job.Group.ListOffsets[indices[3]]);
            var list5 = chunk.GetList<T5>(job.Group.ListOffsets[indices[4]]);
            var list6 = chunk.GetList<T6>(job.Group.ListOffsets[indices[5]]);
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

        public IEnumerable<int> GetRequiredIds()
        {
            yield return ComponentRegistry.Type<T1>.Id;
            yield return ComponentRegistry.Type<T2>.Id;
            yield return ComponentRegistry.Type<T3>.Id;
            yield return ComponentRegistry.Type<T4>.Id;
            yield return ComponentRegistry.Type<T5>.Id;
            yield return ComponentRegistry.Type<T6>.Id;
        }
    }
}
