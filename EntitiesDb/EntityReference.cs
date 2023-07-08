using System.Runtime.CompilerServices;
using EntitiesDb.Components;
using EntitiesDb.Data;

namespace EntitiesDb
{
    internal readonly struct EntityReference
    {
        public EntityReference(EntityGroup group, EntityIndex index)
        {
            Group = group;
            Index = index;
        }

        public EntityGroup Group { get; }
        public EntityIndex Index { get; }
        public uint EntityId => GetEntityId();

        public unsafe void CopyComponentsInto(EntityReference destination)
        {
            if (Group == null || destination.Group == null) return;
            var sourceChunk = Group.GetChunk(Index.Chunk);
            var destinationChunk = destination.Group.GetChunk(Index.Chunk);

            int j = -1;
            int destinationId = -1;
            for (int i = 0; i < Group.ComponentIds.Length; i++)
            {
                var sourceId = Group.ComponentIds[i];

                while (destinationId < sourceId)
                {
                    if (++j >= destination.Group.ComponentIds.Length)
                    {
                        return;
                    }
                    destinationId = destination.Group.ComponentIds[j];
                }

                if (destinationId == sourceId)
                {
                    var size = ComponentRegistry.Get(sourceId).Size;
                    var sourcePtr = sourceChunk.GetComponent(Group.ListOffsets[i], size, Index.List);
                    var destinationPtr = destinationChunk.GetComponent(destination.Group.ListOffsets[j], size, destination.Index.List);
                    Buffer.MemoryCopy(sourcePtr, destinationPtr, size, size);
                }
            }
        }

        public unsafe ref T GetComponent<T>() where T : unmanaged
        {
            if (Group == null)
            {
                throw new Exception($"Component of type '{typeof(T)}' not found in entity.");
            }

            return ref Group.GetComponent<T>(Index.Chunk, Index.List);
        }

        public Entity GetEntity() => new(Group, Group?.GetChunk(Index.Chunk) ?? default, Index.List);

        public unsafe ref T TryGetComponent<T>(out bool found) where T : unmanaged
        {
            if (Group == null)
            {
                found = false;
                return ref Unsafe.NullRef<T>();
            }

            return ref Group.TryGetComponent<T>(Index.Chunk, Index.List, out found);
        }

        private unsafe uint GetEntityId()
        {
            var chunk = Group.GetChunk(Index.Chunk);
            return chunk.EntityIds[Index.List];
        }
    }
}
