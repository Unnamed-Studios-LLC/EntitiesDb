using EntitiesDb.Components;

namespace EntitiesDb
{
    public partial class EntityDatabase
    {
        public EntityDatabase Any<T1>()
            where T1 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            AddtoFilter(ref _queryFilter.Any, id1);
            return this;
        }

        public EntityDatabase Any<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            AddtoFilter(ref _queryFilter.Any, id1);
            AddtoFilter(ref _queryFilter.Any, id2);
            return this;
        }

        public EntityDatabase Any<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            AddtoFilter(ref _queryFilter.Any, id1);
            AddtoFilter(ref _queryFilter.Any, id2);
            AddtoFilter(ref _queryFilter.Any, id3);
            return this;
        }

        public EntityDatabase Any<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            AddtoFilter(ref _queryFilter.Any, id1);
            AddtoFilter(ref _queryFilter.Any, id2);
            AddtoFilter(ref _queryFilter.Any, id3);
            AddtoFilter(ref _queryFilter.Any, id4);
            return this;
        }

        public EntityDatabase Any<T1, T2, T3, T4, T5>()
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
            AddtoFilter(ref _queryFilter.Any, id1);
            AddtoFilter(ref _queryFilter.Any, id2);
            AddtoFilter(ref _queryFilter.Any, id3);
            AddtoFilter(ref _queryFilter.Any, id4);
            AddtoFilter(ref _queryFilter.Any, id5);
            return this;
        }

        public EntityDatabase Any<T1, T2, T3, T4, T5, T6>()
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
            AddtoFilter(ref _queryFilter.Any, id1);
            AddtoFilter(ref _queryFilter.Any, id2);
            AddtoFilter(ref _queryFilter.Any, id3);
            AddtoFilter(ref _queryFilter.Any, id4);
            AddtoFilter(ref _queryFilter.Any, id5);
            AddtoFilter(ref _queryFilter.Any, id6);
            return this;
        }

        public EntityDatabase IncludeDisabled()
        {
            var disabledId = ComponentRegistry.Type<Disabled>.Id;
            RemoveFromFilter(ref _queryFilter.No, disabledId);
            return this;
        }

        public EntityDatabase No<T1>()
            where T1 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            AddtoFilter(ref _queryFilter.No, id1);
            return this;
        }

        public EntityDatabase No<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            AddtoFilter(ref _queryFilter.No, id1);
            AddtoFilter(ref _queryFilter.No, id2);
            return this;
        }

        public EntityDatabase No<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            AddtoFilter(ref _queryFilter.No, id1);
            AddtoFilter(ref _queryFilter.No, id2);
            AddtoFilter(ref _queryFilter.No, id3);
            return this;
        }

        public EntityDatabase No<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            AddtoFilter(ref _queryFilter.No, id1);
            AddtoFilter(ref _queryFilter.No, id2);
            AddtoFilter(ref _queryFilter.No, id3);
            AddtoFilter(ref _queryFilter.No, id4);
            return this;
        }

        public EntityDatabase No<T1, T2, T3, T4, T5>()
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
            AddtoFilter(ref _queryFilter.No, id1);
            AddtoFilter(ref _queryFilter.No, id2);
            AddtoFilter(ref _queryFilter.No, id3);
            AddtoFilter(ref _queryFilter.No, id4);
            AddtoFilter(ref _queryFilter.No, id5);
            return this;
        }

        public EntityDatabase No<T1, T2, T3, T4, T5, T6>()
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
            AddtoFilter(ref _queryFilter.No, id1);
            AddtoFilter(ref _queryFilter.No, id2);
            AddtoFilter(ref _queryFilter.No, id3);
            AddtoFilter(ref _queryFilter.No, id4);
            AddtoFilter(ref _queryFilter.No, id5);
            AddtoFilter(ref _queryFilter.No, id6);
            return this;
        }

        public EntityDatabase With<T1>()
            where T1 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            AddtoFilter(ref _queryFilter.With, id1);
            return this;
        }

        public EntityDatabase With<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            AddtoFilter(ref _queryFilter.With, id1);
            AddtoFilter(ref _queryFilter.With, id2);
            return this;
        }

        public EntityDatabase With<T1, T2, T3>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            AddtoFilter(ref _queryFilter.With, id1);
            AddtoFilter(ref _queryFilter.With, id2);
            AddtoFilter(ref _queryFilter.With, id3);
            return this;
        }

        public EntityDatabase With<T1, T2, T3, T4>()
            where T1 : unmanaged
            where T2 : unmanaged
            where T3 : unmanaged
            where T4 : unmanaged
        {
            var id1 = ComponentRegistry.Type<T1>.Id;
            var id2 = ComponentRegistry.Type<T2>.Id;
            var id3 = ComponentRegistry.Type<T3>.Id;
            var id4 = ComponentRegistry.Type<T4>.Id;
            AddtoFilter(ref _queryFilter.With, id1);
            AddtoFilter(ref _queryFilter.With, id2);
            AddtoFilter(ref _queryFilter.With, id3);
            AddtoFilter(ref _queryFilter.With, id4);
            return this;
        }

        public EntityDatabase With<T1, T2, T3, T4, T5>()
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
            AddtoFilter(ref _queryFilter.With, id1);
            AddtoFilter(ref _queryFilter.With, id2);
            AddtoFilter(ref _queryFilter.With, id3);
            AddtoFilter(ref _queryFilter.With, id4);
            AddtoFilter(ref _queryFilter.With, id5);
            return this;
        }

        public EntityDatabase With<T1, T2, T3, T4, T5, T6>()
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
            AddtoFilter(ref _queryFilter.With, id1);
            AddtoFilter(ref _queryFilter.With, id2);
            AddtoFilter(ref _queryFilter.With, id3);
            AddtoFilter(ref _queryFilter.With, id4);
            AddtoFilter(ref _queryFilter.With, id5);
            AddtoFilter(ref _queryFilter.With, id6);
            return this;
        }
    }
}
