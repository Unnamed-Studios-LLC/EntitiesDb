namespace EntitiesDb.Data
{
    internal partial class EntityGroup
    {
        public void GetComponentListIndex<T0>(Span<int> results)
            where T0 : unmanaged
        {
            var id0 = _componentRegistry.Get<T0>().Id;
            results[0] = -1;
            for (int i = 0; i < ComponentIds.Length; i++)
            {
                var listId = ComponentIds[i];
                if (id0 == listId)
                {
                    results[0] = i;
                }
            }
        }

        public void GetComponentListIndex<T0, T1>(Span<int> results)
            where T0 : unmanaged
            where T1 : unmanaged
        {
            var id0 = _componentRegistry.Get<T0>().Id;
            var id1 = _componentRegistry.Get<T1>().Id;
            results[0] = -1;
            results[1] = -1;
            for (int i = 0; i < ComponentIds.Length; i++)
            {
                var listId = ComponentIds[i];
                if (id0 == listId)
                {
                    results[0] = i;
                }
                else if (id1 == listId)
                {
                    results[1] = i;
                }
            }
        }

        public void GetComponentListIndex<T0, T1, T2>(Span<int> results)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var id0 = _componentRegistry.Get<T0>().Id;
            var id1 = _componentRegistry.Get<T1>().Id;
            var id2 = _componentRegistry.Get<T2>().Id;
            results[0] = -1;
            results[1] = -1;
            results[2] = -1;
            for (int i = 0; i < ComponentIds.Length; i++)
            {
                var listId = ComponentIds[i];
                if (id0 == listId)
                {
                    results[0] = i;
                }
                else if (id1 == listId)
                {
                    results[1] = i;
                }
                else if (id2 == listId)
                {
                    results[2] = i;
                }
            }
        }

        public void GetComponentListIndex<T0, T1, T2, T3>(Span<int> results)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var id0 = _componentRegistry.Get<T0>().Id;
            var id1 = _componentRegistry.Get<T1>().Id;
            var id2 = _componentRegistry.Get<T2>().Id;
            var id3 = _componentRegistry.Get<T3>().Id;
            results[0] = -1;
            results[1] = -1;
            results[2] = -1;
            results[3] = -1;
            for (int i = 0; i < ComponentIds.Length; i++)
            {
                var listId = ComponentIds[i];
                if (id0 == listId)
                {
                    results[0] = i;
                }
                else if (id1 == listId)
                {
                    results[1] = i;
                }
                else if (id2 == listId)
                {
                    results[2] = i;
                }
                else if (id3 == listId)
                {
                    results[3] = i;
                }
            }
        }

        public void GetComponentListIndex<T0, T1, T2, T3, T4>(Span<int> results)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var id0 = _componentRegistry.Get<T0>().Id;
            var id1 = _componentRegistry.Get<T1>().Id;
            var id2 = _componentRegistry.Get<T2>().Id;
            var id3 = _componentRegistry.Get<T3>().Id;
            var id4 = _componentRegistry.Get<T4>().Id;
            results[0] = -1;
            results[1] = -1;
            results[2] = -1;
            results[3] = -1;
            results[4] = -1;
            for (int i = 0; i < ComponentIds.Length; i++)
            {
                var listId = ComponentIds[i];
                if (id0 == listId)
                {
                    results[0] = i;
                }
                else if (id1 == listId)
                {
                    results[1] = i;
                }
                else if (id2 == listId)
                {
                    results[2] = i;
                }
                else if (id3 == listId)
                {
                    results[3] = i;
                }
                else if (id4 == listId)
                {
                    results[4] = i;
                }
            }
        }

        public void GetComponentListIndex<T0, T1, T2, T3, T4, T5>(Span<int> results)
            where T0 : unmanaged
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
            where T5 : unmanaged
        {
            var id0 = _componentRegistry.Get<T0>().Id;
            var id1 = _componentRegistry.Get<T1>().Id;
            var id2 = _componentRegistry.Get<T2>().Id;
            var id3 = _componentRegistry.Get<T3>().Id;
            var id4 = _componentRegistry.Get<T4>().Id;
            var id5 = _componentRegistry.Get<T5>().Id;
            results[0] = -1;
            results[1] = -1;
            results[2] = -1;
            results[3] = -1;
            results[4] = -1;
            results[5] = -1;
            for (int i = 0; i < ComponentIds.Length; i++)
            {
                var listId = ComponentIds[i];
                if (id0 == listId)
                {
                    results[0] = i;
                }
                else if (id1 == listId)
                {
                    results[1] = i;
                }
                else if (id2 == listId)
                {
                    results[2] = i;
                }
                else if (id3 == listId)
                {
                    results[3] = i;
                }
                else if (id4 == listId)
                {
                    results[4] = i;
                }
                else if (id5 == listId)
                {
                    results[5] = i;
                }
            }
        }
    }
}
