using System.Runtime.CompilerServices;
using EntitiesDb.Data;

namespace EntitiesDb
{
    public ref struct Entity
    {
        internal EntityGroup Group;
        internal EntityChunk Chunk;
        internal int ListIndex;

        public unsafe uint Id => Chunk.EntityIds[ListIndex];

        internal Entity(EntityGroup group, EntityChunk chunk, int listIndex)
        {
            Group = group;
            Chunk = chunk;
            ListIndex = listIndex;
        }

        public ref T GetComponent<T>() where T : unmanaged
        {
            if (Group == null)
            {
                throw new Exception($"Component of type '{typeof(T)}' not found in entity.");
            }
            return ref Group.GetComponent<T>(Chunk, ListIndex);
        }
        public ref T TryGetComponent<T>(out bool found) where T : unmanaged
        {
            if (Group == null)
            {
                found = false;
                return ref Unsafe.NullRef<T>();
            }
            return ref Group.TryGetComponent<T>(Chunk, ListIndex, out found);
        }
    }
}
